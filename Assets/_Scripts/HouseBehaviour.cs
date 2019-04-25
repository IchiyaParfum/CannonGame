
using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;


[RequireComponent(typeof(AudioSource))]
public class HouseBehaviour : MonoBehaviour
{
    public int scoreValue;
    public AudioClip destructionSound;
    public AudioClip screamingSound;

    private AudioSource source;
    private GameLogic gameController;
    private float destructionDelay;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameLogic");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameLogic>();
        }
        source = gameObject.AddComponent<AudioSource>();
        destructionDelay = Mathf.Max(destructionSound.length, screamingSound.length);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cannonball"))
        {
            source.PlayOneShot(destructionSound);
            source.PlayOneShot(screamingSound);
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.AddComponent<BoxCollider>();
            }

            GetComponent<Collider>().enabled = false;   //Building cant be hit anymore
            gameController.AddScore(scoreValue);    //Add score on game controller
            Invoke("DestroyBuilding", destructionDelay);
            
        }

    }

    void DestroyBuilding()
    {
        gameController.TargetDestroyed(gameObject);
        Destroy(this.gameObject); //Destroy game object with delay
    }


}
