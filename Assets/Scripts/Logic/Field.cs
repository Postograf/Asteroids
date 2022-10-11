using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
    public Vector2 Center { get; private set; }
    public Vector2 Size { get; private set; }
    public Vector2 Max { get; private set; }
    public Vector2 Min { get; private set; }
    public float Diagonal { get; private set; }

    public Field(Vector2 center, Vector2 size)
    {
        Center = center;
        Size = size;
        Max = Center + Size / 2;
        Min = Center - Size / 2;

        Diagonal = size.magnitude;
    }

    public Vector2 MoveInside(Vector2 vector)
    {
        return new Vector2(
            MoveInside(vector.x, Max.x, Min.x),
            MoveInside(vector.y, Max.y, Min.y)
        );
    }

    private float MoveInside(float value, float max, float min)
    {
        var legth = max - min;

        if (value > max)
        {
            var delta = value - max;
            value = min + delta % legth;
        }
        else if (value < min)
        {
            var delta = min - value;
            value = max - delta % legth;
        }

        return value;
    }

    public Vector2 RandomEdgePoint()
    {
        var axisRandom = Random.Range(0, 2);
        var sideRandom = Random.Range(0, 2);
        float x, y;

        if (axisRandom == 0)
        {
            x = Random.Range(Min.x, Max.x);
            y = Mathf.Lerp(Min.y, Max.y, sideRandom);
        }
        else
        {
            x = Mathf.Lerp(Min.x, Max.x, sideRandom);
            y = Random.Range(Min.y, Max.y);
        }

        return new Vector2(x, y);
    }
}
