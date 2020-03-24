using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Articy.Unity;
using Articy.Unity.Constraints;
using Articy.Unity.Utils;
using Articy.Unity.Interfaces;

using Articy.The_Road_To_Moscow;
using Articy.The_Road_To_Moscow.Templates;
using Articy.The_Road_To_Moscow.Features;
using Articy.The_Road_To_Moscow.GlobalVariables;

using System;
using UnityEngine.UI;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;
using Assets.Scripts.Controllers;
using System.Linq;

public enum TextSpeed { SLOWEST = 5, SLOW = 4, NORMAL = 3, FAST = 2, FASTEST = 1 }

public class GameController : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    #region Dependencies
    private Spawner _spawner;
    private ArticyFlowPlayer _player;
    private SaveSystem _saveSystem;
    private AdvertisementManager _advertisementManager;
    #endregion

    #region Settings
    public TextSpeed textSpeed;
    public bool clampSpeed = true;
    public float clampValue = 2.5f;
    public bool instantMode = false;

    public int secondsToSkip;

    public bool _allowRewinding = true;
    public bool AllowRewinding
    {
        get
        {
            return _allowRewinding;
        }
        set
        {
            _allowRewinding = value;
            Debug.Log($"Rewinding{(_allowRewinding ? " " : " dis")}allowed");
        }
    }

    public bool _playerStandBy = false;
    public bool PlayerStandBy
    {
        get
        {
            return _playerStandBy;
        }
        set
        {
            _playerStandBy = value;
            Debug.Log($"ArticyFlowPlayer is {(_playerStandBy ? "OFF" : "ON")}");
        }
    }

    public bool CanWatchAd { get; set; } = false;

    public bool _gameEnded = false;
    public bool GameEnded
    {
        get
        {
            return _gameEnded;
        }
        set
        {
            _gameEnded = value;
            if (value == true)
            {
                PlayerStandBy = true;
                AllowRewinding = true;
            }
            Debug.Log($"Game ended = {_gameEnded}");
        }
    }
    #endregion

    public DialogueFragment Current { get; private set; }
    public bool ShouldDelay { get; private set; }
    public float NextDelay { get; private set; }

    private float _currentSpeed;
    public Button SpeedUpButton { get; set; }

    public void SkipDelay(float timeToSkipInSeconds)
    {
        var delay = FindObjectOfType<Delay>();

        if (delay.IsRunning)
        {
            Debug.Log("Runtime boost");
            delay.Boost(timeToSkipInSeconds);
            _saveSystem.SubtractSecondssFromExecute(timeToSkipInSeconds);
        }
        else
        {
            Debug.Log("Stoptime boost");
            delay.RemainingTimeInSeconds -= timeToSkipInSeconds;
            delay.Run();
        }

        CanWatchAd = false;
    }

    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
        _player = GetComponent<ArticyFlowPlayer>();
        _saveSystem = GetComponent<SaveSystem>();
        _advertisementManager = GetComponent<AdvertisementManager>();
    }

    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        if (PlayerStandBy)
        {
            return;
        }
        _currentSpeed = (float)textSpeed * 0.02f;

        if (aObject is Dialogue ff)
        {
            _spawner.SpawnPhrase(ff.Text, Color.white, TextPosdition.Middle);
            ShouldDelay = false;
            NextDelay = 0;
        }

        if (aObject is DialogueFragment df)
        {
            ShouldDelay = false;
            NextDelay = 0;
            Current = aObject as DialogueFragment;

            var speaker = df.Speaker as EntityWithTextPosition;
            _spawner.SpawnPhrase(df.Text,speaker.Color, speaker.Template.EntityF.TextPosdition);
        }
        
        if (aObject is Instruction inst)
        {
            float delayInSeconds = (inst as Articy.The_Road_To_Moscow.Delay)?.Template.DelayF.Delay ?? 0;

            if (delayInSeconds > 0)
            {
                ShouldDelay = true;
                NextDelay = delayInSeconds;
            }
            else
            {
                ShouldDelay = false;
                NextDelay = 0;
            }
        }
    }
    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        if (PlayerStandBy)
        {
            return;
        }

        StartCoroutine(OnBranchesUpdatedCoroutine(aBranches));
    }

    private IEnumerator OnBranchesUpdatedCoroutine(IList<Branch> aBranches)
    {
        if (aBranches == null)
        {
            Debug.LogError("No branches found");
            GameObject eg = _spawner.SpawnEndGame(false);
        }

        List<Branch> candidates = new List<Branch>();
        foreach (Branch branch in aBranches)
        {
            // if branch is not DF (empty) - it's a pin - end of flow
            if (!(branch.Target is DialogueFragment) && !(branch.Target is Instruction) && !(branch.Target is Dialogue))
            {
                _spawner.SpawnEndGame(true);
                _saveSystem.LogEvent(Const.LogEvent.LogEndGameWin, string.Empty);
                GameEnded = true;
                yield break;
            }

            if (branch.IsValid)
            {
                candidates.Add(branch);
            }
        }

        // checking for saving - refactor? how?
        if (candidates.Count == 1)
        {
            _saveSystem.LogEvent(Const.LogEvent.LogPhrase, Current.TechnicalName);
            var target = candidates[0].Target;

            _saveSystem.UpdateExecuteElement(((ArticyObject)target).TechnicalName); 
        }
        else if (candidates.Count > 1)
        {
            _saveSystem.LogState();
        }

        var textDelay = !instantMode ? Current.Text.Length * _currentSpeed : 0;

        yield return WaitSeconds(textDelay);

        if (ShouldDelay == true)
        {
            var delayComponent = SetUpDelay(NextDelay);
            delayComponent.OnDelayPassed.AddListener(() => Play(candidates));


            delayComponent.Run();
        }
        else
        {
            Play(candidates);
        }
    }

    public Delay SetUpDelay(float delayInSeconds)
    {
        CanWatchAd = true;

        NotificationManager.ScheduleNotification(DateTime.Now.AddSeconds(delayInSeconds), Const.NotificationKeys.DelayReminder);

        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now.AddSeconds(delayInSeconds);
        _saveSystem.SetStartTimeAndExecuteTime(startTime, endTime);

        var slider = _spawner.SpawnSlider(DateTime.Now, delayInSeconds);
        var delayComponent = slider.GetComponent<Delay>();
        
        var countDownText = slider.GetComponentsInChildren<Text>().Where(x => x.name == "CountDownText").FirstOrDefault();

        delayComponent.OnUpdate.AddListener(() =>
        {
            countDownText.text = TimeSpan.FromSeconds(delayComponent.RemainingTimeInSeconds).ToString(@"hh\:mm\:ss");
            slider.normalizedValue = 1 - Mathf.Lerp(0, 1, delayComponent.RemainingTimeInSeconds / delayInSeconds);
        });

        var speedUpButton = slider.GetComponentInChildren<Button>();
        if (!CanWatchAd)
        {
            speedUpButton.gameObject.SetActive(false);
        }
        speedUpButton.onClick.AddListener(() => _spawner.ShowSkipPopup());

        SpeedUpButton = speedUpButton;

        delayComponent.RemainingTimeInSeconds = delayInSeconds;
        delayComponent.OnDelayPassed.AddListener(() =>
        {
            Destroy(slider.gameObject);
            Destroy(speedUpButton.gameObject);
            delayComponent.OnDelayPassed.RemoveAllListeners();
        });

        return delayComponent;
    }

    private IEnumerator WaitSeconds(float seconds)
    {
        float clampedSeconds;
        if (clampSpeed && !instantMode)
        {
            clampedSeconds = seconds <= clampValue
                ? clampValue
                : seconds;
        }
        else
        {
            clampedSeconds = seconds;
        }

        yield return new WaitForSeconds(clampedSeconds);
    }

    private void Play(List<Branch> candidates)
    {
        if (candidates.Count == 0)
        {
            Debug.LogError("No candidates found.");
            GameObject eg = _spawner.SpawnEndGame(false);
        }
        else if (candidates.Count == 1)
        {
            _player.Play(candidates[0]);
            _saveSystem.LogGlobalVars();
        }
        else
        {
            _spawner.SpawnChoice(candidates);

            // _statisticsManager.TimeFlagStart("choice")
        }
    }

    public IEnumerator OnChoiceChosen(Branch branch)
    {
        yield return new WaitForSeconds(1);

        _player.Play(branch);
        _saveSystem.LogGlobalVars();
    }

    public void RewindToState(string stateId)
    {
        Debug.Log($"Rewinding to state {stateId}");

        PlayerStandBy = false; // если поменять на true - баг (после ревайнда не спавнится выбор, только после перезагрузки игры)
        GameEnded = false;
        AllowRewinding = false;

        _saveSystem.CleanLog(stateId);
        _spawner.ClearScreen();

        // отрисовать лог (пересмотреть и починить в соответствии с Single Responsibility principle)
        _saveSystem.SpawnLog();

        // инициализировать глобальные переменные
        _saveSystem.InitializeGlobalVariables(stateId);

        // set startOn
        var startOn = ArticyDatabase.GetObject(stateId);
        _player.StartOn = startOn;
    }
}
