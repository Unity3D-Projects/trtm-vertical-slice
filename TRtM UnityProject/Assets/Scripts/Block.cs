using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Block
{
    string id;
    string buttonText;
    string[] phrases;
    Sprite image;
    string music;
    Condition[] conditions;
    Instruction[] instructions;
    double delay;
}
