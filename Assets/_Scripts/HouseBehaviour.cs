
using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;

public class HouseBehaviour : MonoBehaviour
{
    public int scoreValue;

    private GameLogic gameController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameLogic");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameLogic>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cannonball"))
        {
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.AddComponent<BoxCollider>();
            }
            GetComponent<Collider>().enabled = false;   //Building cant be hit anymore
            gameController.AddScore(scoreValue);    //Add score on game controller
            Destroy(gameObject, 5); //Destroy game object with delay
            
        }


    }
}
