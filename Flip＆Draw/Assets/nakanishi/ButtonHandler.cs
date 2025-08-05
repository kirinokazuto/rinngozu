using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button myButton;  // �{�^���̎Q��
    public CoinClickHandler coinClickHandler;  // CoinClickHandler �̎Q��

    private void Start()
    {
        // �{�^���̃N���b�N�C�x���g�� EnableHandler() ��o�^
        myButton.onClick.AddListener(coinClickHandler.EnableHandler);
    }
}