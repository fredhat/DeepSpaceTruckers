using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class CollisionManager : Manageable {
    public bool AbsorbColor;
    public float IFrameDuration = 0.1f;
    private float _time;

    private void Start() {
        _time = 0.0f;
    }

    private void Update() {
        _time += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision col) {
        var cm = col.gameObject.GetComponent<ComponentManager>();
        if (_co == null || cm == null) return;
        if (_co.HasComponent(ComponentType.StatManager) && cm.HasComponent(ComponentType.StatManager)) {
            var s1 = _co.GetEntityComponent(ComponentType.StatManager) as StatManager;
            var s2 = cm.GetEntityComponent(ComponentType.StatManager) as StatManager;
            if (s1 != null && s2 != null) {
                var dam = s2.CurrStats.ContactDamage.HasValue ? s2.CurrStats.ContactDamage.Value : 1.0f;
                var col1 = s1.CurrStats.Color;
                var col2 = s2.CurrStats.Color;
                if (col2 == ColorType.Red && col1 == ColorType.Blue || col2 == ColorType.Blue && col1 == ColorType.Red) dam *= 2.0f;
                if (col2 == ColorType.Red && col1 == ColorType.Red || col2 == ColorType.Blue && col1 == ColorType.Blue) {
                    if (AbsorbColor) {
                        AbsorbDamage(s1, dam, ColorType.Green);
                        return;
                    } else {
                        dam *= 2.0f;
                    }
                }
                var armor = s1.CurrStats.Armor.HasValue ? s1.CurrStats.Armor.Value : 0.0f;
                var chance = 0.15f * armor / (0.15f * armor + 1);
                if (Random.Range(0.0f, 1.0f) > chance) {
                    TakeDamage(s1, dam, col2);
                } else {
                    AbsorbDamage(s1, dam, ColorType.Green);
                }
            }
        }
    }
    
    private void TakeDamage(StatManager s, float dam, ColorType ct) {
        if (_time >= IFrameDuration) {
            if (s.CurrStats.WhenHitSound != null) AudioManager.Instance.PlaySound(s.CurrStats.WhenHitSound, transform.position);
            if (!s.CurrStats.IsInvincible) {
                _time = 0.0f;
                if (s.CurrStats.Health.HasValue) {
                    s.CurrStats.Health -= dam;
                } else {
                    s.CurrStats.Health = 0;
                }
                if (s.CurrStats.Health.Value <= 0) return;
                var cm = _co.GetEntityComponent(ComponentType.ColorManager) as ColorManager;
                if (cm != null) cm.BlinkColors(ct);
            }
        }
    }
    
    private void AbsorbDamage(StatManager s, float dam, ColorType ct) {
        if (_time >= IFrameDuration) {
            if (s.CurrStats.WhenAbsorbSound != null) AudioManager.Instance.PlaySound(s.CurrStats.WhenAbsorbSound, transform.position);
            if (!s.CurrStats.IsInvincible) {
                _time = 0.0f;
                var cm = _co.GetEntityComponent(ComponentType.ColorManager) as ColorManager;
                if (cm != null) cm.BlinkColors(ct);
                if (s.CurrStats.AbsorbedDamage.HasValue) {
                    s.CurrStats.AbsorbedDamage += dam;
                }
                else {
                    s.CurrStats.AbsorbedDamage = dam;
                }
            }
        }
    }
    
}
