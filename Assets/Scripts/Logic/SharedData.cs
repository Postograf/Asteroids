using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedData
{
    public GameLogic Logic { get; private set; }
    public float DeltaTime { get; set; }

    public SharedData(GameLogic logic, float delta)
    {
        Logic = logic;
        DeltaTime = delta;
    }
}
