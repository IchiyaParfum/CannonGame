using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneParameters
{
    public Difficulty Difficulty { get; set; }
    public ControllableFactory.Controllables Controllables { get; set; }
    private int level;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            if(value > 0)
            {
                level = value;
            }
        }
    }
    private float volume;
    public float Volume
    {
        get
        {
            return volume;
        }
        set
        {
            if(value >= 0 && value <= 1)
            {
                volume = value;
            }
        }
    }
}

public class MySceneManager
{
    public static MySceneManager Instance = null;

    static MySceneManager()
    {
        if(Instance == null)
        {
            Instance = new MySceneManager();
        }
        //Create default parameters
        SceneParameters p = new SceneParameters();
        p.Difficulty = Difficulty.Medium;
        p.Level = 1;
        p.Volume = 1f;
        Instance.Parameters = p;
    }

    public sealed class Scenes
    {
        public static readonly Scenes Menu = new Scenes("Menu");
        public static readonly Scenes Game = new Scenes("Game");
        public static readonly Scenes Options = new Scenes("Options");

        private Scenes(string value)
        {
            Value = value;      
        }

        public string Value { get; private set; }
    }
    public SceneParameters Parameters { get; set; }

    public void LoadScene(Scenes scene)
    {
        Application.LoadLevel(scene.Value);
    }

}
