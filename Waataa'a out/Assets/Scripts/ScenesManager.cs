// Sugerencia general de Clean Code: usar if de una sola línea con llaves cuando sea legible.

using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadLevel2();
        }
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("LVL1");
        Time.timeScale = 1f;

    }

    public void LoadLevel2() // Sin abreviaciones
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

    public void CloseApplication() 
    {
        Application.Quit();
        Debug.Log("You quit the application");
    }

}
