using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
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

    public Block(string id, string buttonText, string[] phrases, string image, string music, Condition condition, Instruction instruction, double delay, Block[] exits)
    {
        this.id = id;
        this.buttonText = buttonText;
        this.phrases = phrases;
        this.image = image;
        this.music = music;
        this.condition = condition;
        this.instruction = instruction;
        this.delay = delay;
        this.exits = exits;
    }
}
