using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomMoving : MonoBehaviour
{
    // Start is called before the first frame update
    public bool open;
    public float periodicTime=3.0f;
    private Vector3 direction;
    private float timeCounter = 0;
    public float speed = 5.0f;
    private CharacterController controller;
    void Start()
    {
        timeCounter = 0;
        getNewDirection();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            if (timeCounter < periodicTime)
            {
                //transform.Translate(direction * Time.deltaTime, Space.World);
                controller.Move(direction * Time.deltaTime);
                timeCounter+=Time.deltaTime;
            }
            else
            {
                getNewDirection();
                timeCounter = 0;
            }
        }
    }


    void getNewDirection()
    {
        float x = Random.Range(-10, 10);
        float z = Random.Range(-10, 10);
        direction = new Vector3(x, 0, z);
        direction = direction * speed / direction.magnitude;
    }
}
