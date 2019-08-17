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
    private Spawner _spawner;
    private ArticyFlowPlayer _player;
    private SaveSystem _saveSystem;

    private bool _playerStandBy = true;
    public bool PlayerStandBy
    {
        get
        {
            return _playerStandBy;
        }
        set
        {
            _playerStandBy = value;
            Debug.LogWarning($"ArticyFlowPlayer is {!_playerStandBy}"); 
        }
    }

    private PhraseDialogueFragment _current;
    public PhraseDialogueFragment Current {
        get { return _current; }
    }

    private float _currentSpeed;

    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
        _player = GetComponent<ArticyFlowPlayer>();
        _saveSystem = GetComponent<SaveSystem>();
        _current = ScriptableObject.CreateInstance("PhraseDialogueFragment") as PhraseDialogueFragment;
    }

    public TextSpeed textSpeed;
    
    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        if (PlayerStandBy)
        {
            return;
        }

        _current = GetComponent<ArticyFlowPlayer>().PausedOn as PhraseDialogueFragment;
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

        List<Branch> candidates = new List<Branch>();
        foreach (Branch branch in aBranches)
        {
            if (branch.IsValid)
                candidates.Add(branch);
        }

        if (candidates.Count == 1)
        {
            _saveSystem.LogEvent(Const.LogEvent.LogPhrase, Current.Text);
            _saveSystem.UpdateExecuteElement(((PhraseDialogueFragment)candidates[0].Target).TechnicalName);
        }
        if (candidates.Count > 1)
            _saveSystem.LogState();

        StartCoroutine(WaitTimeAndCheckForDelay(candidates, _current.Text.Length * _currentSpeed)); // TODO: clamp to min 1 sec
    }

    private IEnumerator WaitTimeAndCheckForDelay(List<Branch> candidates, float time)
    {
        yield return new WaitForSeconds(time);

        float delay = _current.Template.PhraseFeature.delay;
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
            Debug.LogError("No candidates found.");
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

    // попробовать крутить не по фрагментам, а как-то еще
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
}
