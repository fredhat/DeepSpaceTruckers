using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DampenMovement : EntityMovement {
    public float Dampening, MaxSpeed;
    private Rigidbody _r;
    
    private void Start() {
        _r = GetComponent<Rigidbody>();
    }
    
    protected override void OnFixedUpdate() {
        if (_r == null) return;
        if (_co != null) {
            if (_co.HasComponent(ComponentType.StatManager)) {
                var s = _co.GetEntityComponent(ComponentType.StatManager) as StatManager;
                if (s != null) {
                    MaxSpeed = s.CurrStats.MaxSpeed.HasValue ? s.CurrStats.MaxSpeed.Value : MaxSpeed;
                }
            }
        }
        var vel = _r.velocity;
        vel = Vector3.ClampMagnitude(vel, MaxSpeed);
        vel *= 1 - Time.deltaTime * Dampening;
        _r.velocity = vel;
    }
}