using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


/// <summary>
/// ゲームの終了時に背景のフェードインを制御し、ゲーム結果（勝者または引き分け）を表示するクラス。
/// また、再スタートやメインメニューに遷移するボタンの表示も行う。
/// </summary>
public class BackgroundFadeControl : MonoBehaviour
{
    // ----------------------
    // フィールド
    // ----------------------

    // ゲームの進行を管理する GameDirector
    [SerializeField] private GameDirector _gameDirector;

    // 盤面の管理を行う Board
    [SerializeField] private Board _board;

    // 終了画面で表示する背景オブジェクト（黒・白・グレーの背景）
    [SerializeField] private GameObject _whiteBackground;
    [SerializeField] private GameObject _blackBackground;
    [SerializeField] private GameObject _greyBackground;

    // ゲーム終了後に表示するボタン
    [SerializeField] private GameObject _whiteButton;
    [SerializeField] private GameObject _blackButton;
    [SerializeField] private GameObject _greyButton;

    // 終了処理が一度だけ実行されるようにするフラグ
    private bool _hasTriggered = false;

    // ----------------------
    // パブリックメソッド
    // ----------------------

    /// <summary>
    /// ゲームを再スタートさせる（現在のシーンを再読み込み）。
    /// </summary>
    public void RestartLevel() => SceneManager.LoadSceneAsync("VSPlayer");

    /// <summary>
    /// メインメニューに遷移する。
    /// </summary>
    public void GoToMainMenu() => SceneManager.LoadSceneAsync("Title");

    // ----------------------
    // プライベートメソッド
    // ----------------------

    /// <summary>
    /// 毎フレーム呼ばれる更新処理。
    /// ゲームが終了したら終了画面を表示するための処理を開始。
    /// </summary>
    private void Update()
    {
        // ゲームが終了しており、まだ処理が実行されていなければ終了画面を表示する
        if (!_hasTriggered && _gameDirector.IsGameOver())
        {
            _hasTriggered = true;
            StartCoroutine(showEndScreen()); // 終了画面を表示するコルーチンを開始
        }
    }

    /// <summary>
    /// ゲーム終了後の画面に遷移するコルーチン。
    /// 終了から少し待機し、勝者または引き分けを判定して表示する。
    /// </summary>
    /// <returns>コルーチンの実行</returns>
    private IEnumerator showEndScreen()
    {
        // 少し待ってから終了画面を表示
        yield return new WaitForSeconds(1);

        // 盤面上のコイン数を取得
        var c = _board.GetCoinCount();

        // 黒のコインが多い場合、黒プレイヤーが勝者
        if (c.x > c.y)
        {
            // 黒プレイヤー名を取得し、勝者メッセージを表示
            string player = PlayerPrefs.GetString("black-player-name");
            _blackBackground.GetComponentInChildren<TMP_Text>().text = $"{player}Black Wins!!";
            _blackBackground.SetActive(true); // 背景を表示
            _blackButton.SetActive(true);     // 再スタートボタンを表示
        }
        // 白のコインが多い場合、白プレイヤーが勝者
        else if (c.x < c.y)
        {
            // 白プレイヤー名を取得し、勝者メッセージを表示
            string player = PlayerPrefs.GetString("white-player-name");
            _whiteBackground.GetComponentInChildren<TMP_Text>().text = $"{player}White Wins!!";
            _whiteBackground.SetActive(true); // 背景を表示
            _whiteButton.SetActive(true);     // 再スタートボタンを表示
        }
        // 黒と白のコイン数が同じ場合、引き分け
        else
        {
            _greyBackground.GetComponentInChildren<TMP_Text>().text = $"It's a draw";
            _greyBackground.SetActive(true);  // 引き分け背景を表示
            _greyButton.SetActive(true);      // 再スタートボタンを表示
        }
    }
}