using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonScript : MonoBehaviour
{
    public Sprite OnSprite;
    public Sprite OffSprite;

    private Button button;
    private Image image;
    private bool state = false;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);

        image = GetComponent<Image>();

        UpdateSprite();
    }


    void ButtonClicked()
    {
        state = !state; //Toggle state
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (state)
        {
            image.sprite = OnSprite;
        }
        else
        {
            image.sprite = OffSprite;
        }
    }
}
