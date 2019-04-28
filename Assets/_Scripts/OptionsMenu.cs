using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public MyToggleGroup difficultyToggles;
    public MyToggleGroup controllerToggles;
    
    void Start()
    {
        volumeSlider.onValueChanged.AddListener(delegate { OnValueChanged(volumeSlider.value); });
        difficultyToggles.onChange += DifficultyToggles_onChange;
        controllerToggles.onChange += ControllerToggles_onChange;
    }

    private void ControllerToggles_onChange(object sender, System.EventArgs e)
    {
        MySceneManager.Parameters.Controllables = (ControllableFactory.Controllables)controllerToggles.ActiveToggle;
    }

    private void DifficultyToggles_onChange(object sender, System.EventArgs e)
    {
        MySceneManager.Parameters.Difficulty = (Difficulty)difficultyToggles.ActiveToggle;
    }

    public void OnValueChanged(float value)
    {
        MySceneManager.Parameters.Volume = value;  
    }

    public void OpenMenu()
    {
        MySceneManager.LoadScene(MySceneManager.Scenes.Menu);
    }
}
