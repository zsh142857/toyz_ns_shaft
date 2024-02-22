using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{   
    [SerializeField] float movespeed =5f;
    GameObject currentfloor;
    [SerializeField] int hp;
    [SerializeField] GameObject hpbar;
    [SerializeField] Text scoretext;
    int score;
    float scoreTime;
    AudioSource deathSound;
    [SerializeField] GameObject replaybutton;
    // Start is called before the first frame update
    void Start()//初始預設
    {
        hp=10;
        score=0;
        scoreTime=0f;
        deathSound =GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()//左右鍵移動
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(movespeed*Time.deltaTime,0,0);
            GetComponent<SpriteRenderer>().flipX=true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-movespeed*Time.deltaTime,0,0);
            GetComponent<SpriteRenderer>().flipX=false;
        }
        UpdateScore();
    }
    void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag =="normal")
        {
            Debug.Log(other.contacts[0].normal);
            Debug.Log(other.contacts[1].normal);
            
            if (other.contacts[0].normal == new Vector2(0f,1f)) //用法線判斷當前站的階梯是哪個
            {
                currentfloor=other.gameObject;
                Modifyhp(1); 
               other.gameObject.GetComponent<AudioSource>().Play();
            }
           
        }
        else if(other.gameObject.tag =="nails")
        {
            if (other.contacts[0].normal == new Vector2(0f,1f))//用法線判斷當前站的階梯是哪個
            {
                currentfloor=other.gameObject; 
                Modifyhp(-2);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if(other.gameObject.tag =="ceiling"){
            currentfloor.GetComponent<BoxCollider2D>().enabled=false;
            Modifyhp(-2);
            other.gameObject.GetComponent<AudioSource>().Play();
        }
        
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag=="deathline") //死亡線偵測，遊戲結束
        {
            Die();
        }
        else if(other.gameObject.tag=="Background")//角色在背景內被偵測，持續撥放bgm
        {
            if(hp>0)
            {
               other.gameObject.GetComponent<AudioSource>().Play(); 
            }else
            {
                other.gameObject.GetComponent<AudioSource>().Stop(); 
            }
        }
    }
    
    public void Modifyhp(int num)
    {
        hp+=num;
        if(hp>10) //血條小於10，進入死亡狀態
        {
            hp =10;
        }
        else if(hp<=0)
        {
            hp=0;
            Die();
        }
        Updatehpbar();
    }

    void Updatehpbar()
    {
        for(int i=0;i<hpbar.transform.childCount;i++) //血條，比i大隱藏，剩餘血量顯示
        {
            if (hp>i)
            {
                hpbar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                hpbar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void UpdateScore()//紀錄遊戲時間
    {
        scoreTime +=Time.deltaTime;
        if (scoreTime>1f)
        {
            score++;
            scoreTime=0f;
            scoretext.text="你掙扎了"+score.ToString()+"秒";
        }
    }

    void Die()//死亡狀態變0倍速，放死亡音效，跑出重新按鈕
    {
        
        Time.timeScale=0f;
        deathSound.Play();
        replaybutton.SetActive(true);
    }

    public void replay()//按下重新按鈕，恢復1倍速
    {
        Time.timeScale=1f;
        SceneManager.LoadScene("SampleScene"); //一倍速從頭開始
    }
}
