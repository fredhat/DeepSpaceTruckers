using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ActionManager : Manageable {
    public event Action OnUpdate, OnFixedUpdate;
    public GameObject SourceGO;
    public List<GameObject> TargetPoints = new List<GameObject>();

    private void Update() {
        OnUpdate?.Invoke();
    }

    private void FixedUpdate() {
        OnFixedUpdate?.Invoke();
    }
}