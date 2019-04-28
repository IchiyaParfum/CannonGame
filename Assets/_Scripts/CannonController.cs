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
    void UpdateControl();
}

public struct Boundary
{
    public Vector3 Max { get; set; } 
    public Vector3 Min { get; set; }

    public bool inBounds(Vector3 v, out Vector3 inBoundV)
    {
        bool inBounds = true;
        inBoundV = new Vector3();

        for(int i = 0; i < 3; i++)
        {
            if(v[i] > Max[i] )
            {
                inBoundV[i] = Max[i];
                //If only one coordinate is not in bounds => not in bounds
                inBounds = false;
            }else if(v[i] < Min[i])
            {
                inBoundV[i] = Min[i];
                //If only one coordinate is not in bounds => not in bounds
                inBounds = false;
            }
            else
            {
                inBoundV[i] = v[i];
            }
        }
        return inBounds;
    }

}
public struct ControllableParameters
{
    public float Sensitivity { get; set; }
    public float Speed { get; set; }
    public Boundary Boundary { get; set; }
}

public class KeyboardControllable : Controllable
{
    private Vector3 rotation;
    private ControllableParameters param;

    public KeyboardControllable(ControllableParameters param)
    {
        this.param = param;
    }
    public Quaternion Rotation { get; set; }
    public Vector3 Position { get; set; }

    public void UpdateControl()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rotation[0] -= Time.deltaTime * param.Speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rotation[0] += Time.deltaTime * param.Speed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotation[1] -= Time.deltaTime * param.Speed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotation[1] += Time.deltaTime * param.Speed;
        }
        Rotation = Quaternion.Euler(rotation);
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
    private float speed = 200;
    private float angle = 0f;

    public bool fireClicked()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    public void Update(GameObject target)
    {
        float offset = 100;
        float sens = 0.05f;
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 5f; //The distance between the camera and object
        
        Vector3 object_pos = Camera.main.WorldToScreenPoint(target.transform.position);

        float angleY = 2*Mathf.Atan2(sens*(mouse_pos.x - object_pos.x), mouse_pos.z) * Mathf.Rad2Deg;

        float angleX = - Mathf.Atan2(sens * (mouse_pos.y - object_pos.y - offset), mouse_pos.z) * Mathf.Rad2Deg;

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

    public void UpdateControl()
    {
        //throw new System.NotImplementedException();
    }
}


public class CannonController : MonoBehaviour
{
    public MouseController m;
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


    void Start()
    {
        
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
        Controllable.UpdateControl();  //Update controllable
    }

    public bool isFinished()
    {
        //Finished when cannon has reached end of path
        return current >= path.Length - 1;
    }
}
