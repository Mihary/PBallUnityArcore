using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public Transform ball;

    [Range(0, 1)]
    public float skill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Lerp(transform.position.x, ball.position.x, skill);
        transform.position = newPos;
    }
}
