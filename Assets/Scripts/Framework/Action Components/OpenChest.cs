using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpenChest : EntityAction {
    public TextMeshProUGUI PopUpText;
    public float PopUpDistance = 50.0f;
    private Transform _t, _pt;
    private bool _isPlayerClose;
    
    private void Start() {
        _t = transform;
        _pt = PlayerManager.Instance.gameObject.transform;
        _isPlayerClose = false;
    }
    
    protected override void Update() {
        base.Update();
        if (_pt == null) return;
        if ((_t.position - _pt.position).sqrMagnitude < PopUpDistance) {
            PopUpText.gameObject.SetActive(true);
            _isPlayerClose = true;
        } else {
            PopUpText.gameObject.SetActive(false);
            _isPlayerClose = false;
        }
    }

    protected override void OnUpdate() {
        if (!_isPlayerClose || GameManager.AccumulatedScrap < 50.0f) return;
        if (PlayerManager.Items == null || _co == null) return;
        var item = Random.Range(0, PlayerManager.Items.Count);
        PlayerManager.Instance.UpdateItems(item);
        GameManager.DestroyEntity(_co, -50.0f);
    }
}