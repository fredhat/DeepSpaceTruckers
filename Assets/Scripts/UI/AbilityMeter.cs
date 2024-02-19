using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMeter : Singleton<AbilityMeter> {
    [SerializeField] private Image _cooldownBarPrimary, _cooldownBarSecondary, _cooldownBarUtility;
    private static Dictionary<ControlTypes, List<float>> _timeDelayDict = new Dictionary<ControlTypes, List<float>> {
        { ControlTypes.Primary, new List<float> {1.0f, 0.0f}},
        { ControlTypes.Secondary, new List<float> {1.0f, 0.0f}},
        { ControlTypes.Utility, new List<float> {1.0f, 0.0f}}
    };
    private static Dictionary<ControlTypes, Image> _imageDict = new Dictionary<ControlTypes, Image>();

    protected override void Awake() {
        base.Awake();
        if (_cooldownBarPrimary != null) _imageDict[ControlTypes.Primary] = _cooldownBarPrimary;
        if (_cooldownBarSecondary != null) _imageDict[ControlTypes.Secondary] = _cooldownBarSecondary;
        if (_cooldownBarUtility != null) _imageDict[ControlTypes.Utility] = _cooldownBarUtility;
    }

    private void Update() {
        foreach (var kv in _imageDict) {
            var fill = Mathf.Max(1.0f - _timeDelayDict[kv.Key][1]/_timeDelayDict[kv.Key][0], 0.0f);
            kv.Value.fillAmount = Mathf.MoveTowards(kv.Value.fillAmount, fill, 10 * Time.deltaTime);
        }
    }

    public static void UpdateCooldown(float d, float t, ControlTypes ct) {
        _timeDelayDict[ct][0] = d;
        _timeDelayDict[ct][1] = t;
    }
}