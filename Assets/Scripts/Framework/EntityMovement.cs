using System;
using UnityEngine;

[RequireComponent(typeof(MovementManager))]
public abstract class EntityMovement : Manageable {
    protected MovementManager _mm;
    protected override void Awake() {
        base.Awake();
        _mm = GetComponent<MovementManager>();
    }

    private void OnEnable() {
        _mm.OnUpdate += OnUpdate;
        _mm.OnFixedUpdate += OnFixedUpdate;
    }
    
    private void OnDisable() {
        _mm.OnUpdate -= OnUpdate;
        _mm.OnFixedUpdate -= OnFixedUpdate;
    }

    protected virtual void OnUpdate() {
        
    }
    
    protected virtual void OnFixedUpdate() {
        
    }
}