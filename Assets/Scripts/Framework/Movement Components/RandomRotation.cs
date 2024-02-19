using UnityEngine;

public class RandomRotation : EntityMovement {
    public float RotSpeed = 30.0f;
    public float RotDelta = 0.0f;
    private Transform _t;
    private Vector3 _rotAxis;

    protected override void Awake() {
        base.Awake();
        _t = _mm.Target != null ? _mm.Target.transform : transform;
    }

    private void Start() {
        _rotAxis =  Random.onUnitSphere;
        RotSpeed = Random.Range(RotSpeed - RotDelta, RotSpeed + RotDelta);
    }
    
    protected override void OnUpdate() {
        _t.RotateAround(_t.position, _rotAxis, RotSpeed * Time.deltaTime);
    }
}