using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public enum Difficulty
{
    Easy = 0,
    Medium = 1,
    Hard = 2
}

public class GameLogic : MonoBehaviour
{
    public GameObject[] spawn;
    public GameObject[] targets;
    public CannonController cannonController;
    public Text scoreText;
    public Text centerText;
    public Text levelText;

    private GameState state = GameState.Active;
    private int score;
    private int totalScore;
    private int totalLevels;
    private int totalTargets;
    private int intactTargets;
    private int aliveTargets;


    private enum GameState
    {
        Active,
        GameOver,
        Finished
    }

    void Start()
    {
        totalLevels = spawn.Length;

        //Load targets at random and place them on random spawn locations
        List<int> possible = Enumerable.Range(0, spawn.Length).ToList();
        totalTargets = Mathf.Min(MySceneManager.Instance.Parameters.Level, spawn.Length);
        for (int i = 0; i < totalTargets; i++)
        {
            //Choose random target from list
            //GameObject g = targets[Random.Range(0, targets.Length)];
            GameObject g = getRandomTarget(MySceneManager.Instance.Parameters.Difficulty, targets);

            //Choose random spawn location for target and disable it for next round
            int index = Random.Range(0, possible.Count);
            GameObject s = spawn[possible[index]];
            possible.RemoveAt(index);
            //Create target
            g = Instantiate(g, s.transform.position, s.transform.rotation);
            Target hb = g.GetComponent<Target>();
            hb.gameLogic = this;
            totalScore += hb.scoreValue;

            intactTargets++;
            aliveTargets++;
        }
        UpdateScore();
        UpdateCenter("");
        UpdateLevel(MySceneManager.Instance.Parameters.Level);
    }

    void Update()
    {
        switch (state)
        {
            case GameState.Active:
                //Wait for all buildings to be hit
                if(intactTargets <= 0)
                {
                    //All buildings hit => succeeded
                    UpdateCenter("Level accomplished");
                    state = GameState.Finished;
                }else if (cannonController.isFinished())
                {
                    //Path finished => lost
                    UpdateCenter("Game Over");
                    state = GameState.GameOver;
                }
                break;
            case GameState.Finished:
                //Wait for all buildings to be destroyed               
                if (aliveTargets <= 0)
                {
                    //All targets have been destroyed
                    if (MySceneManager.Instance.Parameters.Level >= totalLevels)
                    {
                        UpdateCenter("You Won");
                        state = GameState.GameOver;
                    }
                    else
                    {
                        //Load next level
                        MySceneManager.Instance.Parameters.Level++;
                        MySceneManager.Instance.LoadScene(MySceneManager.Scenes.Game);
                    }
                }
                break;
            case GameState.GameOver:
                //Wait for user to press any button
                if (Input.anyKeyDown)
                {
                    MySceneManager.Instance.LoadScene(MySceneManager.Scenes.Menu);
                }
                break;
        }
    }

    

    public void TargetHit(Target target)
    {
        AddScore(target.scoreValue);
        intactTargets--;
    }

    public void TargetDestroyed(Target target)
    {
        aliveTargets--;
    }

    GameObject getRandomTarget(Difficulty d, GameObject[] targets)
    {
        //targets array has to be ordered from difficult target to easy target

        float rnd = Random.Range(0f, 1f);   //Probability between 0% and 100%
        float perc = 0f;
        float diff = (int)d + 1;
        for (int i = 0; i <targets.Length; i++)
        {
            float a = (diff + 1) * (targets.Length) / 2f;
            float pi = 1/a * ((1 - diff) / (targets.Length-1) * i + diff); //Probability function 
            Debug.Log("Probability: " + pi + " at index: " + i +" with rnd: " + rnd);
            perc += pi;
            if (rnd < perc)
            {
                return targets[i];
            }
        }
        return targets[targets.Length-1];     
    }

    void AddScore(int newScoreValue)
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

    void UpdateLevel(int level)
    {
        levelText.text = "Level: " + level + "/" + totalLevels;
    }
}