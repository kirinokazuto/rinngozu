using UnityEngine;


namespace Lacobus.Grid
{
    public class Cell<TType> where TType : class
    {
        // フィールド

        private bool _isValid;
        private TType _data;
        private Vector2Int _index;


        // プロパティ

        /// <summary>
        /// このセルが使用可能（有効）であれば true を返します
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
            }
        }

        /// <summary>
        /// このセルに格納されているデータを返します
        /// </summary>
        public TType Data
        {
            get
            {
                return _data;
            }
            set
            {
                if (_isValid)
                    _data = value;
            }
        }

        /// <summary>
        /// このセルのインデックスを返します
        /// </summary>
        public Vector2Int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }


        // パブリックメソッド

        /// <summary>
        /// セル内の値を文字列として整形するオーバーライドメソッドです
        /// </summary>
        public override string ToString() => (Data == null ? "-" : Data.ToString()) + ":" + IsValid;

        /// <summary>
        /// セルのデータとインデックスが同じであれば true を返します
        /// </summary>
        /// <param name="anotherCell">Cell you want to compare against</param>
        public bool IsSame(Cell<TType> anotherCell)
        {
            return Data.Equals(anotherCell.Data) && Index == anotherCell.Index;
        }
    }
}