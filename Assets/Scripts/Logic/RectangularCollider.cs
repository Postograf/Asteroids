using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangularCollider
{
    public Vector2[] Dots { get; private set; }
    public Vector2[] NormalizedPerpendiculars { get; private set; }
    public (Vector2 First, Vector2 Second)[] Edges { get; private set; }
    public Vector2 Center { get; private set; }
    public Vector2 Size { get; private set; }

    public RectangularCollider(Vector2 size)
    {
        Size = size;
    }

    public RectangularCollider Relocate(Vector2 start, Vector2 direction)
    {
        direction.Normalize();
        Center = start + direction * Size.y / 2;
        Dots = new Vector2[4];

        var multiplyer = 1;
        var perpendicular = Vector2.Perpendicular(direction);
        for (int i = 0; i < 4; i++)
        {
            multiplyer = i % 2 == 0 ? 1 : -1;
            if (i >= 2) multiplyer *= -1;
            if (i == 2) start += direction * Size.y;
            Dots[i] = start + multiplyer * perpendicular * Size.x / 2;
        }

        NormalizedPerpendiculars = new Vector2[Dots.Length];
        Edges = new (Vector2 First, Vector2 Second)[Dots.Length];
        for (int i = 0; i < Dots.Length; i++)
        {
            var nextIndex = i + 1 == Dots.Length ? 0 : i + 1;
            
            Edges[i] = (Dots[i], Dots[nextIndex]);
            var edge = Dots[nextIndex] - Dots[i];
            NormalizedPerpendiculars[i] = Vector2.Perpendicular(edge).normalized;
        }

        Debug.DrawLine(Dots[0], Dots[1], Color.white, 100f);
        Debug.DrawLine(Dots[1], Dots[2], Color.white, 100f);
        Debug.DrawLine(Dots[2], Dots[3], Color.white, 100f);
        Debug.DrawLine(Dots[3], Dots[0], Color.white, 100f);

        return this;
    }
}