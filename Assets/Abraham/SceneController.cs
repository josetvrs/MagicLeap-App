using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void StartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }
    public void TecLabScene()
    {
        SceneManager.LoadScene("TecLab");
    }
    public void ProcessDataScene()
    {
        SceneManager.LoadScene("Process Data");
    }
}
