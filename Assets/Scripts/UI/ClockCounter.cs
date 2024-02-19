using TMPro;

public class ClockCounter : Singleton<ClockCounter> {
    private const string _clockStr = "{0:00}:{1:00}:{2:000}";
    private static TextMeshProUGUI _txtCom;

    protected override void Awake() {
        base.Awake();
        _txtCom = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        var timeTemp = GameManager.BossTimer;
        var minutes = (int) timeTemp / 60 ;
        var seconds = (int) timeTemp - 60 * minutes;
        var milliseconds = (int) (1000 * (timeTemp - minutes * 60 - seconds));
        _txtCom.text = string.Format(_clockStr, minutes, seconds, milliseconds);
    }
}