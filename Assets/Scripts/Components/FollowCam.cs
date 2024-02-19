using UnityEngine;

//[RequireComponent(typeof(SphericalMotion))]
public class FollowCam : MonoBehaviour {
    [SerializeField] private Vector3 _camOffset = new Vector3(0.0f, 0.0f, 20.0f);
    private Transform _t, _pt;

    private void Awake() {
        _t = transform;
    }
    private void Start() {
        _pt = PlayerManager.Instance.Trans;
    }
    
    private void LateUpdate() {
        _t.position = _pt.position + - (_pt.forward * _camOffset.z) + (_pt.up * _camOffset.y);
        _t.LookAt(_pt, _pt.up);
    }
}