using Lacobus.Animation;
using UnityEngine;

/// <summary>
/// �R�C���g�[�N���̐�����s���N���X�B
/// �\�Ɨ��i���ƍ��j�̐؂�ւ���A�j���[�V�����E�T�E���h�̍Đ����s���B
/// </summary>
public class Coin : MonoBehaviour
{
    // ----------------------
    // �t�B�[���h
    // ----------------------

    // ���݂̃R�C���̖ʁi�� or ���j
    [SerializeField] private CoinFace _currentFace;

    // �A�j���[�V���������p�̃R���|�[�l���g
    private AnimationHandlerComponent _animationHandler;

    // �T�E���h�Đ������p�̃R���|�[�l���g
    private CoinSoundHandler _soundHandler;

    private bool isWhite = true; // ���̏��

    public int OwnerPlayerId { get; set; }

    // ----------------------
    // �p�u���b�N���\�b�h
    // ----------------------

    /// <summary>
    /// �R�C���̖ʂ𔽓]������i�� ? ���j�A�j���[�V�����ƃT�E���h�t���B
    /// </summary>

    public bool IsWhiteFace()
    {
        return isWhite;
    }

    public void FlipFace()
    {
        // ���݂̖ʂ�؂�ւ���
        switch (_currentFace)
        {
            case CoinFace.black:
                _currentFace = CoinFace.white;
                gameObject.tag = "Whitecoin"; // �^�O�𔒂ɕύX
                break;
            case CoinFace.white:
                _currentFace = CoinFace.black;
                gameObject.tag = "Blackcoin"; // �^�O�����ɕύX
                break;
        }

        // �ʂ̕ύX�ɉ����ăA�j���[�V�������Đ�
        updateRenderer();

        // �T�E���h���Đ�
        playSound();

        isWhite = !isWhite;
    }

    /// <summary>
    /// ���݂̃R�C���̖ʂ��擾����B
    /// </summary>
    /// <returns>���݂̖ʁi�� or ���j</returns>
    public CoinFace GetFace()
    {
        return _currentFace;
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

    /// <summary>
    /// �Q�[���J�n���ɃR�C���ݒu�����Đ��B
    /// </summary>
    private void Start()
    {
        _soundHandler.PlayCoinPlaceSound();
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

/// <summary>
/// �R�C���̖ʂ�\���񋓑́B
/// </summary>
public enum CoinFace
{
    black, // ����
    white  // ����
}