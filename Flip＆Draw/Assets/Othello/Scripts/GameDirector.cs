using UnityEngine;

// ゲーム全体の進行を管理するディレクタークラス。
// プレイヤーのターン管理(スキップも)や、ゲームの終了判定を行う。

public class GameDirector : MonoBehaviour
{
    // ボードの参照（盤面管理）
    [SerializeField] private Board _board;

    // 現在のプレイヤーを切り替えるためのフラグ（false = 黒, true = 白）
    private bool _playerSelector = false;

    // ゲーム終了フラグ
    private bool _isGameOver = false;

    private int currentTurn = 0; // 現在のターン数を管理

    // ゲームが終了しているかどうかを取得する。
    // <returns>ゲーム終了時は true</returns>

    public bool IsGameOver() => _isGameOver;
    public bool IsPlayerTurn() => !_playerSelector;
    public int GetPieceCount() => FindObjectsOfType<Piece>().Length;

    // スキルなどでターン強制上書き中かどうかのフラグ
    private bool _forceTurnOverride = false;

    // フラグ設定用のメソッド（Black_Skipから呼ぶ）
    public void SetForceTurnOverride(bool value)
    {
        _forceTurnOverride = value;
    }

    public void Update()
    {
        if (_isGameOver) return;

        if (!_board.CanPlay()) return;

        // 配置可能なポイントがあるか？
        if (_board.UpdateEligiblePositions(getFace()))
        {
            if (getInput() && _board.PlaceCoinOnBoard(getFace()))
            {
                NextTurn(); // コインを置いたらターンを進める
                _forceTurnOverride = false; // 通常ターン進行ならスキル状態解除
            }
        }
        else if (!_forceTurnOverride) // ← スキル発動中はスキップ判定をしない
        {
            Debug.Log($"{getFace()}は置けないのでスキップされました");

            _playerSelector = !_playerSelector; // 相手にターン渡す
            if (!_board.UpdateEligiblePositions(getFace()))
            {
                _isGameOver = true;
                Debug.Log("両プレイヤーとも置けないためゲーム終了！");
            }
            else
            {
                Debug.Log($"{getFace()} のターンにスキップされました");
            }
        }

        if (_board.IsFull())
        {
            _isGameOver = true;
            Debug.Log("ボードが満杯。ゲーム終了！");
        }


        if (_board.IsFull() ||
            (_board.getAllEligiblePosition(CoinFace.black).Count == 0 &&
             _board.getAllEligiblePosition(CoinFace.white).Count == 0))
        {
            _isGameOver = true;
            Debug.Log("CPU戦でもゲーム終了！");
        }

    }


    // ターンとプレイヤー状態を最初に戻すメソッド
    public void ResetGameState()
    {
        _playerSelector = false;  // 先手（黒）に戻すなどターン初期化
        currentTurn = 0;
        _isGameOver = false;
    }

    public void NextTurn()
    {
        currentTurn++; // ターン数をカウントアップ
        _playerSelector = !_playerSelector; // プレイヤー交代
    }

    public int GetCurrentTurn()
    {
        return currentTurn; // 現在のターン数を返す
    }


    public void ClearMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("EligibleMarker");

        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }
    }

    // 入力を取得する（左クリックが押されたかどうか）。
    // <returns>クリックされた場合は true</returns>
    public bool getInput()
    {
        return Input.GetMouseButtonDown(0); // 0 は左クリック
    }

    // 現在のプレイヤーのコイン面（黒 or 白）を取得
    // <returns>現在のプレイヤーの CoinFace</returns>
    public CoinFace getFace()
    {
        return _playerSelector ? CoinFace.white : CoinFace.black;
    }
}