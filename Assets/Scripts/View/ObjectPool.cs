using Leopotam.EcsLite;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

public enum ObjectType 
{
    Spaceship,
    Asteroid,
    Bullet,
    Ufo
}

[Serializable]
public class TypeViewDictionary : SerializableDictionary<ObjectType, View> { }

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private TypeViewDictionary _prefabs;

    private Transform _transform;
    private Dictionary<ObjectType, Stack<View>> _pool;
    private Dictionary<int, (View View, ObjectType Type)> _spawned;

    public event Action<View> SpaceshipSpawned;
    public event Action<View> SpaceshipDespawned;

    private void Awake()
    {
        _transform = transform;
        _pool = new Dictionary<ObjectType, Stack<View>>();
        var allTypes = typeof(ObjectType).GetFields(BindingFlags.Static | BindingFlags.Public);
        foreach (var type in allTypes)
        {
            _pool[(ObjectType)type.GetValue(null)] = new Stack<View>();
        }

        _spawned = new Dictionary<int, (View, ObjectType)>();
    }

    public View Spawn(ObjectType type, EcsWorld world, int entity)
    {
        ref var transform = ref world.GetPool<TransformComponent>().Get(entity);
        ref var collider = ref world.GetPool<ColliderComponent>().Get(entity);
        View obj;

        if (_pool[type].Count > 0)
        {
            obj = _pool[type].Pop();
        }
        else
        {
            obj = Instantiate(_prefabs[type], _transform);
            obj.gameObject.SetActive(false);
        }

        obj.Init(world, entity);
        obj.Transform.position = transform.Position;
        obj.Transform.up = transform.Direction;
        obj.Transform.localScale = Vector3.one * collider.Collider.Radius * 2;
        obj.gameObject.SetActive(true);
        _spawned[entity] = (obj, type);

        if (type == ObjectType.Spaceship) 
            SpaceshipSpawned?.Invoke(obj);

        return obj;
    }

    public void Despawn(IEnumerable<int> despawned)
    {
        View obj;
        foreach (var entity in despawned)
        {
            var spawned = _spawned[entity];
            _spawned.Remove(entity);

            obj = spawned.View;
            obj.transform.localScale = Vector3.one;
            obj.Disable();
            _pool[spawned.Type].Push(obj);

            if (spawned.Type == ObjectType.Spaceship)
                SpaceshipDespawned?.Invoke(obj);
        }
    }

    public void DespawnAll()
    {
        Despawn(_spawned.Keys.ToList());
    }
}
