using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour
{
    [SerializeField] private GameDirector gameDirector; // �Q�[���̐i�s�󋵂��Ǘ����� GameDirector �ւ̎Q��
    [SerializeField] private Button skillButton;        // �X�L���{�^���� UI �v�f
    [SerializeField] private int usableTurn = 3;        // �X�L�����g�p�\�ɂȂ�^�[����
    [SerializeField] private int maxUses = 3;           // �X�L���̍ő�g�p��
    [SerializeField] private int cooldownTurns = 2;     // �X�L���g�p��̃N�[���^�C���i�^�[�����j
    [SerializeField] private bool isPlayerCard = true;  // ���̃{�^�����v���C���[�p���ǂ���

    private int currentUses = 0;        // ���݂̎g�p�񐔁i�ݐρj
    private int cooldownRemaining = 0; // �N�[���^�C���c��^�[����
    private int lastCheckedTurn = -1;  // �Ō�Ƀ^�[�����`�F�b�N�����^�[���ԍ�

    void Start()
    {
        // �{�^�����N���b�N���ꂽ�Ƃ��̃C�x���g��o�^
        skillButton.onClick.AddListener(OnSkillButtonClicked);
    }

    void Update()
    {
        int currentTurn = gameDirector.GetCurrentTurn();     // ���݂̃^�[�������擾
        bool isPlayerTurn = gameDirector.IsPlayerTurn();     // ���݂��v���C���[�̃^�[�����ǂ���
        bool isMyTurn = (isPlayerCard == isPlayerTurn);      // ���̃{�^���������̃^�[���ɑΉ����Ă��邩

        // �^�[�����i�񂾂�N�[���^�C�������炷
        if (currentTurn != lastCheckedTurn)
        {
            lastCheckedTurn = currentTurn;

            if (cooldownRemaining > 0)
                cooldownRemaining--;
        }

        // �X�L�����g�p�\���ǂ����𔻒�
        bool canUse = currentTurn >= usableTurn &&           // �g�p�\�^�[���ɒB���Ă���
                      currentUses < maxUses &&               // �g�p�񐔂̏���ɒB���Ă��Ȃ�
                      isMyTurn &&                            // �����̃^�[���ł���
                      cooldownRemaining == 0;                // �N�[���^�C�����I�����Ă���

        // �{�^���̑���ۂ�ݒ�
        skillButton.interactable = canUse;
    }

    // �X�L�����g�p���ꂽ���Ƃ��L�^����i�O������Ăяo���j
    public void MarkSkillAsUsed()
    {
        int currentTurn = gameDirector.GetCurrentTurn();

        // �N�[���^�C�����͎g�p�s��
        if (cooldownRemaining > 0)
        {
            return;
        }

        // �g�p�񐔂̏���ɒB���Ă�����g�p�s��
        if (currentUses >= maxUses)
        {
            skillButton.interactable = false;
            return;
        }

        currentUses++;               // �g�p�񐔂��J�E���g
        cooldownRemaining = cooldownTurns; // �N�[���^�C����ݒ�


        // �g�p�񐔂̏���ɒB������{�^���𖳌���
        if (currentUses >= maxUses)
        {
            skillButton.interactable = false;
        }
    }

    // �{�^�����N���b�N���ꂽ�Ƃ��̏���
    private void OnSkillButtonClicked()
    {
        MarkSkillAsUsed(); // �X�L���g�p���L�^
    }


    public int GetCooldownRemaining()
    {
        return cooldownRemaining;
    }

    public int GetCurrentUses()
    {
        return currentUses;
    }

    public int GetMaxUses()
    {
        return maxUses;
    }


}
