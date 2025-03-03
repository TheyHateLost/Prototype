using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonManager : MonoBehaviour
{

    //Plays next scene in BuildIndex
    public void PlayScene()
    {
        SceneManager.LoadScene(1);
    }

    //Plays the scene written down in the Unity Inspector
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Closes the application
    public void ExitGame()
    {
        Application.Quit();
    }
}
