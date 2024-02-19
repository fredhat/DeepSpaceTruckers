using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : EntityAction {
    public bool Alternate;
    public float DamageMult;
    private List<Transform> FirePoints = new List<Transform>();
    private Transform _t;
    private Vector3 _rotAxis;
    private int _curFire;
    private int _pLayer;

    protected override void Awake() {
        base.Awake();
        _t = _am.SourceGO != null ? _am.SourceGO.transform : transform;
        if (_am.TargetPoints.Count > 0) foreach (var t in _am.TargetPoints) FirePoints.Add(t.transform);
        _curFire = 0;
        _pLayer = _t.gameObject.layer;
    }

    /*
    protected override void Update() {
        base.Update();
        foreach (var f in FirePoints) {
            Debug.DrawRay(f.position, f.up * 5, Color.green);
        }
    }
    */
    
    protected override void OnUpdate() {
        if (_time < _delay) return;
        _time = 0.0f;
        if (Alternate) {
            var pos = _am.TargetPoints.Count > 0 ? FirePoints[_curFire].position : _t.position;
            var e = GameManager.GetProjectile();
            var t = e.transform;
            t.position = pos;
            t.up = FirePoints[_curFire].up;
            e.gameObject.layer = _pLayer;
            if (_co != null) {
                if (_co.HasComponent(ComponentType.StatManager) && e.HasComponent(ComponentType.StatManager)) {
                    var s1 = _co.GetEntityComponent(ComponentType.StatManager) as StatManager;
                    var s2 = e.GetEntityComponent(ComponentType.StatManager) as StatManager;
                    if (s1 != null && s2 != null) {
                        s2.CurrStats.ContactDamage = s1.CurrStats.ShotDamage.HasValue ? s1.CurrStats.ShotDamage.Value * DamageMult : DamageMult;
                        s2.CurrStats.AccMult = s1.CurrStats.ShotSpeed.HasValue ? s1.CurrStats.ShotSpeed.Value * s2.CurrStats.AccMult.Value : s2.CurrStats.AccMult.Value;
                        s2.CurrStats.Color = s1.CurrStats.Color;
                        if (e.HasComponent(ComponentType.ColorManager)) {
                            var c = e.GetEntityComponent(ComponentType.ColorManager) as ColorManager;
                            if (c != null) c.SetColors(s2.CurrStats.Color);
                        }
                        s2.CurrStats.Faction = s1.CurrStats.Faction;
                        if (s2.CurrStats.WhenShootSound != null) AudioManager.Instance.PlaySound(s2.CurrStats.WhenShootSound, pos);
                    }
                }
            }
            _curFire = (_curFire + 1) % FirePoints.Count;
        } else {
            _curFire = 0;
            AudioClip clip = null;
            foreach (var f in FirePoints) {
                var pos = _am.TargetPoints.Count > 0 ? f.position : _t.position;
                var e = GameManager.GetProjectile();
                var t = e.transform;
                t.position = pos;
                t.up = f.up;
                e.gameObject.layer = _pLayer;
                if (_co != null) {
                    if (_co.HasComponent(ComponentType.StatManager) && e.HasComponent(ComponentType.StatManager)) {
                        var s1 = _co.GetEntityComponent(ComponentType.StatManager) as StatManager;
                        var s2 = e.GetEntityComponent(ComponentType.StatManager) as StatManager;
                        if (s1 != null && s2 != null) {
                            s2.CurrStats.ContactDamage = s1.CurrStats.ShotDamage.HasValue ? s1.CurrStats.ShotDamage.Value * DamageMult : DamageMult;
                            s2.CurrStats.AccMult = s1.CurrStats.ShotSpeed.HasValue ? s1.CurrStats.ShotSpeed.Value * s2.CurrStats.AccMult.Value : s2.CurrStats.AccMult.Value;
                            s2.CurrStats.Color = s1.CurrStats.Color;
                            if (e.HasComponent(ComponentType.ColorManager)) {
                                var c = e.GetEntityComponent(ComponentType.ColorManager) as ColorManager;
                                if (c != null) c.SetColors(s2.CurrStats.Color);
                            }
                            s2.CurrStats.Faction = s1.CurrStats.Faction;
                            if (s2.CurrStats.WhenShootSound != null) clip = s2.CurrStats.WhenShootSound;
                        }
                    }
                }
                _curFire++;
            }
            if (clip != null) AudioManager.Instance.PlaySound(clip, _t.position);
        }
    }
}