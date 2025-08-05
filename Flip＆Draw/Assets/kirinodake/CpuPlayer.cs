using Lacobus.Animation;
using Lacobus.Grid;// �O���b�h�V�X�e�����g�p
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpuPlayer : MonoBehaviour
{
    // ���݂̃R�C���̖ʁi�� or ���j
    [SerializeField] private CoinFace _currentFace;

    [SerializeField] private Board _board;

    // �A�j���[�V���������p�̃R���|�[�l���g
    private AnimationHandlerComponent _animationHandler;

    // �T�E���h�Đ������p�̃R���|�[�l���g
    private CoinSoundHandler _soundHandler;

    // �Ֆʂ̃T�C�Y�i��F8x8�j
    private int boardSize = 8;

    public Board Board;



    // �Ֆʂ̏�Ԃ�\���i0=�󂫁A1=�v���C���[�A2=CPU�Ȃǁj
    private int[,] board;

    /// <summary>
    /// �Q�[���J�n���ɃR�C���ݒu�����Đ��B
    /// </summary>
    void Start()
    {
        board = new int[boardSize, boardSize];

        // �����ŏ����ՖʃZ�b�g�A�b�v�Ȃ�
        //_soundHandler.PlayCoinPlaceSound();
    }

    // CPU�̃^�[���ŋ�������_���ɒu�����\�b�h
    public void PlayRandom()
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        // �ՖʑS�̂��`�F�b�N���āA�u����ꏊ�����X�g�ɏW�߂�
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

        // �u����ꏊ���Ȃ���Ή������Ȃ�
        if (validMoves.Count == 0)
        {
            return;
        }

        // �����_����1�����I��
        int index = Random.Range(0, validMoves.Count);

        Vector2Int move = validMoves[index];

        // ���u�������i�����̓Q�[���ɍ��킹�Ď����j
        PlacePiece(move.x, move.y);
    }

    // �����Ɂu���̃}�X�ɒu���邩�v�̔��������
    private bool CanPlaceAt(int x, int y)
    {
        // �󂫃}�X���ǂ�����������i�{���̓I�Z�����[���Ŕ���j
        return board[x, y] == 0;
    }

    // ���u�������̗�
    private void PlacePiece(int x, int y)
    {
        // 2��CPU�̋�Ƃ���
        board[x, y] = 2;

        // �Ђ�����Ԃ�����
        // ���݂̖ʂ�؂�ւ���
        switch (_currentFace)
        {
            case CoinFace.black:

               _currentFace = CoinFace.white;

               break;

            case CoinFace.white:

               _currentFace = CoinFace.black;

               break;
        }

        // �ʂ̕ύX�ɉ����ăA�j���[�V�������Đ�
        updateRenderer();

        // �T�E���h���Đ�
        playSound();
    }

    // ----------------------
    // �v���C�x�[�g���\�b�h
    // ----------------------

    /// <summary>
    /// �R���|�[�l���g�̏����������B
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
    /// �ʂ̕ω��ɉ������A�j���[�V�������Đ��B
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
    /// �R�C�����]���̃T�E���h���Đ��B
    /// </summary>
    private void playSound()
    {
        _soundHandler.PlayCoinFlipSound();
    }
}