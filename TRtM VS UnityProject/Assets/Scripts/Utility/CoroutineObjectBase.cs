using Action = System.Action;

using UnityEngine;

public abstract class CoroutineObjectBase
{
    public MonoBehaviour Owner { get; protected set; }
    public Coroutine Coroutine { get; protected set; }

    public bool IsProcessing => Coroutine != null;

    public abstract event Action Finished;
}