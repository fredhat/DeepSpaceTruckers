using TMPro;

public class MoneyCounter : Singleton<MoneyCounter> {
    private const string _moneyStr = "{0:0}";
    private static TextMeshProUGUI _txtCom;

    protected override void Awake() {
        base.Awake();
        _txtCom = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        var moneyTemp = GameManager.AccumulatedScrap;
        _txtCom.text = string.Format(_moneyStr, moneyTemp);
    }
}