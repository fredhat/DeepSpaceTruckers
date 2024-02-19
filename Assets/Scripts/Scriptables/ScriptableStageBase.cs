using System;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableStageBase : ScriptableObject {
    [SerializeField] private StageStats _stats;
    public StageStats BaseStats => _stats;
    public Attractor Prefab;
}

[Serializable]
public struct StageStats {
    public float Gravity;
    public float Radius;
    public float RotSpeed;
    public int MinNumObstacles;
    public int MaxNumObstacles;
    public int MinNumChests;
    public int MaxNumChests;
}