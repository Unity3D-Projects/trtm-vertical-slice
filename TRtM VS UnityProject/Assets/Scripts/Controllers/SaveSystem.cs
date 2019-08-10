using Articy.Unity;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private GameController _controller;
    private ArticyFlowPlayer _player;

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

        CreateNewSaveFile();
    }

    // get type of event
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
    public void UpdateGlobalVars()
    {
        XDocument xDoc = XDocument.Load(_savePath);
        var xmlVars = xDoc.Element("save").Element("vars");
        foreach(XElement var in xmlVars.Descendants())
        {
            var.Value = _player.globalVariables.Variables[var.Attribute("name").Value].ToString();
        }
        xDoc.Save(_savePath);
    }

    public void UpdateExecuteElement(string technicalName)
    {
        XDocument xDoc = XDocument.Load(_savePath);
        xDoc.Element("save").Element("execute").Attribute("tn").Value = technicalName;
        xDoc.Save(_savePath);
    }

    public void CreateNewSaveFile()
    {
        XDocument xDoc = new XDocument(
                    new XElement("save",
                        new XElement("log"),
                        new XElement("execute", new XAttribute("tn", "null")),
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
