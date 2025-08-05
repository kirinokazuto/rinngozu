using UnityEngine;


namespace Lacobus.Grid
{
    public class Cell<TType> where TType : class
    {
        // �t�B�[���h

        private bool _isValid;
        private TType _data;
        private Vector2Int _index;


        // �v���p�e�B

        /// <summary>
        /// ���̃Z�����g�p�\�i�L���j�ł���� true ��Ԃ��܂�
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
        /// ���̃Z���Ɋi�[����Ă���f�[�^��Ԃ��܂�
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
        /// ���̃Z���̃C���f�b�N�X��Ԃ��܂�
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


        // �p�u���b�N���\�b�h

        /// <summary>
        /// �Z�����̒l�𕶎���Ƃ��Đ��`����I�[�o�[���C�h���\�b�h�ł�
        /// </summary>
        public override string ToString() => (Data == null ? "-" : Data.ToString()) + ":" + IsValid;

        /// <summary>
        /// �Z���̃f�[�^�ƃC���f�b�N�X�������ł���� true ��Ԃ��܂�
        /// </summary>
        /// <param name="anotherCell">Cell you want to compare against</param>
        public bool IsSame(Cell<TType> anotherCell)
        {
            return Data.Equals(anotherCell.Data) && Index == anotherCell.Index;
        }
    }
}