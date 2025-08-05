using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button myButton;
    public Image buttonImage;
    public Color pressedColor = Color.gray; // 押した後の色

    void Start()
    {
        myButton.onClick.AddListener(ChangeButtonState);
    }

    void ChangeButtonState()
    {
        myButton.interactable = false; // ボタンを無効化
        buttonImage.color = pressedColor; // 色を変更
    }
}