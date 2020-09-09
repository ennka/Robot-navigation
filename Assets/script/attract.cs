using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attract : MonoBehaviour
{
    public GameObject goal;
    public bool APF = true;
    public int mode;
    public int forceRate1 = 30;
    public int forceRate2 = 5;
    public int test = 20;
    public int slowRate = 10;
    public float maxSpeed = 8.0f;
    public float attractSpeed = 8.0f;

    public float stableTime = 2.5f;//静止如此时间后产生随机扰动
    private float timeCounter = 0f;//静止时间的计数器

    public bool enableRandomPush=false;
    private bool isPushing = false;
    private Vector3 pushDirection;//随机运动的受力方向
    public float pushTime = 1;//随机运动时的受力时间
    public float pushRate = 10.0f;
    private float pushTimeCounter = 0;//随机运动时间的计数器

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goal != null &APF)//如果目标消失，则不产生吸引力。
        {
            double speed = rb.velocity.magnitude;
            Vector3 temp = goal.transform.position - this.transform.position;
            float length = temp.magnitude;
            if (speed > maxSpeed)
            {
                //Debug.Log("开始减速1:"+Time.time);
                GetComponent<Rigidbody>().AddForce(-slowRate * temp / length, ForceMode.Force);
            }

            Vector3 direction = temp / length;//行进的方向
            Vector3 velocity = rb.velocity;//获取物体的速度
            speed = direction[0] * velocity[0] + direction[1] * velocity[1] + direction[2] * velocity[2];
            if (speed < attractSpeed)//如果在向目标物方向运动的速度过快，则目标物不产生吸引力。
            {
                if (mode == 1)
                {
                    updateSpeed1();
                }
                else if (mode==2 | mode==3 | mode == 4)
                {
                    updateSpeed2();
                }
            }
            else
            {
                //Debug.Log("开始减速2:" + Time.time);
            }

            if (enableRandomPush)//如果运行随机扰动
            {
                //静止状态计时
                if (isStable())
                {
                    Debug.Log("静止中:" + timeCounter);
                    timeCounter += Time.deltaTime;
                    if (timeCounter > stableTime)
                    {
                        calculatePush();
                        isPushing = true;
                        pushTimeCounter = pushTime;
                        timeCounter = 0;
                    }
                }
                else
                    timeCounter = 0;

                //随机运动状态计时
                if (isPushing)
                {
                    pushTimeCounter -= Time.deltaTime;
                    GetComponent<Rigidbody>().AddForce(pushRate * pushDirection, ForceMode.Force);
                    Debug.Log("Pushing" + pushDirection);
                    if (pushTimeCounter < 0)
                    {
                        pushTimeCounter = 0;
                        isPushing = false;
                    }
                }
            }
        }
    }

    void updateSpeed1()
    {
        Vector3 temp = goal.transform.position - this.transform.position;
        float length = temp.magnitude;
        GetComponent<Rigidbody>().AddForce(forceRate1 * temp / length, ForceMode.Force);
        //Debug.Log(" force: " + forceRate * temp / length);
    }
    void updateSpeed2()
    {
        Vector3 temp = goal.transform.position - this.transform.position;
        float length = temp.magnitude;
        Vector3 direction = temp / length;//行进的方向
        GetComponent<Rigidbody>().AddForce(forceRate2 * direction*(length*length+test/forceRate2)/10000, ForceMode.Force);
        //Debug.Log(" force: " + forceRate * temp / length);
    }

    void ray()
    {
        //定义一条射线，起点为GO1的物体坐标,终点为GO2的物体坐标
        Ray ray = new Ray(this.transform.position, goal.transform.position - this.transform.position);
        //定义一个光线投射碰撞 
        RaycastHit hit;
        //发射射线长度为100 
        Physics.Raycast(ray, out hit, 1000);
        if (hit.transform != null)
        {
            Debug.Log("blocked:" + hit.transform.name);
        }
        //在Scene中生成这条射线，起点为射线的起点，终点为射线与物体的碰撞点
        Debug.DrawLine(this.transform.position, goal.transform.position);

    }
    bool isStable()
    {
        if (rb.velocity.magnitude < 0.1)
            return true;
        return false;
    }
    void calculatePush()
    {
        Vector3 temp = goal.transform.position - this.transform.position;
        float length = temp.magnitude;
        Vector3 direction = temp / length;//行进的方向
        float result;
        do
        {
            float x = Random.Range(-10, 10);
            float z = Random.Range(-10, 10);
            temp = new Vector3(x, 0, z);
            temp = temp / temp.magnitude;
            result = temp[0] * direction[0] + temp[1] * direction[1];
        } while (result > 0 | result<-0.7);
        pushDirection = temp;

    }
}
