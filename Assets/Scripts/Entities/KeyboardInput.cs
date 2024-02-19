using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class KeyboardInput : MonoBehaviour {
    /*
    public event Action<Vector2> OnWASD;
    public event Action<Vector2> OnMouse;
    private PlayerControls _pc;
    private PlayerInput _pi;
    private Vector2 _mov, _aim;
    
    private void Awake() {
        _pc = new PlayerControls();
        _pi = GetComponent<PlayerInput>();
    }

    private void OnEnable() {
        _pc.Enable();
    }

    private void OnDisable() {
        _pc.Disable();
    }

    private void Update() {
        HandleMovement();
        HandleAiming();
    }
    
    private void HandleMovement() {
        _mov = _pc.Controls.Movement.ReadValue<Vector2>();
        if (_mov.sqrMagnitude > InputManager.Instance._deadzone) {
            InputManager.Instance.IsMoving = true;
            OnWASD?.Invoke(_mov);
        } else {
            InputManager.Instance.IsMoving = false;
        }
    }

    private void HandleAiming() {
        _aim = _pc.Controls.Aim.ReadValue<Vector2>();
        OnMouse?.Invoke(_aim);
    }
    */
}