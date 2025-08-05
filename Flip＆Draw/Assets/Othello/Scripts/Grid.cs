using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lacobus.Grid
{
    /// <summary>
    ///�J�X�^���O���b�h���쐬���邽�߂̃N���X�ł��B
    /// </summary>
    public class Grid<TType> : IEnumerable<Cell<TType>> where TType : class, new()
    {
        // �t�B�[���h

        // �O���b�h�̌��_�i�����̍��W�j���i�[����ϐ�
        private Vector2 _gridOrigin;

        // �O���b�h�̕��ƍ����i�Z�����j���i�[����ϐ�
        private Vector2Int _gridDimension;

        // 1�Z���̕��ƍ������i�[����ϐ�
        private Vector2 _cellDimension;

        // �O���b�h���̃Z���̑������i�[����ϐ�
        private int _totalCellCount;

        // 2�����z��Ƃ��Ċi�[�����Z���̔z��
        private Cell<TType>[,] _cellArray2D;

        // 1�����z��Ƃ��Ċi�[�����Z���̔z��
        private Cell<TType>[] _cellArray1D;

        // �R���X�g���N�^�[

       /// <summary>
/// �f�t�H���g�R���X�g���N�^�B���_��(0,0,0)�A�Z������1x1�A�Z���T�C�Y��(1,1)�ɐݒ肷��B
/// </summary>
public Grid()
    : this(Vector3.zero, 1, 1, Vector2.one) { }

/// <summary>
/// �R���X�g���N�^�B���_�ƃZ�����i2D�x�N�g���j�A�Z���T�C�Y���w�肵�ăO���b�h������������B
/// </summary>
/// <param name="origin">�O���b�h�̌��_�i���[���h���W�j</param>
/// <param name="cellCount">�Z�����iX������Y�����j</param>
/// <param name="cellSize">1�Z���̃T�C�Y�i���ƍ����j</param>
public Grid(Vector3 origin, Vector2Int cellCount, Vector2 cellSize)
    : this(origin, cellCount.x, cellCount.y, cellSize) { }

/// <summary>
/// �R���X�g���N�^�B���_�A���Z�����A�c�Z�����A�Z���T�C�Y���w�肵�ăO���b�h������������B
/// </summary>
/// <param name="origin">�O���b�h�̌��_�i���[���h���W�j</param>
/// <param name="horizontalCellCount">�������̃Z�����i�Œ�1�j</param>
/// <param name="verticalCellCount">�c�����̃Z�����i�Œ�1�j</param>
/// <param name="cellSize">1�Z���̃T�C�Y�i�Œ�0.01�j</param>
public Grid(Vector3 origin, int horizontalCellCount, int verticalCellCount, Vector2 cellSize)
{
    // �O���b�h�̌��_��ݒ�
    _gridOrigin = origin;

    // �Z�����͍Œ�1�ȏ�ɂȂ�悤�ɐ������ݒ�
    _gridDimension = new Vector2Int(Mathf.Max(horizontalCellCount, 1), Mathf.Max(verticalCellCount, 1));

    // �Z���T�C�Y�͍Œ�0.01�ȏ�ɂȂ�悤�ɐ������ݒ�
    _cellDimension = new Vector2(Mathf.Max(cellSize.x, 0.01f), Mathf.Max(cellSize.y, 0.01f));

    // ���Z�������v�Z
    _totalCellCount = GridDimension.x * GridDimension.y;
}


        // �v���p�e

        /// <summary>
        /// �O���b�h�̌��_��Ԃ��܂�
        /// </summary>
        public Vector2 GridOrigin { get { return _gridOrigin; } set { _gridOrigin = value; } }

        /// <summary>
        /// �O���b�h�̃T�C�Y�i���ƍ����j��Ԃ��܂�
        /// </summary>
        public Vector2Int GridDimension { get { return _gridDimension; } }

        /// <summary>
        /// �Z���̃T�C�Y��Ԃ��܂�
        /// </summary>
        public Vector2 CellDimension { get { return _cellDimension; } }

        /// <summary>
        /// �Z���̑�����Ԃ��܂�
        /// </summary>
        public int TotalCellCount { get { return _totalCellCount; } }


        // �p�u���b�N���\�b�h

        /// <summary>
        /// �O���b�h���g�p����O�ɂ��̃��\�b�h���Ăяo���Ă�������
        /// </summary>
        public void PrepareGrid()
        {
            // 2D�z���1D�z����Z�����Ɋ�Â��Đ���
            _cellArray2D = new Cell<TType>[GridDimension.x, GridDimension.y];
            _cellArray1D = new Cell<TType>[TotalCellCount];

            // �S�Z����������
            for (int y = 0; y < GridDimension.y; y++)
            {
                for (int x = 0; x < GridDimension.x; x++)
                {
                    var cell = new Cell<TType>();
                    // �Z���̃C���f�b�N�X��ݒ�
                    cell.Index = new Vector2Int(x, y);
                    // �Z����L���ɐݒ�
                    cell.IsValid = true;
                    // �Z���̃f�[�^��V��������
                    cell.Data = new TType();

                    // 2D�z���1D�z��ɃZ�����i�[
                    _cellArray2D[x, y] = cell;
                    _cellArray1D[x + y * GridDimension.x] = cell;
                }
            }
        }


        /// <summary>
        /// OnDrawGizmos����Ăяo���āA�O���b�h�̃M�Y����`�悷�郁�\�b�h
        /// </summary>
        public void DrawGridLines(Color gridLineColor, Color crossLineColor)
        {
            Gizmos.color = gridLineColor;

            Vector2 corner = GridOrigin + new Vector2(GridDimension.x * CellDimension.x, GridDimension.y * CellDimension.y);

            // ���̃��C���i�㉺�̘g���j
            Gizmos.DrawLine(GridOrigin, new Vector2(corner.x, GridOrigin.y));
            Gizmos.DrawLine(new Vector2(GridOrigin.x, corner.y), corner);

            // �c�̃��C���i���E�̘g���j
            Gizmos.DrawLine(GridOrigin, new Vector2(GridOrigin.x, corner.y));
            Gizmos.DrawLine(new Vector2(corner.x, GridOrigin.y), corner);

            // ���̃O���b�h���i�����̐������j
            for (int h = 1; h < GridDimension.y; ++h)
                Gizmos.DrawLine
                    (
                        GridOrigin + (Vector2.up * h * CellDimension.y),
                        new Vector2(corner.x, GridOrigin.y) + (Vector2.up * h * CellDimension.y)
                    );

            // �c�̃O���b�h���i�����̐������j
            for (int w = 1; w < GridDimension.x; ++w)
                Gizmos.DrawLine
                    (
                        GridOrigin + (Vector2.right * w * CellDimension.x),
                        new Vector2(GridOrigin.x, corner.y) + (Vector2.right * w * CellDimension.x)
                    );
            // �N���X���C���̐F�ɐ؂�ւ��i���̌�̕`��p�j
            Gizmos.color = crossLineColor;
        }



        /// <summary>
        /// �O���b�h���̃Z�����̂�Ԃ��܂�
        /// </summary>
        /// <param name="x">x���W�C���f�b�N�X</param>
        /// <param name="y">y���W�C���f�b�N�X</param>
        public Cell<TType> GetCellRaw(int x, int y)
        {
            // �w�肵�����W (x, y) ���O���b�h���ɂ���ꍇ�͊Y���Z����Ԃ�
            if (IsInside(x, y))
                return _cellArray2D[x, y];
            // �O���b�h�O�̏ꍇ�� null ��Ԃ�
            return null;
        }

        /// <summary>
        /// �O���b�h���̃Z�����̂�Ԃ��܂�
        /// </summary>
        /// <param name="index">�Z���̃C���f�b�N�X</param>
        public Cell<TType> GetCellRaw(Vector2Int index)
        {
            // �C���f�b�N�X�� x, y ���g���� GetCellRaw ���\�b�h���Ăяo���A�Z�����擾���ĕԂ�
            return GetCellRaw(index.x, index.y);
        }

        /// <summary>
        /// �O���b�h���̃Z�����̂�Ԃ��܂�
        /// </summary>
        /// <param name="worldPosition">���[���h���W�̈ʒu</param>
        public Cell<TType> GetCellRaw(Vector3 worldPosition)
        {
            // worldPosition ���O���b�h�� x, y �C���f�b�N�X�ɕϊ��ł�����A���̃Z����Ԃ�
            // �ϊ��ł��Ȃ���� null ��Ԃ�
            if (ConvertToXY(worldPosition, out int x, out int y))
                return _cellArray2D[x, y];
            return null;
        }



        /// <summary>
        /// 2�����Z���z�񎩑̂�Ԃ��܂�
        /// </summary>
        public Cell<TType>[,] GetCellArray2D()
        {
            // 2D �z��Ƃ��Ă̑S�Z����Ԃ�
            return _cellArray2D;
        }

        /// <summary>
        /// 1�����Z���z�񎩑̂�Ԃ��܂�
        /// </summary>
        public Cell<TType>[] GetCellArray1D()
        {
            return _cellArray1D;
        }

        /// <summary>
        /// �O���b�h�Z���̃f�[�^��Ԃ��܂�
        /// </summary>
        /// <param name="x">x���W�C���f�b�N�X</param>
        /// <param name="y">y���W�C���f�b�N�X</param>
        public TType GetCellData(int x, int y)
        {
            // x, y ���O���b�h���ɂ���ꍇ�A���̃Z���̃f�[�^��Ԃ�
            // �O���b�h�O�̏ꍇ�� null ��Ԃ�
            if (IsInside(x, y))
                return _cellArray2D[x, y].Data;
            return null;
        }

        /// <summary>
        /// �O���b�h�Z���̃f�[�^��Ԃ��܂�
        /// </summary>
        /// <param name="index">�Z���̃C���f�b�N�X</param>
        public TType GetCellData(Vector2Int index)
        {
            // index �� x, y ���g���ăZ���̃f�[�^���擾���ĕԂ�
            return GetCellData(index.x, index.y);
        }

        /// <summary>
        /// �O���b�h�Z���̃f�[�^��Ԃ��܂�
        /// </summary>
        /// <param name="worldPosition">�Z���̃��[���h���W�ʒu</param>
        public TType GetCellData(Vector3 worldPosition)
        {
            // ���[���h���W���O���b�h�̃C���f�b�N�X�ix, y�j�ɕϊ����A���̃Z���̃f�[�^���擾���ĕԂ�
            ConvertToXY(worldPosition, out int x, out int y);
            return GetCellData(x, y);
        }


        /// <summary>
        /// �Z���̗L������Ԃ��܂�
        /// </summary>
        /// <param name="x">x �C���f�b�N�X</param>
        /// <param name="y">y �C���f�b�N�X</param>
        public bool GetCellValidity(int x, int y)
        {
            // �w�肵���C���f�b�N�X(x, y)���O���b�h���ɂ���ꍇ�A���̃Z���̗L����(IsValid)��Ԃ�
            // �O���b�h�O�̏ꍇ�� false ��Ԃ�
            if (IsInside(x, y))
                return _cellArray2D[x, y].IsValid;
            return false;
        }

        /// <summary>
        /// �Z���̗L������Ԃ��܂�
        /// </summary>
        /// <param name="index">�Z���̃C���f�b�N�X</param>
        public bool GetCellValidity(Vector2Int index)
        {
            // �w�肵���C���f�b�N�X(x, y)���O���b�h���ɂ���ꍇ�A���̃Z���̗L����(IsValid)��Ԃ�
            // �O���b�h�O�̏ꍇ�� false ��Ԃ�
            return GetCellValidity(index.x, index.y);
        }

        /// <summary>
        /// �Z���̗L������Ԃ��܂�
        /// </summary>
        /// <param name="worldPosition">���̃Z���̃��[���h���W</param>
        public bool GetCellValidity(Vector3 worldPosition)
        {
            // worldPosition ���O���b�h���W�ɕϊ����A�O���b�h���ł���ΊY���Z���̗L����(IsValid)��Ԃ�
            // �O���b�h�O�̏ꍇ�� false ��Ԃ�
            if (ConvertToXY(worldPosition, out int x, out int y))
                return _cellArray2D[x, y].IsValid;
            return false;
        }


        /// <summary>
        /// x �� y ���O���b�h���ɂ���ꍇ�� true ��Ԃ��܂�
        /// </summary>
        /// <param name="x">X �C���f�b�N�X</param>
        /// <param name="y">Y �C���f�b�N�X</param>
        public bool IsInside(int x, int y)
        {
            // x �� y ���O���b�h�͈͓̔��ɂ��邩�`�F�b�N���A�͈͓��Ȃ� true ��Ԃ�
            // �͈͊O�̏ꍇ�� false ��Ԃ�
            if (x >= 0 && y >= 0 && x < GridDimension.x && y < GridDimension.y)
                return true;
            return false;
        }

        /// <summary>
        /// �w�肵�����W���O���b�h���ɂ���ꍇ�� true ��Ԃ��܂�
        /// </summary>
        /// <param name="worldPosition">�Ώۂ̃��[���h���W</param>
        public bool IsInside(Vector3 worldPosition)
        {
            ConvertToXY(worldPosition, out int x, out int y);
            return IsInside(x, y);
        }

        /// <summary>
        /// �w�肵���C���f�b�N�X���O���b�h���ɂ���ꍇ�� true ��Ԃ��܂�
        /// </summary>
        /// <param name="index">�Ώۂ̃C���f�b�N�X</param>
        public bool IsInside(Vector2Int index)
        {
            return IsInside(index.x, index.y);
        }



        /// <summary>
        /// ���[���h���W���O���b�h�̃Z�N�V�����ɕϊ����܂�
        /// </summary>
        /// <param name="worldPosition">�Ώۂ̈ʒu</param>
        /// <param name="x">x���W�̏o�̓p�����[�^</param>
        /// <param name="y">y���W�̏o�̓p�����[�^</param>
        /// <returns>�L���ȃ|�C���g�ł���� true ��Ԃ��܂�</returns>
        public bool ConvertToXY(Vector3 worldPosition, out int x, out int y)
        {
            // worldPosition ���O���b�h�̃C���f�b�N�X�ɕϊ����A���̌��ʂ� index �Ɋi�[����
            bool ret = ConvertToXY(worldPosition, out Vector2Int index);
            // index �� x �� y �����ꂼ�� x �� y �ɑ������
            x = index.x;
            y = index.y;
            // �ϊ��������������ǂ����� true/false �ŕԂ�
            return ret;
        }

        /// <summary>
        /// ���[���h���W���O���b�h�̃Z�N�V�����ɕϊ����܂�
        /// </summary>
        /// <param name="worldPosition">�Ώۂ̈ʒu</param>
        /// <param name="isInside">���̃��[���h���W���O���b�h���ɂ���ꍇ�� true �ɂȂ�܂�</param>
        /// <returns>�ϊ����ꂽ�C���f�b�N�X��Ԃ��܂�</returns>
        public Vector2Int ConvertToXY(Vector3 worldPosition, out bool isInsde)
        {
            isInsde = ConvertToXY(worldPosition, out Vector2Int index);
            return index;
        }

        /// <summary>
        /// ���[���h���W���O���b�h�̃Z�N�V�����ɕϊ����܂�
        /// </summary>
        /// <param name="worldPosition">�Ώۂ̈ʒu</param>
        /// <param name="x">x���W�̏o�̓p�����[�^�[</param>
        /// <param name="y">y���W�̏o�̓p�����[�^�[</param>
        /// <returns>�L���Ȉʒu�ł���� true ��Ԃ��܂�</returns>
        public bool ConvertToXY(Vector3 worldPosition, out Vector2Int index)
        {
            // worldPosition ���O���b�h�̌��_����̑��΍��W�ɕϊ����A�Z���T�C�Y�Ŋ��邱�ƂŃZ���̃C���f�b�N�X���v�Z����
            int x = Mathf.FloorToInt((worldPosition - (Vector3)GridOrigin).x / CellDimension.x);
            int y = Mathf.FloorToInt((worldPosition - (Vector3)GridOrigin).y / CellDimension.y);

            // �v�Z�����C���f�b�N�X�� Vector2Int �^�� index �Ɋi�[����
            index = new Vector2Int(x, y);

            // �v�Z�����C���f�b�N�X���O���b�h�͈͓̔����ǂ����𔻒肵�A���̌��ʂ�Ԃ�
            return IsInside(x, y);
        }


        /// <summary>
        /// �O���b�h�̃Z�N�V���������[���h���W�ɕϊ����܂�
        /// </summary>
        /// <param name="x">�Ώۂ�x���W</param>
        /// <param name="y">�Ώۂ�y���W</param>
        public Vector3 ConvertToWorldPosition(int x, int y)
        {
            return new Vector2(x * CellDimension.x, y * CellDimension.y) + GridOrigin;
        }

        /// <summary>
        /// �O���b�h�̃Z�N�V���������[���h���W�ɕϊ����܂�
        /// </summary>
        /// <param name="x">�Ώۂ�x���W</param>
        /// <param name="y">�Ώۂ�y���W</param>
        public Vector3 ConvertToWorldPosition(Vector2Int index)
        {
            return new Vector2(index.x * CellDimension.x, index.y * CellDimension.y) + GridOrigin;
        }


        /// <summary>
        /// �O���b�h�̋��E��Ԃ��܂�
        /// </summary>
        public Rect GetGridBounds()
        {
            return new Rect(ConvertToWorldPosition(0, 0), new Vector2(CellDimension.x * GridDimension.x, CellDimension.y * GridDimension.y));
        }


        /// <summary>
        /// �w�肵���Z���̋��E��Ԃ��܂�
        /// </summary>
        /// <param name="x">X �C���f�b�N�X</param>
        /// <param name="y">Y �C���f�b�N�X</param>
        /// <returns>�����ȏꍇ�̓[����Ԃ��܂�</returns>
        public Rect GetCellBounds(int x, int y)
        {
            if (IsInside(x, y))
                return new Rect(GetCellCenter(x, y), CellDimension);
            return Rect.zero;
        }
        /// <summary>
        /// �w�肵���Z���̋��E��Ԃ��܂�
        /// </summary>
        /// <param name="index">�Z���̃C���f�b�N�X</param>
        /// <returns>�����ȏꍇ�̓[����Ԃ��܂�</returns>
        public Rect GetCellBounds(Vector2Int index)
        {
            return GetCellBounds(index.x, index.y);
        }


        /// <summary>
        /// 2�����z����̂��ׂẴZ���̋��E��Ԃ��܂�
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
        /// 1�����z����̂��ׂẴZ���̋��E��Ԃ��܂�
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
        /// ���ׂẴZ���̒��S���W���i�[����2�����z���Ԃ��܂�
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
        /// ���ׂẴZ���̒��S���W���i�[����1�����z���Ԃ��܂�
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
        /// �Z���̒��S���W��Ԃ��܂�
        /// </summary>
        /// <param name="x">�Z����X���W</param>
        /// <param name="y">�Z����Y���W</param>
        /// <returns>�Z���������ȏꍇ�͐��̖������Ԃ��A����ȊO�͎��ۂ̍��W��Ԃ��܂�</returns>
        public Vector3 GetCellCenter(int x, int y)
        {
            if (IsInside(x, y))
                return (ConvertToWorldPosition(x, y) + new Vector3(CellDimension.x, CellDimension.y) / 2);
            return Vector3.positiveInfinity;
        }


        /// <summary>
        /// �Z���̒��S���W��Ԃ��܂�
        /// </summary>
        /// <param name="index">�Z���̃C���f�b�N�X</param>
        /// <returns>�Z���������ȏꍇ�͐��̖������Ԃ��A�L���ȏꍇ�͎��ۂ̍��W��Ԃ��܂�</returns>
        public Vector3 GetCellCenter(Vector2Int index)
        {
            return GetCellCenter(index.x, index.y);
        }

        /// <summary>
        /// �Z���̒��S���W��Ԃ��܂�
        /// </summary>
        /// <param name="worldPosition">�Z���̃��[���h���W</param>
        /// <returns>�Z���������ȏꍇ�͐��̖������Ԃ��A�L���ȏꍇ�͎��ۂ̍��W��Ԃ��܂�</returns>
        public Vector3 GetCellCenter(Vector3 worldPosition)
        {
            ConvertToXY(worldPosition, out int x, out int y);
            return GetCellCenter(x, y);
        }


        /// <summary>
        /// �w�肵���C���f�b�N�X�̃Z���ɒl��ݒ肵�܂�
        /// </summary>
        /// <param name="x">x �C���f�b�N�X</param>
        /// <param name="y">y �C���f�b�N�X</param>
        /// <param name="data">�ݒ肷��l</param>
        public void SetCellDataAt(int x, int y, TType data)
        {
            if (IsInside(x, y))
            {
                _cellArray2D[x, y].Data = data;
            }
        }

        /// <summary>
        /// �w�肵���C���f�b�N�X�̃Z���ɒl��ݒ肵�܂�
        /// </summary>
        /// <param name="index">�Z���̃C���f�b�N�X</param>
        /// <param name="data">�ݒ肷��l</param>
        public void SetCellDataAt(Vector2Int index, TType data)
        {
            SetCellDataAt(index.x, index.y, data);
        }

        /// <summary>
        /// �w�肵�����[���h���W�̃Z���ɒl��ݒ肵�܂�
        /// </summary>
        /// <param name="worldPosition">���[���h���W</param>
        /// <param name="value">�ݒ肷��l</param>
        public void SetCellDataAt(Vector3 worldPosition, TType data)
        {
            if (ConvertToXY(worldPosition, out int x, out int y))
            {
                _cellArray2D[x, y].Data = data;
            }
        }


        /// <summary>
        /// �w�肵���C���f�b�N�X�̃Z���̗L����Ԃ�ݒ肵�܂�
        /// </summary>
        /// <param name="x">x �C���f�b�N�X</param>
        /// <param name="y">y �C���f�b�N�X</param>
        public void SetCellValidityAt(int x, int y, bool isValid)
        {
            if (IsInside(x, y))
            {
                _cellArray2D[x, y].IsValid = isValid;
            }
        }

        /// <summary>
        /// �w�肵���C���f�b�N�X�̃Z���̗L����Ԃ�ݒ肵�܂�
        /// </summary>
        /// <param name="index">�Z���̃C���f�b�N�X</param>
        public void SetCellValidityAt(Vector2Int index, bool isValid)
        {
            SetCellValidityAt(index.x, index.y, isValid);
        }

        /// <summary>
        /// �w�肵�����[���h���W�ɂ���Z���̗L����Ԃ�ݒ肵�܂�
        /// </summary>
        /// <param name="worldPosition">�Z���̃��[���h���W</param>
        public void SetCellValidityAt(Vector3 worldPosition, bool isValid)
        {
            if (ConvertToXY(worldPosition, out int x, out int y))
            {
                _cellArray2D[x, y].IsValid = isValid;
            }
        }


        /// <summary>
        /// ���̃O���b�h�����̃O���b�h�Əd�Ȃ��Ă���ꍇ�� true ��Ԃ��܂��B
        /// out �p�����[�^�ɂ́A�d�Ȃ��Ă��邷�ׂẴZ���̃C���f�b�N�X���i�[����܂��B
        /// </summary>
        /// <param name="otherGrid">�d�Ȃ���m�F���鑼�̃O���b�h</param>
        /// <param name="overlappedCellsIndeces">�d�Ȃ��Ă���Z���̃C���f�b�N�X</param>
        public bool Overlaps(Grid<TType> otherGrid, out Vector2Int[] overlappedCellsIndeces)
        {
            // ���̃O���b�h�Ƒ��̃O���b�h�̋��E���擾
            Rect gridABound = this.GetGridBounds();
            Rect gridBBound = otherGrid.GetGridBounds();

            // 2�̃O���b�h���d�Ȃ��Ă��邩����
            bool bDoesGridOverlap = gridABound.Overlaps(gridBBound);

            // �d�Ȃ��Ă���Z���̃C���f�b�N�X���i�[���郊�X�g��������
            List<Vector2Int> overlappedCells = new List<Vector2Int>();

            if (bDoesGridOverlap)
            {
                // ���O���b�h�̃Z���̋��E�����擾
                var cellBoundsArray = this.GetCellBoundsArray2D();
                var gridDimension = this.GridDimension;

                // �O���b�h�̑S�Z���𑖍�
                for (int y = 0; y < gridDimension.y; ++y)
                    for (int x = 0; x < gridDimension.x; ++x)
                        // �Z�����L�������̃Z���̋��E�����O���b�h�̋��E�Əd�Ȃ��Ă���΃��X�g�ɒǉ�
                        if (this.GetCellValidity(x, y) && cellBoundsArray[x, y].Overlaps(gridBBound))
                            overlappedCells.Add(new Vector2Int(x, y));

                // �d�Ȃ����Z���̃C���f�b�N�X�z����o�̓p�����[�^�ɐݒ�
                overlappedCellsIndeces = overlappedCells.ToArray();
            }
            else
                // �d�Ȃ肪�Ȃ���� null ��ݒ�
                overlappedCellsIndeces = null;

            // �O���b�h���d�Ȃ��Ă��邩�ǂ�����Ԃ�
            return bDoesGridOverlap;
        }

        /// <summary>
        /// �I�[�o�[���C�h���ꂽ���\�b�h
        /// /// <returns>�O���b�h�̑S�Z�������܂ޕ�����</returns>
        /// </summary>
        public override string ToString()
        {
            string retValue = "";

            // �c�����Ƀ��[�v
            for (int v = 0; v < GridDimension.y; ++v)
            {
                // �������Ƀ��[�v���Ċe�Z���̏��𕶎���ɒǉ�
                for (int h = 0; h < GridDimension.x; ++h)
                    retValue += _cellArray2D[h, v].ToString() + " ";

                // �s���ɉ��s��ǉ�
                retValue += "\n";
            }
            return retValue;
        }


        // IEnumerable �C���^�[�t�F�[�X�̎���

        public IEnumerator<Cell<TType>> GetEnumerator()
        {
            foreach (var cell in _cellArray1D)
            {
                yield return cell;
            }
        }

        // ��W�F�l���b�N�ł� IEnumerable �C���^�[�t�F�[�X�� GetEnumerator ���\�b�h�̎���
        // �����ŃW�F�l���b�N�ł� GetEnumerator ���Ăяo���ė񋓎q���擾���ĕԂ�
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}