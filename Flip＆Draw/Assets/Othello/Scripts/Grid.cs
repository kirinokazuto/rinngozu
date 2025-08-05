using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lacobus.Grid
{
    /// <summary>
    ///カスタムグリッドを作成するためのクラスです。
    /// </summary>
    public class Grid<TType> : IEnumerable<Cell<TType>> where TType : class, new()
    {
        // フィールド

        // グリッドの原点（左下の座標）を格納する変数
        private Vector2 _gridOrigin;

        // グリッドの幅と高さ（セル数）を格納する変数
        private Vector2Int _gridDimension;

        // 1セルの幅と高さを格納する変数
        private Vector2 _cellDimension;

        // グリッド内のセルの総数を格納する変数
        private int _totalCellCount;

        // 2次元配列として格納されるセルの配列
        private Cell<TType>[,] _cellArray2D;

        // 1次元配列として格納されるセルの配列
        private Cell<TType>[] _cellArray1D;

        // コンストラクター

       /// <summary>
/// デフォルトコンストラクタ。原点を(0,0,0)、セル数を1x1、セルサイズを(1,1)に設定する。
/// </summary>
public Grid()
    : this(Vector3.zero, 1, 1, Vector2.one) { }

/// <summary>
/// コンストラクタ。原点とセル数（2Dベクトル）、セルサイズを指定してグリッドを初期化する。
/// </summary>
/// <param name="origin">グリッドの原点（ワールド座標）</param>
/// <param name="cellCount">セル数（X方向とY方向）</param>
/// <param name="cellSize">1セルのサイズ（幅と高さ）</param>
public Grid(Vector3 origin, Vector2Int cellCount, Vector2 cellSize)
    : this(origin, cellCount.x, cellCount.y, cellSize) { }

/// <summary>
/// コンストラクタ。原点、横セル数、縦セル数、セルサイズを指定してグリッドを初期化する。
/// </summary>
/// <param name="origin">グリッドの原点（ワールド座標）</param>
/// <param name="horizontalCellCount">横方向のセル数（最低1）</param>
/// <param name="verticalCellCount">縦方向のセル数（最低1）</param>
/// <param name="cellSize">1セルのサイズ（最低0.01）</param>
public Grid(Vector3 origin, int horizontalCellCount, int verticalCellCount, Vector2 cellSize)
{
    // グリッドの原点を設定
    _gridOrigin = origin;

    // セル数は最低1以上になるように制限し設定
    _gridDimension = new Vector2Int(Mathf.Max(horizontalCellCount, 1), Mathf.Max(verticalCellCount, 1));

    // セルサイズは最低0.01以上になるように制限し設定
    _cellDimension = new Vector2(Mathf.Max(cellSize.x, 0.01f), Mathf.Max(cellSize.y, 0.01f));

    // 総セル数を計算
    _totalCellCount = GridDimension.x * GridDimension.y;
}


        // プロパテ

        /// <summary>
        /// グリッドの原点を返します
        /// </summary>
        public Vector2 GridOrigin { get { return _gridOrigin; } set { _gridOrigin = value; } }

        /// <summary>
        /// グリッドのサイズ（幅と高さ）を返します
        /// </summary>
        public Vector2Int GridDimension { get { return _gridDimension; } }

        /// <summary>
        /// セルのサイズを返します
        /// </summary>
        public Vector2 CellDimension { get { return _cellDimension; } }

        /// <summary>
        /// セルの総数を返します
        /// </summary>
        public int TotalCellCount { get { return _totalCellCount; } }


        // パブリックメソッド

        /// <summary>
        /// グリッドを使用する前にこのメソッドを呼び出してください
        /// </summary>
        public void PrepareGrid()
        {
            // 2D配列と1D配列をセル数に基づいて生成
            _cellArray2D = new Cell<TType>[GridDimension.x, GridDimension.y];
            _cellArray1D = new Cell<TType>[TotalCellCount];

            // 全セルを初期化
            for (int y = 0; y < GridDimension.y; y++)
            {
                for (int x = 0; x < GridDimension.x; x++)
                {
                    var cell = new Cell<TType>();
                    // セルのインデックスを設定
                    cell.Index = new Vector2Int(x, y);
                    // セルを有効に設定
                    cell.IsValid = true;
                    // セルのデータを新しく生成
                    cell.Data = new TType();

                    // 2D配列と1D配列にセルを格納
                    _cellArray2D[x, y] = cell;
                    _cellArray1D[x + y * GridDimension.x] = cell;
                }
            }
        }


        /// <summary>
        /// OnDrawGizmosから呼び出して、グリッドのギズモを描画するメソッド
        /// </summary>
        public void DrawGridLines(Color gridLineColor, Color crossLineColor)
        {
            Gizmos.color = gridLineColor;

            Vector2 corner = GridOrigin + new Vector2(GridDimension.x * CellDimension.x, GridDimension.y * CellDimension.y);

            // 横のライン（上下の枠線）
            Gizmos.DrawLine(GridOrigin, new Vector2(corner.x, GridOrigin.y));
            Gizmos.DrawLine(new Vector2(GridOrigin.x, corner.y), corner);

            // 縦のライン（左右の枠線）
            Gizmos.DrawLine(GridOrigin, new Vector2(GridOrigin.x, corner.y));
            Gizmos.DrawLine(new Vector2(corner.x, GridOrigin.y), corner);

            // 横のグリッド線（内側の水平線）
            for (int h = 1; h < GridDimension.y; ++h)
                Gizmos.DrawLine
                    (
                        GridOrigin + (Vector2.up * h * CellDimension.y),
                        new Vector2(corner.x, GridOrigin.y) + (Vector2.up * h * CellDimension.y)
                    );

            // 縦のグリッド線（内側の垂直線）
            for (int w = 1; w < GridDimension.x; ++w)
                Gizmos.DrawLine
                    (
                        GridOrigin + (Vector2.right * w * CellDimension.x),
                        new Vector2(GridOrigin.x, corner.y) + (Vector2.right * w * CellDimension.x)
                    );
            // クロスラインの色に切り替え（この後の描画用）
            Gizmos.color = crossLineColor;
        }



        /// <summary>
        /// グリッド内のセル自体を返します
        /// </summary>
        /// <param name="x">x座標インデックス</param>
        /// <param name="y">y座標インデックス</param>
        public Cell<TType> GetCellRaw(int x, int y)
        {
            // 指定した座標 (x, y) がグリッド内にある場合は該当セルを返す
            if (IsInside(x, y))
                return _cellArray2D[x, y];
            // グリッド外の場合は null を返す
            return null;
        }

        /// <summary>
        /// グリッド内のセル自体を返します
        /// </summary>
        /// <param name="index">セルのインデックス</param>
        public Cell<TType> GetCellRaw(Vector2Int index)
        {
            // インデックスの x, y を使って GetCellRaw メソッドを呼び出し、セルを取得して返す
            return GetCellRaw(index.x, index.y);
        }

        /// <summary>
        /// グリッド内のセル自体を返します
        /// </summary>
        /// <param name="worldPosition">ワールド座標の位置</param>
        public Cell<TType> GetCellRaw(Vector3 worldPosition)
        {
            // worldPosition をグリッドの x, y インデックスに変換できたら、そのセルを返す
            // 変換できなければ null を返す
            if (ConvertToXY(worldPosition, out int x, out int y))
                return _cellArray2D[x, y];
            return null;
        }



        /// <summary>
        /// 2次元セル配列自体を返します
        /// </summary>
        public Cell<TType>[,] GetCellArray2D()
        {
            // 2D 配列としての全セルを返す
            return _cellArray2D;
        }

        /// <summary>
        /// 1次元セル配列自体を返します
        /// </summary>
        public Cell<TType>[] GetCellArray1D()
        {
            return _cellArray1D;
        }

        /// <summary>
        /// グリッドセルのデータを返します
        /// </summary>
        /// <param name="x">x座標インデックス</param>
        /// <param name="y">y座標インデックス</param>
        public TType GetCellData(int x, int y)
        {
            // x, y がグリッド内にある場合、そのセルのデータを返す
            // グリッド外の場合は null を返す
            if (IsInside(x, y))
                return _cellArray2D[x, y].Data;
            return null;
        }

        /// <summary>
        /// グリッドセルのデータを返します
        /// </summary>
        /// <param name="index">セルのインデックス</param>
        public TType GetCellData(Vector2Int index)
        {
            // index の x, y を使ってセルのデータを取得して返す
            return GetCellData(index.x, index.y);
        }

        /// <summary>
        /// グリッドセルのデータを返します
        /// </summary>
        /// <param name="worldPosition">セルのワールド座標位置</param>
        public TType GetCellData(Vector3 worldPosition)
        {
            // ワールド座標をグリッドのインデックス（x, y）に変換し、そのセルのデータを取得して返す
            ConvertToXY(worldPosition, out int x, out int y);
            return GetCellData(x, y);
        }


        /// <summary>
        /// セルの有効性を返します
        /// </summary>
        /// <param name="x">x インデックス</param>
        /// <param name="y">y インデックス</param>
        public bool GetCellValidity(int x, int y)
        {
            // 指定したインデックス(x, y)がグリッド内にある場合、そのセルの有効性(IsValid)を返す
            // グリッド外の場合は false を返す
            if (IsInside(x, y))
                return _cellArray2D[x, y].IsValid;
            return false;
        }

        /// <summary>
        /// セルの有効性を返します
        /// </summary>
        /// <param name="index">セルのインデックス</param>
        public bool GetCellValidity(Vector2Int index)
        {
            // 指定したインデックス(x, y)がグリッド内にある場合、そのセルの有効性(IsValid)を返す
            // グリッド外の場合は false を返す
            return GetCellValidity(index.x, index.y);
        }

        /// <summary>
        /// セルの有効性を返します
        /// </summary>
        /// <param name="worldPosition">このセルのワールド座標</param>
        public bool GetCellValidity(Vector3 worldPosition)
        {
            // worldPosition をグリッド座標に変換し、グリッド内であれば該当セルの有効性(IsValid)を返す
            // グリッド外の場合は false を返す
            if (ConvertToXY(worldPosition, out int x, out int y))
                return _cellArray2D[x, y].IsValid;
            return false;
        }


        /// <summary>
        /// x と y がグリッド内にある場合に true を返します
        /// </summary>
        /// <param name="x">X インデックス</param>
        /// <param name="y">Y インデックス</param>
        public bool IsInside(int x, int y)
        {
            // x と y がグリッドの範囲内にあるかチェックし、範囲内なら true を返す
            // 範囲外の場合は false を返す
            if (x >= 0 && y >= 0 && x < GridDimension.x && y < GridDimension.y)
                return true;
            return false;
        }

        /// <summary>
        /// 指定した座標がグリッド内にある場合に true を返します
        /// </summary>
        /// <param name="worldPosition">対象のワールド座標</param>
        public bool IsInside(Vector3 worldPosition)
        {
            ConvertToXY(worldPosition, out int x, out int y);
            return IsInside(x, y);
        }

        /// <summary>
        /// 指定したインデックスがグリッド内にある場合に true を返します
        /// </summary>
        /// <param name="index">対象のインデックス</param>
        public bool IsInside(Vector2Int index)
        {
            return IsInside(index.x, index.y);
        }



        /// <summary>
        /// ワールド座標をグリッドのセクションに変換します
        /// </summary>
        /// <param name="worldPosition">対象の位置</param>
        /// <param name="x">x座標の出力パラメータ</param>
        /// <param name="y">y座標の出力パラメータ</param>
        /// <returns>有効なポイントであれば true を返します</returns>
        public bool ConvertToXY(Vector3 worldPosition, out int x, out int y)
        {
            // worldPosition をグリッドのインデックスに変換し、その結果を index に格納する
            bool ret = ConvertToXY(worldPosition, out Vector2Int index);
            // index の x と y をそれぞれ x と y に代入する
            x = index.x;
            y = index.y;
            // 変換が成功したかどうかを true/false で返す
            return ret;
        }

        /// <summary>
        /// ワールド座標をグリッドのセクションに変換します
        /// </summary>
        /// <param name="worldPosition">対象の位置</param>
        /// <param name="isInside">このワールド座標がグリッド内にある場合は true になります</param>
        /// <returns>変換されたインデックスを返します</returns>
        public Vector2Int ConvertToXY(Vector3 worldPosition, out bool isInsde)
        {
            isInsde = ConvertToXY(worldPosition, out Vector2Int index);
            return index;
        }

        /// <summary>
        /// ワールド座標をグリッドのセクションに変換します
        /// </summary>
        /// <param name="worldPosition">対象の位置</param>
        /// <param name="x">x座標の出力パラメーター</param>
        /// <param name="y">y座標の出力パラメーター</param>
        /// <returns>有効な位置であれば true を返します</returns>
        public bool ConvertToXY(Vector3 worldPosition, out Vector2Int index)
        {
            // worldPosition をグリッドの原点からの相対座標に変換し、セルサイズで割ることでセルのインデックスを計算する
            int x = Mathf.FloorToInt((worldPosition - (Vector3)GridOrigin).x / CellDimension.x);
            int y = Mathf.FloorToInt((worldPosition - (Vector3)GridOrigin).y / CellDimension.y);

            // 計算したインデックスを Vector2Int 型の index に格納する
            index = new Vector2Int(x, y);

            // 計算したインデックスがグリッドの範囲内かどうかを判定し、その結果を返す
            return IsInside(x, y);
        }


        /// <summary>
        /// グリッドのセクションをワールド座標に変換します
        /// </summary>
        /// <param name="x">対象のx座標</param>
        /// <param name="y">対象のy座標</param>
        public Vector3 ConvertToWorldPosition(int x, int y)
        {
            return new Vector2(x * CellDimension.x, y * CellDimension.y) + GridOrigin;
        }

        /// <summary>
        /// グリッドのセクションをワールド座標に変換します
        /// </summary>
        /// <param name="x">対象のx座標</param>
        /// <param name="y">対象のy座標</param>
        public Vector3 ConvertToWorldPosition(Vector2Int index)
        {
            return new Vector2(index.x * CellDimension.x, index.y * CellDimension.y) + GridOrigin;
        }


        /// <summary>
        /// グリッドの境界を返します
        /// </summary>
        public Rect GetGridBounds()
        {
            return new Rect(ConvertToWorldPosition(0, 0), new Vector2(CellDimension.x * GridDimension.x, CellDimension.y * GridDimension.y));
        }


        /// <summary>
        /// 指定したセルの境界を返します
        /// </summary>
        /// <param name="x">X インデックス</param>
        /// <param name="y">Y インデックス</param>
        /// <returns>無効な場合はゼロを返します</returns>
        public Rect GetCellBounds(int x, int y)
        {
            if (IsInside(x, y))
                return new Rect(GetCellCenter(x, y), CellDimension);
            return Rect.zero;
        }
        /// <summary>
        /// 指定したセルの境界を返します
        /// </summary>
        /// <param name="index">セルのインデックス</param>
        /// <returns>無効な場合はゼロを返します</returns>
        public Rect GetCellBounds(Vector2Int index)
        {
            return GetCellBounds(index.x, index.y);
        }


        /// <summary>
        /// 2次元配列内のすべてのセルの境界を返します
        /// </summary>
        public Rect[,] GetCellBoundsArray2D()
        {
            Rect[,] bounds = new Rect[GridDimension.x, GridDimension.y];
            for (int x = 0; x < GridDimension.x; ++x)
                for (int y = 0; y < GridDimension.y; ++y)
                    bounds[x, y] = new Rect(ConvertToWorldPosition(x, y) + new Vector3(CellDimension.x, CellDimension.y) / 2, CellDimension);
            return bounds;
        }

        /// <summary>
        /// 1次元配列内のすべてのセルの境界を返します
        /// </summary>
        public Rect[] GetCellBoundsArray1D()
        {
            Rect[] bounds = new Rect[TotalCellCount];
            for (int y = 0; y < GridDimension.y; ++y)
                for (int x = 0; x < GridDimension.x; ++x)
                    bounds[x + y * GridDimension.x] = new Rect(ConvertToWorldPosition(x, y) + new Vector3(CellDimension.x, CellDimension.y) / 2, CellDimension);
            return bounds;
        }


        /// <summary>
        /// すべてのセルの中心座標を格納した2次元配列を返します
        /// </summary>
        public Vector3[,] GetCellCenterArray2D()
        {
            Vector3[,] cellCenters = new Vector3[GridDimension.x, GridDimension.y];
            for (int y = 0; y < GridDimension.y; ++y)
                for (int x = 0; x < GridDimension.x; ++x)
                    cellCenters[x, y] = ConvertToWorldPosition(x, y) + new Vector3(CellDimension.x, CellDimension.y) / 2;
            return cellCenters;
        }

        /// <summary>
        /// すべてのセルの中心座標を格納した1次元配列を返します
        /// </summary>
        public Vector3[] GetCellCenterArray1D()
        {
            Vector3[] cellCenters = new Vector3[TotalCellCount];
            for (int y = 0; y < GridDimension.y; ++y)
                for (int x = 0; x < GridDimension.x; ++x)
                    cellCenters[x + y * GridDimension.x] = ConvertToWorldPosition(x, y) + new Vector3(CellDimension.x, CellDimension.y) / 2;
            return cellCenters;
        }


        /// <summary>
        /// セルの中心座標を返します
        /// </summary>
        /// <param name="x">セルのX座標</param>
        /// <param name="y">セルのY座標</param>
        /// <returns>セルが無効な場合は正の無限大を返し、それ以外は実際の座標を返します</returns>
        public Vector3 GetCellCenter(int x, int y)
        {
            if (IsInside(x, y))
                return (ConvertToWorldPosition(x, y) + new Vector3(CellDimension.x, CellDimension.y) / 2);
            return Vector3.positiveInfinity;
        }


        /// <summary>
        /// セルの中心座標を返します
        /// </summary>
        /// <param name="index">セルのインデックス</param>
        /// <returns>セルが無効な場合は正の無限大を返し、有効な場合は実際の座標を返します</returns>
        public Vector3 GetCellCenter(Vector2Int index)
        {
            return GetCellCenter(index.x, index.y);
        }

        /// <summary>
        /// セルの中心座標を返します
        /// </summary>
        /// <param name="worldPosition">セルのワールド座標</param>
        /// <returns>セルが無効な場合は正の無限大を返し、有効な場合は実際の座標を返します</returns>
        public Vector3 GetCellCenter(Vector3 worldPosition)
        {
            ConvertToXY(worldPosition, out int x, out int y);
            return GetCellCenter(x, y);
        }


        /// <summary>
        /// 指定したインデックスのセルに値を設定します
        /// </summary>
        /// <param name="x">x インデックス</param>
        /// <param name="y">y インデックス</param>
        /// <param name="data">設定する値</param>
        public void SetCellDataAt(int x, int y, TType data)
        {
            if (IsInside(x, y))
            {
                _cellArray2D[x, y].Data = data;
            }
        }

        /// <summary>
        /// 指定したインデックスのセルに値を設定します
        /// </summary>
        /// <param name="index">セルのインデックス</param>
        /// <param name="data">設定する値</param>
        public void SetCellDataAt(Vector2Int index, TType data)
        {
            SetCellDataAt(index.x, index.y, data);
        }

        /// <summary>
        /// 指定したワールド座標のセルに値を設定します
        /// </summary>
        /// <param name="worldPosition">ワールド座標</param>
        /// <param name="value">設定する値</param>
        public void SetCellDataAt(Vector3 worldPosition, TType data)
        {
            if (ConvertToXY(worldPosition, out int x, out int y))
            {
                _cellArray2D[x, y].Data = data;
            }
        }


        /// <summary>
        /// 指定したインデックスのセルの有効状態を設定します
        /// </summary>
        /// <param name="x">x インデックス</param>
        /// <param name="y">y インデックス</param>
        public void SetCellValidityAt(int x, int y, bool isValid)
        {
            if (IsInside(x, y))
            {
                _cellArray2D[x, y].IsValid = isValid;
            }
        }

        /// <summary>
        /// 指定したインデックスのセルの有効状態を設定します
        /// </summary>
        /// <param name="index">セルのインデックス</param>
        public void SetCellValidityAt(Vector2Int index, bool isValid)
        {
            SetCellValidityAt(index.x, index.y, isValid);
        }

        /// <summary>
        /// 指定したワールド座標にあるセルの有効状態を設定します
        /// </summary>
        /// <param name="worldPosition">セルのワールド座標</param>
        public void SetCellValidityAt(Vector3 worldPosition, bool isValid)
        {
            if (ConvertToXY(worldPosition, out int x, out int y))
            {
                _cellArray2D[x, y].IsValid = isValid;
            }
        }


        /// <summary>
        /// このグリッドが他のグリッドと重なっている場合は true を返します。
        /// out パラメータには、重なっているすべてのセルのインデックスが格納されます。
        /// </summary>
        /// <param name="otherGrid">重なりを確認する他のグリッド</param>
        /// <param name="overlappedCellsIndeces">重なっているセルのインデックス</param>
        public bool Overlaps(Grid<TType> otherGrid, out Vector2Int[] overlappedCellsIndeces)
        {
            // このグリッドと他のグリッドの境界を取得
            Rect gridABound = this.GetGridBounds();
            Rect gridBBound = otherGrid.GetGridBounds();

            // 2つのグリッドが重なっているか判定
            bool bDoesGridOverlap = gridABound.Overlaps(gridBBound);

            // 重なっているセルのインデックスを格納するリストを初期化
            List<Vector2Int> overlappedCells = new List<Vector2Int>();

            if (bDoesGridOverlap)
            {
                // 自グリッドのセルの境界情報を取得
                var cellBoundsArray = this.GetCellBoundsArray2D();
                var gridDimension = this.GridDimension;

                // グリッドの全セルを走査
                for (int y = 0; y < gridDimension.y; ++y)
                    for (int x = 0; x < gridDimension.x; ++x)
                        // セルが有効かつそのセルの境界が他グリッドの境界と重なっていればリストに追加
                        if (this.GetCellValidity(x, y) && cellBoundsArray[x, y].Overlaps(gridBBound))
                            overlappedCells.Add(new Vector2Int(x, y));

                // 重なったセルのインデックス配列を出力パラメータに設定
                overlappedCellsIndeces = overlappedCells.ToArray();
            }
            else
                // 重なりがなければ null を設定
                overlappedCellsIndeces = null;

            // グリッドが重なっているかどうかを返す
            return bDoesGridOverlap;
        }

        /// <summary>
        /// オーバーライドされたメソッド
        /// /// <returns>グリッドの全セル情報を含む文字列</returns>
        /// </summary>
        public override string ToString()
        {
            string retValue = "";

            // 縦方向にループ
            for (int v = 0; v < GridDimension.y; ++v)
            {
                // 横方向にループして各セルの情報を文字列に追加
                for (int h = 0; h < GridDimension.x; ++h)
                    retValue += _cellArray2D[h, v].ToString() + " ";

                // 行末に改行を追加
                retValue += "\n";
            }
            return retValue;
        }


        // IEnumerable インターフェースの実装

        public IEnumerator<Cell<TType>> GetEnumerator()
        {
            foreach (var cell in _cellArray1D)
            {
                yield return cell;
            }
        }

        // 非ジェネリック版の IEnumerable インターフェースの GetEnumerator メソッドの実装
        // 内部でジェネリック版の GetEnumerator を呼び出して列挙子を取得して返す
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}