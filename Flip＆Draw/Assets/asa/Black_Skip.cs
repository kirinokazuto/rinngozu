using UnityEngine;

public class Black_Skip : MonoBehaviour
{
    [SerializeField] private GameDirector gameDirector;
    [SerializeField] private Board board; // Boardの参照を追加

    private bool skillUsed = false;
    private bool pendingOverride = false;
    // スキルによるターン強奪が現在有効かどうかを示すフラグ
    private bool skillInEffect = false;
    private bool wasPlayerTurn = false;

    void Update()
    {
        if (gameDirector.IsGameOver()) return;

        bool currentTurn = gameDirector.IsPlayerTurn();

        if (wasPlayerTurn && !currentTurn && pendingOverride)
        {
            Debug.Log("スキル発動！強制的に自分のターンに戻します");
            ForceTurnBack();
            pendingOverride = false;

            // ⭐ マーカー更新処理を追加
            UpdateMarkers();
        }

        wasPlayerTurn = currentTurn;
    }

    public void ActivateSkill()
    {
        if (!gameDirector.IsPlayerTurn())
        {
            Debug.Log("自分のターン以外はスキルが使えません");
            return;
        }

        Debug.Log("スキップスキル使用予約！");
        pendingOverride = true;
    }


    public void ForceTurnBack()
    {
        var selectorField = typeof(GameDirector).GetField("_playerSelector", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (selectorField != null)
        {
            bool current = (bool)selectorField.GetValue(gameDirector);
            selectorField.SetValue(gameDirector, !current); // ターンを戻す

            Debug.Log($"ターンを強制的に戻しました。現在のプレイヤー: {gameDirector.getFace()}");

            bool hasMoves = board.UpdateEligiblePositions(gameDirector.getFace());
            Debug.Log($"合法手チェック結果: {hasMoves}");

            if (!hasMoves)
            {
                Debug.Log("スキルで戻したが合法手がなかったため相手にスキップ！");
                selectorField.SetValue(gameDirector, current); // 元に戻す（相手のターンに）
                gameDirector.SetForceTurnOverride(false);
            }
            else
            {
                gameDirector.SetForceTurnOverride(true);
            }

            UpdateMarkers();
        }
        else
        {
            Debug.LogError("GameDirectorの_playerSelectorにアクセスできません");
        }
    }




    private void UpdateMarkers()
    {
        // 既存のマーカーを削除
        gameDirector.ClearMarkers();

        // キャッシュクリア（privateフィールドなのでリフレクション利用）
        ClearCachedEligiblePositions();

        // 合法手を再計算 → マーカー描画もされるはず
        bool success = board.UpdateEligiblePositions(gameDirector.getFace());

        Debug.Log(success ? "マーカーを再生成しました" : "合法手なし");
    }

    private void ClearCachedEligiblePositions()
    {
        var blackPointsField = typeof(Board).GetField("_cachedBlackPoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var whitePointsField = typeof(Board).GetField("_cachedWhitePoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (blackPointsField != null)
            blackPointsField.SetValue(board, null);
        if (whitePointsField != null)
            whitePointsField.SetValue(board, null);
    }

}
