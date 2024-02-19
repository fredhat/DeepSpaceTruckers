using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ActionManager))]
public abstract class EntityAction : Manageable {
    public ControlTypes CT;
    public float Delay;
    protected ActionManager _am;
    protected float _time;
    protected float _delay;
    
    protected override void Awake() {
        base.Awake();
        _am = GetComponent<ActionManager>();
        _time = 0.0f;
        _delay = Delay;
    }

    protected virtual void Update() {
        _time += Time.deltaTime;
        if (_co == null || !_co.HasComponent(ComponentType.StatManager)) return;
        var s = _co.GetEntityComponent(ComponentType.StatManager) as StatManager;
        if (s != null) {
            _delay = s.CurrStats.ShotRate.HasValue ? 1 / s.CurrStats.ShotRate.Value * Delay : Delay;
        }
        if (CT != ControlTypes.None && _co.ET == EntityType.Player) AbilityMeter.UpdateCooldown(_delay, _time, CT);
    }

    private void OnEnable() {
        switch (CT) {
            case ControlTypes.None:
                _am.OnUpdate += OnUpdate;
                _am.OnFixedUpdate += OnFixedUpdate;
                break;
            case ControlTypes.Primary:
                InputManager.OnPrimary += OnUpdate;
                break;
            case ControlTypes.Secondary:
                InputManager.OnSecondary += OnUpdate;
                break;
            case ControlTypes.Utility:
                InputManager.OnUtility += OnUpdate;
                break;
            case ControlTypes.Super:
                InputManager.OnSuper += OnUpdate;
                break;
            case ControlTypes.Interact:
                InputManager.OnInteract += OnUpdate;
                break;
        }
    }
    
    private void OnDisable() {
        switch (CT) {
            case ControlTypes.None:
                _am.OnUpdate -= OnUpdate;
                _am.OnFixedUpdate -= OnFixedUpdate;
                break;
            case ControlTypes.Primary:
                InputManager.OnPrimary -= OnUpdate;
                break;
            case ControlTypes.Secondary:
                InputManager.OnSecondary -= OnUpdate;
                break;
            case ControlTypes.Utility:
                InputManager.OnUtility -= OnUpdate;
                break;
            case ControlTypes.Super:
                InputManager.OnSuper -= OnUpdate;
                break;
            case ControlTypes.Interact:
                InputManager.OnInteract += OnUpdate;
                break;
        }
    }

    protected virtual void OnUpdate() {
        
    }
    
    protected virtual void OnFixedUpdate() {
        
    }
}