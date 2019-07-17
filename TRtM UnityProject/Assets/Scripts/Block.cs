using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Block
{
    public string id;
    public string buttonText;
    public string[] phrases;
    public Sprite image;
    public string music;
    public Condition condition;
    public Instruction instruction;
    public double delay;

    public Block[] exits;
}
