using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour, Controllable
{
    public GameObject MouseUp;
    public GameObject MouseDown;
    public GameObject MouseLeft;
    public GameObject MouseRight;

    public Quaternion Rotation { get; set; }
    public Vector3 Position { get; set; }
    private bool MouseIsUp;
    private bool MouseIsDown;
    private bool MouseIsLeft;
    private bool MouseIsRight;
    private float x;
    private float y;
    private ControllableParameters param;

    public enum Key
    {
        MouseUp,
        MouseDown,
        MouseLeft,
        MouseRight
    }

    void Start()
    {
        param = new ControllableParameters();
        param.Speed = 60;
        SetUpButton(MouseUp, Key.MouseUp);
        SetUpButton(MouseDown, Key.MouseDown);
        SetUpButton(MouseLeft, Key.MouseLeft);
        SetUpButton(MouseRight, Key.MouseRight);
    }

    void SetUpButton(GameObject g, Key k)
    {
        EventTrigger trigger = g.AddComponent<EventTrigger>();

        //Pointer enter event
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data, k); });
        trigger.triggers.Add(entryEnter);

        //Pointer exit event
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data, k); });
        trigger.triggers.Add(entryExit);
    }

    public void OnPointerEnterDelegate(PointerEventData data, Key k)
    {
        UpdateButtonState(k, true);
    }

    public void OnPointerExitDelegate(PointerEventData data, Key k)
    {
        UpdateButtonState(k, false);
    }

    public bool fireClicked()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    void UpdateButtonState(Key k, bool state)
    {
        switch (k)
        {
            case Key.MouseDown:
                MouseIsDown = state;
                break;
            case Key.MouseUp:
                MouseIsUp = state;
                break;
            case Key.MouseLeft:
                MouseIsLeft = state;
                break;
            case Key.MouseRight:
                MouseIsRight = state;
                break;
        }
    }

    public void UpdateControl()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("KeyDow");
            if (MouseIsDown)
            {
                x += Time.deltaTime * param.Speed;
            }
            if (MouseIsUp)
            {
                x -= Time.deltaTime * param.Speed;
            }
            if (MouseIsRight)
            {
                y += Time.deltaTime * param.Speed;
            }
            if (MouseIsLeft)
            {
                y -= Time.deltaTime * param.Speed;
            }
        }
        
        
        Rotation = Quaternion.Euler(new Vector3(x, y, 0));
    }
}
