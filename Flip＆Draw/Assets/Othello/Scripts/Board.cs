using Lacobus.Grid;// グリッドシステムを使用
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SceneLoader1;

public class Board : MonoBehaviour
{
    // Fields
    [SerializeField] private Vector2Int _gridDimension;// グリッドのサイズ（幅×高さ）
    [SerializeField] private Vector2 _cellDimension;// 各セルのサイズ
    [SerializeField] private GameObject _whiteCoinPrefab;// 白コインのプレハブ
    [SerializeField] private GameObject _blackCoinPrefab;// 黒コインのプレハブ
    [SerializeField] private GameObject _blackMarkerPrefab;// 黒のマーカーのプレハブ
    [SerializeField] private GameObject _whiteMarkerPrefab;// 白のマーカーのプレハブ
    [SerializeField] private GameDirector gameDirector;
    [SerializeField] private Black_Skip blackSkip;
    [SerializeField] private White_Skip whiteSkip;

    [Range(0.001f, 0.2f)]
    [SerializeField] private float _coinRollSpeed;

    public void ClearCachedPoints()
    {
        _cachedBlackPoints = null;
        _cachedWhitePoints = null;
    }

    private Grid<BoardData> _grid;// グリッドシステム

    private Transform _t;// 現在のオブジェクトの Transform

    private Camera _camera;// メインカメラ

    private CoinFace _latestFace;// 最後に配置されたコインの色
    private Vector2Int _latestPoint;// 最後に配置されたコインの座標

    private List<Vector2Int> _cachedBlackPoints = null; // 黒コインの配置可能リスト（キャッシュ）
    private List<Vector2Int> _cachedWhitePoints = null; // 白コインの配置可能リスト（キャッシュ）

    private GameObject _markerPlaceholder; // マーカーの親オブジェクト（プレースホルダー）

    private bool _canPlay = true; // ゲームのプレイ可否
    private int _coinsPlaced = 0; // 配置済みのコイン数

    private CoinFace _currentTurn = CoinFace.black; // 初期は黒プレイヤー

    public int CPUcount;
    public static int white_count;//白のコインの枚数をカウント

    public class Receiver : MonoBehaviour//変数を受け取る
    {
        void Start()
        {
            int receivedValue = GameData.selectedValue;//受け取った変数管理
        }
    }

    // Properties
    // グリッドの原点座標（中央揃え）
    private Vector3 gridOrigin => _t.position - new Vector3((_gridDimension.x * _cellDimension.x) / 2, (_gridDimension.y * _cellDimension.y) / 2);

    // コインをボード上に配置する
    public bool PlaceCoinOnBoard(CoinFace face)
    {
        if (GameData.selectedValue == 5)//受け取った変数が５ならば処理を実行
        {
            // プレイヤーは黒しか操作できないようにする
            if (_currentTurn != CoinFace.black || face != CoinFace.black)

            return false;
        }

        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

        // マウス座標をグリッドのインデックスに変換
        if (_grid.ConvertToXY(mousePos, out Vector2Int index) && _grid.GetCellData(mousePos).isOccupied == false)
        {
            // 配置できるポイントかチェック
            if (face == CoinFace.black)
            {
                if (_cachedBlackPoints.Contains(index) == false)
                    return false;
            }
            else
            {
                if (_cachedWhitePoints.Contains(index) == false)
                    return false;
            }

            // コインを作成し、ボードに配置
            var coin = makeCoin(face, _grid.GetCellCenter(index));////////////////左クリックでコイン生成

            _grid.GetCellData(index).isOccupied = true;
            _grid.GetCellData(index).coin = coin;

            _latestPoint = index; // 最後に配置されたコインの座標を記録

            _latestFace = face; // 最後に配置されたコインの色を記録

            StartCoroutine(updateCoinCaptures()); // コインの捕獲処理を開始


            clearEligibleMarkers(); // 有効なマーカーをクリア

            _currentTurn = CoinFace.white; // プレイヤーが置いたら次はCPUの白ターン


            return true;
        }
        else
            return false;
    }

    // ボード上の黒と白のコインの数を取得する
    //sum_count = black_count + white_count;

    public Vector2Int GetCoinCount()
    {
        int blacks = 0, whites = 0;

        // グリッド全体を走査して、コインの色をカウント

        for (int i = 0; i < _grid.GridDimension.x; ++i)
        {
            for (int j = 0; j < _grid.GridDimension.y; ++j)
            {
                if (_grid.GetCellData(i, j).isOccupied)
                {
                    if (_grid.GetCellData(i, j).coin.GetFace() == CoinFace.black)
                        ++blacks;
                    else
                        ++whites;
                }
            }
        }

        return new Vector2Int(blacks, whites); // 黒と白のコイン数を返す

    }
    // 現在のプレイヤーの有効な配置位置を更新する
    private bool _isCPUProcessing = false;

    public bool UpdateEligiblePositions(CoinFace face)
    {
        List<Vector2Int> eligiblePoints = getAllEligiblePosition(face);

        //cpの追加
        if (eligiblePoints.Count == 0)
        {
            Debug.Log($"{face} のターンをスキップします");

            // ターンを相手に渡す
            _currentTurn = (face == CoinFace.black) ? CoinFace.white : CoinFace.black;

            // 相手の配置可能位置を更新
            UpdateEligiblePositions(_currentTurn);

            return false;
        }

        // マーカー描画など通常処理
        clearEligibleMarkers();
        drawNewEligibleMarkers(eligiblePoints, face);


        if (!_isCPUProcessing && GameData.selectedValue == 5 && _currentTurn == CoinFace.white)
        {
            _isCPUProcessing = true;
            StartCoroutine(CPUPlaceWhiteAndSwitchTurn());
        }

        //勝利判定
        if (getAllEligiblePosition(CoinFace.black).Count == 0 &&
            getAllEligiblePosition(CoinFace.white).Count == 0)
        {
            Debug.Log("両プレイヤーとも置けないため、ゲーム終了");
            _canPlay = false;
            // 勝敗判定などへ
        }

        switch (face)
        {
            case CoinFace.black:
                // 黒コインの配置可能ポイントが未取得の場合

                if (_cachedBlackPoints == null)
                {
                    // 他のキャッシュデータ（白コインの配置可能ポイント）をクリア

                    if (_cachedWhitePoints != null)
                        _cachedWhitePoints = null;

                    // 黒コインの配置可能ポイントを取得

                    _cachedBlackPoints = getAllEligiblePosition(CoinFace.black);

                    // 配置可能なポイントがない場合、false を返して終了

                    if (_cachedBlackPoints.Count == 0)
                        return false;

                    // 新しい配置可能マーカーを描画

                    drawNewEligibleMarkers(_cachedBlackPoints, CoinFace.black);
                }
                break;
            case CoinFace.white:
                // 白コインの配置可能ポイントが未取得の場合

                if (_cachedWhitePoints == null)
                {
                    // 他のキャッシュデータ（黒コインの配置可能ポイント）をクリア

                    if (_cachedBlackPoints != null)
                        _cachedBlackPoints = null;

                    // 白コインの配置可能ポイントを取得

                    _cachedWhitePoints = getAllEligiblePosition(CoinFace.white);
                    white_count = _cachedWhitePoints.Count;//白のコインの枚数を取得
                    // 配置可能なポイントがない場合、false を返して終了

                    if (_cachedWhitePoints.Count == 0)
                        return false;

                    // 新しい配置可能マーカーを描画
                    drawNewEligibleMarkers(_cachedWhitePoints, CoinFace.white);

                    ////CPUなら白コインを配置///CPUスクリプト

                    if(GameData.selectedValue == 5)//受け取った変数が５ならば処理を実行
                    {
                        if (_cachedWhitePoints != null && _cachedWhitePoints.Count > 0)
                        {
                            int index = UnityEngine.Random.Range(0, _cachedWhitePoints.Count);
                            setCoin(CoinFace.white, _cachedWhitePoints[index]); // CPUが白を置く
                            GameObject.Find("Black_Skip").GetComponent<Black_Skip>().ForceTurnBack();

                            GameDirector director = FindObjectOfType<GameDirector>();
                            if (director != null)
                            {
                                // キャッシュクリア
                                ClearCachedPoints();
                            }
                        }
                        else
                        {
                            GameObject.Find("Black_Skip").GetComponent<Black_Skip>().ForceTurnBack();
                            IsFull();
                        }
                    }
                }
                break;
        }
        // 配置可能なポイントがある場合は true を返す

        return true;
    }
    // ゲームがまだプレイ可能かどうかを返す

    void Update()
    {
        if (_currentTurn == CoinFace.black && _canPlay && Input.GetMouseButtonDown(0))
        {
            PlaceCoinOnBoard(CoinFace.black);

            HandleBlackClick();
        }

        // CPU処理（白ターン時のみ、一度だけ）
        if (_currentTurn == CoinFace.white && GameData.selectedValue == 5 && !_isCPUProcessing)
        {
            _isCPUProcessing = true;
            StartCoroutine(CPUPlaceWhiteAndSwitchTurn());
        }
    }

    void HandleBlackClick()
    {
        // マウス位置をワールド→盤面に変換
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int clickedPos = new Vector2Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y));

        // 黒が置ける場所にクリックしているか確認
        if (_cachedBlackPoints != null && _cachedBlackPoints.Contains(clickedPos))
        {
            _canPlay = false;

            setCoin(CoinFace.black, clickedPos); // 黒石を置く
            StartCoroutine(HandleTurnChange()); // 裏返して白ターンへ
        }
    }

    private IEnumerator CPUPlaceWhiteAndSwitchTurn()
    {
        if (_cachedWhitePoints != null && _cachedWhitePoints.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, _cachedWhitePoints.Count);
            setCoin(CoinFace.white, _cachedWhitePoints[index]); // 白コインを配置

            yield return StartCoroutine(updateCoinCaptures()); // 捕獲アニメーション完了まで待つ

            _cachedWhitePoints = null; // キャッシュもクリア

            _currentTurn = CoinFace.black; // 黒ターンに戻す
            UpdateEligiblePositions(CoinFace.black); // 黒の配置可能位置を更新してマーカーを出す

            _canPlay = true; // 黒が操作できるようにする
        }

        _isCPUProcessing = false; // 終了後に戻す

        if (IsFull())
        {
            _currentTurn = CoinFace.black;
            _isCPUProcessing = false;
            _canPlay = true;
            UpdateEligiblePositions(CoinFace.black);
        }
        yield break;
    }

    private IEnumerator HandleTurnChange()
    {
        yield return StartCoroutine(updateCoinCaptures());

        clearEligibleMarkers();           // 前のターンのマーカー削除
        _cachedBlackPoints = null;        // 黒の候補リセット

        _currentTurn = CoinFace.white;    // ターン変更
        UpdateEligiblePositions(CoinFace.white); // 白の候補を更新
    }

    public bool CanPlay() => _canPlay;
    // ボードが満杯かどうかを判定する

    public bool IsFull()
    {
        if (_coinsPlaced == 64) // 最大64個配置された場合、ゲームを終了

        {
            _canPlay = false;
            return true;
        }
        else return false;
    }

    // コインオブジェクトを生成する
    private Coin makeCoin(CoinFace face, Vector3 worldPosition)
    {
        ++_coinsPlaced;// 配置されたコイン数を更新

        // コインを生成し、適切な Transform に設定
        switch (face)
        {
            case CoinFace.black:
                return Instantiate(_blackCoinPrefab, worldPosition, Quaternion.identity, _t).GetComponent<Coin>();
            case CoinFace.white:
                return Instantiate(_whiteCoinPrefab, worldPosition, Quaternion.identity, _t).GetComponent<Coin>();
            default:
                return null;
        }
    }

    ////コインを自動で設定///CPUスクリプト
    ////public Coin setCoin(CoinFace face, List<Vector2Int> place_pos)リスト削除前スクリプト
    ////全スクリプトplace_pos=placePosに変えた//CPU
    public Coin setCoin(CoinFace face, Vector2Int placePos)
    {
        ++_coinsPlaced;

        _latestPoint = placePos; // ← 追加
        _latestFace = face;      // ← 追加

        Vector3 spawnPos = _grid.GetCellCenter(placePos);

        Coin coin = null;

        switch (face)
        {
            case CoinFace.black:

                coin = Instantiate(_blackCoinPrefab, spawnPos, Quaternion.identity, _t).GetComponent<Coin>();

                break;
            case CoinFace.white:

                coin = Instantiate(_whiteCoinPrefab, spawnPos, Quaternion.identity, _t).GetComponent<Coin>();

                break;
        }

        _grid.GetCellData(placePos).isOccupied = true;
        _grid.GetCellData(placePos).coin = coin;

        StartCoroutine(updateCoinCaptures()); // ← 追加済みならOK

        return coin;
    }

    // マーカーを生成する
    private void makeMark(Vector3 worldPosition, CoinFace face)
    {
        switch (face)
        {
            case CoinFace.black:
                Instantiate(_blackMarkerPrefab, worldPosition, Quaternion.identity, _markerPlaceholder.transform);
                break;
            case CoinFace.white:
                Instantiate(_whiteMarkerPrefab, worldPosition, Quaternion.identity, _markerPlaceholder.transform);
                break;
        }
    }

    // ゲームボードを初期化し、開始時のコイン配置を設定する
    private void initBoard()
    {
        // グリッドの中央座標を計算
        int xCenter = _grid.GridDimension.x / 2;
        int yCenter = _grid.GridDimension.y / 2;

        // 初期配置の4つのコインを作成
        var coin_1 = makeCoin(CoinFace.black, _grid.GetCellCenter(xCenter, yCenter));
        var coin_2 = makeCoin(CoinFace.black, _grid.GetCellCenter(xCenter - 1, yCenter - 1));
        var coin_3 = makeCoin(CoinFace.white, _grid.GetCellCenter(xCenter - 1, yCenter));
        var coin_4 = makeCoin(CoinFace.white, _grid.GetCellCenter(xCenter, yCenter - 1));

        // 各セルの占有情報とコイン情報を登録
        _grid.GetCellData(xCenter, yCenter).isOccupied = true;
        _grid.GetCellData(xCenter, yCenter).coin = coin_1;

        _grid.GetCellData(xCenter - 1, yCenter - 1).isOccupied = true;
        _grid.GetCellData(xCenter - 1, yCenter - 1).coin = coin_2;

        _grid.GetCellData(xCenter - 1, yCenter).isOccupied = true;
        _grid.GetCellData(xCenter - 1, yCenter).coin = coin_3;

        _grid.GetCellData(xCenter, yCenter - 1).isOccupied = true;
        _grid.GetCellData(xCenter, yCenter - 1).coin = coin_4;

        // ゲームのプレイ可能状態を true にする
        _canPlay = true;
    }

    // 横方向（左右）のコインの捕獲対象を判定し、リストとして返す
    // 捕獲可能なコインの座標リストを格納した Dictionary
    // キー `0` は右方向、キー `1` は左方向のコインを示す
    private Dictionary<int, List<Vector2Int>> getHorizontalCoinsToBeCaptured() 
    { 
        bool shouldFlipCoin; // 挟み込めるコインがあるかのフラグ 
        List<Vector2Int> coinsArray = null; // 一時的にコインの座標を格納するリスト 
        Dictionary<int, List<Vector2Int>> coinsToBeFlipped = new Dictionary<int, List<Vector2Int>>(); // 捕獲対象のコインリスト
         
        // **右方向の探索** 
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();
         
        // 最新の配置位置から右方向へ探索 
        for (int x = _latestPoint.x + 1; x < _grid.GridDimension.x; ++x)
        {
            // 空セルに到達した場合、探索終了
            if (_grid.GetCellData(x, _latestPoint.y).isOccupied == false)
                break;
             
            // 相手のコインならフラグを立てて継続
            if (_grid.GetCellData(x, _latestPoint.y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            } 
            else
            { 
                // 途中に相手のコインがなかった場合、捕獲できないので終了
                if (shouldFlipCoin == false)
                    break;
                 
                // 挟み込んだコインをリストに追加
                for (int i = _latestPoint.x + 1; i < x; ++i)
                    coinsArray.Add(new Vector2Int(i, _latestPoint.y));
                 
                break;
            }
        }
        coinsToBeFlipped.Add(0, coinsArray); // 右方向の捕獲対象を追加

        // **左方向の探索**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();

        // 最新の配置位置から左方向へ探索
        for (int x = _latestPoint.x - 1; x >= 0; --x)
        {
            // 空セルに到達した場合、探索終了
            if (_grid.GetCellData(x, _latestPoint.y).isOccupied == false)
                break;
             
            // 相手のコインならフラグを立てて継続
            if (_grid.GetCellData(x, _latestPoint.y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            }
            else
            {
                // 途中に相手のコインがなかった場合、捕獲できないので終了
                if (shouldFlipCoin == false)
                    break;
                 
                // 挟み込んだコインをリストに追加
                for (int i = _latestPoint.x - 1; i > x; --i)
                    coinsArray.Add(new Vector2Int(i, _latestPoint.y));
                 
                break;
            }
        }
         
        coinsToBeFlipped.Add(1, coinsArray); // 左方向の捕獲対象を追加

        return coinsToBeFlipped; // 捕獲対象のリストを返す
    }

    // 縦方向（上下）のコインの捕獲対象を判定し、リストとして返す
    // 捕獲可能なコインの座標リストを格納した Dictionary
    // キー `0` は上方向、キー `1` は下方向のコインを示す
    private Dictionary<int, List<Vector2Int>> getVerticalCoinsToBeCaptured()
    {
        bool shouldFlipCoin; // 挟み込めるコインがあるかのフラグ
        List<Vector2Int> coinsArray = null; // 一時的にコインの座標を格納するリスト
        Dictionary<int, List<Vector2Int>> coinsToBeFlipped = new Dictionary<int, List<Vector2Int>>(); // 捕獲対象のコインリスト
         
        // **上方向の探索**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();
         
        // 最新の配置位置から上方向へ探索
        for (int y = _latestPoint.y + 1; y < _grid.GridDimension.y; ++y)
        {
            // 空セルに到達した場合、探索終了
            if (_grid.GetCellData(_latestPoint.x, y).isOccupied == false)
                break;

            // 相手のコインならフラグを立てて継続
            if (_grid.GetCellData(_latestPoint.x, y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            } 
            else
            { 
                // 途中に相手のコインがなかった場合、捕獲できないので終了
                if (shouldFlipCoin == false)
                    break;
                 
                // 挟み込んだコインをリストに追加
                for (int i = _latestPoint.y + 1; i < y; ++i)
                    coinsArray.Add(new Vector2Int(_latestPoint.x, i));
                 
                break;
            }
        }

        coinsToBeFlipped.Add(0, coinsArray); // 上方向の捕獲対象を追加

        // **下方向の探索**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();

        // 最新の配置位置から下方向へ探索
        for (int y = _latestPoint.y - 1; y >= 0; --y)
        {
            // 空セルに到達した場合、探索終了
            if (_grid.GetCellData(_latestPoint.x, y).isOccupied == false)
                break;

            // 相手のコインならフラグを立てて継続
            if (_grid.GetCellData(_latestPoint.x, y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            }
            else
            {
                // 途中に相手のコインがなかった場合、捕獲できないので終了
                if (shouldFlipCoin == false)
                    break;

                // 挟み込んだコインをリストに追加
                for (int i = _latestPoint.y - 1; i > y; --i)
                    coinsArray.Add(new Vector2Int(_latestPoint.x, i));

                break;
            }
        }

        coinsToBeFlipped.Add(1, coinsArray); // 下方向の捕獲対象を追加

        return coinsToBeFlipped; // 捕獲対象のリストを返す 
    }

    // 斜め方向（上下左右）のコインの捕獲対象を判定し、リストとして返す 
    // 捕獲可能なコインの座標リストを格納した Dictionary 
    // キー `0` は右上（Up Right）、`1` は左上（Up Left）
    // キー `2` は左下（Down Left）、`3` は右下（Down Right）
    private Dictionary<int, List<Vector2Int>> getDiagonalCoinsToBeCaptured()
    {
        bool shouldFlipCoin; // 挟み込めるコインがあるかのフラグ
        List<Vector2Int> coinsArray = null; // 一時的にコインの座標を格納するリスト
        Dictionary<int, List<Vector2Int>> coinsToBeFlipped = new Dictionary<int, List<Vector2Int>>(); // 捕獲対象のコインリスト

        // **右上（Up Right）の探索**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();

        // 最新の配置位置から右上方向へ探索
        for (int x = _latestPoint.x + 1, y = _latestPoint.y + 1; x < _grid.GridDimension.x && y < _grid.GridDimension.y; ++x, ++y)
        {
            // 空セルに到達した場合、探索終了
            if (_grid.GetCellData(x, y).isOccupied == false)
                break;

            // 相手のコインならフラグを立てて継続
            if (_grid.GetCellData(x, y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            }
            else
            {
                // 途中に相手のコインがなかった場合、捕獲できないので終了
                if (shouldFlipCoin == false)
                    break;

                // 挟み込んだコインをリストに追加
                for (int i = _latestPoint.x + 1, j = _latestPoint.y + 1; i < x && j < y; ++i, ++j)
                    coinsArray.Add(new Vector2Int(i, j));

                break;
            }
        }

        coinsToBeFlipped.Add(0, coinsArray); // 右上方向の捕獲対象を追加

        // **左上（Up Left）の探索**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();

        // 最新の配置位置から左上方向へ探索
        for (int x = _latestPoint.x - 1, y = _latestPoint.y + 1; x >= 0 && y < _grid.GridDimension.y; --x, ++y)
        {
            if (_grid.GetCellData(x, y).isOccupied == false)
                break;

            if (_grid.GetCellData(x, y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            }
            else
            {
                if (shouldFlipCoin == false)
                    break;

                for (int i = _latestPoint.x - 1, j = _latestPoint.y + 1; i > x; --i, ++j)
                    coinsArray.Add(new Vector2Int(i, j));

                break;
            }
        }

        coinsToBeFlipped.Add(1, coinsArray); // 左上方向の捕獲対象を追加

        // **左下（Down Left）の探索**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();

        for (int x = _latestPoint.x - 1, y = _latestPoint.y - 1; x >= 0 && y >= 0; --x, --y)
        {
            if (_grid.GetCellData(x, y).isOccupied == false)
                break;

            if (_grid.GetCellData(x, y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            }
            else
            {
                if (shouldFlipCoin == false)
                    break;

                for (int i = _latestPoint.x - 1, j = _latestPoint.y - 1; i > x && j > y; --i, --j)
                    coinsArray.Add(new Vector2Int(i, j));

                break;
            }
        }

        coinsToBeFlipped.Add(2, coinsArray); // 左下方向の捕獲対象を追加

        // **右下（Down Right）の探索**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();

        for (int x = _latestPoint.x + 1, y = _latestPoint.y - 1; x < _grid.GridDimension.x && y >= 0; ++x, --y)
        {
            if (_grid.GetCellData(x, y).isOccupied == false)
                break;

            if (_grid.GetCellData(x, y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            }
            else
            {
                if (shouldFlipCoin == false)
                    break;

                for (int i = _latestPoint.x + 1, j = _latestPoint.y - 1; i < x && j > y; ++i, --j)
                    coinsArray.Add(new Vector2Int(i, j));

                break;
            }
        }

        coinsToBeFlipped.Add(3, coinsArray); // 右下方向の捕獲対象を追加

        return coinsToBeFlipped; // 捕獲対象のリストを返す
    }

    // コインの捕獲状態を更新するコルーチン
    // 配置されたコインに応じて、挟み込まれた相手のコインを反転する

    public IEnumerator updateCoinCaptures()
    {
        _canPlay = false; // コインの更新中はプレイを一時的に停止

        // 横・縦・斜め方向の捕獲対象のコインリストを取得
        var hor = getHorizontalCoinsToBeCaptured();
        var ver = getVerticalCoinsToBeCaptured();
        var dia = getDiagonalCoinsToBeCaptured();

        // 各方向の捕獲対象リストを取得
        var r = hor[0];  // 右
        var l = hor[1];  // 左
        var u = ver[0];  // 上
        var d = ver[1];  // 下
        var ur = dia[0]; // 右上
        var ul = dia[1]; // 左上
        var dl = dia[2]; // 左下
        var dr = dia[3]; // 右下

        // 最大8回の反転処理を行う（徐々に反転させるアニメーション効果を付与）
        for (int i = 0; i < 8; ++i)
        {
            // **横方向のコイン反転**
            if (i < r.Count) // 右方向
                _grid.GetCellData(r[i]).coin.FlipFace();
            if (i < l.Count) // 左方向
                _grid.GetCellData(l[i]).coin.FlipFace();

            // **縦方向のコイン反転**
            if (i < u.Count) // 上方向
                _grid.GetCellData(u[i]).coin.FlipFace();
            if (i < d.Count) // 下方向
                _grid.GetCellData(d[i]).coin.FlipFace();

            // **斜め方向のコイン反転**
            if (i < ur.Count) // 右上方向
                _grid.GetCellData(ur[i]).coin.FlipFace();
            if (i < ul.Count) // 左上方向
                _grid.GetCellData(ul[i]).coin.FlipFace();
            if (i < dl.Count) // 左下方向
                _grid.GetCellData(dl[i]).coin.FlipFace();
            if (i < dr.Count) // 右下方向
                _grid.GetCellData(dr[i]).coin.FlipFace();

            yield return new WaitForSeconds(_coinRollSpeed); // 各反転処理の間に待機時間を設定（視覚的演出）
        }

        _canPlay = true; // コイン更新処理完了後にプレイ可能状態に戻す
    }

    // 指定されたコインの色に応じて、配置可能な座標を取得する
    // <returns>配置可能な座標のリスト</returns>

    public List<Vector2Int> getAllEligiblePosition(CoinFace face)
    {
        List<Vector2Int> points = new List<Vector2Int>(); // 配置可能な座標を格納するリスト
        bool shouldFlip = false; // 挟み込めるコインがあるかの判定フラグ

        // **盤面全体を走査**
        for (int x = 0; x < _grid.GridDimension.x; ++x)
        {
            for (int y = 0; y < _grid.GridDimension.y; ++y)
            {
                // 既に占有されているセルで、対象コインと同じ色なら探索を開始
                if (_grid.GetCellData(x, y).isOccupied == true && _grid.GetCellData(x, y).coin.GetFace() == face)
                {
                    Vector2Int targetPoint = new Vector2Int(x, y);

                    // 水平方向
                    // 右
                    shouldFlip = false;
                    for (int i = targetPoint.x + 1; i < _grid.GridDimension.x; ++i)
                    {
                        if (_grid.GetCellData(i, targetPoint.y).isOccupied)
                        {
                            if (_grid.GetCellData(i, targetPoint.y).coin.GetFace() != face)
                                shouldFlip = true;
                            else
                                break;
                        }
                        else
                        {
                            if (shouldFlip)
                            {
                                points.Add(new Vector2Int(i, targetPoint.y));
                                break;
                            }
                            else
                                break;
                        }
                    }

                    // 左
                    shouldFlip = false;
                    for (int i = targetPoint.x - 1; i >= 0; --i)
                    {
                        if (_grid.GetCellData(i, targetPoint.y).isOccupied)
                        {
                            if (_grid.GetCellData(i, targetPoint.y).coin.GetFace() != face)
                                shouldFlip = true;
                            else
                                break;
                        }
                        else
                        {
                            if (shouldFlip)
                            {
                                points.Add(new Vector2Int(i, targetPoint.y));
                                break;
                            }
                            else
                                break;
                        }
                    }

                    // 垂直方向
                    // 上
                    shouldFlip = false;
                    for (int i = targetPoint.y + 1; i < _grid.GridDimension.y; ++i)
                    {
                        if (_grid.GetCellData(targetPoint.x, i).isOccupied)
                        {
                            if (_grid.GetCellData(targetPoint.x, i).coin.GetFace() != face)
                                shouldFlip = true;
                            else
                                break;
                        }
                        else
                        {
                            if (shouldFlip)
                            {
                                points.Add(new Vector2Int(targetPoint.x, i));
                                break;
                            }
                            else
                                break;
                        }
                    }

                    // 下
                    shouldFlip = false;
                    for (int i = targetPoint.y - 1; i >= 0; --i)
                    {
                        if (_grid.GetCellData(targetPoint.x, i).isOccupied)
                        {
                            if (_grid.GetCellData(targetPoint.x, i).coin.GetFace() != face)
                                shouldFlip = true;
                            else
                                break;
                        }
                        else
                        {
                            if (shouldFlip)
                            {
                                points.Add(new Vector2Int(targetPoint.x, i));
                                break;
                            }
                            else
                                break;
                        }
                    }

                    // 斜め方向
                    // 右上
                    shouldFlip = false;
                    for (int i = targetPoint.x + 1, j = targetPoint.y + 1; i < _grid.GridDimension.x && j < _grid.GridDimension.y; ++i, ++j)
                    {
                        if (_grid.GetCellData(i, j).isOccupied)
                        {
                            if (_grid.GetCellData(i, j).coin.GetFace() != face)
                                shouldFlip = true;
                            else
                                break;
                        }
                        else
                        {
                            if (shouldFlip)
                            {
                                points.Add(new Vector2Int(i, j));
                                break;
                            }
                            else
                                break;
                        }
                    }

                    // Up left
                    shouldFlip = false;
                    for (int i = targetPoint.x - 1, j = targetPoint.y + 1; i >= 0 && j < _grid.GridDimension.y; --i, ++j)
                    {
                        if (_grid.GetCellData(i, j).isOccupied)
                        {
                            if (_grid.GetCellData(i, j).coin.GetFace() != face)
                                shouldFlip = true;
                            else
                                break;
                        }
                        else
                        {
                            if (shouldFlip)
                            {
                                points.Add(new Vector2Int(i, j));
                                break;
                            }
                            else
                                break;
                        }
                    }

                    // Down left
                    shouldFlip = false;
                    for (int i = targetPoint.x - 1, j = targetPoint.y - 1; i >= 0 && j >= 0; --i, --j)
                    {
                        if (_grid.GetCellData(i, j).isOccupied)
                        {
                            if (_grid.GetCellData(i, j).coin.GetFace() != face)
                                shouldFlip = true;
                            else
                                break;
                        }
                        else
                        {
                            if (shouldFlip)
                            {
                                points.Add(new Vector2Int(i, j));
                                break;
                            }
                            else
                                break;
                        }
                    }

                    // Down right
                    shouldFlip = false;
                    for (int i = targetPoint.x + 1, j = targetPoint.y - 1; i < _grid.GridDimension.x && j >= 0; ++i, --j)
                    {
                        if (_grid.GetCellData(i, j).isOccupied)
                        {
                            if (_grid.GetCellData(i, j).coin.GetFace() != face)
                                shouldFlip = true;
                            else
                                break;
                        }
                        else
                        {
                            if (shouldFlip)
                            {
                                points.Add(new Vector2Int(i, j));
                                break;
                            }
                            else
                                break;
                        }
                    }
                }
            }
        }

        return points;// 配置可能な座標のリストを返す
    }

    // 現在の配置可能なマーカーをすべて削除する
    public void clearEligibleMarkers()
    {
        destroyPlaceholderChildren(); // マーカーのプレースホルダーの子オブジェクトを削除
    }

    // 新しい配置可能なマーカーを描画する
    // <param name="eligiblePoints">配置可能な座標リスト</param>
    // <param name="face">コインの色（黒または白）</param>
    public void drawNewEligibleMarkers(List<Vector2Int> eligiblePoints, CoinFace face)
    {
        foreach (var p in eligiblePoints)
            makeMark(_grid.GetCellCenter(p), face); // 指定座標にマーカーを生成
    }
 
    // マーカーのプレースホルダーの子オブジェクトをすべて削除する
    public void destroyPlaceholderChildren()
    {
        var t = _markerPlaceholder.transform;

        // すべての子オブジェクトを削除
        while (t.childCount > 0)
            DestroyImmediate(t.GetChild(0).gameObject);
    }

    // 初期化処理
    public void Awake()
    {
        _canPlay = false; // ゲーム開始時はプレイ不可
        _t = transform; // Transform を取得
        _camera = Camera.main; // メインカメラを取得

        // マーカーのプレースホルダーオブジェクトを作成
        _markerPlaceholder = new GameObject("-Marker Placeholder-");

        // グリッドを作成・準備
        _grid = new Grid<BoardData>(gridOrigin, _gridDimension, _cellDimension);
        _grid.PrepareGrid();

        initBoard(); // 盤面を初期化
    }
}
// **ボードのデータクラス**

public class BoardData
{
    public bool isOccupied = false; // セルが占有されているかどうか
    public Coin coin; // 配置されたコイン情報
}