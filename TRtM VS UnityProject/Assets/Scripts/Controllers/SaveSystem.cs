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

    private GameState GameState;
    private XDocument Xdoc;


    private string _savePath = Const.SavePath;
    public bool HasFile { get; set; } = false;

    private void Awake()
    {
        _controller = GetComponent<GameController>();
        _player = GetComponent<ArticyFlowPlayer>();

        GameState = new GameState(_player.globalVariables);
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
        Xdoc?.Element("save").Element("log").Add(new XElement(eventType, content));
    }

    public void LogState()
    {
        // log state
    }

    private void CreateNewSaveFile(string path)
    {
        Xdoc = new XDocument(_savePath);
        Xdoc.Add(new XElement("save",
                 new XElement("log"),
                 new XElement("vars"),
                 new XElement("execute", new XAttribute("tn", _controller.Current?.TechnicalName ?? "")),
                 new XElement("states")));

        var globalVars = GameState.Vars;
        var vars = Xdoc.Element("save").Element("vars");
        foreach (var v in globalVars)
        {
            vars.Add(new XElement("var", new XAttribute("name", v.Key), v.Value));
        }
    }
}
