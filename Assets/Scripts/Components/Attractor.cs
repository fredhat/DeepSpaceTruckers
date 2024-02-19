using UnityEngine;

public class Attractor : MonoBehaviour {
    private StageStats _stats;
    private Transform _t;
    
    private void Awake() {
        _t = transform;
    }
    
    public void Attract(Transform t, Rigidbody r) {
        var gravityDir = (t.position - _t.position).normalized;
        var bodyUp = -t.forward;
        var rot = t.rotation;
        var orbitDist = t.position.sqrMagnitude - Mathf.Pow(_stats.Radius, 2);
        if (orbitDist > 0.0f) {
            r.AddForce(gravityDir * (r.mass * _stats.Gravity));
        } else {
            r.AddForce(-gravityDir * (r.mass * _stats.Gravity));
        }
        var targetRot = Quaternion.FromToRotation(bodyUp, gravityDir) * rot;
        t.rotation = Quaternion.Slerp(rot, targetRot, _stats.RotSpeed * Time.deltaTime);
    }
    
    public void SetStats(StageStats stats) => _stats = stats;
}