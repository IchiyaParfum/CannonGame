
using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;


[RequireComponent(typeof(AudioSource))]
public class Target : MonoBehaviour
{
    public GameLogic gameLogic;
    public int scoreValue;
    public AudioClip destructionSound;
    public AudioClip screamingSound;
    public bool IsDestroyed { get; private set; }   //Property read only

    private AudioSource source;
    private float destructionDelay;


    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();    //Create AudioSource component on game object to play sounds afterwards
        destructionDelay = Mathf.Max(destructionSound.length, screamingSound.length); 

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cannonball"))
        {
            IsDestroyed = true;
            source.PlayOneShot(destructionSound);
            source.PlayOneShot(screamingSound);

            //Add Ridgid body and BoxCollider to children => Explosion
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.AddComponent<BoxCollider>();
            }

            GetComponent<Collider>().enabled = false;   //Building cant be hit anymore
            gameLogic.TargetHit(this);    //Tell game logic that target has been hit
            Invoke("DestroyBuilding", destructionDelay);    //Destroy target object after animation sound
            
        }

    }

    void DestroyBuilding()
    {
        gameLogic.TargetDestroyed(this);    //Tell game logic that target has been destroyed => Animation finished
        Destroy(this.gameObject); //Destroy game object immediately
    }


}
