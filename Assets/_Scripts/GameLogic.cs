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
        for(int i = 0; i < MySceneManager.GetSceneArgs(); i++)
        {
            GameObject g = targets[Random.Range(0, targets.Length)];
            totalScore += g.GetComponent<HouseBehaviour>().scoreValue;

            int rnd = Random.Range(0, spawns.Count);
            GameObject s = (GameObject) spawns[rnd];
            spawns.RemoveAt(rnd);

            tar.Add(Instantiate(g, s.transform.position, s.transform.rotation));
        }
        UpdateScore();
    }

    void Update()
    {
        if(isFinished())
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                MySceneManager.LoadScene("Minigame", MySceneManager.GetSceneArgs() + 1);
            }

            Time.timeScale = 0; //Stops physics
            centerText.text = "Level finished\nPress R to continue";
        }
        else if (isGameOver())
        {
            //TODO Load title screen
        }
        
    }

    bool isFinished()
    {
        return tar.Count == 0;
    }

    bool isGameOver()
    {
        return false;
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


    void GameOver()
    {
        centerText.text = "Game Over";
    }
}