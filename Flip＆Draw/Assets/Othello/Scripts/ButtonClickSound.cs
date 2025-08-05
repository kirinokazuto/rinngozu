using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �{�^�����N���b�N���ꂽ�Ƃ��Ɍ��ʉ����Đ�����R���|�[�l���g�B
/// �{�^���ɃA�^�b�`���邱�ƂŁA�����I�� AudioSource ��ǉ����A
/// �w�肵�����ʉ����Đ�����悤�ɂȂ�B
/// </summary>
public class ButtonClickSound : MonoBehaviour
{
    // ----------------------
    // �t�B�[���h
    // ----------------------

    // �Đ�����N���b�N���i�C���X�y�N�^�[�Őݒ�j
    [SerializeField] private AudioClip _clip;

    // AudioSource �R���|�[�l���g�i�����I�ɒǉ������j
    private AudioSource _as;

    // ----------------------
    // �v���C�x�[�g���\�b�h
    // ----------------------

    /// <summary>
    /// �R���|�[�l���g���L�������ꂽ�Ƃ��ɌĂ΂�郁�\�b�h�B
    /// AudioSource ��ǉ����A�{�^���N���b�N���ɃT�E���h���Đ�����悤���X�i�[��o�^�B
    /// </summary>
    private void OnEnable()
    {
        // AudioSource �R���|�[�l���g������ GameObject �ɒǉ�
        _as = gameObject.AddComponent<AudioSource>();

        // �Đ����� AudioClip ��ݒ�
        _as.clip = _clip;

        // �I�u�W�F�N�g���������ꂽ���_�Ŏ����Đ����Ȃ��悤�ɐݒ�
        _as.playOnAwake = false;

        // Button �R���|�[�l���g���擾���A�N���b�N���ɃT�E���h���Đ����郊�X�i�[��o�^
        gameObject.GetComponent<Button>().onClick.AddListener(() => {
            _as.Play();  // �N���b�N���ꂽ����ʉ����Đ�
        });
    }
}