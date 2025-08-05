using UnityEngine;
using UnityEngine.UI; // または TextMeshPro を使う場合は using TMPro;

public class TurnDisplay : MonoBehaviour
{
    // ゲームの進行状況を管理する GameDirector スクリプトへの参照
    [SerializeField] private GameDirector _gameDirector;

    // ターン数とプレイヤー情報を表示する UI テキスト
    // TextMeshPro を使う場合は Text を TMP_Text に変更する

    [SerializeField] private Text _turnText;

    void Update()
    {
        // GameDirector と UI テキストが正しく設定されているか確認
        if (_gameDirector != null && _turnText != null)
        {
            // 現在のプレイヤーが「黒」か「白」かを判定

            string player = _gameDirector.IsPlayerTurn() ? "黒" : "白";

            // 現在のターン数を取得
            int turn = _gameDirector.GetCurrentTurn();

            // UI テキストを更新して、ターン数とプレイヤーの番を表示
            _turnText.text = $"ターン: {turn}（{player}の番）";
        }
    }
}
