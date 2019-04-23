using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

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

public class GameController : MonoBehaviour
{
    public float speed;
    public float cbSpeed;
    public float movement; 
    public Transform[] path;
    public GameObject[] targets;
    public Boundary boundary;
    public GameObject cannonball;
    public float fireRate;
    public Text scoreText;
    public Text centerText;

    private int score;
    private int current;
    private Quaternion rotation;
    private float nextFire;
    

    void Start()
    {
        print(Time.timeScale);
        Time.timeScale = 1;
        score = 0;
        UpdateScore();
        rotation = new Quaternion();

        for(int i = MySceneManager.GetSceneArgs() ; i < targets.Length; i++)
        {
            targets[i].SetActive(false);
        }
    }

    void Update()
    {
        CheckState();   
        
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject g = Instantiate(cannonball, transform.position, transform.rotation);
            g.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, cbSpeed));
        }

        if(transform.position != path[current].position)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, path[current].position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else if(current < path.Length - 1)
        {
            current++;
        }
        else
        {
            GameOver();
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rotation[0] -= Time.deltaTime * movement;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rotation[0] += Time.deltaTime * movement;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotation[1] -= Time.deltaTime * movement;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotation[1] += Time.deltaTime * movement;
        }
        transform.rotation = Quaternion.Euler(rotation[0], rotation[1], rotation[2]);
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void CheckState()
    {
        if (score >= MySceneManager.GetSceneArgs()*100)
        {
            LevelFinished();
        }
    }
    void LevelFinished()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            MySceneManager.LoadScene("Minigame", MySceneManager.GetSceneArgs() + 1);
        }

        Time.timeScale = 0; //Stops physics
        centerText.text = "Level finished\nPress R to continue";
    }

    void GameOver()
    {
        centerText.text = "Game Over";
    }
}