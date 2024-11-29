using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Q4Player : MonoBehaviour
{
    [SerializeField] private bool playOnStart = false;
    [Min(0.00001f)][SerializeField] private float duration = 1.0f;
    [Tooltip("Set to -1 for endless")][SerializeField] private sbyte playcount = -1;
    

    public float Duration { get => duration; protected set => duration = value; }
    public sbyte Playcount { get => playcount; protected set => playcount = value; }


    public bool IsPlaying { get; private set; } = false;
    protected float Counter { get; private set; } = 0.0f;
    private byte playCounter = 0;

    protected virtual void Start()
    {
        IsPlaying = false;
        if (playOnStart) { Play(); }
        else { this.enabled = false; }
    }

    public void Play()
    {
        Counter = 0.0f;
        playCounter = 0;
        this.enabled = true;
        OnPlay();
    }
    public void Pause()
    {
        if (!IsPlaying) { Stop(); }
        else { this.enabled = false; OnPause(); }
    }
    public void Continue()
    {
        if (!IsPlaying) { Play(); }
        else { this.enabled = true; OnContinue(); }
    }
    public void Stop()
    {
        Counter = 0.0f;
        playCounter = 0;
        this.enabled = false;
        OnStop();
    }
    private void End()
    {
        Counter = 0.0f;
        playCounter = 0;
        this.enabled = false;
        OnEnd();
    }


    public void Update()
    {
        Counter += Time.deltaTime / Duration;
        if (Counter >= 1.0f)
        {
            if (Playcount > 0)
            {
                playCounter++;
                if (playCounter >= Playcount)
                { Counter = 1.0f; }
                else { Counter -= 1.0f; OnReplay(); }
            }
            else { Counter -= 1.0f; OnReplay(); }
        }
        OnUpdate();
        if (Playcount > 0 && playCounter >= Playcount) { End(); }
    }

    protected abstract void OnUpdate();
    public virtual void OnPlay() { IsPlaying = true;  }
    public virtual void OnPause() {  }
    public virtual void OnContinue() { }
    public virtual void OnStop() { IsPlaying = false;  }
    public virtual void OnEnd() { IsPlaying = false;  }
    public virtual void OnReplay() {  }
}