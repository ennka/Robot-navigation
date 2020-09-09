using UnityEngine;
using System.Collections;
//玩家控制器
public class PlayerCtrl : MonoBehaviour
{

    public GameObject wayLook;//寻路线的红点
    public float moveSpeed = 10f;//角色前进速度

    private CharacterController cc;//角色控制器
    private Transform waysParent;//寻路线的放置位置
    private Ray ray;//射线检测鼠标点击点
    private RaycastHit hit;
    private bool IsMove = false;//是否正在寻路过程

    void Start()
    {
        cc = GetComponent<CharacterController>();
        waysParent = GameObject.Find("Ways").transform;
        Debug.Log("Start!");

    }

    void Update()
    {
        //鼠标单击移动
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("你已单击鼠标。");
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);//获取主相机到鼠标点击点的射线

            //检测射线是否碰撞到地面：地面的层级是9
            if (Physics.Raycast(ray, out hit, 1 << 9))
            {
                Debug.Log("射线碰撞到地面。");
                //往目标点移动过去
                //return;
                Vector3 starPoint = new Vector3(transform.position.x, 0, transform.position.z);//寻路的起点
                Vector3 targetPoint = new Vector3(hit.point.x, 0, hit.point.z);//寻路的终点

                if (!IsMove)
                {
                    Debug.Log("调用自动寻路函数。");
                    StartCoroutine(AutoMove(starPoint, targetPoint));//开启自动寻路                    
                }

            }
        }

    }
    /// <summary>
    /// 自动寻路协程
    /// </summary>
    /// <returns>The move.</returns>
    /// <param name = "starPoint" > 起点.</ param >
    /// < param name= "targetPoint" > 目标点.</ param >
    IEnumerator AutoMove(Vector3 starPoint, Vector3 targetPoint)
    {
        IsMove = true;

        yield return new WaitForFixedUpdate();
        //运用A星算法计算出到起点到目标点的最佳路径
        Debug.Log("调用AStarRun.cs中的AStarFindWay函数。");
        Vector3[] ways = GetComponent<AStarRun>().AStarFindWay(starPoint, targetPoint);


        if (ways.Length == 0)
        {
            IsMove = false;
            yield break;
        }

        //打印显示出寻路线
        foreach (var v in ways)
        {
            GameObject way = Instantiate<GameObject>(wayLook);

            Debug.Log("wayLook无异常。");

            way.transform.parent = waysParent;
            way.transform.localPosition = v;
            way.transform.rotation = Quaternion.identity;
            way.transform.localScale = Vector3.one;
        }
        //让玩家开始沿着寻路线移动
        int i = 0;
        Vector3 target = new Vector3(ways[i].x, transform.position.y, ways[i].z);
        transform.LookAt(target);
        while (true)
        {
            yield return new WaitForFixedUpdate();
            Debug.Log("run run run !!!");

            cc.SimpleMove(transform.forward * moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 1f)
            {
                Debug.Log("run is ok !!!");
                ++i;
                if (i >= ways.Length)
                    break;
                target = new Vector3(ways[i].x, transform.position.y, ways[i].z);
                transform.LookAt(target);
            }
        }

        //移动完毕，删除移动路径
        for (int child = waysParent.childCount - 1; child >= 0; --child)
            Destroy(waysParent.GetChild(child).gameObject);

        //等待执行下一次自动寻路
        IsMove = false;
    }
}