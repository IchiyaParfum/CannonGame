using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControllableFactory
{
    private ControllableFactory factory;
    public enum Controllables
    {
        Keyboard = 0,
        Mouse = 1
    }
    public struct ControllableParameters
    {
        public float Sensitivity { get; set; }
        public float Speed { get; set; }
        public Boundary Boundary { get; set; }
    }
    public static ControllableFactory getInstance()
    {
        return new ControllableFactory();
    }
    public Controllable createControllable(Controllables c, ControllableParameters p)
    {
        switch (c)
        {
            case Controllables.Keyboard:
                GameObject.FindGameObjectWithTag("MouseController").SetActive(false);
                return new KeyboardControllable(p);
            case Controllables.Mouse:
                GameObject.FindGameObjectWithTag("MouseController").SetActive(true);
                return new MouseControllable(getMouseController(), p);
        }
        return null;
    }
    private MouseController getMouseController()
    {
        GameObject g = GameObject.FindGameObjectWithTag("MouseController");
        return g.GetComponent<MouseController>();
    }
    public Controllable createControllable(Controllables c)
    {
        ControllableParameters p = new ControllableParameters();
        p.Boundary = new Boundary(new Vector3(-180, -180, -180), new Vector3(180, 180, 180));
        p.Sensitivity = 1;
        switch (c)
        {
            case Controllables.Keyboard:
                p.Speed = 80;
                break;
            case Controllables.Mouse:
                p.Speed = 80;
                break;
        }
        return createControllable(c, p);
    }
    private class KeyboardControllable : Controllable
    {
        private Vector3 rotation;
        private ControllableParameters param;

        public KeyboardControllable(ControllableParameters param)
        {
            this.param = param;
        }
        public Quaternion Rotation { get; set; }
        public Vector3 Position { get; set; }

        public void Update()
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
    private class MouseControllable : Controllable
    {
        public Quaternion Rotation { get; set; }
        public Vector3 Position { get; set; }

        private MouseController mc;
        private ControllableParameters param;
        private Vector3 rotation = new Vector3();

        public MouseControllable(MouseController mc, ControllableParameters param)
        {
            this.param = param;
            this.mc = mc;

        }
        public bool fireClicked()
        {
            return Input.GetKeyDown(KeyCode.Mouse0);
        }

        public void Update()
        {
            //Only change position if left controll is pressed => Easier to control and shoot together
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (mc.Keys[MouseController.Key.MouseUp].Focused)
                {
                    rotation[0] -= Time.deltaTime * param.Speed;
                }
                if (mc.Keys[MouseController.Key.MouseDown].Focused)
                {
                    rotation[0] += Time.deltaTime * param.Speed;
                }
                if (mc.Keys[MouseController.Key.MouseLeft].Focused)
                {
                    rotation[1] -= Time.deltaTime * param.Speed;
                }
                if (mc.Keys[MouseController.Key.MouseRight].Focused)
                {
                    rotation[1] += Time.deltaTime * param.Speed;
                }
                param.Boundary.inBounds(rotation, out rotation);
                Rotation = Quaternion.Euler(rotation);
            }
        }
    }
    
}
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
public class Boundary
{
    public Vector3 Max { get; set; } 
    public Vector3 Min { get; set; }

    public Boundary(Vector3 min, Vector3 max)
    {
        this.Min = min;
        this.Max = max;
    }
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
public class CannonController : MonoBehaviour
{
    public GameObject cannonball;
    public Transform spawnPosition;
    public float fireRate;
    public float cannonballSpeed;
    public float pathSpeed;
    public AudioClip shootSound;
    public UnityEngine.UI.Slider pathProgress;

    public Controllable controllable { get; set; }

    private AudioSource source;
    private Vector3[] path;
    private int current;
    private float nextFire;

    void Start()
    {
        controllable = ControllableFactory.getInstance().createControllable(MySceneManager.Parameters.Controllables);

        source = gameObject.AddComponent<AudioSource>();    //Add audio source to play sounds afterwards

        try
        {
            //Try getting the path points from linerenderer component
            LineRenderer lr = GetComponent<LineRenderer>();
            path = new Vector3[lr.positionCount];
            lr.GetPositions(path);
        }
        catch(UnityException ex)
        {
            Debug.Log(ex);
        }
        
        //Init slider to display remaining path
        pathProgress.minValue = 0;
        pathProgress.maxValue = path.Length - 1;
        pathProgress.value = pathProgress.minValue;
    }

    void Update()
    {
        //Fire a cannonball if button clicked
        if (controllable.fireClicked() && Time.time > nextFire)
        {
            //Fire a cannonball with sound
            MyAudioManager.Instance.PlayEffect(shootSound);
            GameObject g = Instantiate(cannonball, spawnPosition.position, transform.rotation);
            g.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, cannonballSpeed));
            nextFire = Time.time + fireRate;
        }

    }

    void FixedUpdate()
    {
        //Get rotation from controllable
        controllable.Update();
        transform.rotation = controllable.Rotation;

        //Move cannon along path
        if (transform.position != path[current])
        {
            Vector3 next = Vector3.MoveTowards(transform.position, path[current], pathSpeed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(next);
        }
        else if (current < path.Length - 1)
        {
            current++;
            pathProgress.value = current;
        }
    }

    public bool isFinished()
    {
        //Finished when cannon has reached end of path
        return current >= path.Length - 1;
    }
}
