using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Delay : MonoBehaviour
{
    public bool IsRunning;

    public UnityEvent OnDelayPassed;

    public UnityEvent OnUpdate;

    [SerializeField] private float _remainingTimeInSeconds;
    public float RemainingTimeInSeconds
    {
        get => _remainingTimeInSeconds;
        set
        {
            if (!IsRunning)
            {
                _remainingTimeInSeconds = value;
            }
            else
            {
                Debug.LogError("Trying to set delay time when delay is running");
            }
        }
    }

    private void Awake()
    {
        OnDelayPassed = new UnityEvent();
        OnUpdate = new UnityEvent();
    }

    void Start()
    {
        Run();
    }

    public void Run()
    {
        StartCoroutine(RunCoroutine());
        IsRunning = true;
    }

    public void Stop()
    {
        StopCoroutine(RunCoroutine());
        IsRunning = false;
    }

    public void Boost(float timeInSeconds)
    {
        Stop();
        _remainingTimeInSeconds -= timeInSeconds;
        Run();
    }

    IEnumerator RunCoroutine()
    {

        while (_remainingTimeInSeconds > 0)
        {
            _remainingTimeInSeconds -= Time.deltaTime / 2;
            yield return null;
            OnUpdate.Invoke();
        }
        
        if (OnDelayPassed != null)
        {
            OnDelayPassed.Invoke();
        }
        else
        {
            Debug.LogError("Trying to invoke OnDelayPassed callback when it is null");

        }
    }
}
