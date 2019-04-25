using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySceneManager
{
    private static int nOfTargets = 1;

    public static void LoadScene(string name, int nOfTargets)
    {
        MySceneManager.nOfTargets = nOfTargets;
        Application.LoadLevel(name);
    }

    public static int GetSceneArgs()
    {
        return MySceneManager.nOfTargets;
    }
}

public class GameLogic : MonoBehaviour
{
    public GameObject[] spawn;
    public GameObject[] targets;
    public CannonController cannonController;
    public Text scoreText;
    public Text centerText;

    private int score;
    private int totalScore;
    private ArrayList tar;
      

    void Start()
    {
        Time.timeScale = 1;
        score = 0;
        totalScore = 0;

        //Load targets at random into loadedTargets and place them on random spawn locations
        ArrayList spawns = new ArrayList(spawn);
        tar = new ArrayList();
        for(int i = 0; i < Mathf.Min(MySceneManager.GetSceneArgs(), spawn.Length); i++)
        {
            GameObject g = targets[Random.Range(0, targets.Length)];
            totalScore += g.GetComponent<HouseBehaviour>().scoreValue;

            int rnd = Random.Range(0, spawns.Count);
            GameObject s = (GameObject) spawns[rnd];
            spawns.RemoveAt(rnd);

            tar.Add(Instantiate(g, s.transform.position, s.transform.rotation));
        }
        UpdateScore();
        UpdateCenter("");
    }

    void Update()
    {
        if(isFinished())
        {
            Time.timeScale = 0; //Stops physics
            UpdateCenter("Level finished\nPress R to continue");
            if (Input.GetKeyDown(KeyCode.R))
            {
                MySceneManager.LoadScene("Minigame", MySceneManager.GetSceneArgs() + 1);
            }
        }
        else if (isGameOver())
        {
            Time.timeScale = 0; //Stops physics
            UpdateCenter("Game Over\nPress any key to restart");
            if (Input.anyKeyDown)
            {
                MySceneManager.LoadScene("Minigame", 1);
            }
        }
        
    }

    bool isFinished()
    {
        return tar.Count == 0;
    }

    bool isGameOver()
    {
        return cannonController.isFinished();
    }

    public void TargetDestroyed(GameObject target)
    {
        if (target.tag.Equals("Target"))
        {
            tar.Remove(target);
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score + "/" + totalScore;
    }


    void UpdateCenter(string text)
    {
        centerText.text = text;
    }
}