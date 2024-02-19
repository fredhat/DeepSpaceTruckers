using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemBar : Singleton<ItemBar> {
    public List<TextMeshProUGUI> ItemCounts = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> ItemPopups = new List<TextMeshProUGUI>();
    public float DisplayDuration = 1.0f;
    private List<float> ItemPopupTimes = new List<float>();
    private const string _numStr = "{0:0}";

    protected override void Awake() {
        base.Awake();
        if (ItemPopups.Count < 1) return;
        foreach (var i in ItemPopups) {
            ItemPopupTimes.Add(0.0f);
        }
    }

    private void Update() {
        var itemsTemp = PlayerManager.Items;
        if (itemsTemp == null) return;
        for (var i = 0; i < Mathf.Min(ItemCounts.Count, itemsTemp.Count); i++) {
            ItemCounts[i].text = string.Format(_numStr, itemsTemp[i]);
        }
        for (var i = 0; i < ItemPopupTimes.Count; i++) {
            if (ItemPopupTimes[i] > 0.0f) {
                ItemPopupTimes[i] -= Time.deltaTime;
            }
            ItemPopups[i].gameObject.SetActive(ItemPopupTimes[i] > 0.0f);
        }
    }

    public void TriggerPopup(int i) {
        if (i >= ItemPopups.Count) return;
        ItemPopups[i].gameObject.SetActive(true);
        ItemPopupTimes[i] = DisplayDuration;
    }
}