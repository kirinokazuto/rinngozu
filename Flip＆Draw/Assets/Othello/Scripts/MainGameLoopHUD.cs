using UnityEngine;
using TMPro;


/// <summary>
/// �Q�[�����̃v���C���[���ƃR�C���̐���\������HUD�i�w�b�h�A�b�v�f�B�X�v���C�j�N���X�B
/// �Q�[�����̍��E���v���C���[�̖��O�ƃR�C���̐����X�V���ĕ\������B
/// </summary>
public class MainGameLoopHUD : MonoBehaviour
{
    // ----------------------
    // �t�B�[���h
    // ----------------------

    // �Ֆʂ̊Ǘ����s�� Board
    [SerializeField] private Board _board;

    // �v���C���[���ƃR�C������\�����邽�߂�TMP�iTextMeshPro�j�R���|�[�l���g
    [SerializeField] private TMP_Text _blackPlayer; // ���v���C���[�̖��O�ƃR�C����
    [SerializeField] private TMP_Text _whitePlayer; // ���v���C���[�̖��O�ƃR�C����

    // �v���C���[�̖��O�i���Ɣ��j
    private string _blackPlayerName;
    private string _whitePlayerName;

    // ----------------------
    // �v���C�x�[�g���\�b�h
    // ----------------------

    /// <summary>
    /// �����������B�v���C���[�̖��O��PlayerPrefs����ǂݍ��ށB
    /// �v���C���[�����ݒ肳��Ă��Ȃ��ꍇ�A�f�t�H���g���i"Black Player" / "White Player"�j���g�p�B
    /// </summary>
    private void Awake()
    {
        // PlayerPrefs����v���C���[�����擾�A���݂��Ȃ��ꍇ�̓f�t�H���g�l���g�p
        _blackPlayerName = PlayerPrefs.GetString("black-player-name", "Black Player");
        _whitePlayerName = PlayerPrefs.GetString("white-player-name", "White Player");
    }

    /// <summary>
    /// ���t���[���X�V����鏈���B
    /// �Ֆʂ��v���C�\�ȏꍇ�A�v���C���[���ƃR�C���̐����X�V���ĕ\������B
    /// </summary>
    private void Update()
    {
        // �Q�[�����v���C�\�ȏ�Ԃ����m�F
        if (_board.CanPlay())
        {
            // �Ֆʂ̃R�C���̐����擾
            var c = _board.GetCoinCount();

            // ���v���C���[�Ɣ��v���C���[�̃R�C������\��
            _blackPlayer.text = $"{_blackPlayerName} : {c.x}";
            _whitePlayer.text = $"{_whitePlayerName} : {c.y}";
        }
    }
}