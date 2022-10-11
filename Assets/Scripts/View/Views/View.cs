using Leopotam.EcsLite;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public EcsWorld World { get; private set; }
    public int Entity { get; private set; }

    private void Awake()
    {
        Transform = transform;
    }

    public void Init(EcsWorld world, int entity)
    {
        World = world;
        Entity = entity;
    }

    private void Update()
    {
        ref var transform = ref World.GetPool<TransformComponent>().Get(Entity);
        Transform.position = transform.Position;
        Transform.up = transform.Direction;
    }

    public void Disable()
    {
        World = null;
        Entity = -1;
        gameObject.SetActive(false);
    }
}
