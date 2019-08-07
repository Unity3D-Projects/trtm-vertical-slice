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

    private string _currentBlockName { get { return _controller.Current.TechnicalName; } }

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
        var states = _xDoc.Element("save").Element("states");
        states.Add(GetGlobalVarsXml("state"), new XAttribute("id", _currentBlockName));

        Debug.Log($"State {_currentBlockName} was saved");
    }

    // удалять и GetGlobalVarsXml()? или переписывать каждую
    public void UpdateGlobalVars()
    {
        var xmlVars = _xDoc.Element("save").Element("vars");
        foreach(XElement var in xmlVars.Descendants())
        {
            var.Value = _player.globalVariables.Variables[var.Attribute("name").Value].ToString();
        }
    }

    public void UpdateExecuteElement(string technicalName)
    {
        _xDoc.Element("execute").Attribute("tn").Value = technicalName;
    }

    public void CreateNewSaveFile(string path)
    {
        _xDoc = new XDocument(_savePath);

        _xDoc.Add(new XElement("save",
                  new XElement("log"),
                  new XElement("execute", new XAttribute("tn", _currentBlockName)),
                  new XElement("states")));
        _xDoc.Add(GetGlobalVarsXml("vars"));
    }

    XElement GetGlobalVarsXml(string elementName)
    {
        XElement varsContainer = new XElement(elementName);
        foreach (var v in _player.globalVariables.Variables)
        {
            varsContainer.Add(new XElement("var", new XAttribute("name", v.Key), v.Value));
        }
        return varsContainer;
    }
}
