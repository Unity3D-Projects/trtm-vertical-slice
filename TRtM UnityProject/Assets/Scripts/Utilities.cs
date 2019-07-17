using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B83.LogicExpressionParser;
using UnityEngine.UI;

public class Utilities : MonoBehaviour
{
    public PrefabManager prefabManager;

    private void Awake()
    {
        prefabManager = FindObjectOfType<PrefabManager>();
    }

    public static bool ParseExpression(string exp)
    {
        Parser parser = new Parser();
        LogicExpression parsed = parser.Parse(exp);
        SetContext(ref parsed);
        return parsed.GetResult();
    }

    public static void ExecuteInstructions(string instrs)
    {
        var db = Database.Instance;

        string[] separated = instrs.Split(';');

        foreach (string instr in separated)
        {
            if (instr.Contains("+="))
            {
                string[] sides = instr.Split(new string[] { "+=" }, System.StringSplitOptions.None);
                db[sides[0]] += int.Parse(sides[1]);
            }
            else if (instr.Contains("*="))
            {
                string[] sides = instr.Split(new string[] { "*=" }, System.StringSplitOptions.None);
                db[sides[0]] *= int.Parse(sides[1]);
            }
            else if (instr.Contains("="))
            {
                string[] sides = instr.Split(new string[] { "=" }, System.StringSplitOptions.None);
                db[sides[0]] = int.Parse(sides[1]);
            }
        }
    }

    public void SpawnPhrase(string phrase)
    {
        Instantiate(prefabManager.phrasePrefab, prefabManager.content);
    }

    public void SpawnButton(string text, UnityEngine.Events.UnityAction call)
    {
        GameObject buttonGroup = Instantiate(prefabManager.buttonGroup, prefabManager.content);
        Button button = Instantiate(prefabManager.buttonPrefab, buttonGroup.transform);
        button.onClick.AddListener(call);
    }

    public void SpawnEndgame()
    {
        Instantiate(prefabManager.endgamePrefab, prefabManager.content);
    }

    /* -----------------------------*/

    static void SetContext(ref LogicExpression le)
    {
        var db = Database.Instance;
        string[] vars = new string[db.Keys.Count];
        db.Keys.CopyTo(vars, 0);
        foreach (string var in vars)
        {
            le.Context[var].Set(db[var]);
        }
    }
}
