using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fungus;

public class LoadMainLvl : MonoBehaviour {

    public string mainScene;
    public GameObject loadingScreen;
    public Slider loadingBar;

    public void LoadMainWorld()
    {
        StartCoroutine(LoadMain());
    }

    IEnumerator LoadMain()
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
