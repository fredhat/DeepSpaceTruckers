using UnityEngine;

public class RandomSpin : EntityMovement {
    public float SpinSpeed = 0.0f;
    public float SpinDelta = 0.0f;
    private Transform _t;
    private Vector3 _rotAxis;

    protected override void Awake() {
        base.Awake();
        _t = _mm.Target != null ? _mm.Target.transform : transform;
    }

    private void Start() {
        _rotAxis =  _t.up;
        _t.RotateAround(_t.position, _rotAxis, Random.Range(0.0f, 360.0f));
        SpinSpeed = Random.Range(SpinSpeed - SpinDelta, SpinSpeed + SpinDelta);
    }
    
    protected override void OnUpdate() {
        _t.RotateAround(_t.position, _rotAxis, SpinSpeed * Time.deltaTime);
    }
}