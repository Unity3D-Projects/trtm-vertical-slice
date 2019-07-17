using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Instruction
{
    string expression;

    public Instruction(string expression)
    {
        this.expression = expression;
    }

    public void Execute()
    {
        Utilities.ExecuteInstructions(expression);
    }
}
