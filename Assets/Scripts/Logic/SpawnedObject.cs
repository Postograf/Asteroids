using Leopotam.EcsLite;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject
{
    private EcsWorld _world;
    private int _entity;

    public event Action<int> Inited;

    public SpawnedObject(EcsWorld world, int entity)
    {
        _world = world;
        _entity = entity;
    }

    public SpawnedObject Add<T>(T component)
        where T : struct
    {
        ref var c = ref _world.GetPool<T>().Add(_entity);
        c = component;
        return this;
    }

    public int End()
    {
        Inited?.Invoke(_entity);
        return _entity;
    }
}
