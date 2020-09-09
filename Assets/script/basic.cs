using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic : MonoBehaviour
{
    public float forceRate1 = 1.0f;
    public float forceRate2 = 1.0f;
    public float distanceParameter = 10000f;
    public float centerDistance = 1.0f;
    private bool open ;
    private Rigidbody rb;
    private GameObject goal;
    private int mode;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        open = GetComponent<attract>().APF;
        goal = GetComponent<attract>().goal;
        mode= GetComponent<attract>().mode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void avoidBarrier(GameObject other)
    {
        if (open)
        {
            Vector3 pr = this.transform.position;
            Vector3 br = other.transform.position;
            float length = (pr - br).magnitude;
            //Debug.Log(pr/length);
            if (mode == 1 | mode == 2)
            {
                rb.AddForce((pr - br) * forceRate1 * ((1 / length - 1 / 15) * (1 / length - 1 / 15)));
            }
            else if (mode == 3)
            {
                if (goal != null)
                {
                    Vector3 temp = goal.transform.position - this.transform.position;
                    //Debug.Log(1 - Mathf.Exp(-temp.magnitude / distanceParameter) +"???");
                    rb.AddForce((pr - br) * forceRate1 *(1 - Mathf.Exp(-temp.magnitude / distanceParameter))* ((1 / length - 1 / 15) * (1 / length - 1 / 15)));
                }
            }
            else if (mode == 4)
            {
                if (goal != null)
                {
                    Vector3 temp = goal.transform.position - this.transform.position;
                    rb.AddForce((pr - br) * forceRate2 * (1 - Mathf.Exp(-temp.magnitude / distanceParameter)) / ((length - centerDistance) * (length - centerDistance)));
                }
            }

        }
    }
}
