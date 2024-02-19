using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class Attractee : Manageable {
    private Attractor _a;
    private Transform _t;
    private Rigidbody _r;
    
    private void Start() {
        _r = GetComponent<Rigidbody>();
        _r.constraints = RigidbodyConstraints.FreezeRotation;
        _r.useGravity = false;
        _t = transform;
    }

    private void Update() {
        //if (_r.isKinematic || _a is null) return;
        //_a.Attract(_t, _r);
    }

    private void FixedUpdate() {
        if (_a == null) return;
        _a.Attract(_t, _r);
    }

    private void OnEnable() {
        StageManager.OnAttractorChanged += SetAttractor;
    }

    private void OnDisable() {
        StageManager.OnAttractorChanged -= SetAttractor;
    }

    public void SetAttractor(Attractor a) => _a = a;
}