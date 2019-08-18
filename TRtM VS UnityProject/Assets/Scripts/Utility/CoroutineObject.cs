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

public sealed class CoroutineObject<T1, T2> : CoroutineObjectBase
{
    public Func<T1, T2, IEnumerator> Routine { get; private set; }

    public override event Action Finished;

    public CoroutineObject(MonoBehaviour owner, Func<T1, T2, IEnumerator> routine)
    {
        Owner = owner;
        Routine = routine;
    }

    private IEnumerator Process(T1 arg1, T2 arg2)
    {
        yield return Routine.Invoke(arg1, arg2);

        Coroutine = null;

        Finished?.Invoke();
    }

    public void Start(T1 arg1, T2 arg2)
    {
        Stop();

        Coroutine = Owner.StartCoroutine(Process(arg1, arg2));
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