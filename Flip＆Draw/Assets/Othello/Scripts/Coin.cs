using Lacobus.Animation;
using UnityEngine;

/// <summary>
/// コイントークンの制御を行うクラス。
/// 表と裏（白と黒）の切り替えやアニメーション・サウンドの再生を行う。
/// </summary>
public class Coin : MonoBehaviour
{
    // ----------------------
    // フィールド
    // ----------------------

    // 現在のコインの面（白 or 黒）
    [SerializeField] private CoinFace _currentFace;

    // アニメーション処理用のコンポーネント
    private AnimationHandlerComponent _animationHandler;

    // サウンド再生処理用のコンポーネント
    private CoinSoundHandler _soundHandler;

    private bool isWhite = true; // 仮の状態

    public int OwnerPlayerId { get; set; }

    // ----------------------
    // パブリックメソッド
    // ----------------------

    /// <summary>
    /// コインの面を反転させる（白 ? 黒）アニメーションとサウンド付き。
    /// </summary>

    public bool IsWhiteFace()
    {
        return isWhite;
    }

    public void FlipFace()
    {
        // 現在の面を切り替える
        switch (_currentFace)
        {
            case CoinFace.black:
                _currentFace = CoinFace.white;
                gameObject.tag = "Whitecoin"; // タグを白に変更
                break;
            case CoinFace.white:
                _currentFace = CoinFace.black;
                gameObject.tag = "Blackcoin"; // タグを黒に変更
                break;
        }

        // 面の変更に応じてアニメーションを再生
        updateRenderer();

        // サウンドを再生
        playSound();

        isWhite = !isWhite;
    }

    /// <summary>
    /// 現在のコインの面を取得する。
    /// </summary>
    /// <returns>現在の面（白 or 黒）</returns>
    public CoinFace GetFace()
    {
        return _currentFace;
    }

    // ----------------------
    // プライベートメソッド
    // ----------------------

    /// <summary>
    /// コンポーネントの初期化処理。
    /// </summary>
    private void Awake()
    {
        _animationHandler = GetComponent<AnimationHandlerComponent>();
        _soundHandler = GetComponent<CoinSoundHandler>();
    }

    /// <summary>
    /// ゲーム開始時にコイン設置音を再生。
    /// </summary>
    private void Start()
    {
        _soundHandler.PlayCoinPlaceSound();
    }

    /// <summary>
    /// 面の変化に応じたアニメーションを再生。
    /// </summary>
    private void updateRenderer()
    {
        switch (_currentFace)
        {
            case CoinFace.black:
                _animationHandler.PlayState("white_to_black");
                break;
            case CoinFace.white:
                _animationHandler.PlayState("black_to_white");
                break;
        }
    }

    /// <summary>
    /// コイン反転時のサウンドを再生。
    /// </summary>
    private void playSound()
    {
        _soundHandler.PlayCoinFlipSound();
    }
}

/// <summary>
/// コインの面を表す列挙体。
/// </summary>
public enum CoinFace
{
    black, // 黒面
    white  // 白面
}