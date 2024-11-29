using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Q4MultiPlayer : Q4Player
{
    [SerializeField] private List<Q4PlayerReference> components = new List<Q4PlayerReference>();

    [SerializeField] protected UnityEvent onPlay;
    [SerializeField] protected UnityEvent onPause;
    [SerializeField] protected UnityEvent onContinue;
    [SerializeField] protected UnityEvent onStop;
    [SerializeField] protected UnityEvent onEnd;
    [SerializeField] protected UnityEvent onReplay;

    private List<Q4PlayerReference> activeComponents = new List<Q4PlayerReference>();
    private List<Q4PlayerReference> usedComponents = new List<Q4PlayerReference>();

    private float playedTime = 0.0f;

    public override void OnContinue()
    {
        activeComponents.ForEach(x => x.Player.OnContinue());
        onContinue.Invoke();
        base.OnContinue();
    }

    public override void OnEnd()
    {
        for (int i = activeComponents.Count - 1; i >= 0; i--)
        {
            activeComponents[i].Player.OnEnd();
            components.Add(activeComponents[i]);
        }
        usedComponents.ForEach(x => components.Add(x));

        activeComponents.Clear();
        usedComponents.Clear();

        playedTime = 0.0f;
        onEnd.Invoke();
        base.OnEnd();
    }

    public override void OnPause()
    {
        activeComponents.ForEach(x => x.Player.OnPause());
        onPause.Invoke();
        base.OnPause();
    }

    public override void OnPlay()
    {
        activeComponents.ForEach(x =>
        {
            x.Player.OnStop();
            components.Add(x);
        });
        usedComponents.ForEach(x => components.Add(x));

        activeComponents.Clear();
        usedComponents.Clear();


        playedTime = 0.0f;

        for (int i = components.Count - 1; i >= 0; i--)
        {
            if (components[i].StartTime <= playedTime)
            {
                activeComponents.Add(components[i]);
                components[i].Player.OnPlay();
                components.RemoveAt(i);
            }
        }
        onPlay.Invoke();
        base.OnPlay();
    }

    public override void OnStop()
    {
        activeComponents.ForEach(x =>
        {
            x.Player.OnStop();
            components.Add(x);
        });
        usedComponents.ForEach(x => components.Add(x));

        activeComponents.Clear();
        usedComponents.Clear();

        playedTime = 0.0f;

        onStop.Invoke();
        base.OnStop();
    }

    public override void OnReplay()
    {
        activeComponents.ForEach(x =>
        {
            components.Add(x);
        });
        usedComponents.ForEach(x =>
        {
            components.Add(x);
        });

        activeComponents.Clear();
        usedComponents.Clear();


        playedTime = 0.0f;

        for (int i = components.Count - 1; i >= 0; i--)
        {
            if (components[i].StartTime <= playedTime)
            {
                activeComponents.Add(components[i]);
                components[i].Player.OnPlay();
                components.RemoveAt(i);
            }
        }
        onReplay.Invoke();
        base.OnReplay();
    }

    protected override void OnUpdate()
    {
        playedTime += Time.deltaTime;
        if (playedTime >= Duration) { playedTime = Duration; }


        for (int i = activeComponents.Count - 1; i >= 0; i--)
        {
            activeComponents[i].Player.Update();
            if (!activeComponents[i].Player.IsPlaying)
            {
                activeComponents[i].Player.OnEnd();
                usedComponents.Add(activeComponents[i]);
                activeComponents.RemoveAt(i);
            }
        }

        for (int i = components.Count - 1; i >= 0; i--)
        {
            if (components[i].StartTime <= playedTime)
            {
                activeComponents.Add(components[i]);
                components[i].Player.OnPlay();
                components.RemoveAt(i);
            }
        }
    }
}

[System.Serializable]
public class Q4PlayerReference
{
    [SerializeField] private Q4Player player;
    [SerializeField] private float startTime = 0.0f;

    public Q4Player Player => player;
    public float StartTime => startTime;
}