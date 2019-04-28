using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public ToggleGroup difficultyToggles;
    public ToggleGroup controllerToggles;

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(delegate { OnValueChanged(volumeSlider.value); });
    }

    public void OnValueChanged(float value)
    {
        Debug.Log("Changed");
    }
}
