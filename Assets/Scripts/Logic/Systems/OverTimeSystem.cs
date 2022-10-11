using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTimeSystem : IEcsRunSystem
{
    protected float _maxDelay;
    protected float _delay;

    public void Run(IEcsSystems systems)
    {
        var logic = systems.GetShared<SharedData>();
        _delay -= logic.DeltaTime;
        if (_delay <= 0)
        {
            RunOverTime(systems);
            _delay = _maxDelay;
        }
    }

    protected virtual void RunOverTime(IEcsSystems systems) { }
}
