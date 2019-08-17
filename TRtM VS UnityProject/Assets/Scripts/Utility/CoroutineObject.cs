using System;
using System.Collections;

using UnityEngine;

public sealed class CoroutineObject<T> : CoroutineObjectBase
{
    public Func<T, IEnumerator> Routine { get; private set; }

    public override event Action Finished;

    public CoroutineObject(MonoBehaviour owner, Func<T, IEnumerator> routine)
    {
        Owner = owner;
        Routine = routine;
    }

    private IEnumerator Process(T arg)
    {
        yield return Routine.Invoke(arg);

        Coroutine = null;

        Finished?.Invoke();
    }

    public void Start(T arg)
    {
        Stop();

        Coroutine = Owner.StartCoroutine(Process(arg));
    }

    public void Stop()
    {
        if (IsProcessing)
        {
            Owner.StopCoroutine(Coroutine);

            Coroutine = null;
        }
    }
}