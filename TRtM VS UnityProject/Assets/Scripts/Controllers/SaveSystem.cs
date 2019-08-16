using Articy.Test.GlobalVariables;
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

public class SaveSystem : MonoBehaviour
{
    private GameController _controller;
    private ArticyFlowPlayer _player;

    private string _savePath = Const.SavePath;

    private void Awake()
    {
        _controller = GetComponent<GameController>();
        _player = GetComponent<ArticyFlowPlayer>();
    }

    private void Start()
    {
        _controller.PlayerStandBy = true;

        if (!SaveFileExists())
        {
            CreateNewSaveFile();
        }
        else
        {
            LoadGame();
        }
    }

    public bool SaveFileExists()
    {
        return File.Exists(_savePath);
    }

    public void LoadGame()
    {
        XDocument xDoc = XDocument.Load(_savePath);

        // отрисовать лог

        // инициализировать глобальные переменные
        InitializeGlobalVariables(xDoc);

        // проверить дилей
        var xExecute = xDoc.Element("save").Element("execute");
        var delay = CheckForDelay(xExecute);

        var executeId = xExecute.Attribute(Const.XmlAliases.ExecuteId).Value;
        var startOn = ArticyDatabase.GetObject(executeId);
        _player.StartOn = startOn;
        
        if (delay > 0)
        {
            var coroutine = new CoroutineObject<float>(_controller, _controller.ExecuteWithDelay);
            coroutine.Finished += () =>
            {
                _controller.PlayerStandBy = false;
                _player.StartOn = startOn;
            };
            coroutine.Start(delay);
        } else
        {
            _controller.PlayerStandBy = false;
        }
    }

    private void InitializeGlobalVariables(XDocument xDoc)
    {
        Dictionary<string, object> loadedVars = new Dictionary<string, object>();
        var xVars = xDoc.Element("save").Element("vars").Descendants();
        foreach (var xVar in xVars)
        {
            var varName = xVar.Attribute("name").Value;
            var varValue = xVar.Value;
            loadedVars.Add(varName, varValue);
        }
        ArticyDatabase.DefaultGlobalVariables.Variables = loadedVars;
    }

    private float CheckForDelay(XElement xExecute)
    {
        var executeTime = DateTime.Parse(xExecute.Attribute(Const.XmlAliases.ExecuteTime)?.Value ?? DateTime.Now.ToString());

        if (executeTime != null && DateTime.Now < executeTime)
        {
            return (float)(executeTime - DateTime.Now).TotalMinutes;
        }

        return 0;
    }

    // get type of event ???
    public void LogEvent(string eventType, string content)
    {
        XDocument xDoc = XDocument.Load(_savePath);
        xDoc.Element("save").Element("log").Add(new XElement(eventType, content));
        xDoc.Save(_savePath);
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
        } else
        {
            Debug.LogWarning("Overwriting the state...");
            OverwriteState(stateWithThisName);
        }
    }

    public void SetExecuteTime(DateTime executeTime)
    {
        XDocument xDoc = XDocument.Load(_savePath);
        XElement xExecute = xDoc.Element("save").Element("execute");
        XAttribute xExecuteTime = xExecute.Attribute(Const.XmlAliases.ExecuteTime);
        if (xExecuteTime != null)
        {
            xExecuteTime.Value = executeTime.ToString();
        } else
        {
            xExecute.Add(new XAttribute(Const.XmlAliases.ExecuteTime, executeTime.ToString()));
        }
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
            Debug.LogWarning($"{GetType().Name}: Document already has a state called \"{stateName}\".");
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
        var xmlVars = xDoc.Element("save").Element("vars").Descendants();
        foreach(XElement xVar in xmlVars)
        {
            xVar.Value = _player.globalVariables.Variables[xVar.Attribute("name").Value].ToString();
        }
        xDoc.Save(_savePath);
    }

    public void UpdateExecuteElement(string technicalName)
    {
        XDocument xDoc = XDocument.Load(_savePath);
        xDoc.Element("save").Element("execute").Attribute(Const.XmlAliases.ExecuteId).Value = technicalName;
        xDoc.Save(_savePath);
    }

    public void CreateNewSaveFile()
    {
        XDocument xDoc = new XDocument(
                    new XElement("save",
                        new XElement("log"),
                        new XElement("execute", new XAttribute(Const.XmlAliases.ExecuteId, _player.StartOn.TechnicalName)),
                        new XElement("states")));
        xDoc.Element("save").Add(GetGlobalVars("vars"));
        
        xDoc.Save(_savePath);
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
}
