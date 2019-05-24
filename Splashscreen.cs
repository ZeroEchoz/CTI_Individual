using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splashscreen : MonoBehaviour {

    [Tooltip("0: play, 1: Options, 2: Credits, 3: Exit")]
    public string[] sceneNames;
    public Slider loadingbar;
    public GameObject loadingScreen;
    public GameObject creditsScreen;
    public GameObject[] creditsPages;
    public GameObject[] creditsButtons;

    public GameObject controlsPage;
    public Image airPage;
    public Image groundPage;

    void Start () {
        loadingScreen.SetActive(false);
        creditsScreen.SetActive(false);
        controlsPage.SetActive(false);

        Time.timeScale = 1;

        creditsButtons[0].SetActive(false);
        creditsPages[1].SetActive(false);

	}
	
	void Update () {
		
	}

    public void PlayScene()
    {
        StartCoroutine(LoadAsyncro());
    }

    public void CreditsPage()
    {
        creditsScreen.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        creditsScreen.SetActive(false);
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

    public void BackButton()
    {
        if (controlsPage.activeSelf)
        {
            controlsPage.SetActive(false);
        }
    }

    public void NextButtonCreds()
    {
        if (creditsPages[0].activeSelf
            && !creditsPages[1].activeSelf)
        {
            creditsPages[1].SetActive(true);
            creditsPages[0].SetActive(false);

            creditsButtons[0].SetActive(true);
            creditsButtons[1].SetActive(false);
            creditsButtons[2].SetActive(false);
        }
    }

    public void PreviousButtonCreds()
    {
        if (!creditsPages[0].activeSelf
             && creditsPages[1].activeSelf)
        {
            creditsPages[1].SetActive(false);
            creditsPages[0].SetActive(true);

            creditsButtons[0].SetActive(false);
            creditsButtons[1].SetActive(true);
            creditsButtons[2].SetActive(true);
        }
    }

    public void ControlsButton()
    {
        if (!controlsPage.activeSelf)
        {
            controlsPage.SetActive(true);
            airPage.enabled = true;
            groundPage.enabled = false;
        }
    }


    IEnumerator LoadAsyncro ()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNames[0]);

        while (!operation.isDone)
        {
            loadingScreen.SetActive(true);
            loadingbar.value = operation.progress / 0.9f;

            yield return null;
        }
    }

    public IEnumerator LoadAsyncroMain()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNames[1]);

        while (!operation.isDone)
        {
            loadingScreen.SetActive(true);
            loadingbar.value = operation.progress / 0.9f;

            yield return null;
        }
    }
}
