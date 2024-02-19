using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class RandomMovement : EntityMovement {
    public float AccMult = 1.0f;
    public float AccBase = 1000.0f;
    public float RotSpeed = 1.0f;
    public float MoveDuration = 3.0f;
    public float MoveDelta = 2.0f;
    public float CollisionCheckDist = 5.0f;
    public float PlayerNoticeDistance = 1600.0f;
    private Rigidbody _r;
    private Transform _t, _pt;
    private float _time, _angle, _move;
    private bool _col;
    
    private void Start() {
        _t = _mm.Target != null ? _mm.Target.transform : transform;
        _pt = PlayerManager.Instance.Trans;
        _r = GetComponent<Rigidbody>();
        _move = Random.Range(MoveDuration - MoveDelta, MoveDuration + MoveDelta);
        _time = 0.0f;
        _angle = 0.0f;
        _col = false;
    }

    protected override void OnUpdate() {
        _time += Time.deltaTime;
        if (_r == null) return;
        if (_co != null) {
            if (_co.HasComponent(ComponentType.StatManager)) {
                var s = _co.GetEntityComponent(ComponentType.StatManager) as StatManager;
                if (s != null) {
                    AccMult = s.CurrStats.AccMult.HasValue ? s.CurrStats.AccMult.Value : AccMult;
                }
            }
        }
        var c = CheckCollision();
        if (c) {
            RecalcMovement();
            _col = true;
        } else if (!_col) {
            var np = (_t.position - _pt.position).sqrMagnitude < PlayerNoticeDistance;
            if (np) {
                var newDirection = Vector3.RotateTowards(_t.position, _t.position - _pt.position, RotSpeed * Time.deltaTime, 0.0f);
                _t.rotation = Quaternion.LookRotation(newDirection, _t.position);
                _r.AddForce(_t.up * (_r.mass * AccBase * AccMult * Time.deltaTime));
                return;
            }
        }
        if (_time > _move) {
            RecalcMovement();
            _col = false;
        }
        var rot = _angle * (c ? RotSpeed * 5 : RotSpeed) * Time.deltaTime;
        _t.RotateAround(_t.position, _t.forward, rot);
        _angle -= rot;
        _r.AddForce(_t.up * (_r.mass * AccBase * AccMult * Time.deltaTime));
    }

    private bool CheckCollision() {
        RaycastHit hit;
        if (_r.SweepTest(_t.up, out hit, CollisionCheckDist)) {
            var col = hit.transform.gameObject;
            if (col != null) {
                var cm = col.GetComponent<ComponentManager>();
                if (cm != null) {
                    return cm.ET == EntityType.Interactable || 
                           cm.ET == EntityType.Obstacle ||
                           cm.ET == EntityType.Player;
                }
            }
        }
        return false;
    }
    
    private void RecalcMovement() {
        _angle = Random.Range(30.0f, 180.0f) * (Random.Range(0, 2) * 2 - 1);
        _time = 0.0f;
        _move = Random.Range(MoveDuration - MoveDelta, MoveDuration + MoveDelta);
    }
}