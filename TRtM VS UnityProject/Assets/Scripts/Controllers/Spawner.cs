using Articy.Test;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    private GameController _controller;
    private PrefabManager _prefabManager;
    private ArticyFlowPlayer _player;
    private SaveSystem _saveSystem;

    private void Awake()
    {
        _prefabManager = GetComponent<PrefabManager>();
        _player = GetComponent<ArticyFlowPlayer>();
        _saveSystem = GetComponent<SaveSystem>();
        _controller = GetComponent<GameController>();
    }

    public GameObject SpawnPhrase(string text)
    {
        GameObject p = Instantiate(_prefabManager.phrasePrefab, _prefabManager.content);
        p.GetComponentInChildren<Text>().text = text;
        return p;
    }

    public GameObject SpawnChoice(List<Branch> candidates)
    {
        GameObject bg = SpawnButtonGroup();
        foreach (Branch p in candidates)
        {
            IObjectWithMenuText target = p.Target as IObjectWithMenuText;
            // Передаем в том числе всех кандидатов, чтобы после выбора они все залогались
            SpawnButton(bg.transform, target.MenuText, p, candidates);
        }
        return bg;
    }

    public GameObject SpawnButtonGroup()
    {
        GameObject bg = Instantiate(_prefabManager.buttonGroup, _prefabManager.content);
        bg.GetComponent<ArticyReference>().reference = (ArticyRef)_controller.Current;
        return bg;
    }

    public Button SpawnButtonFromLog(Transform buttonGroup, string text, bool clicked)
    {
        Button b = Instantiate(_prefabManager.buttonPrefab, buttonGroup);
        b.GetComponentInChildren<Text>().text = text;

        if (clicked)
        {
            var colors = b.colors;
            colors.highlightedColor = Color.green;
            colors.normalColor = Color.green;
            colors.pressedColor = Color.green;
            colors.selectedColor = Color.green;
            b.colors = colors;
        }

        b.onClick.AddListener(() =>
        {
            // rewind to state
        });
        return b;
    }

    public Button SpawnButton(Transform buttonGroup, string text, Branch exit, List<Branch> candidates)
    {
        Button b = Instantiate(_prefabManager.buttonPrefab, buttonGroup);
        b.GetComponentInChildren<Text>().text = text;
        b.onClick.AddListener(() =>
        {
            _saveSystem.LogChoice(exit, candidates);

            // TODO: refactor
            var colors = b.colors;
            colors.highlightedColor = Color.green;
            colors.normalColor = Color.green;
            colors.pressedColor = Color.green;
            colors.selectedColor = Color.green;
            b.colors = colors;

            var siblingButtons = buttonGroup.gameObject.GetComponentsInChildren<Button>().ToList();
            foreach (Button sibling in siblingButtons)
            {
                sibling.onClick.RemoveAllListeners();
                sibling.onClick.AddListener(() =>
                {
                    // rewind to state
                });
            }

            StartCoroutine(_controller.PlayAndWaitConstantTimeOnClick(exit));
        });
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

    public GameObject SpawnDelayBlock()
    {
        GameObject delayBlock = Instantiate(_prefabManager.delayBlockPrefab, _prefabManager.content);
        return delayBlock;
    }
}
