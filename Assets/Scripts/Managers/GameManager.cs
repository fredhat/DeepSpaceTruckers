using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager> {
    [SerializeField] public float SpawnInterval = 0.1f;
    [SerializeField] public float BossInterval = 300f;
    public static GameState State { get; private set; }
    public static float AccumulatedScrap { get; private set; }
    public static float BossTimer { get; private set; }
    public static readonly float EnemySpawnRate = 0.1f;
    private static readonly Dictionary<GameState, Action> _stateDict = new Dictionary<GameState, Action> {
        { GameState.Init, HandleInit },
        { GameState.Spawning, HandleSpawning },
        { GameState.Boss, HandleBoss }
    };
    private static Dictionary<EntityType, HashSet<ComponentManager>> _spawnedEntityDict = new Dictionary<EntityType, HashSet<ComponentManager>> {
        { EntityType.Obstacle, new HashSet<ComponentManager>() },
        { EntityType.Projectile, new HashSet<ComponentManager>() },
        { EntityType.Enemy, new HashSet<ComponentManager>() },
        { EntityType.Interactable, new HashSet<ComponentManager>() },
        { EntityType.Boss, new HashSet<ComponentManager>() }
    };
    private static readonly List<ComponentManager> _projectilePool = new List<ComponentManager>(_projectilePoolSize);
    public static HashSet<ComponentManager> Enemies => _spawnedEntityDict[EntityType.Enemy];
    private static readonly int _projectilePoolSize = 1000;
    private static float _spawnTimer;

    private void Start() {
        UpdateGameState(GameState.Init);
    }

    private void Update() {
        BossTimer -= Time.deltaTime;
        if (BossTimer < 0) BossTimer = 0.0f;
        _spawnTimer += Time.deltaTime;
        if (BossTimer == 0.0f) UpdateGameState(GameState.Boss);
    }
    
    public static void UpdateGameState(GameState newState) {
        if (State == newState) return;
        State = newState;
        _stateDict[newState]();
    }
    
    private static void PoolProjectiles() {
        _projectilePool.Clear();
        var v0 = Vector3.zero;
        for (var i = 0; i < _projectilePoolSize; i++) {
            var s = SpawnEntity(EntityType.Projectile, v0);
            s.gameObject.SetActive(false);
        }
    }
    
    public static ComponentManager GetProjectile() {
        ComponentManager proj;
        if (_projectilePool.Count > 0) {
            proj = _projectilePool[_projectilePool.Count - 1];
            proj.gameObject.SetActive(true);
            _projectilePool.RemoveAt(_projectilePool.Count - 1);
        } else {
            proj = SpawnEntity(EntityType.Projectile, Vector3.zero);
        }
        return proj;
    }
    
    public static void ReturnProjectile(ComponentManager proj) {
        if (proj.HasComponent(ComponentType.StatManager)) {
            var s = proj.GetEntityComponent(ComponentType.StatManager) as StatManager;
            if (s != null) s.Reset();
        }
        proj.gameObject.SetActive(false);
        _projectilePool.Add(proj);
    }
    
    public static ComponentManager SpawnEntity(EntityType t, Vector3 pos) {
        var scriptableEntity = EntityManager.GetRandomEntity(t);
        var spawnedEntity = Instantiate(scriptableEntity.Prefab);
        spawnedEntity.transform.position = pos;
        spawnedEntity.SetAttractor(StageManager.SkyBox);
        if (spawnedEntity.HasComponent(ComponentType.StatManager)) {
            var s = spawnedEntity.GetEntityComponent(ComponentType.StatManager) as StatManager;
            if (s != null) {
                s.SetStats(scriptableEntity.BaseStats);
                if (spawnedEntity.HasComponent(ComponentType.ColorManager)) {
                    var c = spawnedEntity.GetEntityComponent(ComponentType.ColorManager) as ColorManager;
                    if (c != null) c.SetColors(s.CurrStats.Color);
                }
            }
        }
        var hSet = _spawnedEntityDict[t];
        hSet?.Add(spawnedEntity);
        return spawnedEntity;
    }
    
    public static void DestroyEntity(ComponentManager e, float value) {
        if (e.ET == EntityType.Player) {
            ScreenManager.UpdateSceneState(SceneState.GameOver);
        } else if (e.ET == EntityType.Boss) {
            ScreenManager.UpdateSceneState(SceneState.GameWon);
        } else {
            var hSet = _spawnedEntityDict.TryGetValue(e.ET, out var value1) ? value1 : null;
            if (hSet == null) return;
            hSet.Remove(e);
            if (value != 0) AccumulatedScrap += value;
            if (e.HasComponent(ComponentType.StatManager)) {
                var sm = e.GetEntityComponent(ComponentType.StatManager) as StatManager;
                if (sm != null) {
                    if (sm.CurrStats.WhenDieSound != null) AudioManager.Instance.PlaySound(sm.CurrStats.WhenDieSound, e.transform.position);
                }
            }
            if (e.ET == EntityType.Projectile) {
                ReturnProjectile(e);
            } else {
                Destroy(e.gameObject);
            }
        }
    }

    private static Vector3 GetSpawnLoc() {
        var spawnLoc = StageManager.GetRandomSpawnPoint();
        while (Physics.CheckSphere(spawnLoc, 5.0f)) {
            spawnLoc = StageManager.GetRandomSpawnPoint();
        }
        return spawnLoc;
    }
    
    private static void HandleInit() {
        BossTimer = Instance.BossInterval;
        _spawnTimer = 0.0f;
        for (var i = 0; i < StageManager.GetRandomNumObstacles(); i++) _ = SpawnEntity(EntityType.Obstacle, GetSpawnLoc());
        for (var i = 0; i < StageManager.GetRandomNumChests(); i++) _ = SpawnEntity(EntityType.Interactable, GetSpawnLoc());
        PoolProjectiles();
        UpdateGameState(GameState.Spawning);
    }
    
    private static void HandleSpawning() {
        SpawnEnemy(EntityType.Enemy);
    }

    private static void SpawnEnemy(EntityType t) {
        var v = GetSpawnLoc();
        _ = SpawnEntity(EntityType.Enemy, v);
        if (State == GameState.Spawning) Instance.StartCoroutine(Extensions.WaitFor(1.0f / Instance.SpawnInterval, SpawnEnemy, EntityType.Enemy));
    }
    
    private static void HandleBoss() {
        var v = GetSpawnLoc();
        _ = SpawnEntity(EntityType.Boss, v);
    }
}

public enum GameState {
    None = 0,
    Init = 1,
    Spawning = 2,
    Boss = 3
}