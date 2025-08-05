using UnityEngine;

public class PieceSelector : MonoBehaviour
{
    private bool isSelecting = false; // �R�}�I�����[�h�t���O
    private GameObject selectedPiece; // �I�����ꂽ�R�}
    public Sprite playerPieceSprite; // �ύX����X�v���C�g�iInspector�Őݒ�j

    // �{�^������������I�����[�h�ɓ���
    public void StartSelectionMode()
    {
        isSelecting = true;
    }

    void Update()
    {
        // �I�����[�h���ɃN���b�N���ꂽ�I�u�W�F�N�g���m�F
        if (isSelecting && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Piece")) // "Piece"�^�O�t���̃I�u�W�F�N�g�̂ݑΏ�
            {
                selectedPiece = hit.collider.gameObject;
                ChangePieceSprite(selectedPiece); // �X�v���C�g�ύX����
                isSelecting = false; // �I�����[�h����
            }
        }
    }

    // �I�������R�}�̃X�v���C�g��ύX
    public void ChangePieceSprite(GameObject piece)
    {
        if (piece != null && playerPieceSprite != null)
        {
            piece.GetComponent<SpriteRenderer>().sprite = playerPieceSprite;
        }
    }
}