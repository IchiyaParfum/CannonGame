using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Controllable
{
    bool fireClicked();
    Quaternion Rotation
    {
        get;
        set;
    }
    Vector3 Position
    {
        get;
        set;
    }
    void Update();
}

public class KeyboardControllable : Controllable
{
    private Quaternion rotation;
    private int movement = 10;

    public Quaternion Rotation { get; set; }
    public Vector3 Position { get; set; }

    public void Update()
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
        Rotation = Quaternion.Euler(rotation[0], rotation[1], rotation[2]);
    } 

    public bool fireClicked()
    {
        return Input.GetKeyDown(KeyCode.LeftControl);
    }

}

public class MouseControllable : Controllable
{
    private Vector3 rotation = new Vector3();
    private float tile = 4f;
    public Quaternion Rotation { get; set; }
    public Vector3 Position { get; set; }
    private float speed = 60;
    private float angle = 0f;
    public bool fireClicked()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    public void Update(GameObject target)
    {
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 5f; //The distance between the camera and object
        
        Vector3 object_pos = Camera.main.WorldToScreenPoint(target.transform.position);

        float angleY = 2*Mathf.Atan2(mouse_pos.x - object_pos.x, mouse_pos.z) * Mathf.Rad2Deg;
        float angleX = - Mathf.Atan2(mouse_pos.y - object_pos.y, mouse_pos.z) * Mathf.Rad2Deg;

        float dw = Time.deltaTime * speed;
        Debug.Log(angleX);
        if (angleX >= 0 && rotation[0] + dw < angleX)
        {
            Debug.Log("Add");
            rotation[0] += dw;
            
        }
        else if(angleX < 0 && rotation[0] - dw > angleX)
        {
            Debug.Log("Sub");
            rotation[0] -= dw;
        }

        if (angleY >= 0 && rotation[1] + dw < angleY)
        {
            Debug.Log("Add");
            rotation[1] += dw;

        }
        else if (angleY < 0 && rotation[1] - dw > angleY)
        {
            Debug.Log("Sub");
            rotation[1] -= dw;
        }
        Rotation = Quaternion.Euler(rotation);

        //rotation[0] -= Input.GetAxis("Mouse Y") * tile;
        //rotation[1] += Input.GetAxis("Mouse X") * tile;
        //Rotation = Quaternion.Euler(rotation[0], rotation[1], rotation[2]);


    }

    public void Update()
    {
        //throw new System.NotImplementedException();
    }
}

public class CannonController : MonoBehaviour
{
    public float speed;
    public float cbSpeed;
    public float movement;
    public GameObject cannonball;
    public Transform spawnPosition;
    public float fireRate;
    public AudioClip shootSound;
    public UnityEngine.UI.Slider pathProgress;

    public Controllable Controllable { get; set; }

    private AudioSource source;
    private Vector3[] path;
    private int current;
    private float nextFire;
    private MouseControllable m;


    void Start()
    {
        m = new MouseControllable();
        Controllable = m;
        source = gameObject.AddComponent<AudioSource>();

        LineRenderer lr = GetComponent<LineRenderer>();
        path = new Vector3[lr.positionCount];
        lr.GetPositions(path);

        pathProgress.minValue = 0;
        pathProgress.maxValue = path.Length - 1;
        pathProgress.value = pathProgress.minValue;
    }

    void Update()
    {
        if (Controllable.fireClicked() && Time.time > nextFire)
        {          
            source.PlayOneShot(shootSound);
            GameObject g = Instantiate(cannonball, spawnPosition.position, transform.rotation);
            g.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, cbSpeed));
            nextFire = Time.time + fireRate;
        }

        transform.rotation = Controllable.Rotation;

        if (transform.position != path[current])
        {
            Vector3 next = Vector3.MoveTowards(transform.position, path[current], speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(next);
            
        }
        else if (current < path.Length - 1)
        {
            current++;
            pathProgress.value = current;
        }
    }

    void FixedUpdate()
    {
        m.Update(gameObject);  //Update controllable
    }

    public bool isFinished()
    {
        //Finished when cannon has reached end of path
        return current >= path.Length - 1;
    }
}
