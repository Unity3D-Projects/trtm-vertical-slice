using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{

    private GameController _controller;
    private ArticyFlowPlayer _player;
    private XDocument _xDoc;


    private string _savePath = Const.SavePath;
    public bool HasFile { get; set; } = false;

    private void Awake()
    {
        _controller = GetComponent<GameController>();
        _player = GetComponent<ArticyFlowPlayer>();
    }

    private void Start()
    {
        // is there any save at path?
        if (!HasFile)
        {
            CreateNewSaveFile(_savePath);
        }
    }

    // get type of event
    public void LogEvent(string eventType, string content)
    {
        // check if xdoc is instantiated
        _xDoc?.Element("save").Element("log").Add(new XElement(eventType, content));
    }

    public void LogState()
    {
        var current = _controller.Current;
        var states = _xDoc.Element("save").Element("states");
        states.Add(GetGlobalVarsXml("state"), new XAttribute("id", current.TechnicalName));

        Debug.Log($"State {current.TechnicalName} was saved");
    }

    public void CreateNewSaveFile(string path)
    {
        var current = _controller.Current;
        _xDoc = new XDocument(_savePath);

        _xDoc.Add(new XElement("save",
                  new XElement("log"),
                  new XElement("vars"),
                  new XElement("execute", new XAttribute("tn", current.TechnicalName)),
                  new XElement("states")));
        
        var xmlVars = _xDoc.Element("save").Element("vars");
        GetGlobalVarsXml("vars");
    }

    XElement GetGlobalVarsXml(string elementName)
    {
        XElement state = new XElement("state");
        foreach (var v in _player.globalVariables.Variables)
        {
            state.Add(new XElement("var", new XAttribute("name", v.Key), v.Value));
        }
        return state;
    }
}
