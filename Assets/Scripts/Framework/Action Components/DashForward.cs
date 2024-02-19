using System.Collections.Generic;
using UnityEngine;

public class DashForward : EntityAction {
    public float BonusAccMult, BonusMaxSpeed;
    public float BuffDuration;
    private Transform _t;
    private StatManager _s;
    
    private void Start() {
        _t = transform;
    }
    
    protected override void OnUpdate() {
        if (_time < Delay) return;
        _time = 0.0f;
        if (_co != null) {
            if (_co.HasComponent(ComponentType.StatManager)) {
                _s = _co.GetEntityComponent(ComponentType.StatManager) as StatManager;
                if (_s != null) {
                    if (_s.CurrStats.AccMult.HasValue) _s.CurrStats.AccMult *= BonusAccMult;
                    if (_s.CurrStats.MaxSpeed.HasValue) _s.CurrStats.MaxSpeed += BonusMaxSpeed;
                    _s.CurrStats.IsInvincible = true;
                    _s.ApplyBuff(UndoBuff, BuffDuration);
                }
            }
        }
    }

    private void UndoBuff() {
        if (_s.CurrStats.AccMult.HasValue) _s.CurrStats.AccMult /= BonusAccMult;
        if (_s.CurrStats.MaxSpeed.HasValue) _s.CurrStats.MaxSpeed -= BonusMaxSpeed;
        _s.CurrStats.IsInvincible = false;
    }
}