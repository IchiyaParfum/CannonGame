using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MyToggleGroup : MonoBehaviour
{
    public List<Toggle> toggles;

    public delegate void OnChangeHandler(object sender, EventArgs e);
    public event OnChangeHandler onChange;

    public int ActiveToggle { get; private set; }

    void Start()
    {
        bool on = false;
        //Register method on all toggles
        for(int i = 0; i<toggles.Count;i++)
        {
            Toggle t = toggles[i];
            toggles[i].onValueChanged.AddListener(delegate {
                onValueChanged(t);
            });
            //If one button was already on => turn all following buttongs off
            if (on)
            {
                toggles[i].isOn = false;
                ActiveToggle = i;
            }
            else
            {
                on = toggles[i].isOn;
            }    
        }
        //Activate first toggle if no other has been on at the beginning
        if (!on)
        {
            toggles[0].isOn = true;
            ActiveToggle = 0;
        }
    }

    void onValueChanged(Toggle t)
    {
        lock (toggles)
        {
            if (t.isOn)
            {
                ActiveToggle = toggles.IndexOf(t);

                //Deselect all other toggles if one was selected
                for (int i = 0; i < toggles.Count; i++)
                {
                    if (toggles[i] != t)
                    {
                        toggles[i].isOn = false; //This causes the toggle button to fire another onValueChanged event which enters this method => Multithreading problems (ActiveToggle Index has to be set before changing state)
                    }
                }
                onChange?.Invoke(this, new EventArgs());
            }
            else
            {
                //Set toggle back on if no other has been selected
                if (t == toggles[ActiveToggle])
                {
                    t.isOn = true;
                }
            }
        }
    }
}
