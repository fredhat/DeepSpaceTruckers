using UnityEngine;
using UnityEngine.UI;

public class HealthMeter : Singleton<HealthMeter> {
    [SerializeField] private Image _healthbar;
    private const float _maxHealth = 50.0f;

    private void Update() {
        var fill = Mathf.Min(PlayerManager.Health, _maxHealth) / _maxHealth;
        _healthbar.fillAmount = Mathf.MoveTowards(_healthbar.fillAmount, Mathf.Max(fill, 0.0f), Time.deltaTime);
    }
}