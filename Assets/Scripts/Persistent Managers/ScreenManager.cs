using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using System.Threading.Tasks; // Enable if testing loading

public class ScreenManager : Singleton<ScreenManager> {
    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    [SerializeField] private AudioClip _startClip, _loseClip, _winClip;
    public static SceneState State { get; private set; }
    private static readonly Dictionary<SceneState, Action> LoadSceneDict = new Dictionary<SceneState, Action> {
        { SceneState.MainMenu, LoadMainMenu },
        { SceneState.MainGame, LoadMainGame },
        { SceneState.GameOver, LoadGameOver },
        { SceneState.GameWon, LoadGameWon }
    };
    private static float _targetAmount;
    private static bool _isLoading;
    private static bool _firstTime = true;
    
    private void Start() => UpdateSceneState(SceneState.MainMenu);
    
    private void OnEnable() {
        InputManager.OnAny += AnyKeyPressed;
    }

    private void OnDisable() {
        InputManager.OnAny -= AnyKeyPressed;
    }
    
    private void Update() {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _targetAmount, 3 * Time.deltaTime);
    }
    
    private void AnyKeyPressed() {
        if (State == SceneState.MainMenu && !_isLoading) UpdateSceneState(SceneState.MainGame);
    }
    
    #pragma warning disable CS1998
    private static async void LoadScene(string sceneName) {
        Instance._progressBar.fillAmount = 0;
        _targetAmount = 0;
        _isLoading = true;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        if (scene is null) return;
        scene.allowSceneActivation = false;
        Instance._loaderCanvas.SetActive(true);
        do {
            //await Task.Delay(100); // Enable to slow down loading for testing purposes
            _targetAmount = scene.progress * 1.2f;
        } while (scene.progress < 0.9f); 
        //await Task.Delay(1000); // Enable to slow down loading for testing purposes
        scene.allowSceneActivation = true;
        Instance._loaderCanvas.SetActive(false);
        _isLoading = false;
    }
    #pragma warning restore CS1998
    
    public static void UpdateSceneState(SceneState newState) {
        if (_isLoading) return;
        if (State == newState) return;
        State = newState;
        LoadSceneDict[newState]();
    }

    private static void LoadMainMenu() {
        AudioManager.Instance.StopMusic();
        if (_firstTime) {
            _firstTime = false;
        } else {
            LoadScene("MainMenu");
        }
    }
    
    private static void LoadMainGame() {
        AudioManager.Instance.PlayMusic();
        if (Instance._startClip != null) AudioManager.Instance.PlaySound(Instance._startClip);
        LoadScene("MainGame");
    }
    
    private static void LoadGameOver() {
        AudioManager.Instance.StopMusic();
        if (Instance._loseClip  != null) AudioManager.Instance.PlaySound(Instance._loseClip);
        LoadScene("GameOver");
    }
    
    private static void LoadGameWon() {
        AudioManager.Instance.StopMusic();
        if (Instance._winClip  != null) AudioManager.Instance.PlaySound(Instance._winClip);
        LoadScene("GameWon");
    }
}

public enum SceneState {
    None = 0,
    MainMenu = 1,
    MainGame = 2,
    GameOver = 3,
    GameWon = 4
}