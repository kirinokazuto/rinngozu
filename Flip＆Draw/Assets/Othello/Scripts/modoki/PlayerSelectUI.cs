/*using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// �v���C���[������͂��ăQ�[�����J�n���邽�߂�UI���Ǘ�����N���X�B
/// ���E���v���C���[�̖��O����͂��A�u�Q�[���J�n�v�{�^���������ƃQ�[�����J�n�����B
/// </summary>
public class PlayerSelectUI : MonoBehaviour
{
    // ----------------------
    // �t�B�[���h
    // ----------------------

    // ���v���C���[�̖��O����͂��邽�߂�TMP_InputField
    [SerializeField] private TMPro.TMP_InputField _blackPlayerName;

    // ���v���C���[�̖��O����͂��邽�߂�TMP_InputField
    [SerializeField] private TMPro.TMP_InputField _whitePlayerName;

    // �Q�[���J�n�{�^��
    [SerializeField] private Button _startGame;

    // ----------------------
    // �v���C�x�[�g���\�b�h
    // ----------------------

    /// <summary>
    /// �����������B
    /// �v���C���[���̓��̓t�B�[���h�Ƀ��X�i�[��ǉ����A�u�Q�[���J�n�v�{�^���̓����ݒ�B
    /// PlayerPrefs�����Z�b�g���A�v���C���[����ۑ����邽�߂̏�����ݒ�B
    /// </summary>
    private void Awake()
    {
        // PlayerPrefs�̑S�f�[�^���폜
        PlayerPrefs.DeleteAll();

        // ���v���C���[���̓��͂��I���������ɁA���͂��ꂽ���O��PlayerPrefs�ɕۑ�
        _blackPlayerName.onEndEdit.AddListener((string val) => {
            PlayerPrefs.SetString("black-player-name", val);  // PlayerPrefs�ɍ��v���C���[����ۑ�
            PlayerPrefs.Save();  // PlayerPrefs�̃f�[�^���f�B�X�N�ɕۑ�
        });

        // ���v���C���[���̓��͂��I���������ɁA���͂��ꂽ���O��PlayerPrefs�ɕۑ�
        _whitePlayerName.onEndEdit.AddListener((string val) => {
            PlayerPrefs.SetString("white-player-name", val);  // PlayerPrefs�ɔ��v���C���[����ۑ�
            PlayerPrefs.Save();  // PlayerPrefs�̃f�[�^���f�B�X�N�ɕۑ�
        });

        // �Q�[���J�n�{�^�����N���b�N���ꂽ���ɁA"MainLoop"�V�[����񓯊��œǂݍ���
        _startGame.onClick.AddListener(() => {
            SceneManager.LoadSceneAsync("MainLoop");  // �Q�[���{�҂̃V�[���ɑJ��
        });
    }
}*/