using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMenu : MonoBehaviour {

    public string menuName;

    public void ReturnMenu()
    {
        SceneManager.LoadScene(menuName);
    }
}
