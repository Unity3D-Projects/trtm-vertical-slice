using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B83.LogicExpressionParser;

public static class Utilities
{
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
