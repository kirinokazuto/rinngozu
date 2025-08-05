using UnityEngine;

public class White_Skip : MonoBehaviour
{
    [SerializeField] private GameDirector gameDirector;
    [SerializeField] private Board board;

    private bool pendingOverride = false;
    private bool wasPlayerTurn = false;

    void Update()
    {
        if (gameDirector.IsGameOver()) return;

        bool currentTurn = !gameDirector.IsPlayerTurn(); // 白ターン判定は黒の逆なのでこうします

        if (wasPlayerTurn && !currentTurn && pendingOverride)
        {
            Debug.Log("白側スキル発動！強制的に自分のターンに戻します");
            ForceTurnBack();
            pendingOverride = false;

            UpdateMarkers();
        }

        wasPlayerTurn = currentTurn;
    }

    public void ActivateSkill()
    {
        if (!(!gameDirector.IsPlayerTurn())) // 自分のターン＝白ターンかどうか
        {
            Debug.Log("自分のターン以外はスキルが使えません");
            return;
        }

        Debug.Log("白側スキップスキル使用予約！");
        pendingOverride = true;
    }

    private void ForceTurnBack()
    {
        var selectorField = typeof(GameDirector).GetField("_playerSelector", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (selectorField != null)
        {
            bool current = (bool)selectorField.GetValue(gameDirector);
            selectorField.SetValue(gameDirector, !current);
            Debug.Log("白側ターンが強制的に戻されました！");
        }
        else
        {
            Debug.LogError("GameDirectorの_playerSelectorにアクセスできません");
        }
    }

    private void UpdateMarkers()
    {
        gameDirector.ClearMarkers();

        ClearCachedEligiblePositions();

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
