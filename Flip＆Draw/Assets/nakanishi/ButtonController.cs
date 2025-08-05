using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button myButton;
    public Image buttonImage;
    public Color pressedColor = Color.gray; // ��������̐F

    void Start()
    {
        myButton.onClick.AddListener(ChangeButtonState);
    }

    void ChangeButtonState()
    {
        myButton.interactable = false; // �{�^���𖳌���
        buttonImage.color = pressedColor; // �F��ύX
    }
}