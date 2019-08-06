using Articy.Unity;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    private PrefabManager _prefabManager;
    private ArticyFlowPlayer _flowPlayer;
    private SaveSystem _saveSystem;

    private void Awake()
    {
        _prefabManager = GetComponent<PrefabManager>();
        _flowPlayer = GetComponent<ArticyFlowPlayer>();
        _saveSystem = GetComponent<SaveSystem>();
    }

    public GameObject SpawnPhrase(string text)
    {
        GameObject p = Instantiate(_prefabManager.phrasePrefab, _prefabManager.content);
        p.GetComponentInChildren<Text>().text = text;
        _saveSystem.LogEvent(Const.EventType.PhraseEvent, text);
        return p;
    }

    public GameObject SpawnChoice(List<Branch> candidates)
    {
        GameObject bg = SpawnButtonGroup();
        foreach (Branch p in candidates)
        {
            IObjectWithMenuText target = p.Target as IObjectWithMenuText;
            SpawnButton(bg.transform, target.MenuText, p);
            _saveSystem.LogEvent(Const.EventType.ButtonEvent, target.MenuText);
        }
        return bg;
    }

    public GameObject SpawnButtonGroup()
    {
        GameObject bg = Instantiate(_prefabManager.buttonGroup, _prefabManager.content);
        return bg;
    }

    public Button SpawnButton(Transform buttonGroup, string text, Branch exit)
    {
        Button b = Instantiate(_prefabManager.buttonPrefab, buttonGroup);
        b.GetComponentInChildren<Text>().text = text;
        b.onClick.AddListener(() => _flowPlayer.Play(exit));
        return b;
    }

    public Slider SpawnSlider(DateTime start, double delay)
    {
        double timePassed = (DateTime.Now - start).TotalMinutes;
        Slider s = Instantiate(_prefabManager.sliderPrefab, _prefabManager.content);
        s.value = (float)(timePassed / delay);
        // log slider or something
        return s;
    }
}
