/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// ���C�����j���[��UI������Ǘ�����N���X�B
/// "Start"�{�^����"Exit"�{�^���ɂ��ꂼ��Ή����铮���ݒ�B
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    // ----------------------
    // �t�B�[���h
    // ----------------------

    // �Q�[���X�^�[�g�{�^��
    [SerializeField] private Button _startButton;

    // �Q�[���I���{�^��
    [SerializeField] private Button _exitButton;

    // ----------------------
    // �v���C�x�[�g���\�b�h
    // ----------------------

    /// <summary>
    /// �����������B
    /// �{�^���ɃN���b�N�C�x���g���X�i�[��o�^���A�e�{�^���̓�����`�B
    /// </summary>
    private void Awake()
    {
        // �X�^�[�g�{�^�����N���b�N���ꂽ���ɁA"PlayerSelect"�V�[���ɑJ��
        _startButton.onClick.AddListener(() => { SceneManager.LoadScene("PlayerSelect"); });

        // �G�O�W�b�g�{�^�����N���b�N���ꂽ���ɁA�A�v���P�[�V�������I��
        _exitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}*/