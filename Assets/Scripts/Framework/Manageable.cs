using System;
using UnityEngine;

[RequireComponent(typeof(ComponentManager))]
public abstract class Manageable : MonoBehaviour {
    [SerializeField] public ComponentType Type;
    protected ComponentManager _co;

    protected virtual void Awake() {
        _co = gameObject.GetComponent<ComponentManager>();
    }
}