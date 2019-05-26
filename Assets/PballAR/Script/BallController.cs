using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 velocity;
    [Range(0, 1)]
    public float speed = 0.1f;
  
   // private Rigidbody rb;
    void Start()
    {
        resetBall();
        //rb = GetComponent<Rigidbody>();
    }
    void resetBall() {
        transform.position = Vector3.zero;
                   float z = Random.Range(0, 2) * 2f - 1f;
        float x = Random.Range(0, 2) * 2f - 1f * Random.Range(0.0f, 1f);
        velocity = new Vector3(x, 0, z);
    }


    // Update is called once per frame
    void fixedUpdate()
    {
        velocity = velocity.normalized * speed;
        transform.position += velocity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.name)
        {
            case "westBorder":
            case "eastBorder":
            case "southBorder":
            case "northBorder":
                velocity.x = -1f;
                return;
               
        }
    }
}
