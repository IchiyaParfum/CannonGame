using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class CannonController : MonoBehaviour
{
    public float speed;
    public Boundary boundary;
    public GameObject cannonball;
    public float fireRate;

    private float nextFire;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject g = Instantiate(cannonball, transform.position, transform.rotation);
            g.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0,speed,0));
        }
    }

    void FixedUpdate()
    {
      
    }
}