using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("OnePlayer");
    }
}
