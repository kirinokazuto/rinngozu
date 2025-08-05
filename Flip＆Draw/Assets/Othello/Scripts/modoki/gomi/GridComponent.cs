/*using UnityEngine;


namespace Lacobus.Grid
{
    public sealed class GridComponent : MonoBehaviour
    {
        // �t�B�[���h 

        [SerializeField] private GridComponentDataContainer _gcData;
        // �O���b�h�̐ݒ�f�[�^��ێ�����R���e�i�B�C���X�y�N�^�[����ݒ�\�B

        [SerializeField] private bool _useSimpleSpriteRendering = false;
        // �P���ȃX�v���C�g�`�惂�[�h���g�p���邩�ǂ����̃t���O�B

        [SerializeField] private Sprite _defaultSimpleSprite = null;
        // �P���ȃX�v���C�g�`����g�p����ꍇ�̃f�t�H���g�X�v���C�g�B


        private Grid<DefaultCell> _grid = null;
        // �O���b�h�f�[�^��ێ�����ϐ��BDefaultCell�^�̃Z�����Ǘ��B

        private Transform _t;
        // ���̃I�u�W�F�N�g��Transform�R���|�[�l���g���L���b�V�����邽�߂̕ϐ��B


        // �v���p�e�B

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


        // ���J���\�b�h

        /// <summary>
        /// ����̃C���f�b�N�X�̃X�v���C�g��ύX���邽�߂ɂ��̃��\�b�h���Ăяo���܂�
        /// </summary>
        /// <param name="index">�Ώۂ̃C���f�b�N�X</param>
        /// <param name="targetSprite">�ύX����X�v���C�g</param>
        public void SetSpriteAt(Vector2Int index, Sprite targetSprite)
        {
            if (_grid.IsInside(index))
                _grid.GetCellData(index).ChangeSprite(targetSprite);
        }

        /// <summary>
        /// ����̃C���f�b�N�X�ʒu�̃X�v���C�g��ύX���邽�߂ɂ��̃��\�b�h���Ăяo���܂�
        /// </summary>
        /// <param name="x">X���W�̃C���f�b�N�X</param>
        /// <param name="y">Y���W�̃C���f�b�N�X</param>
        /// <param name="targetSprite">�ύX����X�v���C�g</param>
        public void SetSpriteAt(int x, int y, Sprite targetSprite)
        {
            if (_grid.IsInside(x, y))
                _grid.GetCellData(x, y).ChangeSprite(targetSprite);
        }

        /// <summary>
        /// ����̃��[���h���W��̃X�v���C�g��ύX���邽�߂ɂ��̃��\�b�h���Ăяo���܂�
        /// </summary>
        /// <param name="worldPosition">�Ώۂ̃��[���h���W</param>
        /// <param name="targetSprite">�ύX����X�v���C�g</param>
        public void SetSpriteAt(Vector3 worldPosition, Sprite targetSprite)
        {
            if (_grid.IsInside(worldPosition))
                _grid.GetCellData(worldPosition).ChangeSprite(targetSprite);
        }



        /// <summary>
        /// ����̃C���f�b�N�X�ʒu�̃X�v���C�g�̐F��ύX���邽�߂ɂ��̃��\�b�h���Ăяo���܂�
        /// </summary>
        /// <param name="index">�Ώۂ̃C���f�b�N�X</param>
        /// <param name="targetColor">�ύX����F</param>
        public void SetSpriteColorAt(Vector2Int index, Color targetColor)
        {
            if (_grid.IsInside(index))
                _grid.GetCellData(index).ChangeColor(targetColor);
        }

        /// <summary>
        /// ����̃C���f�b�N�X�ʒu�̃X�v���C�g�̐F��ύX���邽�߂ɂ��̃��\�b�h���Ăяo���܂�
        /// </summary>
        /// <param name="x">X���W�̃C���f�b�N�X</param>
        /// <param name="y">Y���W�̃C���f�b�N�X</param>
        /// <param name="targetColor">�ύX����F</param>
        public void SetSpriteColorAt(int x, int y, Color targetColor)
        {
            if (_grid.IsInside(x, y))
                _grid.GetCellData(x, y).ChangeColor(targetColor);
        }

        /// <summary>
        /// ����̃��[���h���W��̃X�v���C�g�̐F��ύX���邽�߂ɂ��̃��\�b�h���Ăяo���܂�
        /// </summary>
        /// <param name="worldPosition">�Ώۂ̃��[���h���W</param>
        /// <param name="targetColor">�ύX����F</param>
        public void SetSpriteColorAt(Vector3 worldPosition, Color targetColor)
        {
            if (_grid.IsInside(worldPosition))
                _grid.GetCellData(worldPosition).ChangeColor(targetColor);
        }



        /// <summary>
        /// ����̃C���f�b�N�X�ʒu�̃X�v���C�g�̐F��ύX���邽�߂ɂ��̃��\�b�h���Ăяo���܂�
        /// </summary>
        /// <param name="index">�Ώۂ̃C���f�b�N�X</param>
        /// <param name="size">�Ώۂ̃T�C�Y</param>
        public void SetSpriteSizeAt(Vector2Int index, Vector2 size)
        {
            if (_grid.IsInside(index))
                _grid.GetCellData(index).ChangeSpriteSize(size);
        }

        /// <summary>
        /// ����̃C���f�b�N�X�ʒu�̃X�v���C�g�̐F��ύX���邽�߂ɂ��̃��\�b�h���Ăяo���܂�
        /// </summary>
        /// <param name="x">X���W�̃C���f�b�N�X</param>
        /// <param name="y">Y���W�̃C���f�b�N�X</param>
        /// <param name="size">�Ώۂ̃T�C�Y</param>
        public void SetSpriteSizeAt(int x, int y, Vector2 size)
        {
            if (_grid.IsInside(x, y))
                _grid.GetCellData(x, y).ChangeSpriteSize(size);
        }

        /// <summary>
        /// ����̃��[���h���W��̃X�v���C�g�̐F��ύX���邽�߂ɂ��̃��\�b�h���Ăяo���܂�
        /// </summary>
        /// <param name="worldPosition">�Ώۂ̃��[���h���W</param>
        /// <param name="size">�Ώۂ̃T�C�Y</param>
        public void SetSpriteSizeAt(Vector2 worldPosition, Vector2 size)
        {
            if (_grid.IsInside(worldPosition))
                _grid.GetCellData(worldPosition).ChangeSpriteSize(size);
        }


        //  ���C�t�T�C�N�����\�b�h

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
            // ���t���[���A�O���b�h�̌��_���X�V����
            _grid.GridOrigin = _gcData.gridOffset + (Vector2)_t.position;
        }

        private void OnValidate()
        {
            // �C���X�y�N�^�[��Œl���ύX���ꂽ�Ƃ��ɃO���b�h���č쐬����
            _grid = new Grid<DefaultCell>(gridOrigin, _gcData.gridDimension, _gcData.cellDimension);
        }

        private void OnDrawGizmos()
        {
            if (_gcData.shouldDrawGizmos == false)
                return;// Gizmos��`�悵�Ȃ��ݒ�Ȃ牽�����Ȃ�

            // �O���b�h�̌��_���X�V���Ă���O���b�h����`�悷��
            _grid.GridOrigin = _gcData.gridOffset + (Vector2)transform.position;
            _grid.DrawGridLines(_gcData.gridLineColor, _gcData.crossLineColor);
        }

        private void setupSimpleSpriteRendering()
        {
            // �O���b�h���̑S�Z�������[�v����
            foreach (var c in _grid)
            {
                // �Z���̃C���f�b�N�X�𖼑O�ɂ����V����GameObject���쐬���ASpriteRenderer�R���|�[�l���g��t��
                GameObject go = new GameObject($"{c.Index}", typeof(SpriteRenderer));

                // �쐬����GameObject��SpriteRenderer���Z���̃f�[�^�ɕۑ�
                c.Data.sr = go.GetComponent<SpriteRenderer>();

                // �f�t�H���g�̃X�v���C�g��SpriteRenderer�ɐݒ�
                c.Data.sr.sprite = _defaultSimpleSprite;

                // �V����GameObject�����̃I�u�W�F�N�g�̎q�ɐݒ�
                go.transform.parent = _t;

                // �Z���̒��S�ʒu��GameObject��z�u
                go.transform.position = _grid.GetCellCenter(c.Index);

                // �Z���̑傫���ɍ��킹��GameObject�̃X�P�[����ݒ�
                go.transform.localScale = _grid.CellDimension;
            }
        }


        // // �l�X�g���ꂽ�^

        private enum OffsetType
        {
            Preset,  // ��^�i�v���Z�b�g�j
            Custom   // �J�X�^��
        }

        private enum PresetTypes
        {
            TopRight,      // �E��
            TopCenter,     // �㒆��
            TopLeft,       // ����
            MiddleRight,   // �E����
            MiddleCenter,  // ����
            MiddleLeft,    // ������
            BottomRight,   // �E��
            BottomCenter,  // ������
            BottomLeft     // ����
        }

        [System.Serializable]
        private class GridComponentDataContainer
        {
            // �O���b�h�֘A�̐ݒ�
            [SerializeField]
            public Vector2Int gridDimension = new Vector2Int();  // �O���b�h�̃T�C�Y�i�Z�����j
            [SerializeField]
            public Vector2 cellDimension = new Vector2();        // �Z���̃T�C�Y
            [SerializeField]
            public Vector2 gridOffset = new Vector2();           // �O���b�h�̃I�t�Z�b�g�i�ʒu�����j

            // Gizmos��G�f�B�^�֘A�̐ݒ�
            [SerializeField]
            public OffsetType offsetType = OffsetType.Preset;    // �I�t�Z�b�g�̃^�C�v�i�v���Z�b�g or �J�X�^���j
            [SerializeField]
            public PresetTypes presetType = PresetTypes.BottomLeft;  // �v���Z�b�g�̎�ށi�ʒu�j
            [SerializeField]
            public bool shouldDrawGizmos = false;                // Gizmos��`�悷�邩�ǂ���
            [SerializeField]
            public Color gridLineColor;                           // �O���b�h���̐F
            [SerializeField]
            public Color crossLineColor;                          // �N���X���C���̐F
        }

        public class DefaultCell
        {
            // �t�B�[���h

            public SpriteRenderer sr;  // �Z���ɕR�Â�SpriteRenderer


            // ���J���\�b�h 

            public void ChangeSprite(Sprite sprite)
            {
                sr.sprite = sprite;  // �X�v���C�g��ύX����
            }

            public void ChangeColor(Color color)
            {
                sr.color = color;    // �X�v���C�g�̐F��ύX����
            }

            public void ChangeSpriteSize(Vector2 size)
            {
                sr.transform.localScale = size;  // �X�v���C�g�̃T�C�Y�i�X�P�[���j��ύX����
            }
        }
    }
}*/