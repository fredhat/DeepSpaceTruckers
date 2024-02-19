using System;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableEntityBase : ScriptableObject {
    [SerializeField] private Stats _stats;
    public Stats BaseStats => _stats;
    public EntityType ET;
    public ComponentManager Prefab;
}