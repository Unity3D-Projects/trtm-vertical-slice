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
        p.name = text.Length >= 20 ? Trim(text, 20) : text;
        p.GetComponentInChildren<Text>().text = text;
        return p;
    }

    private string Trim(string text, int index)
    {
        string newText = text.Substring(0, index);
        return newText;
    }

    public GameObject SpawnChoice(List<Branch> candidates)
    {
        GameObject bg = SpawnButtonGroup();
        foreach (Branch p in candidates)
        {
            IObjectWithMenuText target = p.Target as IObjectWithMenuText;
            // Передаем в том числе всех кандидатов, чтобы после выбора они все залогались
            SpawnButton(bg, target.MenuText, p, candidates);
        }
        return bg;
    }

    public GameObject SpawnButtonGroup()
    {
        GameObject bg = Instantiate(_prefabManager.buttonGroup, _prefabManager.content);
        bg.GetComponent<ArticyReference>().reference = (ArticyRef)_controller.Current;
        return bg;
    }

    // в иерархии писать "Choice (2), если выбора 2, и т.д.
    public Button SpawnButtonFromLog(GameObject buttonGroup, string text, bool clicked)
    {
        Button b = Instantiate(_prefabManager.buttonPrefab, buttonGroup.transform);
        b.name = text;
        b.GetComponentInChildren<Text>().text = text;

        if (clicked)
        {
            var colors = b.GetComponent<ButtonColors>().pressedColors;
            b.colors = colors;
        } else
        {
            var colors = b.GetComponent<ButtonColors>().notPressedColors;
            b.colors = colors;
        }

        b.onClick.AddListener(() =>
        {
            if (_controller.AllowRewinding)
            {
                var reference = buttonGroup.GetComponent<ArticyReference>().reference;
                var id = ((PhraseDialogueFragment)reference).TechnicalName;
                _controller.RewindToState(id);
            }
        });
        return b;
    }
    // в иерархии писать "Choice (2), если выбора 2, и т.д.
    // попробовать не лямбды а отдельные хендлеры для кнопок (и ревайнд)
    public Button SpawnButton(GameObject buttonGroup, string text, Branch exit, List<Branch> candidates)
    {
        Button b = Instantiate(_prefabManager.buttonPrefab, buttonGroup.transform);
        b.name = text;
        b.GetComponentInChildren<Text>().text = text;
        b.onClick.AddListener(() =>
        {
            _saveSystem.LogChoice(exit, candidates);

            var siblingButtons = buttonGroup.gameObject.GetComponentsInChildren<Button>().ToList();
            foreach (Button sibling in siblingButtons)
            {
                var sColors = b.GetComponent<ButtonColors>().notPressedColors;
                sibling.colors = sColors;
                sibling.onClick.RemoveAllListeners();
                sibling.onClick.AddListener(() =>
                {
                    if (_controller.AllowRewinding)
                    {
                        var reference = buttonGroup.GetComponent<ArticyReference>().reference;
                        var id = ((PhraseDialogueFragment)reference).TechnicalName;
                        _controller.RewindToState(id);
                    }
                });
            }
            var bColors = b.GetComponent<ButtonColors>().pressedColors;
            b.colors = bColors;

            StartCoroutine(_controller.PlayAndWaitConstantTimeOnClick(exit));
        });
        return b;
    }

    public Slider SpawnSlider(DateTime start, double delay)
    {
        double timePassed = (DateTime.Now - start).TotalMinutes;
        Slider s = Instantiate(_prefabManager.sliderPrefab, _prefabManager.content);
        s.interactable = false;
        s.value = (float)(timePassed / delay);
        // log slider or something
        return s;
    }

    public GameObject SpawnDelayBlock()
    {
        GameObject delayBlock = Instantiate(_prefabManager.delayBlockPrefab, _prefabManager.content);
        return delayBlock;
    }

    public GameObject SpawnEndGame(bool win)
    {
        GameObject endGame = Instantiate(_prefabManager.endgamePrefab, _prefabManager.content);
        if (win)
        {
            endGame.GetComponent<Image>().color = endGame.GetComponent<EndGameColors>().youWinColor;
        }
        else
        {
            endGame.GetComponent<Image>().color = endGame.GetComponent<EndGameColors>().youLoseColor;
        }
        return endGame;
    }

    public void ClearScreen()
    {
        foreach (Transform child in _prefabManager.content)
        {
            Destroy(child.gameObject);
        }
    }
}
