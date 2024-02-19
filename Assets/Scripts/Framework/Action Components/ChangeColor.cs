using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : EntityAction {
    protected override void OnUpdate() {
        if (_time < Delay) return;
        _time = 0.0f;
        if (_co == null) return;
        if (_co.HasComponent(ComponentType.StatManager)) {
            var s = _co.GetEntityComponent(ComponentType.StatManager) as StatManager;
            if (s != null) {
                var c = s.CurrStats.Color;
                var nc = ColorType.None;
                if (c == ColorType.Blue) nc = ColorType.Red;
                if (c == ColorType.Red) nc = ColorType.Blue;
                if (_co.HasComponent(ComponentType.ColorManager)) {
                    var cm = _co.GetEntityComponent(ComponentType.ColorManager) as ColorManager;
                    if (cm != null) cm.SetColors(nc);
                }
                s.CurrStats.Color = nc;
            }
        }
    }
}