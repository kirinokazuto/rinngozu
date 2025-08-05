using Lacobus.Animation;
using Lacobus.Grid;// グリッドシステムを使用
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpuPlayer : MonoBehaviour
{
    // 現在のコインの面（白 or 黒）
    [SerializeField] private CoinFace _currentFace;

    [SerializeField] private Board _board;

    // アニメーション処理用のコンポーネント
    private AnimationHandlerComponent _animationHandler;

    // サウンド再生処理用のコンポーネント
    private CoinSoundHandler _soundHandler;

    // 盤面のサイズ（例：8x8）
    private int boardSize = 8;

    public Board Board;



    // 盤面の状態を表す（0=空き、1=プレイヤー、2=CPUなど）
    private int[,] board;

    /// <summary>
    /// ゲーム開始時にコイン設置音を再生。
    /// </summary>
    void Start()
    {
        board = new int[boardSize, boardSize];

        // ここで初期盤面セットアップなど
        //_soundHandler.PlayCoinPlaceSound();
    }

    // CPUのターンで駒をランダムに置くメソッド
    public void PlayRandom()
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        // 盤面全体をチェックして、置ける場所をリストに集める
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                if (CanPlaceAt(x, y))
                {
                    validMoves.Add(new Vector2Int(x, y));
                }
            }
        }

        // 置ける場所がなければ何もしない
        if (validMoves.Count == 0)
        {
            return;
        }

        // ランダムに1か所選ぶ
        int index = Random.Range(0, validMoves.Count);

        Vector2Int move = validMoves[index];

        // 駒を置く処理（ここはゲームに合わせて実装）
        PlacePiece(move.x, move.y);
    }

    // ここに「そのマスに置けるか」の判定を書く
    private bool CanPlaceAt(int x, int y)
    {
        // 空きマスかどうかだけ判定（本当はオセロルールで判定）
        return board[x, y] == 0;
    }

    // 駒を置く処理の例
    private void PlacePiece(int x, int y)
    {
        // 2をCPUの駒とする
        board[x, y] = 2;

        // ひっくり返す処理
        // 現在の面を切り替える
        switch (_currentFace)
        {
            case CoinFace.black:

               _currentFace = CoinFace.white;

               break;

            case CoinFace.white:

               _currentFace = CoinFace.black;

               break;
        }

        // 面の変更に応じてアニメーションを再生
        updateRenderer();

        // サウンドを再生
        playSound();
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

    public CoinFace GetFace()
    {
        return _currentFace;
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