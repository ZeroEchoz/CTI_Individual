using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {

    public GameObject player;
    public GameObject pauseMenu;
    public GameObject tabs;
    [Space(10)]
    public GameObject controlsPage;
    public Image airPage;
    public Image groundPage;
    [Space(10)]
    public GameObject skipAskPage;
    [Space(10)]
    [Tooltip("Name of the Main Menu Scene")]
    public string mainMenuScene;
    public string mainScene;

    public GameObject loadingScreen;
    public Slider loadingBar;
    public Toggle invertGlidingToggleButton;

    CharControl charControlScript;

    [Tooltip("All UI windows that can be toggled on/off, e.g Map, Inventory, etc. " +
        "NOT INCLUDING PAUSE MENU")]
    public GameObject[] UI_Windows;
    bool windowsOpen;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void Start () {
        airPage.enabled = false;
        groundPage.enabled = false;
        pauseMenu.SetActive(false);
        controlsPage.SetActive(false);
        loadingScreen.SetActive(false);

        #region Assigning Parameters
        charControlScript = player.GetComponent<CharControl>();
        #endregion
    }

    private void Update()
    {
        
        if (controlsPage.activeSelf)
        {
            //windowsOpen = true;
        } else if (!controlsPage.activeSelf)
        {
            //windowsOpen = false;
        }

        TogglePause();
        CheckOpenWindows();


        //disable ability to open any other windows if pause menu is active
        if (pauseMenu.activeSelf)
        {
            for (int i = 0; i < UI_Windows.Length; i++)
            {
                UI_Windows[i].SetActive(false);
            }
        }
    }

    public void CheckOpenWindows()
    {
        int windowOpenCount = 0;
        //go through all window gameobjects and check if any are active.
        //if they are, set windowsOpen = true
        for (int i = 0; i < UI_Windows.Length; i++)
        {
            if (UI_Windows[i].activeSelf) //if any window is open
            {
                windowOpenCount++;
            }
        }

        if (windowOpenCount > 0)
        {
            windowsOpen = true;
        } else
        {
            windowsOpen = false;
        }

        print(windowOpenCount);
        
    }

    public void TogglePause()
    {
        #region If All Windows Closed
        //IF THERE ARE NOW WINDOWS OPEN
        if (Input.GetKeyDown(KeyCode.Escape)
            && !windowsOpen) //only activate when all windows are closed
        {
            if (!pauseMenu.activeSelf)
            {
                Cursor.visible = true;
                player.GetComponent<CharControl>().sfxControlScript.ButtonIn();
                pauseMenu.SetActive(true);
                Time.timeScale = 0;

            }
            else if (pauseMenu.activeSelf)
            {
                player.GetComponent<CharControl>().sfxControlScript.ButtonOut();
                ContinueButton();
            }
        }
        #endregion

        #region If Windows Still Open
        //IF THERE ARE WINDOWS STILL OPEN
        if (Input.GetKeyDown(KeyCode.Escape)
            && windowsOpen)
        {
            for (int i = 0; i < UI_Windows.Length; i++)
            {
                UI_Windows[i].SetActive(false);
            }
            tabs.SetActive(false);
            player.GetComponent<CharControl>().sfxControlScript.InventoryClose();
            charControlScript.UnfreezeCam();
        }
        #endregion
    }

    public void ContinueButton()
    {
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        Cursor.visible = true;
        pauseMenu.SetActive(false);
        charControlScript.UnfreezeCam();
        StartCoroutine(LoadAsyncro());
    }

    public void InvertGlidingToggle()
    {
        switch (invertGlidingToggleButton.isOn)
        {
            case true:
                charControlScript.invertGliding = true;
                break;


            case false:
                charControlScript.invertGliding = false;
                break;
        }
    }

    public void ControlsButton()
    {
        if (!controlsPage.activeSelf)
        {
            controlsPage.SetActive(true);
            airPage.enabled = true;
        }
        pauseMenu.SetActive(false);
    }

    public void BackButton()
    {
        if (controlsPage.activeSelf)
        {
            controlsPage.SetActive(false);
        }
        pauseMenu.SetActive(true);
    }

    public void ControlsAirPage()
    {
        airPage.enabled = true;
        groundPage.enabled = false;
    }

    public void ControlsGroundPage()
    {
        groundPage.enabled = true;
        airPage.enabled = false;
    }

    public void SkipTutorial()
    {
        pauseMenu.SetActive(false);
        skipAskPage.SetActive(true);
    }

    public void SkipNo()
    {
        pauseMenu.SetActive(true);
        skipAskPage.SetActive(false);
    }

    public void SkipYes()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        skipAskPage.SetActive(false);
        StartCoroutine(LoadMainLvl());
    }

    IEnumerator LoadAsyncro()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(mainMenuScene);

        while (!operation.isDone)
        {
            loadingScreen.SetActive(true);
            loadingBar.value = operation.progress / 0.9f;
            Cursor.visible = true;

            yield return null;
        }
    }

    IEnumerator LoadMainLvl()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(mainScene);

        while (!operation.isDone)
        {
            loadingScreen.SetActive(true);
            loadingBar.value = operation.progress / 0.9f;

            yield return null;
        }
    }
}
