using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Coinオブジェクトをクリックしたときに反転処理を行うスクリプト。
/// また、UIボタンを押すことでランダムなコインまたは横一列のコインを反転させる。
/// </summary>

public class Random_White : MonoBehaviour
{
    // このオブジェクトにアタッチされているCoinコンポーネント
    private Coin _coin;

    // ランダムに1列（同じY座標）のコインをすべて反転させるUIボタン
    [SerializeField] private Button randomFlipButton;

    [SerializeField] private Board _board;

    public GameDirector gameDirector; // GameDirectorを参照

    // シーン上のすべてのcoinretuインスタンスを保持する静的リスト
    private static List<Random_White> allCoins = new List<Random_White>();

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
        // 自分のコマ（Blackcoinタグ）を取得
        var myCoins = allCoins
            .Where(c => c.gameObject.CompareTag("Blackcoin"))
            .ToList();

        // 相手のコマ（Whitecoinタグ）を取得
        var opponentCoins = allCoins
            .Where(c => c.gameObject.CompareTag("Whitecoin"))
            .ToList();

        // ランダムで反転させる数（3〜5個）
        int myFlipCount = Mathf.Min(UnityEngine.Random.Range(3, 6), myCoins.Count);
        int opponentFlipCount = Mathf.Min(UnityEngine.Random.Range(3, 6), opponentCoins.Count);

        // ランダムに選んで反転（Blackcoin → Whitecoin）
        var selectedMyCoins = myCoins
            .OrderBy(c => UnityEngine.Random.value)
            .Take(myFlipCount);

        foreach (var coin in selectedMyCoins)
        {
            coin._coin.FlipFace(); // 自分の駒を相手の色に
        }

        // ランダムに選んで反転（Whitecoin → Blackcoin）
        var selectedOpponentCoins = opponentCoins
            .OrderBy(c => UnityEngine.Random.value)
            .Take(opponentFlipCount);

        foreach (var coin in selectedOpponentCoins)
        {
            coin._coin.FlipFace(); // 相手の駒を自分の色に
        }

        // マーカー削除と再配置
        GameDirector director = FindObjectOfType<GameDirector>();
        if (director != null)
        {
            _board.clearEligibleMarkers(); // これが実際のマーカー削除
            _board.ClearCachedPoints();    // キャッシュもクリア

            _board.UpdateEligiblePositions(
                director.IsPlayerTurn() ? CoinFace.black : CoinFace.white
            );

        }
    }
}