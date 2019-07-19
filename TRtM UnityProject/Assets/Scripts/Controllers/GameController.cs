using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public Block currentBlock;
    public Sprite currentImage;

    public AudioController audioController;
    public Database database;
    public SaveSystem saveSystem;
    public PrefabManager prefabManager;
    public Utilities utilities;
    
    // Game entry point
    private void Start()
    {
        // Spawn first block



        // Loop
        Loop();

    }

    void SwitchBlock(Block block)
    {
        if (currentBlock.delay == 0)
        {
            currentBlock = block;
            Loop();
        }
        else
        {
            StartCoroutine(DelayedSwitch(DateTime.Now, block));
        }
        
    }

    IEnumerator DelayedSwitch(DateTime startedAt, Block block)
    {

        DateTime shouldEndAt = startedAt.AddMinutes(currentBlock.delay);
        double secondsPassed = (DateTime.Now - startedAt).TotalSeconds;
        double progress = (secondsPassed / currentBlock.delay);

        while (progress < 1)
        {
            progress += (Time.deltaTime / currentBlock.delay);
            yield return null;
        }

        currentBlock = block;
        Loop();
    }

    void Loop()
    {
        // Execute instructions

        currentBlock.instruction.Execute();

        // Spawn phrases

        foreach (string phrase in currentBlock.phrases)
        {
            utilities.SpawnPhrase(phrase);
        }

        // Set candidates

        if (currentBlock.exits.Length == 0)
            utilities.SpawnEndgame();
        else
        {
            List<Block> candidates = new List<Block>();

            foreach (Block exit in currentBlock.exits)
            {
                if (exit.condition.Check())
                    candidates.Add(exit);
            }

            // Check candidates

            if (candidates.Count <= 0)
            {
                // Error
            }
            else if (candidates.Count == 1)
            {
                SwitchBlock(candidates.First());
            }
            else
            {
                // Spawn buttons

                foreach (Block candidate in candidates)
                {
                    utilities.SpawnButton(candidate.buttonText, () => SwitchBlock(candidate));
                }
            }
        }
    }
}
