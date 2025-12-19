using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Hook this to your Play button OnClick()
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // <-- put your gameplay scene name here
        // or: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}