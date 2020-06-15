using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private InputManager input;
    private bool inOptions = false;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject soundOptionsMenu;
    public GameObject controllerOptionsMenu;

    public static bool isPaused;
    private void Start()
    {
        Time.timeScale = 1;
        input = GetComponent<InputManager>();
        if (input == null)
        {
            input = PlayerMovement.MyPlayer.GetComponent<InputManager>();

            if (input == null)
            {
                Debug.Log("The Main menu script cannot find a reference to the input manager");
                Destroy(this.gameObject);
            }
        }

        if (pauseMenu == null)
        {
            Debug.LogError("No pause menu applied, please add a pause menu");
        }

        if (optionsMenu == null)
        {
            Debug.LogError("No options menu applied, please add an options menu");
        }
        
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        Debug.Log("input.Buttons()");

        if (input.Buttons() == "Pause")
        {
            Debug.Log(FindObjectsOfType<InputManager>().Length);
            Debug.Log("HereLigmaMom");
            if (isPaused == false)
            {
                pauseMenu.SetActive(true);
                isPaused = true;
                Time.timeScale = 0;
            }
            else
            {
                if (!inOptions)
                {
                    UnPause();
                }
            }
        }
    }

    public void ShowOptions()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        inOptions = true;
    }
    public void ShowControls()
    {
        controllerOptionsMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    public void ShowSound()
    {
        soundOptionsMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }


    public void BackToPauseMenu()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        inOptions = false;
    }

    public void BackToOptionsMenu()
    {
        soundOptionsMenu.SetActive(false);
        controllerOptionsMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RespawnCharacter()
    {
        PlayerMovement.MyPlayer.Respawn();

        Scene[] currentScenes = SceneManager.GetAllScenes();

        foreach (var scene in currentScenes)
        {
            if (scene.name.Contains("Player") || scene.name.Contains("Test") || scene.name.Contains("lvl1_Main"))
            {
                continue;
            }
            SceneManager.UnloadSceneAsync(scene);
            SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
            PlayerMovement.MyPlayer.SwitchState(PlayerMovement.MyPlayer.airState);
            UnPause();
        }
    }
}
