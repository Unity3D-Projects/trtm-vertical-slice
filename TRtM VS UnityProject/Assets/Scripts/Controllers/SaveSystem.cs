using Articy.The_Road_To_Moscow.GlobalVariables;
using Articy.Unity.Utils;
using Articy.Unity;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using System.Collections.Generic;
using Articy.Unity.Interfaces;
using System.Collections;
using Articy.The_Road_To_Moscow;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    private GameController _controller;
    private ArticyFlowPlayer _player;
    private Spawner _spawner;

    private string _savePath = Const.SavePath;

    private void Awake()
    {
        _controller = GetComponent<GameController>();
        _player = GetComponent<ArticyFlowPlayer>();
        _spawner = GetComponent<Spawner>();
    }

    private void Start()
    {
        _controller.PlayerStandBy = true;
        _controller.GameEnded = false;
        _controller.AllowRewinding = false;
        _controller.CanWatchAd = false;

        if (!SaveFileExists())
        {
            CreateNewSaveFile();
            _controller.PlayerStandBy = false;
        }
        else
        {
            Debug.Log($"Loading saved game");
            LoadGame();
        }
    }



    public bool SaveFileExists()
    {
        return File.Exists(_savePath);
    }

    public XDocument ProvideXDoc()
    {
        XDocument xDoc = XDocument.Load(_savePath);
        return xDoc;
    }

    public void LoadGame()
    {
        SpawnLog();

        InitializeGlobalVariables();

        XDocument xDoc = XDocument.Load(_savePath);

        var anyEndGame = xDoc.Element("save").Element("log").Elements("endGame").Any();
        if (anyEndGame)
        {
            Debug.Log("Game already ended.");
            _controller.GameEnded = true;
            return;
        }

        // set startOn
        var xExecute = xDoc.Element("save").Element("execute");
        var executeId = xExecute.Attribute(Const.XmlAliases.ExecuteId).Value;
        _player.StartOn = ArticyDatabase.GetObject(executeId);

        // проверить дилей
        var result = CheckForDelay(xDoc);

        _controller.CanWatchAd = result.canWatchAd;

        if (result.remainingInSeconds > 0)
        {
            var delayComponent = _controller.SetUpDelay(result.remainingInSeconds);
            delayComponent.OnDelayPassed.AddListener(() => { _controller.PlayerStandBy = false; });

            delayComponent.Run();
        }
        else
        {
            _controller.PlayerStandBy = false;
        }
    }

    public void SpawnLog()
    {
        XDocument xDoc = XDocument.Load(_savePath);

        var log = xDoc.Element("save").Element("log");
        foreach (XElement logEvent in log.Elements())
        {
            switch (logEvent.Name.LocalName)
            {
                case (Const.XmlAliases.Phrase):
                    var phrase = ArticyDatabase.GetObject(logEvent.Value) as DialogueFragment;
                    var speaker = phrase.Speaker as EntityTemplate;
                    _spawner.SpawnPhrase(phrase.Text, speaker.Color, speaker.Template.EntityFeature.Text_Position);
                    break;

                case (Const.XmlAliases.ButtonGroup):
                    GameObject bg = _spawner.SpawnButtonGroup();
                    bg.GetComponent<ArticyReference>().reference = (ArticyRef)ArticyDatabase.GetObject(logEvent.Attribute("id").Value);
                    foreach (XElement xButton in logEvent.Elements())
                    {
                        var blockWithMenuText = ArticyDatabase.GetObject(xButton.Value) as DialogueFragment;
                        Button b = _spawner.SpawnButtonFromLog(
                            bg,
                            blockWithMenuText.MenuText,
                            bool.Parse(xButton.Attribute(Const.XmlAliases.ButtonPressedAttributte).Value));
                    }
                    break;
                case (Const.XmlAliases.EndGame):
                    if (bool.Parse(logEvent.Attribute(Const.XmlAliases.EndGameWinAttributte).Value))
                    {
                        _spawner.SpawnEndGame(true);
                    }
                    else
                    {
                        _spawner.SpawnEndGame(false);
                    }
                    break;
            }
        }
    }

    public void InitializeGlobalVariables(string id = null)
    {
        XDocument xDoc = XDocument.Load(_savePath);

        var loadedVars = id == null
           ? CopyVars(xDoc.Element("save").Element("vars"))
           : CopyVars(xDoc.Element("save").Element("states").Elements().Where(e => e.Attribute("id").Value == id).FirstOrDefault());

        ArticyDatabase.DefaultGlobalVariables.Variables = loadedVars;
    }

    public void SaveAdWatchedTime()
    {
        XDocument xDoc = XDocument.Load(_savePath);

        xDoc.Element("save").Element("logic").Element("lastWatchAdTime").Value = DateTime.Now.ToString();

        xDoc.Save(_savePath);
    }

    private (DateTime startTime, float remainingInSeconds, bool canWatchAd) CheckForDelay(XDocument xDoc)
    {
        var xExecute = xDoc.Element("save").Element("execute");

        var executeTime = DateTime.Parse(xExecute.Attribute(Const.XmlAliases.ExecuteTime)?.Value ?? DateTime.Now.ToString());
        var startTime = DateTime.Parse(xExecute.Attribute(Const.XmlAliases.StartTime)?.Value ?? DateTime.Now.ToString());
        var lastWatchAdTime = DateTime.Parse(xDoc.Element("save").Element("logic").Element("lastWatchAdTime").Value);

        var canWatchAd = DateTime.Now.Subtract(lastWatchAdTime) >= TimeSpan.FromMinutes(Const.WatchAdFrequencyInMinutes);

        if (executeTime != null && DateTime.Now < executeTime)
        {
            var remainingInSeconds = (float)(executeTime - DateTime.Now).TotalSeconds;
            return (startTime, remainingInSeconds, canWatchAd);
        }

        return (startTime, 0, canWatchAd);
    }

    private Dictionary<string, object> CopyVars(XElement elementToCopyFrom)
    {
        Dictionary<string, object> loadedVars = new Dictionary<string, object>();
        foreach (XElement xVar in elementToCopyFrom.Elements())
        {
            var varName = xVar.Attribute("name").Value;
            var varValue = xVar.Value;
            loadedVars.Add(varName, varValue);
        }
        return loadedVars;
    }

    public void LogEvent(Const.LogEvent type, string content)
    {
        XDocument xDoc = XDocument.Load(_savePath);
        var xLog = xDoc.Element("save").Element("log");
        switch (type)
        {
            case Const.LogEvent.LogPhrase:
                xLog.Add(new XElement(Const.XmlAliases.Phrase, content));
                break;
            case Const.LogEvent.LogButtonGroup:
                xLog.Add(new XElement(
                        Const.XmlAliases.ButtonGroup,
                            new XAttribute("id", _controller.Current.TechnicalName),
                        content));
                break;

            case Const.LogEvent.LogButton:
                xLog.Elements(Const.XmlAliases.ButtonGroup).Last()
                    .Add(new XElement(
                        Const.XmlAliases.Button,
                            new XAttribute(Const.XmlAliases.ButtonPressedAttributte, false),
                        content));
                break;

            case Const.LogEvent.LogButtonPressed:
                xLog.Elements(Const.XmlAliases.ButtonGroup).Last()
                    .Add(new XElement(
                        Const.XmlAliases.Button,
                            new XAttribute(Const.XmlAliases.ButtonPressedAttributte, true),
                        content));
                break;
            case Const.LogEvent.LogEndGameWin:
                xLog.Add(new XElement(Const.XmlAliases.EndGame,
                            new XAttribute(Const.XmlAliases.EndGameWinAttributte, true),
                            content));
                break;

            case Const.LogEvent.LogEndGameLose:
                xLog.Add(new XElement(Const.XmlAliases.EndGame,
                            new XAttribute(Const.XmlAliases.EndGameWinAttributte, false),
                            content));
                break;
        }
        xDoc.Save(_savePath);
    }

    public void LogChoice(Branch exit, List<Branch> candidates)
    {
        LogEvent(Const.LogEvent.LogPhrase, _controller.Current.TechnicalName);
        LogEvent(Const.LogEvent.LogButtonGroup, string.Empty);
        foreach (Branch branch in candidates)
        {
            var branchTarget = branch.Target as DialogueFragment;
            var exitTarget = exit.Target as DialogueFragment;
            if (branchTarget.TechnicalName == (exitTarget.TechnicalName))
            {
                LogEvent(Const.LogEvent.LogButtonPressed, exitTarget.TechnicalName);
                continue;
            }
            LogEvent(Const.LogEvent.LogButton, branchTarget.TechnicalName);
        }
    }

    public void LogState()
    {
        XDocument xDoc = XDocument.Load(_savePath);
        var currentBlockName = _controller.Current.TechnicalName;
        var states = xDoc.Element("save").Element("states");
        if (!HasState(states, currentBlockName, out XElement stateWithThisName))
        {
            CreateState(states, currentBlockName);
            xDoc.Save(_savePath);
            Debug.Log($"State {currentBlockName} was saved");
        }
        else
        {
            Debug.Log("Overwriting the state...");
            OverwriteState(stateWithThisName);
        }
    }

    public void LoadState(string id)
    {
        XDocument xDoc = XDocument.Load(_savePath);
        var df = ArticyDatabase.GetObject(id) as DFTemplate;
    }

    public void SetStartTimeAndExecuteTime(DateTime startTime, DateTime executeTime)
    {
        XDocument xDoc = XDocument.Load(_savePath);
        XElement xExecute = xDoc.Element("save").Element("execute");

        XAttribute xStartTime = xExecute.Attribute(Const.XmlAliases.StartTime);
        XAttribute xExecuteTime = xExecute.Attribute(Const.XmlAliases.ExecuteTime);

        if (xExecuteTime == null)
            xExecute.Add(new XAttribute(Const.XmlAliases.ExecuteTime, executeTime.ToString()));
        else
            xExecuteTime.Value = executeTime.ToString();

        if (xStartTime == null)
            xExecute.Add(new XAttribute(Const.XmlAliases.StartTime, startTime.ToString()));
        else
            xStartTime.Value = startTime.ToString();

        xDoc.Save(_savePath);
    }

    private bool HasState(XElement statesElement, string stateName, out XElement stateWithThisName)
    {
        stateWithThisName = statesElement
            .Elements()
            .Where(e => e.FirstAttribute.Value == stateName)
            .FirstOrDefault();

        if (stateWithThisName != null)
        {
            Debug.Log($"{GetType().Name}: Document already has a state called \"{stateName}\".");
            return true;
        }
        else return false;
    }

    private void CreateState(XElement states, string id)
    {
        var state = GetGlobalVars("state");
        state.Add(new XAttribute("id", id));
        states.Add(state);
    }
    private void OverwriteState(XElement state)
    {
        state.Descendants().Remove();
        state.Add(GetGlobalVars("state"));
    }

    // TODO менять значение той переменной, которая поменялась
    public void LogGlobalVars()
    {
        XDocument xDoc = XDocument.Load(_savePath);
        xDoc.Element("save").Element("vars").Remove();
        xDoc.Element("save").Add(GetGlobalVars("vars"));
        xDoc.Save(_savePath);
    }

    public void UpdateExecuteElement(string technicalName)
    {
        XDocument xDoc = XDocument.Load(_savePath);
        xDoc.Element("save").Element("execute").Attribute(Const.XmlAliases.ExecuteId).Value = technicalName;
        xDoc.Save(_savePath);
    }
    public void SubtractSecondssFromExecute(float time)
    {
        XDocument xDoc = XDocument.Load(_savePath);
        var xExecute = xDoc.Element("save").Element("execute");

        var xExecuteTime = xExecute.Attribute(Const.XmlAliases.ExecuteTime);
        DateTime eBefore = DateTime.Parse(xExecuteTime.Value);
        DateTime eAfter = eBefore.AddSeconds(-time);
        xExecuteTime.Value = eAfter.ToString();

        var xStartTime = xExecute.Attribute(Const.XmlAliases.StartTime);
        DateTime sBefore = DateTime.Parse(xStartTime.Value);
        DateTime sAfter = sBefore.AddSeconds(-time);
        xStartTime.Value = sAfter.ToString();

        xDoc.Save(_savePath);
    }

    private void CreateNewSaveFile()
    {
        XDocument xDoc = new XDocument(
                    new XElement("save",
                        new XElement("log"),
                        new XElement("execute", new XAttribute(Const.XmlAliases.ExecuteId, _player.StartOn.TechnicalName)),
                        new XElement("states"),
                        new XElement("logic",
                            new XElement("lastWatchAdTime", DateTime.MinValue.ToString()))));
        xDoc.Element("save").Add(GetGlobalVars("vars"));

        xDoc.Save(_savePath);
    }

    public void DeleteSaveFile()
    {
        File.Delete(_savePath);
    }

    private XElement GetGlobalVars(string elementName)
    {
        XElement varsContainer = new XElement(elementName);
        foreach (var v in _player.globalVariables.Variables)
        {
            varsContainer.Add(new XElement("var", new XAttribute("name", v.Key), v.Value));
        }
        return varsContainer;
    }

    public void CleanLog(string idToCleanUntil)
    {
        XDocument xDoc = XDocument.Load(_savePath);

        var xExecute = xDoc.Element("save").Element("execute");
        xExecute.Attribute(Const.XmlAliases.ExecuteId).Value = idToCleanUntil;
        if (xExecute.Attribute(Const.XmlAliases.ExecuteTime) != null)
        {
            xExecute.Attribute(Const.XmlAliases.ExecuteTime).Remove();
        }

        var logElements = xDoc.Element("save").Element("log").Elements();
        for (int i = logElements.Count() - 1; i > 0; i--)
        {
            var current = logElements.LastOrDefault();
            if (current.Name == Const.XmlAliases.ButtonGroup &&
                current.Attribute("id") != null &&
                current.Attribute("id").Value == idToCleanUntil)
            {
                current.PreviousNode.Remove();
                current.Remove();
                break;
            }
            current.Remove();
        }
        xDoc.Save(_savePath);
    }
}
