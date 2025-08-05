using UnityEngine;
using TMPro;


/// <summary>
/// ゲーム中のプレイヤー名とコインの数を表示するHUD（ヘッドアップディスプレイ）クラス。
/// ゲーム中の黒・白プレイヤーの名前とコインの数を更新して表示する。
/// </summary>
public class MainGameLoopHUD : MonoBehaviour
{
    // ----------------------
    // フィールド
    // ----------------------

    // 盤面の管理を行う Board
    [SerializeField] private Board _board;

    // プレイヤー名とコイン数を表示するためのTMP（TextMeshPro）コンポーネント
    [SerializeField] private TMP_Text _blackPlayer; // 黒プレイヤーの名前とコイン数
    [SerializeField] private TMP_Text _whitePlayer; // 白プレイヤーの名前とコイン数

    // プレイヤーの名前（黒と白）
    private string _blackPlayerName;
    private string _whitePlayerName;

    // ----------------------
    // プライベートメソッド
    // ----------------------

    /// <summary>
    /// 初期化処理。プレイヤーの名前をPlayerPrefsから読み込む。
    /// プレイヤー名が設定されていない場合、デフォルト名（"Black Player" / "White Player"）を使用。
    /// </summary>
    private void Awake()
    {
        // PlayerPrefsからプレイヤー名を取得、存在しない場合はデフォルト値を使用
        _blackPlayerName = PlayerPrefs.GetString("black-player-name", "Black Player");
        _whitePlayerName = PlayerPrefs.GetString("white-player-name", "White Player");
    }

    /// <summary>
    /// 毎フレーム更新される処理。
    /// 盤面がプレイ可能な場合、プレイヤー名とコインの数を更新して表示する。
    /// </summary>
    private void Update()
    {
        // ゲームがプレイ可能な状態かを確認
        if (_board.CanPlay())
        {
            // 盤面のコインの数を取得
            var c = _board.GetCoinCount();

            // 黒プレイヤーと白プレイヤーのコイン数を表示
            _blackPlayer.text = $"{_blackPlayerName} : {c.x}";
            _whitePlayer.text = $"{_whitePlayerName} : {c.y}";
        }
    }
}