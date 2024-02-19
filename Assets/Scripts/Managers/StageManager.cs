using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageManager : Singleton<StageManager> {
    [SerializeField] public Vector3 SpawnPoint = Vector3.zero;
    public static StageState State { get; private set; }
    private static readonly Dictionary<StageState, Action> StageDict = new Dictionary<StageState, Action> {
        { StageState.Init, HandleInit },
        { StageState.Playing, HandlePlaying }
    };
    public static Attractor SkyBox { get; private set; }
    public static event Action<Attractor> OnAttractorChanged;
    public static float Radius;
    private static ScriptableStage _stage;
    
    protected override void Awake() {
        base.Awake();
        _stage = EntityManager.GetRandomStage();
        Radius = _stage.BaseStats.Radius;
    }

    private void Start() {
        UpdateStageState(StageState.Init);
    }
    
    public static void UpdateStageState(StageState newState) {
        if (State == newState) return;
        State = newState;
        StageDict[newState]();
    }
    
    private static void HandleInit() {
        if (!(SkyBox is null)) Destroy(SkyBox);
        SkyBox = Instantiate(_stage.Prefab);
        SkyBox.SetStats(_stage.BaseStats);
        SkyBox.transform.position = Instance.SpawnPoint;
        OnAttractorChanged?.Invoke(SkyBox);
        UpdateStageState(StageState.Playing);
    }
    
    private static void HandlePlaying() {
        // Insert logic if necessary
    }

    public static Vector3 GetRandomSpawnPoint() => Random.onUnitSphere * _stage.BaseStats.Radius;
    
    public static int GetRandomNumObstacles() => Random.Range(_stage.BaseStats.MinNumObstacles, _stage.BaseStats.MaxNumObstacles);
    
    public static int GetRandomNumChests() => Random.Range(_stage.BaseStats.MinNumChests, _stage.BaseStats.MaxNumChests);
}

public enum StageState {
    None = 0,
    Init = 1,
    Playing = 2
}