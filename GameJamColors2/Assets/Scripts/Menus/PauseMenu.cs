using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenuUI = null, optionsMenuUI = null; // Referencias al menú de pausa y de opciones

    [SerializeField]
    GameObject pauseFirstButton = null, optionsFirstButton = null; // Botones que se seleccionan al abrir el menú correspondiente

    float timeScale;

    void Update()
    {       
        if (GameManager.instance.needToResume)
        {
            GameManager.instance.needToResume = false;
            Resume();
            if (optionsMenuUI.activeSelf)
                optionsMenuUI.SetActive(false);
        }
        else if(GameManager.instance.needToPause)
        {
            GameManager.instance.needToPause = false;
            timeScale = Time.timeScale; // Guarda en una variable la escala del tiempo (por si acaso es distinta de 1)
            Debug.Log(timeScale);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Pause();                
        }
           
    }

    public void Resume() // Continúa el juego y cierra el menú
    {
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = timeScale;
        GameManager.instance.gameIsPaused = false;
    }

    public void Pause() // Para el juego y abre el menú
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameManager.instance.gameIsPaused = true;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void Options() // Abre el menú de opciones
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void Exit() // Sale al menú principal
    {
        Time.timeScale = 1;
        GameManager.instance.gameIsPaused = false;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
