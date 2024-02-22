using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class floor : MonoBehaviour
{   
    [SerializeField] float movespeed=2f; 
    public float timer;
    public float D=0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   timer+=Time.deltaTime;
        if(timer>4)
        {
            timer=0f;
            D++;
        }
        transform.Translate(0,movespeed*Time.deltaTime+0.003f*D,0);
        if (transform.position.y>6f)   //階梯到頂刪除，在下面生成階梯
        {
            Destroy(gameObject);
            transform.parent.GetComponent<floormanager>().SpawnFloor();
        }
    }
}
