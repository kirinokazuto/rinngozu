using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// ����̃{�^�����E�N���b�N����Ǝw�肳�ꂽ�摜��\������X�N���v�g�B
/// ���̃X�N���v�g�̓{�^����GameObject�ɃA�^�b�`���Ă��������B
/// </summary>
public class cardtextcheck : MonoBehaviour, IPointerClickHandler
{
    UnityEngine.UI.Image myImage; // Unity��Image���g���ꍇ

    // �\����������Image�iInspector�Őݒ�j
    public Image targetImage;

    public Image targetImage2;

    public Image targetImage3;

    public Image targetImage4;  

    // �}�E�X�N���b�N�C�x���g����

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (targetImage != null)
            {
                // targetImage ��\���E��\���؂�ւ�
                targetImage.gameObject.SetActive(!targetImage.gameObject.activeSelf);

                // ���� Image ���\���ɂ���
                if (targetImage2 != null)
                    targetImage2.gameObject.SetActive(false);

                if (targetImage3 != null)
                    targetImage3.gameObject.SetActive(false);

                if (targetImage4 != null)
                    targetImage4.gameObject.SetActive(false);

            }

        }
    }
}