using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : Singleton<EntityManager> {
    private static List<ScriptableStage> _stages;
    private static List<ScriptableEntity> _entities;
    private static Dictionary<EntityType, List<ScriptableEntity>> _entityDict = new Dictionary<EntityType, List<ScriptableEntity>> {
        { EntityType.None, new List<ScriptableEntity>() },
        { EntityType.Obstacle, new List<ScriptableEntity>() },
        { EntityType.Projectile, new List<ScriptableEntity>() },
        { EntityType.Enemy, new List<ScriptableEntity>() },
        { EntityType.Interactable, new List<ScriptableEntity>() },
        { EntityType.Boss, new List<ScriptableEntity>() }
    };

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    private static void AssembleResources() {
        _stages = Resources.LoadAll<ScriptableStage>("Stages").ToList();
        _entities = Resources.LoadAll<ScriptableEntity>("Entities").ToList();
        foreach (var e in _entities.Where(e => _entityDict.ContainsKey(e.ET))) _entityDict[e.ET].Add(e);
    }
    
    public static List<ScriptableStage> GetStages() => _stages;
    public static ScriptableStage GetRandomStage() => _stages.Rand();
    public static List<ScriptableEntity> GetEntities() => _entities;
    public static ScriptableEntity GetRandomEntity() => _entities.Rand();
    public static ScriptableEntity GetRandomEntity(EntityType e) => _entityDict[e].Rand();
}