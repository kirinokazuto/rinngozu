using UnityEngine;
using static SceneLoader1;

public class GameExitManager : MonoBehaviour
{
    // ゲームを終了させる
    public void ExitGame()
    {
#if UNITY_EDITOR

        GameData.selectedValue = 0; // 任意の値

        // エディタ上では再生を停止
        UnityEditor.EditorApplication.isPlaying = false;

#else
        // ビルド版ではアプリケーションを終了
        Application.Quit();
#endif
    }
}
