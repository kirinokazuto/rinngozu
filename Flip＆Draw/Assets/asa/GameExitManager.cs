using UnityEngine;
using static SceneLoader1;

public class GameExitManager : MonoBehaviour
{
    // �Q�[�����I��������
    public void ExitGame()
    {
#if UNITY_EDITOR

        GameData.selectedValue = 0; // �C�ӂ̒l

        // �G�f�B�^��ł͍Đ����~
        UnityEditor.EditorApplication.isPlaying = false;

#else
        // �r���h�łł̓A�v���P�[�V�������I��
        Application.Quit();
#endif
    }
}
