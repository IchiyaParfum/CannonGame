using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        MySceneManager.LoadScene("Minigame", 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
