using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Coinオブジェクトをクリックしたときに反転処理を行うスクリプト。
/// また、UIボタンを押すことでランダムなコインまたは横一列のコインを反転させる。
/// </summary>

public class coinretu : MonoBehaviour
{
    // このオブジェクトにアタッチされているCoinコンポーネント
    private Coin _coin;

    // ランダムに1列（同じY座標）のコインをすべて反転させるUIボタン
    [SerializeField] private Button randomFlipButton;
    [SerializeField] private Board _board;

    public GameDirector gameDirector; // GameDirectorを参照
    private int currentUses = 0;        // 現在の使用回数
    private int cooldownRemaining = 0; // クールタイム残りターン数
    private int lastCheckedTurn = -1;  // 最後にターンをチェックしたターン番号

    // シーン上のすべてのcoinretuインスタンスを保持する静的リスト
    private static List<coinretu> allCoins = new List<coinretu>();

    // スクリプトが有効化されたときに呼ばれる初期化処理
    private void Awake()
    {
        // Coinコンポーネントの取得
        _coin = GetComponent<Coin>();

        // このインスタンスを全コインリストに追加
        allCoins.Add(this);

        // ランダム反転ボタンにイベントリスナーを登録
        if (randomFlipButton != null)
        {
            randomFlipButton.onClick.AddListener(FlipRandom);
        }
    }

    // オブジェクトが破棄されたときに、リストから削除
    private void OnDestroy()
    {
        allCoins.Remove(this);
    }

    // Y座標が同じコイン（3~4をランダムで選び、すべて反転させる
    private void FlipRandom()
    {
        // 相手の駒（Whitecoinタグ）をすべて取得
        var opponentCoins = allCoins
         .Where(c => c.gameObject.CompareTag("Whitecoin"))
         .ToList();

        // 対象がいなければ終了
        if (opponentCoins.Count == 0) return;

        // 反転する数をランダムに決定（3〜5個、ただし最大は相手の駒の数）
        int flipCount = Mathf.Min(UnityEngine.Random.Range(3, 6), opponentCoins.Count);

        // ランダムに選んで反転
        var selectedCoins = opponentCoins
     .OrderBy(c => UnityEngine.Random.value)
     .Take(flipCount);

        foreach (var coin in selectedCoins)
        {
            coin._coin.FlipFace();
        }

        // マーカー削除と再配置
        GameDirector director = FindObjectOfType<GameDirector>();
        if (director != null)
        {
            _board.clearEligibleMarkers(); // これが実際のマーカー削除
            // キャッシュクリア
            _board.ClearCachedPoints();
            // 配置可能位置の更新
            _board.GetComponent<Board>().UpdateEligiblePositions(director.IsPlayerTurn() ? CoinFace.black : CoinFace.white);
        }
    }
}