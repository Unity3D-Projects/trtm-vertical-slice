using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Instruction
{
    string expression;

    void Execute(string expression)
    {
        Utilities.ExecuteInstructions(expression);
    }
}
