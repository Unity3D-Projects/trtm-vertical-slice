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
            Debug.LogWarning($"Rewinding{(_allowRewinding ? " " : " dis")}allowed");
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
            Debug.LogWarning($"ArticyFlowPlayer is {(_playerStandBy ? "OFF" : "ON")}"); 
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
            Debug.LogWarning($"Game ended = {_gameEnded}");
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

        Current = GetComponent<ArticyFlowPlayer>().PausedOn as PhraseDialogueFragment;
        _currentSpeed = (float)textSpeed * 0.02f;

        if (aObject is PhraseDialogueFragment df)
        {
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
                candidates.Add(branch);
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
        yield return new WaitForSeconds(time);

        float delay = Current.Template.PhraseFeature.delay;
        if (delay > 0)
        {
            StartCoroutine(PlayWithDelay(candidates, delay));
        }
        else
            Play(candidates);
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
        } else
            _spawner.SpawnChoice(candidates);
    }

    public IEnumerator PlayAndWaitConstantTimeOnClick(Branch branch)
    {
        yield return new WaitForSeconds(1);
        _player.Play(branch);
    }

    private IEnumerator PlayWithDelay(List<Branch> candidates, float delay)
    {
        DateTime endTime = DateTime.Now.AddMinutes(delay);
        _saveSystem.SetExecuteTime(endTime);
        GameObject delayBlock = _spawner.SpawnDelayBlock();
        Slider slider = _spawner.SpawnSlider(DateTime.Now, delay);
        while (DateTime.Now <= endTime)
        {
            slider.value += Time.deltaTime / (delay * 60f);
            yield return null;
        }
        Destroy(slider.gameObject);
        Destroy(delayBlock.gameObject);
        Play(candidates);
    }
    
    public IEnumerator ExecuteWithDelay(float delay)
    {
        GameObject delayBlock = _spawner.SpawnDelayBlock();
        Slider slider = _spawner.SpawnSlider(DateTime.Now, delay);
        float remaining = delay * 60;
        while (remaining > 0)
        {
            remaining -= Time.deltaTime;
            slider.value += Time.deltaTime / (delay * 60);
            yield return null;
        }
        Destroy(delayBlock.gameObject); // будет ли работать без gameObject?
        Destroy(slider.gameObject);
    }

    public void RewindToState(string stateId)
    {
        Debug.Log($"Rewinding to state {stateId}");

        PlayerStandBy = false;
        GameEnded = false;
        AllowRewinding = false;

        _saveSystem.CleanLog(stateId);
        _spawner.ClearScreen();
        _saveSystem.LoadGame(stateId);
    }
}
