using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        MyAudioManager.Instance.PlayMusic();
    }
    public void PlayGame()
    {
        MyAudioManager.Instance.StopMusic();
        MySceneManager.Instance.Parameters.Level = 1;
        MySceneManager.Instance.LoadScene(MySceneManager.Scenes.Game);
    }

    public void OpenOptions()
    {
        MySceneManager.Instance.LoadScene(MySceneManager.Scenes.Options);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
