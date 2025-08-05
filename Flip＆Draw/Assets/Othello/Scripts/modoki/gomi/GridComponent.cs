/*using UnityEngine;


namespace Lacobus.Grid
{
    public sealed class GridComponent : MonoBehaviour
    {
        // フィールド 

        [SerializeField] private GridComponentDataContainer _gcData;
        // グリッドの設定データを保持するコンテナ。インスペクターから設定可能。

        [SerializeField] private bool _useSimpleSpriteRendering = false;
        // 単純なスプライト描画モードを使用するかどうかのフラグ。

        [SerializeField] private Sprite _defaultSimpleSprite = null;
        // 単純なスプライト描画を使用する場合のデフォルトスプライト。


        private Grid<DefaultCell> _grid = null;
        // グリッドデータを保持する変数。DefaultCell型のセルを管理。

        private Transform _t;
        // このオブジェクトのTransformコンポーネントをキャッシュするための変数。


        // プロパティ

        private Transform t
        {
            get
            {
                if (_t)
                    return _t;
                else
                {
                    _t = transform;
                    return _t;
                }
            }
        }
        private Vector2 gridOrigin
        {
            get
            {
                return _gcData.gridOffset + (Vector2)transform.position;
            }
        }
        public Grid<DefaultCell> Grid { get { return _grid; } }


        // 公開メソッド

        /// <summary>
        /// 特定のインデックスのスプライトを変更するためにこのメソッドを呼び出します
        /// </summary>
        /// <param name="index">対象のインデックス</param>
        /// <param name="targetSprite">変更するスプライト</param>
        public void SetSpriteAt(Vector2Int index, Sprite targetSprite)
        {
            if (_grid.IsInside(index))
                _grid.GetCellData(index).ChangeSprite(targetSprite);
        }

        /// <summary>
        /// 特定のインデックス位置のスプライトを変更するためにこのメソッドを呼び出します
        /// </summary>
        /// <param name="x">X座標のインデックス</param>
        /// <param name="y">Y座標のインデックス</param>
        /// <param name="targetSprite">変更するスプライト</param>
        public void SetSpriteAt(int x, int y, Sprite targetSprite)
        {
            if (_grid.IsInside(x, y))
                _grid.GetCellData(x, y).ChangeSprite(targetSprite);
        }

        /// <summary>
        /// 特定のワールド座標上のスプライトを変更するためにこのメソッドを呼び出します
        /// </summary>
        /// <param name="worldPosition">対象のワールド座標</param>
        /// <param name="targetSprite">変更するスプライト</param>
        public void SetSpriteAt(Vector3 worldPosition, Sprite targetSprite)
        {
            if (_grid.IsInside(worldPosition))
                _grid.GetCellData(worldPosition).ChangeSprite(targetSprite);
        }



        /// <summary>
        /// 特定のインデックス位置のスプライトの色を変更するためにこのメソッドを呼び出します
        /// </summary>
        /// <param name="index">対象のインデックス</param>
        /// <param name="targetColor">変更する色</param>
        public void SetSpriteColorAt(Vector2Int index, Color targetColor)
        {
            if (_grid.IsInside(index))
                _grid.GetCellData(index).ChangeColor(targetColor);
        }

        /// <summary>
        /// 特定のインデックス位置のスプライトの色を変更するためにこのメソッドを呼び出します
        /// </summary>
        /// <param name="x">X座標のインデックス</param>
        /// <param name="y">Y座標のインデックス</param>
        /// <param name="targetColor">変更する色</param>
        public void SetSpriteColorAt(int x, int y, Color targetColor)
        {
            if (_grid.IsInside(x, y))
                _grid.GetCellData(x, y).ChangeColor(targetColor);
        }

        /// <summary>
        /// 特定のワールド座標上のスプライトの色を変更するためにこのメソッドを呼び出します
        /// </summary>
        /// <param name="worldPosition">対象のワールド座標</param>
        /// <param name="targetColor">変更する色</param>
        public void SetSpriteColorAt(Vector3 worldPosition, Color targetColor)
        {
            if (_grid.IsInside(worldPosition))
                _grid.GetCellData(worldPosition).ChangeColor(targetColor);
        }



        /// <summary>
        /// 特定のインデックス位置のスプライトの色を変更するためにこのメソッドを呼び出します
        /// </summary>
        /// <param name="index">対象のインデックス</param>
        /// <param name="size">対象のサイズ</param>
        public void SetSpriteSizeAt(Vector2Int index, Vector2 size)
        {
            if (_grid.IsInside(index))
                _grid.GetCellData(index).ChangeSpriteSize(size);
        }

        /// <summary>
        /// 特定のインデックス位置のスプライトの色を変更するためにこのメソッドを呼び出します
        /// </summary>
        /// <param name="x">X座標のインデックス</param>
        /// <param name="y">Y座標のインデックス</param>
        /// <param name="size">対象のサイズ</param>
        public void SetSpriteSizeAt(int x, int y, Vector2 size)
        {
            if (_grid.IsInside(x, y))
                _grid.GetCellData(x, y).ChangeSpriteSize(size);
        }

        /// <summary>
        /// 特定のワールド座標上のスプライトの色を変更するためにこのメソッドを呼び出します
        /// </summary>
        /// <param name="worldPosition">対象のワールド座標</param>
        /// <param name="size">対象のサイズ</param>
        public void SetSpriteSizeAt(Vector2 worldPosition, Vector2 size)
        {
            if (_grid.IsInside(worldPosition))
                _grid.GetCellData(worldPosition).ChangeSpriteSize(size);
        }


        //  ライフサイクルメソッド

        private void Awake()
        {
            _t = transform;

            // Create grid here
            _grid = new Grid<DefaultCell>(gridOrigin, _gcData.gridDimension, _gcData.cellDimension);
            _grid.PrepareGrid();

            if (_useSimpleSpriteRendering)
                setupSimpleSpriteRendering();
        }

        private void Update()
        {
            // 毎フレーム、グリッドの原点を更新する
            _grid.GridOrigin = _gcData.gridOffset + (Vector2)_t.position;
        }

        private void OnValidate()
        {
            // インスペクター上で値が変更されたときにグリッドを再作成する
            _grid = new Grid<DefaultCell>(gridOrigin, _gcData.gridDimension, _gcData.cellDimension);
        }

        private void OnDrawGizmos()
        {
            if (_gcData.shouldDrawGizmos == false)
                return;// Gizmosを描画しない設定なら何もしない

            // グリッドの原点を更新してからグリッド線を描画する
            _grid.GridOrigin = _gcData.gridOffset + (Vector2)transform.position;
            _grid.DrawGridLines(_gcData.gridLineColor, _gcData.crossLineColor);
        }

        private void setupSimpleSpriteRendering()
        {
            // グリッド内の全セルをループ処理
            foreach (var c in _grid)
            {
                // セルのインデックスを名前にした新しいGameObjectを作成し、SpriteRendererコンポーネントを付加
                GameObject go = new GameObject($"{c.Index}", typeof(SpriteRenderer));

                // 作成したGameObjectのSpriteRendererをセルのデータに保存
                c.Data.sr = go.GetComponent<SpriteRenderer>();

                // デフォルトのスプライトをSpriteRendererに設定
                c.Data.sr.sprite = _defaultSimpleSprite;

                // 新しいGameObjectをこのオブジェクトの子に設定
                go.transform.parent = _t;

                // セルの中心位置にGameObjectを配置
                go.transform.position = _grid.GetCellCenter(c.Index);

                // セルの大きさに合わせてGameObjectのスケールを設定
                go.transform.localScale = _grid.CellDimension;
            }
        }


        // // ネストされた型

        private enum OffsetType
        {
            Preset,  // 定型（プリセット）
            Custom   // カスタム
        }

        private enum PresetTypes
        {
            TopRight,      // 右上
            TopCenter,     // 上中央
            TopLeft,       // 左上
            MiddleRight,   // 右中央
            MiddleCenter,  // 中央
            MiddleLeft,    // 左中央
            BottomRight,   // 右下
            BottomCenter,  // 下中央
            BottomLeft     // 左下
        }

        [System.Serializable]
        private class GridComponentDataContainer
        {
            // グリッド関連の設定
            [SerializeField]
            public Vector2Int gridDimension = new Vector2Int();  // グリッドのサイズ（セル数）
            [SerializeField]
            public Vector2 cellDimension = new Vector2();        // セルのサイズ
            [SerializeField]
            public Vector2 gridOffset = new Vector2();           // グリッドのオフセット（位置調整）

            // Gizmosやエディタ関連の設定
            [SerializeField]
            public OffsetType offsetType = OffsetType.Preset;    // オフセットのタイプ（プリセット or カスタム）
            [SerializeField]
            public PresetTypes presetType = PresetTypes.BottomLeft;  // プリセットの種類（位置）
            [SerializeField]
            public bool shouldDrawGizmos = false;                // Gizmosを描画するかどうか
            [SerializeField]
            public Color gridLineColor;                           // グリッド線の色
            [SerializeField]
            public Color crossLineColor;                          // クロスラインの色
        }

        public class DefaultCell
        {
            // フィールド

            public SpriteRenderer sr;  // セルに紐づくSpriteRenderer


            // 公開メソッド 

            public void ChangeSprite(Sprite sprite)
            {
                sr.sprite = sprite;  // スプライトを変更する
            }

            public void ChangeColor(Color color)
            {
                sr.color = color;    // スプライトの色を変更する
            }

            public void ChangeSpriteSize(Vector2 size)
            {
                sr.transform.localScale = size;  // スプライトのサイズ（スケール）を変更する
            }
        }
    }
}*/