using System.Collections;
using UnityEngine;

public class GameWonManager : Singleton<GameWonManager> {
    private static float _delay = 1.0f;
    
    private void OnEnable() {
        StartCoroutine(KeyWait());
    }
    
    private void OnDisable() {
        InputManager.OnAny -= AnyKeyPressed;
    }
    
    private void AnyKeyPressed() {
        ScreenManager.UpdateSceneState(SceneState.MainGame);
    }

    private IEnumerator KeyWait() {
        yield return new WaitForSeconds(_delay);
        InputManager.OnAny += AnyKeyPressed;
    }
}