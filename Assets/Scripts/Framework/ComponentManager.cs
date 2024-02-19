using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class ComponentManager : MonoBehaviour {
    public EntityType ET = EntityType.None;
    private HashSet<ComponentType> _hasComponent = new HashSet<ComponentType>();
    public bool HasComponent(ComponentType t) => _hasComponent.Contains(t);
    private Dictionary<ComponentType, List<Manageable>> _attachedComponents = new Dictionary<ComponentType, List<Manageable>>();
    private GameObject _go;
    
    private void Awake() {
        _go = gameObject;
        var l = _go.GetComponents<Manageable>();
        foreach (var m in l) {
            _hasComponent.Add(m.Type);
            if(!_attachedComponents.ContainsKey(m.Type)) _attachedComponents[m.Type] = new List<Manageable>();
            _attachedComponents[m.Type].Add(m);
        }
    }
    private void Start() {
        
    }

    private void Update() {
        
    }

    
    public List<Manageable> GetEntityComponents(ComponentType t) {
        if (!HasComponent(t)) return null;
        var c = _attachedComponents[t];
        return c;
    }
    
    public Manageable GetEntityComponent(ComponentType t) {
        var c = GetEntityComponents(t);
        if (c == null || c.Count < 1) return null;
        return c[0];
    }
    
    public void SetAttractor(Attractor a) {
        if (!HasComponent(ComponentType.Attractee)) return;
        var atr = _attachedComponents[ComponentType.Attractee][0] as Attractee;
        if (atr != null) atr.SetAttractor(a);
    }
}

public enum EntityType {
    None = 0,
    Player = 1,
    Obstacle = 2,
    Enemy = 3,
    Projectile = 4,
    Interactable = 5,
    Boss = 6
}

public enum ComponentType {
    ActionManager = 0,
    InputManager = 1,
    ItemManager = 2,
    StatManager = 3,
    ColissionManager = 4,
    ColorManager = 5,
    MovementManager = 6,
    EntityAction = 7,
    EntityMovement = 8,
    Attractee = 9
}