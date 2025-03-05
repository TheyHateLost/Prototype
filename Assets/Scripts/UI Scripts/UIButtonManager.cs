using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonManager : MonoBehaviour
{
    [SerializeField]private GameObject Menu;

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
    //
    public void OpenMenu()
    {
        Menu.SetActive(true);
    }

    public void CloseMenu()
    {
        Menu.SetActive(false);
    }
}
