using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig9 : MonoBehaviour
{
    public Vector3 positionMin = Vector3.zero;
    public Vector3 positionMax = Vector3.one;

    public GameObject moveTowards;

    public float randomStrength = 1f;

    public float sphereSize = 0.05f;

    public Color color = Color.white;

    // Declare a mover object
    private Mover1_9 mover;

    // Start is called before the first frame update
    void Start()
    {
        // Create a Mover object
        mover = new Mover1_9(sphereSize, randomStrength, positionMin, positionMax, moveTowards, color);
    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        mover.Update();
    }
}

public class Mover1_9
{
    // The window limits
    private Vector3 minPos, maxPos;
    private Rigidbody rb;
    private float randStrength;

    private GameObject bias;
    


    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover1_9(float size, float randomStrength, Vector3 min, Vector3 max, GameObject moveTowards, Color color)
    {
        bias = moveTowards;
        randStrength = randomStrength;
        mover.AddComponent<SphereCollider>();
        mover.AddComponent<Rigidbody>();
        rb = mover.GetComponent<Rigidbody>();
        rb.drag = 0.7f;
        rb.useGravity = false;
        rb.mass = size * 0.2f;
        mover.transform.localScale = Vector3.one * size;
        minPos = min;
        maxPos = max;
        mover.transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;

        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        r.material.SetColor("_Color", color);
    }

    public void Update()
    {
        float randomMag = rb.mass * randStrength;
        Vector3 vect = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        vect += (bias.transform.position - mover.transform.position).normalized;
        vect = vect.normalized * randomMag;
        rb.AddForce(vect, ForceMode.Impulse);

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

    private Vector2 checkBounds(float loc, float vel, float min, float max)
    {
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




