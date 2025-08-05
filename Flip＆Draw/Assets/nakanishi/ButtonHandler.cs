using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button myButton;  // ボタンの参照
    public CoinClickHandler coinClickHandler;  // CoinClickHandler の参照

    private void Start()
    {
        // ボタンのクリックイベントに EnableHandler() を登録
        myButton.onClick.AddListener(coinClickHandler.EnableHandler);
    }
}