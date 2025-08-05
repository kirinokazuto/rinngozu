using Lacobus.Grid;// �O���b�h�V�X�e�����g�p
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SceneLoader1;

public class Board : MonoBehaviour
{
    // Fields
    [SerializeField] private Vector2Int _gridDimension;// �O���b�h�̃T�C�Y�i���~�����j
    [SerializeField] private Vector2 _cellDimension;// �e�Z���̃T�C�Y
    [SerializeField] private GameObject _whiteCoinPrefab;// ���R�C���̃v���n�u
    [SerializeField] private GameObject _blackCoinPrefab;// ���R�C���̃v���n�u
    [SerializeField] private GameObject _blackMarkerPrefab;// ���̃}�[�J�[�̃v���n�u
    [SerializeField] private GameObject _whiteMarkerPrefab;// ���̃}�[�J�[�̃v���n�u
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

    private Grid<BoardData> _grid;// �O���b�h�V�X�e��

    private Transform _t;// ���݂̃I�u�W�F�N�g�� Transform

    private Camera _camera;// ���C���J����

    private CoinFace _latestFace;// �Ō�ɔz�u���ꂽ�R�C���̐F
    private Vector2Int _latestPoint;// �Ō�ɔz�u���ꂽ�R�C���̍��W

    private List<Vector2Int> _cachedBlackPoints = null; // ���R�C���̔z�u�\���X�g�i�L���b�V���j
    private List<Vector2Int> _cachedWhitePoints = null; // ���R�C���̔z�u�\���X�g�i�L���b�V���j

    private GameObject _markerPlaceholder; // �}�[�J�[�̐e�I�u�W�F�N�g�i�v���[�X�z���_�[�j

    private bool _canPlay = true; // �Q�[���̃v���C��
    private int _coinsPlaced = 0; // �z�u�ς݂̃R�C����

    private CoinFace _currentTurn = CoinFace.black; // �����͍��v���C���[

    public int CPUcount;
    public static int white_count;//���̃R�C���̖������J�E���g

    public class Receiver : MonoBehaviour//�ϐ����󂯎��
    {
        void Start()
        {
            int receivedValue = GameData.selectedValue;//�󂯎�����ϐ��Ǘ�
        }
    }

    // Properties
    // �O���b�h�̌��_���W�i���������j
    private Vector3 gridOrigin => _t.position - new Vector3((_gridDimension.x * _cellDimension.x) / 2, (_gridDimension.y * _cellDimension.y) / 2);

    // �R�C�����{�[�h��ɔz�u����
    public bool PlaceCoinOnBoard(CoinFace face)
    {
        if (GameData.selectedValue == 5)//�󂯎�����ϐ����T�Ȃ�Ώ��������s
        {
            // �v���C���[�͍���������ł��Ȃ��悤�ɂ���
            if (_currentTurn != CoinFace.black || face != CoinFace.black)

            return false;
        }

        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

        // �}�E�X���W���O���b�h�̃C���f�b�N�X�ɕϊ�
        if (_grid.ConvertToXY(mousePos, out Vector2Int index) && _grid.GetCellData(mousePos).isOccupied == false)
        {
            // �z�u�ł���|�C���g���`�F�b�N
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

            // �R�C�����쐬���A�{�[�h�ɔz�u
            var coin = makeCoin(face, _grid.GetCellCenter(index));////////////////���N���b�N�ŃR�C������

            _grid.GetCellData(index).isOccupied = true;
            _grid.GetCellData(index).coin = coin;

            _latestPoint = index; // �Ō�ɔz�u���ꂽ�R�C���̍��W���L�^

            _latestFace = face; // �Ō�ɔz�u���ꂽ�R�C���̐F���L�^

            StartCoroutine(updateCoinCaptures()); // �R�C���̕ߊl�������J�n


            clearEligibleMarkers(); // �L���ȃ}�[�J�[���N���A

            _currentTurn = CoinFace.white; // �v���C���[���u�����玟��CPU�̔��^�[��


            return true;
        }
        else
            return false;
    }

    // �{�[�h��̍��Ɣ��̃R�C���̐����擾����
    //sum_count = black_count + white_count;

    public Vector2Int GetCoinCount()
    {
        int blacks = 0, whites = 0;

        // �O���b�h�S�̂𑖍����āA�R�C���̐F���J�E���g

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

        return new Vector2Int(blacks, whites); // ���Ɣ��̃R�C������Ԃ�

    }
    // ���݂̃v���C���[�̗L���Ȕz�u�ʒu���X�V����
    private bool _isCPUProcessing = false;

    public bool UpdateEligiblePositions(CoinFace face)
    {
        List<Vector2Int> eligiblePoints = getAllEligiblePosition(face);

        //cp�̒ǉ�
        if (eligiblePoints.Count == 0)
        {
            Debug.Log($"{face} �̃^�[�����X�L�b�v���܂�");

            // �^�[���𑊎�ɓn��
            _currentTurn = (face == CoinFace.black) ? CoinFace.white : CoinFace.black;

            // ����̔z�u�\�ʒu���X�V
            UpdateEligiblePositions(_currentTurn);

            return false;
        }

        // �}�[�J�[�`��Ȃǒʏ폈��
        clearEligibleMarkers();
        drawNewEligibleMarkers(eligiblePoints, face);


        if (!_isCPUProcessing && GameData.selectedValue == 5 && _currentTurn == CoinFace.white)
        {
            _isCPUProcessing = true;
            StartCoroutine(CPUPlaceWhiteAndSwitchTurn());
        }

        //��������
        if (getAllEligiblePosition(CoinFace.black).Count == 0 &&
            getAllEligiblePosition(CoinFace.white).Count == 0)
        {
            Debug.Log("���v���C���[�Ƃ��u���Ȃ����߁A�Q�[���I��");
            _canPlay = false;
            // ���s����Ȃǂ�
        }

        switch (face)
        {
            case CoinFace.black:
                // ���R�C���̔z�u�\�|�C���g�����擾�̏ꍇ

                if (_cachedBlackPoints == null)
                {
                    // ���̃L���b�V���f�[�^�i���R�C���̔z�u�\�|�C���g�j���N���A

                    if (_cachedWhitePoints != null)
                        _cachedWhitePoints = null;

                    // ���R�C���̔z�u�\�|�C���g���擾

                    _cachedBlackPoints = getAllEligiblePosition(CoinFace.black);

                    // �z�u�\�ȃ|�C���g���Ȃ��ꍇ�Afalse ��Ԃ��ďI��

                    if (_cachedBlackPoints.Count == 0)
                        return false;

                    // �V�����z�u�\�}�[�J�[��`��

                    drawNewEligibleMarkers(_cachedBlackPoints, CoinFace.black);
                }
                break;
            case CoinFace.white:
                // ���R�C���̔z�u�\�|�C���g�����擾�̏ꍇ

                if (_cachedWhitePoints == null)
                {
                    // ���̃L���b�V���f�[�^�i���R�C���̔z�u�\�|�C���g�j���N���A

                    if (_cachedBlackPoints != null)
                        _cachedBlackPoints = null;

                    // ���R�C���̔z�u�\�|�C���g���擾

                    _cachedWhitePoints = getAllEligiblePosition(CoinFace.white);
                    white_count = _cachedWhitePoints.Count;//���̃R�C���̖������擾
                    // �z�u�\�ȃ|�C���g���Ȃ��ꍇ�Afalse ��Ԃ��ďI��

                    if (_cachedWhitePoints.Count == 0)
                        return false;

                    // �V�����z�u�\�}�[�J�[��`��
                    drawNewEligibleMarkers(_cachedWhitePoints, CoinFace.white);

                    ////CPU�Ȃ甒�R�C����z�u///CPU�X�N���v�g

                    if(GameData.selectedValue == 5)//�󂯎�����ϐ����T�Ȃ�Ώ��������s
                    {
                        if (_cachedWhitePoints != null && _cachedWhitePoints.Count > 0)
                        {
                            int index = UnityEngine.Random.Range(0, _cachedWhitePoints.Count);
                            setCoin(CoinFace.white, _cachedWhitePoints[index]); // CPU������u��
                            GameObject.Find("Black_Skip").GetComponent<Black_Skip>().ForceTurnBack();

                            GameDirector director = FindObjectOfType<GameDirector>();
                            if (director != null)
                            {
                                // �L���b�V���N���A
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
        // �z�u�\�ȃ|�C���g������ꍇ�� true ��Ԃ�

        return true;
    }
    // �Q�[�����܂��v���C�\���ǂ�����Ԃ�

    void Update()
    {
        if (_currentTurn == CoinFace.black && _canPlay && Input.GetMouseButtonDown(0))
        {
            PlaceCoinOnBoard(CoinFace.black);

            HandleBlackClick();
        }

        // CPU�����i���^�[�����̂݁A��x�����j
        if (_currentTurn == CoinFace.white && GameData.selectedValue == 5 && !_isCPUProcessing)
        {
            _isCPUProcessing = true;
            StartCoroutine(CPUPlaceWhiteAndSwitchTurn());
        }
    }

    void HandleBlackClick()
    {
        // �}�E�X�ʒu�����[���h���Ֆʂɕϊ�
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int clickedPos = new Vector2Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y));

        // �����u����ꏊ�ɃN���b�N���Ă��邩�m�F
        if (_cachedBlackPoints != null && _cachedBlackPoints.Contains(clickedPos))
        {
            _canPlay = false;

            setCoin(CoinFace.black, clickedPos); // ���΂�u��
            StartCoroutine(HandleTurnChange()); // ���Ԃ��Ĕ��^�[����
        }
    }

    private IEnumerator CPUPlaceWhiteAndSwitchTurn()
    {
        if (_cachedWhitePoints != null && _cachedWhitePoints.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, _cachedWhitePoints.Count);
            setCoin(CoinFace.white, _cachedWhitePoints[index]); // ���R�C����z�u

            yield return StartCoroutine(updateCoinCaptures()); // �ߊl�A�j���[�V���������܂ő҂�

            _cachedWhitePoints = null; // �L���b�V�����N���A

            _currentTurn = CoinFace.black; // ���^�[���ɖ߂�
            UpdateEligiblePositions(CoinFace.black); // ���̔z�u�\�ʒu���X�V���ă}�[�J�[���o��

            _canPlay = true; // ��������ł���悤�ɂ���
        }

        _isCPUProcessing = false; // �I����ɖ߂�

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

        clearEligibleMarkers();           // �O�̃^�[���̃}�[�J�[�폜
        _cachedBlackPoints = null;        // ���̌�⃊�Z�b�g

        _currentTurn = CoinFace.white;    // �^�[���ύX
        UpdateEligiblePositions(CoinFace.white); // ���̌����X�V
    }

    public bool CanPlay() => _canPlay;
    // �{�[�h�����t���ǂ����𔻒肷��

    public bool IsFull()
    {
        if (_coinsPlaced == 64) // �ő�64�z�u���ꂽ�ꍇ�A�Q�[�����I��

        {
            _canPlay = false;
            return true;
        }
        else return false;
    }

    // �R�C���I�u�W�F�N�g�𐶐�����
    private Coin makeCoin(CoinFace face, Vector3 worldPosition)
    {
        ++_coinsPlaced;// �z�u���ꂽ�R�C�������X�V

        // �R�C���𐶐����A�K�؂� Transform �ɐݒ�
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

    ////�R�C���������Őݒ�///CPU�X�N���v�g
    ////public Coin setCoin(CoinFace face, List<Vector2Int> place_pos)���X�g�폜�O�X�N���v�g
    ////�S�X�N���v�gplace_pos=placePos�ɕς���//CPU
    public Coin setCoin(CoinFace face, Vector2Int placePos)
    {
        ++_coinsPlaced;

        _latestPoint = placePos; // �� �ǉ�
        _latestFace = face;      // �� �ǉ�

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

        StartCoroutine(updateCoinCaptures()); // �� �ǉ��ς݂Ȃ�OK

        return coin;
    }

    // �}�[�J�[�𐶐�����
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

    // �Q�[���{�[�h�����������A�J�n���̃R�C���z�u��ݒ肷��
    private void initBoard()
    {
        // �O���b�h�̒������W���v�Z
        int xCenter = _grid.GridDimension.x / 2;
        int yCenter = _grid.GridDimension.y / 2;

        // �����z�u��4�̃R�C�����쐬
        var coin_1 = makeCoin(CoinFace.black, _grid.GetCellCenter(xCenter, yCenter));
        var coin_2 = makeCoin(CoinFace.black, _grid.GetCellCenter(xCenter - 1, yCenter - 1));
        var coin_3 = makeCoin(CoinFace.white, _grid.GetCellCenter(xCenter - 1, yCenter));
        var coin_4 = makeCoin(CoinFace.white, _grid.GetCellCenter(xCenter, yCenter - 1));

        // �e�Z���̐�L���ƃR�C������o�^
        _grid.GetCellData(xCenter, yCenter).isOccupied = true;
        _grid.GetCellData(xCenter, yCenter).coin = coin_1;

        _grid.GetCellData(xCenter - 1, yCenter - 1).isOccupied = true;
        _grid.GetCellData(xCenter - 1, yCenter - 1).coin = coin_2;

        _grid.GetCellData(xCenter - 1, yCenter).isOccupied = true;
        _grid.GetCellData(xCenter - 1, yCenter).coin = coin_3;

        _grid.GetCellData(xCenter, yCenter - 1).isOccupied = true;
        _grid.GetCellData(xCenter, yCenter - 1).coin = coin_4;

        // �Q�[���̃v���C�\��Ԃ� true �ɂ���
        _canPlay = true;
    }

    // �������i���E�j�̃R�C���̕ߊl�Ώۂ𔻒肵�A���X�g�Ƃ��ĕԂ�
    // �ߊl�\�ȃR�C���̍��W���X�g���i�[���� Dictionary
    // �L�[ `0` �͉E�����A�L�[ `1` �͍������̃R�C��������
    private Dictionary<int, List<Vector2Int>> getHorizontalCoinsToBeCaptured() 
    { 
        bool shouldFlipCoin; // ���ݍ��߂�R�C�������邩�̃t���O 
        List<Vector2Int> coinsArray = null; // �ꎞ�I�ɃR�C���̍��W���i�[���郊�X�g 
        Dictionary<int, List<Vector2Int>> coinsToBeFlipped = new Dictionary<int, List<Vector2Int>>(); // �ߊl�Ώۂ̃R�C�����X�g
         
        // **�E�����̒T��** 
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();
         
        // �ŐV�̔z�u�ʒu����E�����֒T�� 
        for (int x = _latestPoint.x + 1; x < _grid.GridDimension.x; ++x)
        {
            // ��Z���ɓ��B�����ꍇ�A�T���I��
            if (_grid.GetCellData(x, _latestPoint.y).isOccupied == false)
                break;
             
            // ����̃R�C���Ȃ�t���O�𗧂ĂČp��
            if (_grid.GetCellData(x, _latestPoint.y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            } 
            else
            { 
                // �r���ɑ���̃R�C�����Ȃ������ꍇ�A�ߊl�ł��Ȃ��̂ŏI��
                if (shouldFlipCoin == false)
                    break;
                 
                // ���ݍ��񂾃R�C�������X�g�ɒǉ�
                for (int i = _latestPoint.x + 1; i < x; ++i)
                    coinsArray.Add(new Vector2Int(i, _latestPoint.y));
                 
                break;
            }
        }
        coinsToBeFlipped.Add(0, coinsArray); // �E�����̕ߊl�Ώۂ�ǉ�

        // **�������̒T��**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();

        // �ŐV�̔z�u�ʒu���獶�����֒T��
        for (int x = _latestPoint.x - 1; x >= 0; --x)
        {
            // ��Z���ɓ��B�����ꍇ�A�T���I��
            if (_grid.GetCellData(x, _latestPoint.y).isOccupied == false)
                break;
             
            // ����̃R�C���Ȃ�t���O�𗧂ĂČp��
            if (_grid.GetCellData(x, _latestPoint.y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            }
            else
            {
                // �r���ɑ���̃R�C�����Ȃ������ꍇ�A�ߊl�ł��Ȃ��̂ŏI��
                if (shouldFlipCoin == false)
                    break;
                 
                // ���ݍ��񂾃R�C�������X�g�ɒǉ�
                for (int i = _latestPoint.x - 1; i > x; --i)
                    coinsArray.Add(new Vector2Int(i, _latestPoint.y));
                 
                break;
            }
        }
         
        coinsToBeFlipped.Add(1, coinsArray); // �������̕ߊl�Ώۂ�ǉ�

        return coinsToBeFlipped; // �ߊl�Ώۂ̃��X�g��Ԃ�
    }

    // �c�����i�㉺�j�̃R�C���̕ߊl�Ώۂ𔻒肵�A���X�g�Ƃ��ĕԂ�
    // �ߊl�\�ȃR�C���̍��W���X�g���i�[���� Dictionary
    // �L�[ `0` �͏�����A�L�[ `1` �͉������̃R�C��������
    private Dictionary<int, List<Vector2Int>> getVerticalCoinsToBeCaptured()
    {
        bool shouldFlipCoin; // ���ݍ��߂�R�C�������邩�̃t���O
        List<Vector2Int> coinsArray = null; // �ꎞ�I�ɃR�C���̍��W���i�[���郊�X�g
        Dictionary<int, List<Vector2Int>> coinsToBeFlipped = new Dictionary<int, List<Vector2Int>>(); // �ߊl�Ώۂ̃R�C�����X�g
         
        // **������̒T��**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();
         
        // �ŐV�̔z�u�ʒu���������֒T��
        for (int y = _latestPoint.y + 1; y < _grid.GridDimension.y; ++y)
        {
            // ��Z���ɓ��B�����ꍇ�A�T���I��
            if (_grid.GetCellData(_latestPoint.x, y).isOccupied == false)
                break;

            // ����̃R�C���Ȃ�t���O�𗧂ĂČp��
            if (_grid.GetCellData(_latestPoint.x, y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            } 
            else
            { 
                // �r���ɑ���̃R�C�����Ȃ������ꍇ�A�ߊl�ł��Ȃ��̂ŏI��
                if (shouldFlipCoin == false)
                    break;
                 
                // ���ݍ��񂾃R�C�������X�g�ɒǉ�
                for (int i = _latestPoint.y + 1; i < y; ++i)
                    coinsArray.Add(new Vector2Int(_latestPoint.x, i));
                 
                break;
            }
        }

        coinsToBeFlipped.Add(0, coinsArray); // ������̕ߊl�Ώۂ�ǉ�

        // **�������̒T��**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();

        // �ŐV�̔z�u�ʒu���牺�����֒T��
        for (int y = _latestPoint.y - 1; y >= 0; --y)
        {
            // ��Z���ɓ��B�����ꍇ�A�T���I��
            if (_grid.GetCellData(_latestPoint.x, y).isOccupied == false)
                break;

            // ����̃R�C���Ȃ�t���O�𗧂ĂČp��
            if (_grid.GetCellData(_latestPoint.x, y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            }
            else
            {
                // �r���ɑ���̃R�C�����Ȃ������ꍇ�A�ߊl�ł��Ȃ��̂ŏI��
                if (shouldFlipCoin == false)
                    break;

                // ���ݍ��񂾃R�C�������X�g�ɒǉ�
                for (int i = _latestPoint.y - 1; i > y; --i)
                    coinsArray.Add(new Vector2Int(_latestPoint.x, i));

                break;
            }
        }

        coinsToBeFlipped.Add(1, coinsArray); // �������̕ߊl�Ώۂ�ǉ�

        return coinsToBeFlipped; // �ߊl�Ώۂ̃��X�g��Ԃ� 
    }

    // �΂ߕ����i�㉺���E�j�̃R�C���̕ߊl�Ώۂ𔻒肵�A���X�g�Ƃ��ĕԂ� 
    // �ߊl�\�ȃR�C���̍��W���X�g���i�[���� Dictionary 
    // �L�[ `0` �͉E��iUp Right�j�A`1` �͍���iUp Left�j
    // �L�[ `2` �͍����iDown Left�j�A`3` �͉E���iDown Right�j
    private Dictionary<int, List<Vector2Int>> getDiagonalCoinsToBeCaptured()
    {
        bool shouldFlipCoin; // ���ݍ��߂�R�C�������邩�̃t���O
        List<Vector2Int> coinsArray = null; // �ꎞ�I�ɃR�C���̍��W���i�[���郊�X�g
        Dictionary<int, List<Vector2Int>> coinsToBeFlipped = new Dictionary<int, List<Vector2Int>>(); // �ߊl�Ώۂ̃R�C�����X�g

        // **�E��iUp Right�j�̒T��**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();

        // �ŐV�̔z�u�ʒu����E������֒T��
        for (int x = _latestPoint.x + 1, y = _latestPoint.y + 1; x < _grid.GridDimension.x && y < _grid.GridDimension.y; ++x, ++y)
        {
            // ��Z���ɓ��B�����ꍇ�A�T���I��
            if (_grid.GetCellData(x, y).isOccupied == false)
                break;

            // ����̃R�C���Ȃ�t���O�𗧂ĂČp��
            if (_grid.GetCellData(x, y).coin.GetFace() != _latestFace)
            {
                shouldFlipCoin = true;
                continue;
            }
            else
            {
                // �r���ɑ���̃R�C�����Ȃ������ꍇ�A�ߊl�ł��Ȃ��̂ŏI��
                if (shouldFlipCoin == false)
                    break;

                // ���ݍ��񂾃R�C�������X�g�ɒǉ�
                for (int i = _latestPoint.x + 1, j = _latestPoint.y + 1; i < x && j < y; ++i, ++j)
                    coinsArray.Add(new Vector2Int(i, j));

                break;
            }
        }

        coinsToBeFlipped.Add(0, coinsArray); // �E������̕ߊl�Ώۂ�ǉ�

        // **����iUp Left�j�̒T��**
        shouldFlipCoin = false;
        coinsArray = new List<Vector2Int>();

        // �ŐV�̔z�u�ʒu���獶������֒T��
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

        coinsToBeFlipped.Add(1, coinsArray); // ��������̕ߊl�Ώۂ�ǉ�

        // **�����iDown Left�j�̒T��**
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

        coinsToBeFlipped.Add(2, coinsArray); // ���������̕ߊl�Ώۂ�ǉ�

        // **�E���iDown Right�j�̒T��**
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

        coinsToBeFlipped.Add(3, coinsArray); // �E�������̕ߊl�Ώۂ�ǉ�

        return coinsToBeFlipped; // �ߊl�Ώۂ̃��X�g��Ԃ�
    }

    // �R�C���̕ߊl��Ԃ��X�V����R���[�`��
    // �z�u���ꂽ�R�C���ɉ����āA���ݍ��܂ꂽ����̃R�C���𔽓]����

    public IEnumerator updateCoinCaptures()
    {
        _canPlay = false; // �R�C���̍X�V���̓v���C���ꎞ�I�ɒ�~

        // ���E�c�E�΂ߕ����̕ߊl�Ώۂ̃R�C�����X�g���擾
        var hor = getHorizontalCoinsToBeCaptured();
        var ver = getVerticalCoinsToBeCaptured();
        var dia = getDiagonalCoinsToBeCaptured();

        // �e�����̕ߊl�Ώۃ��X�g���擾
        var r = hor[0];  // �E
        var l = hor[1];  // ��
        var u = ver[0];  // ��
        var d = ver[1];  // ��
        var ur = dia[0]; // �E��
        var ul = dia[1]; // ����
        var dl = dia[2]; // ����
        var dr = dia[3]; // �E��

        // �ő�8��̔��]�������s���i���X�ɔ��]������A�j���[�V�������ʂ�t�^�j
        for (int i = 0; i < 8; ++i)
        {
            // **�������̃R�C�����]**
            if (i < r.Count) // �E����
                _grid.GetCellData(r[i]).coin.FlipFace();
            if (i < l.Count) // ������
                _grid.GetCellData(l[i]).coin.FlipFace();

            // **�c�����̃R�C�����]**
            if (i < u.Count) // �����
                _grid.GetCellData(u[i]).coin.FlipFace();
            if (i < d.Count) // ������
                _grid.GetCellData(d[i]).coin.FlipFace();

            // **�΂ߕ����̃R�C�����]**
            if (i < ur.Count) // �E�����
                _grid.GetCellData(ur[i]).coin.FlipFace();
            if (i < ul.Count) // �������
                _grid.GetCellData(ul[i]).coin.FlipFace();
            if (i < dl.Count) // ��������
                _grid.GetCellData(dl[i]).coin.FlipFace();
            if (i < dr.Count) // �E������
                _grid.GetCellData(dr[i]).coin.FlipFace();

            yield return new WaitForSeconds(_coinRollSpeed); // �e���]�����̊Ԃɑҋ@���Ԃ�ݒ�i���o�I���o�j
        }

        _canPlay = true; // �R�C���X�V����������Ƀv���C�\��Ԃɖ߂�
    }

    // �w�肳�ꂽ�R�C���̐F�ɉ����āA�z�u�\�ȍ��W���擾����
    // <returns>�z�u�\�ȍ��W�̃��X�g</returns>

    public List<Vector2Int> getAllEligiblePosition(CoinFace face)
    {
        List<Vector2Int> points = new List<Vector2Int>(); // �z�u�\�ȍ��W���i�[���郊�X�g
        bool shouldFlip = false; // ���ݍ��߂�R�C�������邩�̔���t���O

        // **�ՖʑS�̂𑖍�**
        for (int x = 0; x < _grid.GridDimension.x; ++x)
        {
            for (int y = 0; y < _grid.GridDimension.y; ++y)
            {
                // ���ɐ�L����Ă���Z���ŁA�ΏۃR�C���Ɠ����F�Ȃ�T�����J�n
                if (_grid.GetCellData(x, y).isOccupied == true && _grid.GetCellData(x, y).coin.GetFace() == face)
                {
                    Vector2Int targetPoint = new Vector2Int(x, y);

                    // ��������
                    // �E
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

                    // ��
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

                    // ��������
                    // ��
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

                    // ��
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

                    // �΂ߕ���
                    // �E��
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

        return points;// �z�u�\�ȍ��W�̃��X�g��Ԃ�
    }

    // ���݂̔z�u�\�ȃ}�[�J�[�����ׂč폜����
    public void clearEligibleMarkers()
    {
        destroyPlaceholderChildren(); // �}�[�J�[�̃v���[�X�z���_�[�̎q�I�u�W�F�N�g���폜
    }

    // �V�����z�u�\�ȃ}�[�J�[��`�悷��
    // <param name="eligiblePoints">�z�u�\�ȍ��W���X�g</param>
    // <param name="face">�R�C���̐F�i���܂��͔��j</param>
    public void drawNewEligibleMarkers(List<Vector2Int> eligiblePoints, CoinFace face)
    {
        foreach (var p in eligiblePoints)
            makeMark(_grid.GetCellCenter(p), face); // �w����W�Ƀ}�[�J�[�𐶐�
    }
 
    // �}�[�J�[�̃v���[�X�z���_�[�̎q�I�u�W�F�N�g�����ׂč폜����
    public void destroyPlaceholderChildren()
    {
        var t = _markerPlaceholder.transform;

        // ���ׂĂ̎q�I�u�W�F�N�g���폜
        while (t.childCount > 0)
            DestroyImmediate(t.GetChild(0).gameObject);
    }

    // ����������
    public void Awake()
    {
        _canPlay = false; // �Q�[���J�n���̓v���C�s��
        _t = transform; // Transform ���擾
        _camera = Camera.main; // ���C���J�������擾

        // �}�[�J�[�̃v���[�X�z���_�[�I�u�W�F�N�g���쐬
        _markerPlaceholder = new GameObject("-Marker Placeholder-");

        // �O���b�h���쐬�E����
        _grid = new Grid<BoardData>(gridOrigin, _gridDimension, _cellDimension);
        _grid.PrepareGrid();

        initBoard(); // �Ֆʂ�������
    }
}
// **�{�[�h�̃f�[�^�N���X**

public class BoardData
{
    public bool isOccupied = false; // �Z������L����Ă��邩�ǂ���
    public Coin coin; // �z�u���ꂽ�R�C�����
}