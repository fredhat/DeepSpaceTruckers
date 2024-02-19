using System;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementManager : Manageable {
    public event Action OnUpdate, OnFixedUpdate;
    public GameObject Target;

    private void Update() {
        OnUpdate?.Invoke();
    }

    private void FixedUpdate() {
        OnFixedUpdate?.Invoke();
    }
}