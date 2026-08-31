using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

    public void LoadLvl1()
    {
        SceneManager.LoadScene("LVL1");
        Time.timeScale = 1f;

    }

    public void LoadLvl2()
    {
        SceneManager.LoadScene("LVL2");
        Time.timeScale = 1f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }

    public void CloseApp()
    {
        Application.Quit();
        Debug.Log("You quit the application");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadLvl2();
        }
    }
}
