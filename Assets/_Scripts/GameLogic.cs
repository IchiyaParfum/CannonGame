﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum GameState
{
    Active,
    GameOver,
    Finished
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
    private int intactTargets;
    private int aliveTargets;
    
    
    public GameLogic()
    {
        
    }

    void Start()
    {
        totalLevels = spawn.Length;

        //Load targets at random and place them on random spawn locations
        List<int> possible = Enumerable.Range(0, spawn.Length).ToList();
        for (int i = 0; i < Mathf.Min(1, spawn.Length); i++)
        {
            //Choose random target from list
            GameObject g = targets[Random.Range(0, targets.Length)];
            Target hb = g.GetComponent<Target>();
            hb.gameLogic = this;
            totalScore += hb.scoreValue;
            //Choose random spawn location for target and disable it for next round
            int index = Random.Range(0, possible.Count);
            GameObject s = spawn[possible[index]];
            possible.RemoveAt(index);
            //Create target
            Instantiate(g, s.transform.position, s.transform.rotation);
            intactTargets++;
            aliveTargets++;
        }
        UpdateScore();
        UpdateCenter("");
        UpdateLevel(3);
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
                    if (totalLevels >= totalLevels)
                    {
                        UpdateCenter("You Won");
                    }
                    else
                    {
                        //Load next level with one more target
                        MySceneManager.LoadScene(MySceneManager.Scenes.Game);
                    }
                }
                break;
            case GameState.GameOver:
                //Wait for user to press any button
                if (Input.anyKeyDown)
                {
                    MySceneManager.LoadScene(MySceneManager.Scenes.Game);
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