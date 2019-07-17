using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class Condition
{
    string expression;
    
    bool Check()
    {
        return Utilities.ParseExpression(expression);
    }
}
