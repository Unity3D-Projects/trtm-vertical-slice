using Articy.The_Road_To_Moscow;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Assets.Scripts.Controllers;
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
    private AdvertisementManager _advertisementManager;

    private void Awake()
    {
        _prefabManager = GetComponent<PrefabManager>();
        _player = GetComponent<ArticyFlowPlayer>();
        _saveSystem = GetComponent<SaveSystem>();
        _controller = GetComponent<GameController>();
        _advertisementManager = GetComponent<AdvertisementManager>();
    }

    public GameObject ShowSkipPopup()
    {
        var popup = Instantiate(_prefabManager.skipPopupPrefab, _prefabManager.canvas);

        var buttons = popup.GetComponentsInChildren<Button>();

        var yesButton = buttons.Where(x => x.GetComponentInChildren<Text>().text == "Да").FirstOrDefault();
        var noButton = buttons.Where(x => x.GetComponentInChildren<Text>().text == "Нет").FirstOrDefault();

        yesButton.onClick.AddListener(() => _advertisementManager.ShowAd("rewardedVideo"));
        noButton.onClick.AddListener(() => Destroy(popup.gameObject));

        return popup;
    }

    private void SkipPopupCallback(GameObject popup)
    {
        _controller.SkipDelay(1);
        Destroy(popup.gameObject);
    }

    public GameObject SpawnPhrase(string text, Color color, Text_Position side)
    {
        GameObject prefab;
        if (side == Text_Position.Middle)
        {
            prefab = Instantiate(_prefabManager.middlePrefab, _prefabManager.content);
        }
        else
        {
            prefab = Instantiate(_prefabManager.sidesPrefab, _prefabManager.content);
        }

        prefab.name = text.Length >= 20 ? Trim(text, 20) : text;
        prefab.GetComponentInChildren<Text>().text = text;
        prefab.GetComponentInChildren<Text>().color = color;

        switch (side)
        {
            case Text_Position.Left:
                foreach (var lg in prefab.GetComponentsInChildren<LayoutGroup>())
                {
                    lg.childAlignment = TextAnchor.MiddleLeft;
                }
                //prefab.GetComponentsInChildren<LayoutGroup>()[2].padding.left = 30;
                //prefab.GetComponentsInChildren<LayoutGroup>()[2].padding.right = 20;
                prefab.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
                break;

            case Text_Position.Right:
                foreach (var lg in prefab.GetComponentsInChildren<LayoutGroup>())
                {
                    lg.childAlignment = TextAnchor.MiddleRight;
                }
                //prefab.GetComponentsInChildren<LayoutGroup>()[2].padding.left = 20;
                //prefab.GetComponentsInChildren<LayoutGroup>()[2].padding.right = 30;
                prefab.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
                break;
        }

        return prefab;
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
        }
        else
        {
            var colors = b.GetComponent<ButtonColors>().notPressedColors;
            b.colors = colors;
        }

        b.onClick.AddListener(() =>
        {
            if (_controller.AllowRewinding)
            {
                var reference = buttonGroup.GetComponent<ArticyReference>().reference;
                var id = ((DialogueFragment)reference).TechnicalName;
                _controller.RewindToState(id);
            }
        });
        return b;
    }
    // в иерархии писать "Choice (2), если выбора 2, и т.д.
    public Button SpawnButton(GameObject buttonGroup, string text, Branch exit, List<Branch> candidates)
    {
        Button b = Instantiate(_prefabManager.buttonPrefab, buttonGroup.transform);
        b.name = text;
        b.GetComponentInChildren<Text>().text = text;
        b.onClick.AddListener(() => ButtonOnClickEventHandler(b, buttonGroup, exit, candidates));
        return b;
    }

    public Slider SpawnSlider(DateTime start, double delay)
    {
        double timePassed = (DateTime.Now - start).TotalMinutes;
        Slider s = Instantiate(_prefabManager.sliderPrefab, _prefabManager.content);
        s.interactable = false;
        s.value = (float)(timePassed / delay);
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
        if (win) endGame.GetComponent<Image>().color = endGame.GetComponent<EndGameColors>().youWinColor;
        else endGame.GetComponent<Image>().color = endGame.GetComponent<EndGameColors>().youLoseColor;
        return endGame;
    }

    public void ClearScreen()
    {
        foreach (Transform child in _prefabManager.content)
        {
            Destroy(child.gameObject);
        }
    }

    private void ButtonOnClickEventHandler(Button button, GameObject buttonGroup, Branch exit, List<Branch> candidates)
    {
        _saveSystem.LogChoice(exit, candidates);

        var siblingButtons = buttonGroup.gameObject.GetComponentsInChildren<Button>().ToList();
        foreach (Button sibling in siblingButtons)
        {
            var sColors = button.GetComponent<ButtonColors>().notPressedColors;
            sibling.colors = sColors;
            sibling.onClick.RemoveAllListeners();
            sibling.onClick.AddListener(() => ButtonOnSecondClickEventHandler(buttonGroup));
        }
        var bColors = button.GetComponent<ButtonColors>().pressedColors;
        button.colors = bColors;

        StartCoroutine(_controller.OnChoiceChosen(exit));
    }

    private void ButtonOnSecondClickEventHandler(GameObject buttonGroup)
    {
        if (_controller.AllowRewinding)
        {
            var reference = buttonGroup.GetComponent<ArticyReference>().reference;
            var id = ((DialogueFragment)reference).TechnicalName;
            _controller.RewindToState(id);
        }
    }
}
