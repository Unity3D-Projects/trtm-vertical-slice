using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Articy.Unity;
using Articy.Unity.Constraints;
using Articy.Unity.Utils;
using Articy.Unity.Interfaces;

using Articy.Test;
using Articy.Test.Templates;
using Articy.Test.Features;
using Articy.Test.GlobalVariables;

using System;
using UnityEngine.UI;
using System.Xml.Linq;

public enum TextSpeed { SLOWEST = 5, SLOW = 4, NORMAL = 3, FAST = 2, FASTEST = 1 }

public class GameController : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    #region Dependencies
    private Spawner _spawner;
    private ArticyFlowPlayer _player;
    private SaveSystem _saveSystem;
    #endregion

    #region Settings
    public TextSpeed textSpeed;
    public bool clampSpeedToOne = true;
    [Space]

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

    public PhraseDialogueFragment Current { get; private set; }
    private float _currentSpeed;

    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
        _player = GetComponent<ArticyFlowPlayer>();
        _saveSystem = GetComponent<SaveSystem>();
    }
    
    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        if (PlayerStandBy)
        {
            return;
        }
        _currentSpeed = (float)textSpeed * 0.02f;
        
        if (aObject is PhraseDialogueFragment df)
        {
            Current = aObject as PhraseDialogueFragment;
            _spawner.SpawnPhrase(df.Text);
        }
    }
    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        if (PlayerStandBy)
        {
            return;
        }

        if (aBranches == null)
        {
            Debug.LogError("No branches found");
            GameObject eg = _spawner.SpawnEndGame(false);
        }

        List<Branch> candidates = new List<Branch>();
        foreach (Branch branch in aBranches)
        {
            // if branch is not DF (empty) - it's a pin - end of flow
            if (!(branch.Target is PhraseDialogueFragment))
            {
                _spawner.SpawnEndGame(true);
                _saveSystem.LogEvent(Const.LogEvent.LogEndGameWin, string.Empty);
                GameEnded = true;
                return;
            }
            if (branch.IsValid)
            {
                candidates.Add(branch);
            }
        }

        // checking for saving - refactor
        if (candidates.Count == 1)
        {
            _saveSystem.LogEvent(Const.LogEvent.LogPhrase, Current.Text);
            _saveSystem.UpdateExecuteElement(((PhraseDialogueFragment)candidates[0].Target).TechnicalName);
        }
        else if (candidates.Count > 1)
        {
            _saveSystem.LogState();
        }

        StartCoroutine(WaitTimeAndCheckForDelay(candidates, Current.Text.Length * _currentSpeed)); // TODO: clamp to min 1 sec
    }

    private IEnumerator WaitTimeAndCheckForDelay(List<Branch> candidates, float time)
    {
        float seconds;
        if (clampSpeedToOne) seconds = time <= 1 ? 1 : time;
        else seconds = time;
        yield return new WaitForSeconds(seconds);

        float delay = Current.Template.PhraseFeature.delay;
        if (delay > 0)
        {
            // обязательно ли оборачивать? нет, потому что Play() еще не вызван и можно ждать. 
            StartCoroutine(PlayWithDelay(candidates, delay));
        }
        else
        {
            Play(candidates);
        }
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
        }
    }

    public IEnumerator PlayAndWaitConstantTimeOnClick(Branch branch)
    {
        yield return new WaitForSeconds(1);
        
        _player.Play(branch);
        _saveSystem.LogGlobalVars();
    }

    private IEnumerator PlayWithDelay(List<Branch> candidates, float m_timeToWait)
    {
        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now.AddMinutes(m_timeToWait);
        _saveSystem.SetStartTimeAndExecuteTime(startTime, endTime);

        GameObject delayBlock = _spawner.SpawnDelayBlock();
        Slider slider = _spawner.SpawnSlider(DateTime.Now, m_timeToWait);

        float m_Remaining = m_timeToWait;
        while (m_Remaining > 0)
        {
            m_Remaining -= Time.deltaTime / 60;
            Debug.Log(m_Remaining);
            delayBlock.GetComponentInChildren<Text>().text = TimeSpan.FromMinutes(m_Remaining).ToString(@"hh\:mm\:ss");
            slider.value += Time.deltaTime / (m_timeToWait * 60);
            yield return null;
        }
        Destroy(slider.gameObject);
        Destroy(delayBlock.gameObject);
        Play(candidates);
    }
    
    public IEnumerator ExecuteWithDelay(DateTime startTime, float m_timeToWait)
    {
        DateTime endTime = DateTime.Now.AddMinutes(m_timeToWait);
        var totalDelay = (endTime - startTime).TotalMinutes;

        GameObject delayBlock = _spawner.SpawnDelayBlock();
        Slider slider = _spawner.SpawnSlider(startTime, totalDelay);

        float m_Remaining = m_timeToWait;
        while (m_Remaining > 0)
        {
            m_Remaining -= Time.deltaTime / 60;
            delayBlock.GetComponentInChildren<Text>().text = TimeSpan.FromMinutes(m_Remaining).ToString(@"hh\:mm\:ss");
            slider.value += (float)(Time.deltaTime / (totalDelay * 60));
            yield return null;
        }
        Destroy(delayBlock.gameObject); // будет ли работать без gameObject?
        Destroy(slider.gameObject);
    }

    public void RewindToState(string stateId)
    {
        Debug.LogWarning($"Rewinding to state {stateId}");

        PlayerStandBy = false; // если поменять на true - баг (после ревайнда не спавнится выбор, только после перезагрузки игры)
        GameEnded = false;
        AllowRewinding = false;

        _saveSystem.CleanLog(stateId);
        _spawner.ClearScreen();

        // отрисовать лог (пересмотреть и починить в соответствии с Single Responsibility principle)
        XDocument xDoc = _saveSystem.ProvideXDoc();
        _saveSystem.SpawnLog(xDoc);

        // инициализировать глобальные переменные
        _saveSystem.InitializeGlobalVariables(stateId);

        // set startOn
        var startOn = ArticyDatabase.GetObject(stateId);
        _player.StartOn = startOn;
    }
}
