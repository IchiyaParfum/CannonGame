
using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class HouseBehaviour : MonoBehaviour
{
    public int scoreValue;
    public AudioSource sound;

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
            Invoke("DestroyBuilding", 3);
        }

    }

    void DestroyBuilding()
    {
        gameController.TargetDestroyed(gameObject);
        Destroy(this.gameObject); //Destroy game object with delay
    }


}
