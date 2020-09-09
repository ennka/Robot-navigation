using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrier : MonoBehaviour
{
    // Start is called before the first frame update

    private basic robot;
    void Start()
    {
        robot = (basic)GameObject.FindGameObjectWithTag("robot").GetComponent<basic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag=="robot")
        {
            robot.avoidBarrier(this.gameObject);
        }
    }
}
