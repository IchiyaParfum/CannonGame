using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        MySceneManager.Parameters.Level = 1;
        MySceneManager.LoadScene(MySceneManager.Scenes.Game);
    }

    public void OpenOptions()
    {
        MySceneManager.LoadScene(MySceneManager.Scenes.Options);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
