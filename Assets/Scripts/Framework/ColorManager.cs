using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ColorManager : Manageable {
    public GameObject BlinkTarget, ColorTarget, FactionTarget;
    public float blinkDuration = 0.1f;
    public Material RedMaterial, BlueMaterial;
    public bool UseBlinkMesh;
    private static Dictionary<ColorType, Color> _colorDict = new Dictionary<ColorType, Color> {
        { ColorType.None, Color.yellow },
        { ColorType.Red, Color.red },
        { ColorType.Blue, Color.blue },
        { ColorType.Green, Color.green }
    };
    private bool _blinkingColor;
    private float _blinkCompleteTime;
    private Material[] _materialsB, _materialsC, _materialsF;
    private Color[] _originalColorsB, _originalColorsC;
    private Renderer _r;
    
    /*
    protected override void Awake() {
        base.Awake();
        _materialsB = Utils.GetAllMaterials(BlinkTarget != null ? BlinkTarget : gameObject);
        _materialsC = Utils.GetAllMaterials(ColorTarget != null ? ColorTarget : gameObject);
        if (FactionTarget != null) _materialsF = Utils.GetAllMaterials(FactionTarget);
        _originalColorsB = new Color[_materialsB.Length];
        for(var i = 0; i < _materialsB.Length; i++) {
            _originalColorsB[i] = _materialsB[i].color;
        }
        if (BlinkTarget == ColorTarget) {
            _originalColorsC = _originalColorsB;
        } else {
            _originalColorsC = new Color[_materialsC.Length];
            for(var i = 0; i < _materialsC.Length; i++) {
                _originalColorsC[i] = _materialsC[i].color;
            }
        }
    }
    */
    
    private void OnEnable() {
        _materialsB = Utils.GetAllMaterials(BlinkTarget != null ? BlinkTarget : gameObject);
        _materialsC = Utils.GetAllMaterials(ColorTarget != null ? ColorTarget : gameObject);
        if (ColorTarget != null) _r = ColorTarget.GetComponent<Renderer>();
        if (FactionTarget != null) _materialsF = Utils.GetAllMaterials(FactionTarget);
        _originalColorsB = new Color[_materialsB.Length];
        for(var i = 0; i < _materialsB.Length; i++) {
            _originalColorsB[i] = _materialsB[i].color;
        }
        if (BlinkTarget == ColorTarget) {
            _originalColorsC = _originalColorsB;
        } else {
            _originalColorsC = new Color[_materialsC.Length];
            for(var i = 0; i < _materialsC.Length; i++) {
                _originalColorsC[i] = _materialsC[i].color;
            }
        }
    }
    
    private void Update() {
        if (_blinkingColor && Time.time > _blinkCompleteTime) RevertColors();
        if (FactionTarget == null) return;
        if (_co.HasComponent(ComponentType.StatManager)) {
            var s = _co.GetEntityComponent(ComponentType.StatManager) as StatManager;
            if (s != null) {
                if (s.CurrStats.Faction == FactionType.Player)
                    foreach (var m in _materialsF)
                        m.color = Color.white;
                if (s.CurrStats.Faction == FactionType.Enemy)
                    foreach (var m in _materialsF)
                        m.color = Color.black;
            }
        }
    }

    public void SetColors(ColorType c) {
        if (c == ColorType.None) return;
        if (c == ColorType.Blue && _r != null && BlueMaterial != null) {
            _r.material = BlueMaterial;
        } else if (c == ColorType.Red && _r != null && RedMaterial != null) {
            _r.material = RedMaterial;
        } else {
            foreach (var m in _materialsC) {
                var al = m.color.a;
                var co = _colorDict[c];
                co.a = al;
                m.color = co;
            }
        }
        for(var i = 0; i < _materialsC.Length; i++) {
            _originalColorsC[i] = _materialsC[i].color;
        }
    }
    
    public void BlinkColors(ColorType c) {
        foreach (var m in _materialsB) {
            var al = m.color.a;
            var co = _colorDict[c];
            co.a = al;
            m.color = co;
        }
        if (UseBlinkMesh) BlinkTarget.SetActive(true);
        _blinkingColor = true;
        _blinkCompleteTime = Time.time + blinkDuration;
    }
    
    private void RevertColors() {
        if (UseBlinkMesh) BlinkTarget.SetActive(false);
        for(var i = 0; i < _materialsB.Length; i++) _materialsB[i].color = _originalColorsB[i];
        _blinkingColor = false;
    }
}