
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBehaviour : MonoBehaviour
{
    public int scoreValue;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Cannonball"))
        {
            Debug.Log("Cannonball");
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.AddComponent<BoxCollider>();
            }
            gameController.AddScore(scoreValue);
        }
    }
}
