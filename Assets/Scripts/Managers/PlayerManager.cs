using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ComponentManager))]
public class PlayerManager : Singleton<PlayerManager> {
    [SerializeField] private float _rotMult = 100.0f;
    [SerializeField] private float _accBase = 1000.0f;
    [SerializeField] private float _accMult = 2.0f;
    [SerializeField] private float _maxSpeed = 20.0f;
    [SerializeField] private float _dampening = 5.0f;
    [SerializeField] private GameObject _ship;
    private static readonly float _spawnTime = 0.5f;
    public static PlayerState State { get; private set; }
    public static float Health { get; private set; }
    public static List<float> Items { get; private set; }
    public static Vector3 SpawnPoint;
    private static readonly Dictionary<PlayerState, Action> _playerStateDict = new Dictionary<PlayerState, Action> {
        { PlayerState.Spawning, HandleSpawning },
        { PlayerState.Playing, HandlePlaying },
        { PlayerState.Dead, HandleDead }
    };
    private Transform _t, _st;
    public Transform Trans => _t;
    private Rigidbody _r;
    private ComponentManager _cm;
    private Vector3 _movTarget;
    private float _angle;

    protected override void Awake() {
        base.Awake();
        _t = transform;
        _st = _ship.transform;
        _r = GetComponent<Rigidbody>();
        _cm = GetComponent<ComponentManager>();
    }

    private void Start() {
        UpdatePlayerState(PlayerState.Spawning);
    }
    
    private void OnEnable() {
        InputManager.OnLeftJoystick += MovePlayer;
        InputManager.OnWASD += MovePlayer;
        InputManager.OnRightJoystick += RotatePlayerAxis;
        InputManager.OnMouse += RotatePlayerMouse;
    }

    private void OnDisable() {
        InputManager.OnLeftJoystick -= MovePlayer;
        InputManager.OnWASD -= MovePlayer;
        InputManager.OnRightJoystick -= RotatePlayerAxis;
        InputManager.OnMouse -= RotatePlayerMouse;
    }

    private void Update() {
        if (Instance._cm == null) return;
        var s = Instance._cm.GetEntityComponent(ComponentType.StatManager) as StatManager;
        if (s != null) {
            Health = s.CurrStats.Health.HasValue ? s.CurrStats.Health.Value : 0.0f;
            Items = s.CurrStats.ItemList;
        }
    }
    
    private void FixedUpdate() {
        if (State != PlayerState.Playing) return;
        var vel = _r.velocity;
        if (_cm != null) {
            if (_cm.HasComponent(ComponentType.StatManager)) {
                var s = _cm.GetEntityComponent(ComponentType.StatManager) as StatManager;
                if (s != null) {
                    _maxSpeed = s.CurrStats.MaxSpeed.HasValue ? s.CurrStats.MaxSpeed.Value : _maxSpeed;
                }
            }
        }
        vel = Vector3.ClampMagnitude(vel, _maxSpeed);
        if (!InputManager.IsMoving) {
            vel *= 1 - Time.deltaTime * _dampening;
        }
        _r.velocity = vel;
    }
    
    private void MovePlayer(Vector2 vec) {
        if (State != PlayerState.Playing) return;
        _movTarget = new Vector3(vec.x, vec.y, 0.0f).normalized;
        if (_cm != null) {
            if (_cm.HasComponent(ComponentType.StatManager)) {
                var s = _cm.GetEntityComponent(ComponentType.StatManager) as StatManager;
                if (s != null) {
                    _accMult = s.CurrStats.AccMult.HasValue ? s.CurrStats.AccMult.Value : _accMult;
                }
            }
        }
        _r.AddForce(_t.TransformDirection(_movTarget * (_r.mass * _accBase * _accMult * Time.deltaTime)));
    }
    
    private void RotatePlayerAxis(Vector2 vec) {
        if (State != PlayerState.Playing) return;
        var inputAngle = Extensions.CircularClamp(-Mathf.Atan2(vec.x, vec.y));
        var targetRot = Extensions.ShortestArc(inputAngle, _angle) * _rotMult * Time.deltaTime;
        _st.RotateAround(_t.position, _t.forward, targetRot * Mathf.Rad2Deg);
        _angle = Extensions.CircularClamp(_angle + targetRot);
    }

    private void RotatePlayerMouse(Vector2 vec) {
        if (State != PlayerState.Playing) return;
        var mouseRatioX = Input.mousePosition.x / Screen.width;
        var mouseRatioY = Input.mousePosition.y / Screen.height;
        RotatePlayerAxis(new Vector2(mouseRatioX - 0.5f, mouseRatioY - 0.5f));
    }
    
    public static void UpdatePlayerState(PlayerState newState) {
        if (State == newState) return;
        State = newState;
        _playerStateDict[newState]();
    }

    public void UpdateItems(int i) {
        if (_cm == null || !_cm.HasComponent(ComponentType.StatManager)) return;
        var s = _cm.GetEntityComponent(ComponentType.StatManager) as StatManager;
        if (s != null) {
            Items[i]++;
            s.UpdateStats(i);
        }
    }

    private static void HandleSpawning() {
        SpawnPoint = new Vector3(0.0f, 0.0f, -StageManager.Radius);
        Instance._t.position = SpawnPoint;
        if (Instance._cm != null) {
            var s = Instance._cm.GetEntityComponent(ComponentType.StatManager) as StatManager;
            if (s != null) {
                Instance._maxSpeed = s.CurrStats.MaxSpeed.HasValue ? s.CurrStats.MaxSpeed.Value : Instance._maxSpeed;
                Instance._accMult *= s.CurrStats.AccMult.HasValue ? s.CurrStats.AccMult.Value : 1.0f;
                Health = s.CurrStats.Health.HasValue ? s.CurrStats.Health.Value : 0.0f;
                if (Instance._cm.HasComponent(ComponentType.ColorManager)) {
                    var c = Instance._cm.GetEntityComponent(ComponentType.ColorManager) as ColorManager;
                    if (c != null) c.SetColors(s.CurrStats.Color);
                }
            }
        }
        Instance.StartCoroutine(Extensions.WaitFor(_spawnTime, UpdatePlayerState, PlayerState.Playing));
    }
    
    private static void HandlePlaying() {
        //Debug.Log("Playing");
    }
    
    private static void HandleDead() {
        //Debug.Log("Dead");
    }
    
    public enum PlayerState {
        None = 0,
        Spawning = 1,
        Playing = 2,
        Dead = 3
    }
}