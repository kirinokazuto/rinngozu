using UnityEngine;
using UnityEngine.SceneManagement;
using static SceneLoader1;
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        GameData.selectedValue = 0; // �C�ӂ̒l
        SceneManager.LoadScene(sceneName);
    }
}