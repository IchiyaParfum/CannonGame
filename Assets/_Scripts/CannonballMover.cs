using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballMover : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        //Destroy the cannonball if it collides with any target
        Destroy(this.gameObject);
    }
}
