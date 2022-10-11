using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TransformComponent
{
    public Vector2 Position { get; set; }

    private Vector2 _direction;
    public Vector2 Direction 
    {
        get => _direction;
        set => _direction = value.normalized;
    }
}