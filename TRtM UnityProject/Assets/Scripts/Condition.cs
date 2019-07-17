using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class Condition
{
    string expression;
    
    public Condition(string expression)
    {
        this.expression = expression;
    }

    public bool Check()
    {
        return Utilities.ParseExpression(expression);
    }
}
