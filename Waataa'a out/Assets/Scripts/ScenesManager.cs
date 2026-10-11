// Sugerencia general de Clean Code: usar if de una sola línea con llaves cuando sea legible.

using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

    public void LoadLvl1() // Sin abreviaciones
    {
        SceneManager.LoadScene("LVL1");
        Time.timeScale = 1f;

    }

    public void LoadLvl2() // Sin abreviaciones
    {
        SceneManager.LoadScene("LVL2");
        Time.timeScale = 1f;
    }
    public void MovementPruebas() // Usar un verbo en el nombre del método; Usar inglés
    {
        SceneManager.LoadScene(3); // Revisar: usar una constante o configuración con nombre
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

    public void CloseApp() // Sin abreviaciones
    {
        Application.Quit();
        Debug.Log("You quit the application");
    }

    public void OnTriggerEnter2D(Collider2D collision) // Respetar el orden de los miembros
    {
        if (collision.CompareTag("Player"))
        {
            LoadLvl2();
        }
    }
}
