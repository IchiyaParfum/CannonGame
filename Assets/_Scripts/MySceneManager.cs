using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySceneManager
{
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

    public static Dictionary<Scenes, object> Parameters { get; private set; }

    public static void LoadScene(Scenes scene)
    {
        Application.LoadLevel(scene.Value);
    }

    public static void SaveParameters(Scenes scene, object data)
    {
        if (Parameters.ContainsKey(scene))
        {
            //Remove data if it already exists
            Parameters.Remove(scene);   
        }
        Parameters.Add(scene, data);
    }
}
