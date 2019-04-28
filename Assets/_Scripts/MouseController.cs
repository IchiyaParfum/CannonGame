using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FocusButton
{
    public bool Focused { get; private set; }
    private GameObject g;

    public FocusButton(GameObject g)
    {
        this.g = g; //Keep reference to button
        AddListener();
    }

    private void AddListener()
    {
        EventTrigger trigger = g.AddComponent<EventTrigger>();

        //Pointer enter event
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { Focused = true; });
        trigger.triggers.Add(entryEnter);

        //Pointer exit event
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { Focused = false; });
        trigger.triggers.Add(entryExit);
    }
}

public class MouseController : MonoBehaviour
{
    //Buttons
    public GameObject MouseUp;
    public GameObject MouseDown;
    public GameObject MouseLeft;
    public GameObject MouseRight;

    public enum Key
    {
        MouseUp,
        MouseDown,
        MouseLeft,
        MouseRight
    }

    public Dictionary<Key, FocusButton> Keys { get; private set; }

    public MouseController()
    {
        Keys = new Dictionary<Key, FocusButton>();
    }

    void Start()
    {
        Keys.Add(Key.MouseDown, new FocusButton(MouseDown));
        Keys.Add(Key.MouseUp, new FocusButton(MouseUp));
        Keys.Add(Key.MouseLeft, new FocusButton(MouseLeft));
        Keys.Add(Key.MouseRight, new FocusButton(MouseRight));
    }

}
