using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputManager : Singleton<InputManager> {
    [SerializeField] public float _deadzone = 0.1f;
    public static event Action<Vector2> OnLeftJoystick;
    public static event Action<Vector2> OnRightJoystick;
    public static event Action<Vector2> OnWASD;
    public static event Action<Vector2> OnMouse;
    public static event Action OnAny;
    public static event Action OnPrimary;
    public static event Action OnSecondary;
    public static event Action OnUtility;
    public static event Action OnSuper;
    public static event Action OnInteract;
    public static event Action<ControlScheme> OnInputSchemeChanged;
    public static ControlScheme CurrentControlScheme { get; private set; }
    public InputActionAsset InputActions;
    public InputUser User;
    public static bool IsMoving;
    private PlayerControls _pc;
    private Vector2 _mov, _aim;
    private bool _any, _pa, _sa, _ua, _xa, _ia;

    protected override void Awake() {
        base.Awake();
        IsMoving = false;
        _pc = new PlayerControls();
    }
    
    private void Start() => StartAutoControlSchemeSwitching();
    
    private void OnDestroy() => StopAutoControlSchemeSwitching();
    
    private void OnEnable() {
        _pc.Enable();
    }

    private void OnDisable() {
        _pc.Disable();
    }

    private void Update() {
        HandleMovement();
        HandleAiming();
        HandleActions();
        HandleAny();
    }
    
    private void HandleMovement() {
        _mov = _pc.Controls.Movement.ReadValue<Vector2>();
        if (_mov.sqrMagnitude > 0.0f) {
            IsMoving = true;
            if (CurrentControlScheme == ControlScheme.Gamepad) {
                //Debug.Log("Move Gamepad");
                OnLeftJoystick?.Invoke(_mov);
            } else {
                //Debug.Log("Move Keyboard");
                OnWASD?.Invoke(_mov);
            }
        } else {
            IsMoving = false;
        }
    }

    private void HandleAiming() {
        _aim = _pc.Controls.Aim.ReadValue<Vector2>();
        if (CurrentControlScheme == ControlScheme.Gamepad) {
            if (Mathf.Abs(_aim.x) > _deadzone || Mathf.Abs(_aim.y) > _deadzone) {
                OnRightJoystick?.Invoke(_aim);
                //Debug.Log("Aim Gamepad");
            }
        } else {
            OnMouse?.Invoke(_aim);
            //Debug.Log("Aim Keyboard");
        }
    }
    
    private void HandleActions() {
        _pa = _pc.Controls.PrimaryAction.IsPressed();
        _sa = _pc.Controls.SecondaryAction.IsPressed();
        _ua = _pc.Controls.UtilityAction.IsPressed();
        _xa = _pc.Controls.SuperAction.IsPressed();
        _ia = _pc.Controls.InteractAction.IsPressed();
        if (_pa) OnPrimary?.Invoke();
        if (_sa) OnSecondary?.Invoke();
        if (_ua) OnUtility?.Invoke();
        if (_xa) OnSuper?.Invoke();
        if (_ia) OnInteract?.Invoke();
    }
    
    private void HandleAny() {
        _any = _pc.Controls.Any.IsPressed();
        if (_any) OnAny?.Invoke();
    }
   
    void StartAutoControlSchemeSwitching() {
        User = InputUser.CreateUserWithoutPairedDevices();
        User.AssociateActionsWithUser(InputActions.actionMaps[0]);
        ++InputUser.listenForUnpairedDeviceActivity;
        InputUser.onUnpairedDeviceUsed += InputUser_onUnpairedDeviceUsed;
        User.UnpairDevices();
    }
    private void StopAutoControlSchemeSwitching() {
        InputUser.onUnpairedDeviceUsed -= InputUser_onUnpairedDeviceUsed;      
        if (InputUser.listenForUnpairedDeviceActivity>0)
            --InputUser.listenForUnpairedDeviceActivity;      
        User.UnpairDevicesAndRemoveUser();
    }  
 
    private void InputUser_onUnpairedDeviceUsed(InputControl ctrl, UnityEngine.InputSystem.LowLevel.InputEventPtr eventPtr) {
        var device = ctrl.device;
        if (CurrentControlScheme == ControlScheme.Keyboard && (device is Pointer || device is Keyboard)) {
            InputUser.PerformPairingWithDevice(device, User);
            if (OnInputSchemeChanged != null) OnInputSchemeChanged(ControlScheme.Keyboard);
            SetUserControlScheme(ControlScheme.Keyboard);
            return;
        }
        if (device is Gamepad) {
            if (OnInputSchemeChanged != null) OnInputSchemeChanged(ControlScheme.Gamepad);
            CurrentControlScheme = ControlScheme.Gamepad;
            SetUserControlScheme(ControlScheme.Gamepad);
        } else if (device is Keyboard || device is Pointer) {
            if (OnInputSchemeChanged != null) OnInputSchemeChanged(ControlScheme.Keyboard);
            CurrentControlScheme = ControlScheme.Keyboard;
            SetUserControlScheme(ControlScheme.Keyboard);
        } else return;
        User.UnpairDevices();
        InputUser.PerformPairingWithDevice(device, User);
    }
    public void SetUserControlScheme(ControlScheme scheme) {
        User.ActivateControlScheme(InputActions.controlSchemes[(int)scheme]);
    }
    public enum ControlScheme {
        Gamepad = 0,
        Keyboard = 1
    }
}

public enum ControlTypes {
    None = 0,
    Primary = 1,
    Secondary = 2,
    Utility = 3,
    Super = 4,
    Interact = 5
}