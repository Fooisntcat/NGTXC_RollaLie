using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenUI_ButtonClick : MonoBehaviour
{
    [SerializeField] private GameObject debugPanel;
    private bool isVisible = false;

    public void RestartGame()
    {
        Destroy(GameObject.Find("BG Music"));
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleDebug()
    {
        isVisible = !isVisible;
        debugPanel.SetActive(isVisible);
    }
}
