/*using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// プレイヤー名を入力してゲームを開始するためのUIを管理するクラス。
/// 黒・白プレイヤーの名前を入力し、「ゲーム開始」ボタンを押すとゲームが開始される。
/// </summary>
public class PlayerSelectUI : MonoBehaviour
{
    // ----------------------
    // フィールド
    // ----------------------

    // 黒プレイヤーの名前を入力するためのTMP_InputField
    [SerializeField] private TMPro.TMP_InputField _blackPlayerName;

    // 白プレイヤーの名前を入力するためのTMP_InputField
    [SerializeField] private TMPro.TMP_InputField _whitePlayerName;

    // ゲーム開始ボタン
    [SerializeField] private Button _startGame;

    // ----------------------
    // プライベートメソッド
    // ----------------------

    /// <summary>
    /// 初期化処理。
    /// プレイヤー名の入力フィールドにリスナーを追加し、「ゲーム開始」ボタンの動作を設定。
    /// PlayerPrefsをリセットし、プレイヤー名を保存するための処理を設定。
    /// </summary>
    private void Awake()
    {
        // PlayerPrefsの全データを削除
        PlayerPrefs.DeleteAll();

        // 黒プレイヤー名の入力が終了した時に、入力された名前をPlayerPrefsに保存
        _blackPlayerName.onEndEdit.AddListener((string val) => {
            PlayerPrefs.SetString("black-player-name", val);  // PlayerPrefsに黒プレイヤー名を保存
            PlayerPrefs.Save();  // PlayerPrefsのデータをディスクに保存
        });

        // 白プレイヤー名の入力が終了した時に、入力された名前をPlayerPrefsに保存
        _whitePlayerName.onEndEdit.AddListener((string val) => {
            PlayerPrefs.SetString("white-player-name", val);  // PlayerPrefsに白プレイヤー名を保存
            PlayerPrefs.Save();  // PlayerPrefsのデータをディスクに保存
        });

        // ゲーム開始ボタンがクリックされた時に、"MainLoop"シーンを非同期で読み込む
        _startGame.onClick.AddListener(() => {
            SceneManager.LoadSceneAsync("MainLoop");  // ゲーム本編のシーンに遷移
        });
    }
}*/