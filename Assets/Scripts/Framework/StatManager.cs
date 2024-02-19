using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class StatManager : Manageable {
    public Stats CurrStats;
    public Stats BaseStats;
    private Dictionary<Action, float> _dictBuffs = new Dictionary<Action, float>();
    private float _time, _healTime;
    private float _healMod = 0.01f;

    /*
    protected override void Awake() {
        base.Awake();
        if (BaseStats != null) {
            CurrStats = new Stats();
            CurrStats.Merge(BaseStats);
        }
        _time = 0.0f;
    }
    */
    
    private void OnEnable() {
        if (BaseStats != null) {
            CurrStats = new Stats();
            CurrStats.Merge(BaseStats);
        }
        _time = 0.0f;
    }

    private void Update() {
        _time += Time.deltaTime;
        _healTime += Time.deltaTime;
        if (CurrStats.Health.HasValue && CurrStats.Health.Value <= 0 ) {
            GameManager.DestroyEntity(_co, CurrStats.PointValue.HasValue ? CurrStats.PointValue.Value : 0.0f);
        }
        if (CurrStats.Lifetime.HasValue && _time > CurrStats.Lifetime.Value) {
            GameManager.DestroyEntity(_co, 0.0f);
        }
        foreach (var kv in _dictBuffs.ToList()) {
            _dictBuffs[kv.Key] -= Time.deltaTime;
            if (_dictBuffs[kv.Key] < 0) {
                kv.Key();
                _dictBuffs.Remove(kv.Key);
            }
        }
    }

    private void FixedUpdate() {
        if (!CurrStats.Health.HasValue || !BaseStats.Health.HasValue || !CurrStats.RegenRate.HasValue) return;
        if (CurrStats.Health.Value < BaseStats.Health.Value) {
            CurrStats.Health += CurrStats.RegenRate.Value * _healMod;
        }
    }

    public void Reset() {
        var scriptableEntity = EntityManager.GetRandomEntity(_co.ET);
        BaseStats = new Stats();
        SetStats(scriptableEntity.BaseStats);
        var r = GetComponent<Rigidbody>();
        if (r != null) {
            r.velocity = Vector3.zero;
            r.ResetInertiaTensor();
            r.ResetCenterOfMass();
        }
    }
    
    public void SetStats(Stats stats) {
        BaseStats.Merge(stats);
        CurrStats = new Stats();
        CurrStats.Merge(BaseStats);
    }
    
    public void UpdateStats(int i) {
        switch (i) {
            case 0:
                CurrStats.ShotDamage += 2;
                break;
            case 1:
                CurrStats.ShotRate += 0.2f;
                break;
            case 2:
                CurrStats.RegenRate += 1;
                break;
            case 3:
                CurrStats.Armor += 1;
                break;
            case 4:
                CurrStats.MaxSpeed += 3;
                break;
        }
        ItemBar.Instance.TriggerPopup(i);
    }

    public void ApplyBuff(Action a, float dur) {
        _dictBuffs[a] = dur;
    }
}