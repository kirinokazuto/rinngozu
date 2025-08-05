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
        GameData.selectedValue = 5; // ”CˆÓ‚Ì’l
        SceneManager.LoadScene(sceneName);
    }
}
