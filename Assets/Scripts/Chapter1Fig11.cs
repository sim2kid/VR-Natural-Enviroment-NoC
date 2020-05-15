using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig11 : MonoBehaviour
{

    public List<Mover1_11> Movers = new List<Mover1_11>();
    public GameObject objectToFollow;

    public Vector3 positionMin = Vector3.zero;
    public Vector3 positionMax = Vector3.one;

    public float sphereSize = 0.05f;
    public float maxSpeed = 5f;

    public float attractionStrength = 1f;
    public int amountMovers = 30;


    // Start is called before the first frame update
    void Start()
    {
        // We need to instantiate our Movers and add them to a list
        while (Movers.Count < amountMovers)
        {

            Movers.Add(new Mover1_11(sphereSize, maxSpeed, positionMin, positionMax));
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 followPosition;
        if (objectToFollow == null)
        {
            followPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else 
        {
            followPosition = objectToFollow.transform.position;
        }

        for (int i = 0; i < Movers.Count; i++)
        {
            Movers[i].AccelerateTowards(followPosition, attractionStrength);
            Movers[i].Update();
        }
    }

}

public class Mover1_11
{
    // The basic properties of a mover class
    private float topSpeed;

    public Rigidbody rb;

    Vector3 minPos, maxPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover1_11(float size, float maxSpeed, Vector3 min, Vector3 max)
    {
        mover.AddComponent<SphereCollider>();
        mover.AddComponent<Rigidbody>();
        rb = mover.GetComponent<Rigidbody>();
        rb.useGravity = false;
        mover.transform.localScale = Vector3.one * size;
        minPos = min;
        maxPos = max;
        mover.transform.position = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z)); 
        rb.velocity = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        topSpeed = maxSpeed;

        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        r.material.SetColor("_Color", Color.black);
    }

    public void AccelerateTowards(Vector3 target, float strength) 
    {
        Vector3 directionOfTravel = target - mover.transform.position;
        rb.AddForce(directionOfTravel.normalized * strength);
    }

    public void Update()
    {
        // Limit Velocity to the top speed
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, topSpeed);

        Vector3 location = mover.transform.position;
        Vector3 velocity = rb.velocity;

        Vector2 temp = checkBounds(location.x, velocity.x, minPos.x, maxPos.x);
        location.x = temp.x;
        velocity.x = temp.y;

        temp = checkBounds(location.y, velocity.y, minPos.y, maxPos.y);
        location.y = temp.x;
        velocity.y = temp.y;

        temp = checkBounds(location.z, velocity.z, minPos.z, maxPos.z);
        location.z = temp.x;
        velocity.z = temp.y;

        // Updates the GameObject of this movement
        mover.transform.position = location;
        rb.velocity = velocity;
    }

    private Vector2 checkBounds(float loc, float vel, float min, float max) {
        if (loc > max) 
        {
            loc = max;
            if (vel > 0) 
            {
                vel *= -1;
            }
        } 
        else if (loc < min)
        {
            loc = min;
            if (vel < 0)
            {
                vel *= -1;
            }
        }

        return new Vector2(loc, vel);
    }
    
}
