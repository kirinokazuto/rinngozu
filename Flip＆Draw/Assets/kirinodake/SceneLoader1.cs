using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader1 : MonoBehaviour
{
    public static class GameData
    {
        public static int selectedValue;
    }

    public void LoadScene1(string sceneName)
    {
        GameData.selectedValue = 5; // �C�ӂ̒l
        SceneManager.LoadScene(sceneName);
    }
}
