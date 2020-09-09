using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="robot")
        {
            Destroy(this.gameObject);
            UnityEditor.EditorUtility.DisplayDialog("成功", "你已经成功抵达地点，花费时间:"+Time.time+"秒。", "感觉不错", "感觉很好");
        }
    }
}
