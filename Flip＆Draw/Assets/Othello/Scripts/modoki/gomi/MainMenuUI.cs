/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// メインメニューのUI操作を管理するクラス。
/// "Start"ボタンと"Exit"ボタンにそれぞれ対応する動作を設定。
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    // ----------------------
    // フィールド
    // ----------------------

    // ゲームスタートボタン
    [SerializeField] private Button _startButton;

    // ゲーム終了ボタン
    [SerializeField] private Button _exitButton;

    // ----------------------
    // プライベートメソッド
    // ----------------------

    /// <summary>
    /// 初期化処理。
    /// ボタンにクリックイベントリスナーを登録し、各ボタンの動作を定義。
    /// </summary>
    private void Awake()
    {
        // スタートボタンがクリックされた時に、"PlayerSelect"シーンに遷移
        _startButton.onClick.AddListener(() => { SceneManager.LoadScene("PlayerSelect"); });

        // エグジットボタンがクリックされた時に、アプリケーションを終了
        _exitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}*/