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
using System.Threading.Tasks;
using System;
using UnityEngine.UI;

public enum TextSpeed { SLOWEST = 5, SLOW = 4, NORMAL = 3, FAST = 2, FASTEST = 1 }

public class GameController : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    private Spawner _spawner;

    private void Awake()
    {
        _spawner = FindObjectOfType<Spawner>();
    }
    private PhraseDialogueFragment _current;
    private float _currentSpeed;

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

        StartCoroutine(Continue(candidates, _current.Text.Length * _currentSpeed));
    }

    IEnumerator Continue(List<Branch> candidates, float time)
    {
        yield return new WaitForSeconds(time);
        float delay = _current.Template.PhraseFeature.delay;
        if (delay > 0)
            StartCoroutine(HandleDelayedPlay(candidates, delay));
        else
            HandlePlay(candidates);
    }

    void HandlePlay(List<Branch> candidates)
    {
        if (candidates.Count == 0)
            print("Error");
        else if (candidates.Count == 1)
        {
            var target = candidates[0].Target as PhraseDialogueFragment;
            print(target.TechnicalName);
            if (target.MenuText.Length > 0)
                _spawner.SpawnChoice(candidates);
            else
                GetComponent<ArticyFlowPlayer>().Play(candidates[0]);
        } else
            _spawner.SpawnChoice(candidates);
    }

    IEnumerator HandleDelayedPlay(List<Branch> candidates, float delay)
    {
        Slider slider = _spawner.SpawnSlider(DateTime.Now, delay);
        print(slider.value);
        while (slider.value < 1)
        {
            print("debug");
            slider.value += Time.deltaTime / delay;
            yield return null;
        }
        Destroy(slider.gameObject);
        HandlePlay(candidates);
    }
}
