using UnityEngine;

public class PieceSelector : MonoBehaviour
{
    private bool isSelecting = false; // コマ選択モードフラグ
    private GameObject selectedPiece; // 選択されたコマ
    public Sprite playerPieceSprite; // 変更するスプライト（Inspectorで設定）

    // ボタンを押したら選択モードに入る
    public void StartSelectionMode()
    {
        isSelecting = true;
    }

    void Update()
    {
        // 選択モード中にクリックされたオブジェクトを確認
        if (isSelecting && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Piece")) // "Piece"タグ付きのオブジェクトのみ対象
            {
                selectedPiece = hit.collider.gameObject;
                ChangePieceSprite(selectedPiece); // スプライト変更処理
                isSelecting = false; // 選択モード解除
            }
        }
    }

    // 選択したコマのスプライトを変更
    public void ChangePieceSprite(GameObject piece)
    {
        if (piece != null && playerPieceSprite != null)
        {
            piece.GetComponent<SpriteRenderer>().sprite = playerPieceSprite;
        }
    }
}