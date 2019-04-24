using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float speed;
    public float cbSpeed;
    public float movement;
    public GameObject cannonball;
    public Transform[] path;
    public float fireRate;
    private int current;
    private Quaternion rotation;
    private float nextFire;



    void Start()
    {
        rotation = new Quaternion();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject g = Instantiate(cannonball, transform.position, transform.rotation);
            g.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, cbSpeed));
        }

        if (transform.position != path[current].position)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, path[current].position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else if (current < path.Length - 1)
        {
            current++;
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
}
