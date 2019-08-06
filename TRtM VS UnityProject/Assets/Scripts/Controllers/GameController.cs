﻿using System.Collections;
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
using System.Threading.Tasks;
using System;
using UnityEngine.UI;

public enum TextSpeed { SLOWEST = 5, SLOW = 4, NORMAL = 3, FAST = 2, FASTEST = 1 }

public class GameController : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    // controls spawner, flow player, save system, 
    private Spawner _spawner;
    private ArticyFlowPlayer _player;

    private PhraseDialogueFragment _current;
    public PhraseDialogueFragment Current {
        get { return _current;
        }
    }

    private float _currentSpeed;

    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
        _player = GetComponent<ArticyFlowPlayer>();
    }

    public TextSpeed textSpeed;

    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        _current = GetComponent<ArticyFlowPlayer>().PausedOn as PhraseDialogueFragment;
        _currentSpeed = (int)textSpeed * 0.02f;

        if (aObject is PhraseDialogueFragment df)
        {
            _spawner.SpawnPhrase(df.Text);
        }
    }
    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        List<Branch> candidates = new List<Branch>();

        foreach (Branch branch in aBranches)
        {
            if (branch.IsValid)
                candidates.Add(branch);
        }

        if (candidates.Count > 1)
        {
            // save new state before choice
        }

        StartCoroutine(WaitTimeAndCheckForDelay(candidates, _current.Text.Length * _currentSpeed));
    }

    private IEnumerator WaitTimeAndCheckForDelay(List<Branch> candidates, float time)
    {
        yield return new WaitForSeconds(time);

        float delay = _current.Template.PhraseFeature.delay;
        if (delay > 0)
            StartCoroutine(PlayWithDelay(candidates, delay));
        else
            Play(candidates);
    }

    private void Play(List<Branch> candidates)
    {
        if (candidates.Count == 0)
            Debug.LogError("No candidates found.");
        else if (candidates.Count == 1)
        {
            var technicalName = ((PhraseDialogueFragment)candidates[0].Target).TechnicalName;
            // set <execute tn="technicalName" />

            _player.Play(candidates[0]);
        } else
            _spawner.SpawnChoice(candidates);
    }

    private IEnumerator PlayWithDelay(List<Branch> candidates, float delay)
    {
        Slider slider = _spawner.SpawnSlider(DateTime.Now, delay);
        while (slider.value < 1)
        {
            slider.value += Time.deltaTime / delay;
            yield return null;
        }
        Destroy(slider.gameObject);
        Play(candidates);
    }
}