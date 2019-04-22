using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class GameController : MonoBehaviour
{
    public float speed;
    public float movement;
    public Transform[] path;
    public Boundary boundary;
    public GameObject cannonball;
    public float fireRate;
    public Text scoreText;

    private int score;
    private int current;
    private Quaternion rotation;
    private float nextFire;
    

    void Start()
    {
        score = 0;
        UpdateScore();
        rotation = new Quaternion();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject g = Instantiate(cannonball, transform.position, transform.rotation);
            g.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, speed));
        }

        if(transform.position != path[current].position)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, path[current].position, movement * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else
        {
            current = (current + 1) % path.Length;
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
}