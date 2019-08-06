using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using Articy.Unity;

public class GameState
{
    public string Execute { get; set; } //tech name string
    public Dictionary<string, object> Vars { get; set; } //articy global vars obj
    public string Log { get; set; } //
    public string States { get; set; }

    public GameState(BaseGlobalVariables bgv)
    {
        Vars = bgv.Variables;
    }
}
