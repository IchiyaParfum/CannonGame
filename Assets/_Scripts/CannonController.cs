using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float speed;
    public float cbSpeed;
    public float movement;
    public GameObject cannonball;
    public Transform spawnPosition;
    public float fireRate;
    public AudioClip shootSound;

    private AudioSource source;
    private Vector3[] path;
    private int current;
    private Quaternion rotation;
    private float nextFire;



    void Start()
    {
        rotation = new Quaternion();
        source = gameObject.AddComponent<AudioSource>();

        LineRenderer lr = GetComponent<LineRenderer>();
        path = new Vector3[lr.positionCount];
        lr.GetPositions(path);

    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {          
            source.PlayOneShot(shootSound);
            GameObject g = Instantiate(cannonball, spawnPosition.position, transform.rotation);
            g.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, cbSpeed));
            nextFire = Time.time + fireRate;
        }
                
        if (transform.position != path[current])
        {
            Vector3 next = Vector3.MoveTowards(transform.position, path[current], speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(next);
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

    public bool isFinished()
    {
        //Finished when cannon has reached end of path
        return current >= path.Length - 1f;
    }
}
