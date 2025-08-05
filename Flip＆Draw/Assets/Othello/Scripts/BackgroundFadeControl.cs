using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


/// <summary>
/// �Q�[���̏I�����ɔw�i�̃t�F�[�h�C���𐧌䂵�A�Q�[�����ʁi���҂܂��͈��������j��\������N���X�B
/// �܂��A�ăX�^�[�g�⃁�C�����j���[�ɑJ�ڂ���{�^���̕\�����s���B
/// </summary>
public class BackgroundFadeControl : MonoBehaviour
{
    // ----------------------
    // �t�B�[���h
    // ----------------------

    // �Q�[���̐i�s���Ǘ����� GameDirector
    [SerializeField] private GameDirector _gameDirector;

    // �Ֆʂ̊Ǘ����s�� Board
    [SerializeField] private Board _board;

    // �I����ʂŕ\������w�i�I�u�W�F�N�g�i���E���E�O���[�̔w�i�j
    [SerializeField] private GameObject _whiteBackground;
    [SerializeField] private GameObject _blackBackground;
    [SerializeField] private GameObject _greyBackground;

    // �Q�[���I����ɕ\������{�^��
    [SerializeField] private GameObject _whiteButton;
    [SerializeField] private GameObject _blackButton;
    [SerializeField] private GameObject _greyButton;

    // �I����������x�������s�����悤�ɂ���t���O
    private bool _hasTriggered = false;

    // ----------------------
    // �p�u���b�N���\�b�h
    // ----------------------

    /// <summary>
    /// �Q�[�����ăX�^�[�g������i���݂̃V�[�����ēǂݍ��݁j�B
    /// </summary>
    public void RestartLevel() => SceneManager.LoadSceneAsync("VSPlayer");

    /// <summary>
    /// ���C�����j���[�ɑJ�ڂ���B
    /// </summary>
    public void GoToMainMenu() => SceneManager.LoadSceneAsync("Title");

    // ----------------------
    // �v���C�x�[�g���\�b�h
    // ----------------------

    /// <summary>
    /// ���t���[���Ă΂��X�V�����B
    /// �Q�[�����I��������I����ʂ�\�����邽�߂̏������J�n�B
    /// </summary>
    private void Update()
    {
        // �Q�[�����I�����Ă���A�܂����������s����Ă��Ȃ���ΏI����ʂ�\������
        if (!_hasTriggered && _gameDirector.IsGameOver())
        {
            _hasTriggered = true;
            StartCoroutine(showEndScreen()); // �I����ʂ�\������R���[�`�����J�n
        }
    }

    /// <summary>
    /// �Q�[���I����̉�ʂɑJ�ڂ���R���[�`���B
    /// �I�����班���ҋ@���A���҂܂��͈��������𔻒肵�ĕ\������B
    /// </summary>
    /// <returns>�R���[�`���̎��s</returns>
    private IEnumerator showEndScreen()
    {
        // �����҂��Ă���I����ʂ�\��
        yield return new WaitForSeconds(1);

        // �Ֆʏ�̃R�C�������擾
        var c = _board.GetCoinCount();

        // ���̃R�C���������ꍇ�A���v���C���[������
        if (c.x > c.y)
        {
            // ���v���C���[�����擾���A���҃��b�Z�[�W��\��
            string player = PlayerPrefs.GetString("black-player-name");
            _blackBackground.GetComponentInChildren<TMP_Text>().text = $"{player}Black Wins!!";
            _blackBackground.SetActive(true); // �w�i��\��
            _blackButton.SetActive(true);     // �ăX�^�[�g�{�^����\��
        }
        // ���̃R�C���������ꍇ�A���v���C���[������
        else if (c.x < c.y)
        {
            // ���v���C���[�����擾���A���҃��b�Z�[�W��\��
            string player = PlayerPrefs.GetString("white-player-name");
            _whiteBackground.GetComponentInChildren<TMP_Text>().text = $"{player}White Wins!!";
            _whiteBackground.SetActive(true); // �w�i��\��
            _whiteButton.SetActive(true);     // �ăX�^�[�g�{�^����\��
        }
        // ���Ɣ��̃R�C�����������ꍇ�A��������
        else
        {
            _greyBackground.GetComponentInChildren<TMP_Text>().text = $"It's a draw";
            _greyBackground.SetActive(true);  // ���������w�i��\��
            _greyButton.SetActive(true);      // �ăX�^�[�g�{�^����\��
        }
    }
}