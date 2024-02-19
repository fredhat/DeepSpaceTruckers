using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForwardMovement : EntityMovement {
    public float AccMult = 1.0f;
    public float AccBase = 1000.0f;
    private Rigidbody _r;
    private Transform _t;
    
    private void Start() {
        _t = _mm.Target != null ? _mm.Target.transform : transform;
        _r = GetComponent<Rigidbody>();
    }
    
    protected override void OnUpdate() {
        if (_r == null) return;
        if (_co != null) {
            if (_co.HasComponent(ComponentType.StatManager)) {
                var s = _co.GetEntityComponent(ComponentType.StatManager) as StatManager;
                if (s != null) {
                    AccMult = s.CurrStats.AccMult.HasValue ? s.CurrStats.AccMult.Value : AccMult;
                }
            }
        }
        _r.AddForce(_t.up * (_r.mass * AccBase * AccMult * Time.deltaTime));
    }
}