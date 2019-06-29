using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject Line1,Line2,Line3,Line4;
    GameObject player;
    Vector3 playerPos;
    bool rayFlag1,rayFlag2,rayFlag3,rayFlag4;
    float cnt = 0;//関数実行回数指定用の変数
    void Start()
    {
        rayFlag1 = false;
        rayFlag2 = false;
        rayFlag3 = false;
        rayFlag4 = false;
        player = GameObject.Find("Player");
    }
    void Update()
    {
        //オブジェクト位置からRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(gameObject.transform.position);

        //Rayの長さ
        float maxDistance = 0.1f;

        //Ratの生成・描画
        //RaycastHit2D hit1 = Physics2DExtentsion.RaycastAndDraw(transform.position + new Vector3(0, 0.51f, 0), new Vector2(0, 1), maxDistance);
        //RaycastHit2D hit2 = Physics2DExtentsion.RaycastAndDraw(transform.position + new Vector3(0, -0.51f, 0), new Vector2(0, -1), maxDistance);
        //RaycastHit2D hit3 = Physics2DExtentsion.RaycastAndDraw(transform.position + new Vector3(0.51f, 0, 0), new Vector2(1, 0), maxDistance);
        //RaycastHit2D hit4 = Physics2DExtentsion.RaycastAndDraw(transform.position + new Vector3(-0.51f, 0, 0), new Vector2(-1, 0), maxDistance);
        
        //Rayの生成
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(0, 0.51f, 0), new Vector2(0, 1), maxDistance);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(0, -0.51f, 0), new Vector2(0, -1), maxDistance);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position + new Vector3(0.51f, 0, 0), new Vector2(1, 0), maxDistance);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position + new Vector3(-0.51f, 0, 0), new Vector2(-1, 0), maxDistance);
        

        //Playerタグ以外のオブジェクトと衝突時判定
        if (hit1.collider)
        {
            if(hit1.collider.gameObject.tag !="Player")
            {
                rayFlag1 = true;
            }
        }
        if (hit2.collider)
        {
            if(hit2.collider.gameObject.tag != "Player")
            {
                rayFlag2 = true;
            }
        }
        if (hit3.collider)
        {
            if(hit3.collider.gameObject.tag != "Player")
            {
                rayFlag3 = true;
            }
            
        }
        if (hit4.collider)
        {
            if(hit4.collider.gameObject.tag !="Player")
            {
                rayFlag4 = true;
            }
        }

        playerPos = player.transform.position;
        float dis = Vector3.Distance(playerPos, transform.position);//Playerととの距離判定
        //距離によってミニマップに描画(disの値によって距離変更可能)
        if(dis<10&&cnt <=0)
        {
            if(rayFlag1)
            {
                Instantiate(Line1, transform.position, Quaternion.identity);
            }
            if(rayFlag2)
            {
                Instantiate(Line2, transform.position, Quaternion.identity);
            }
            if(rayFlag3)
            {
                Instantiate(Line3, transform.position, Quaternion.identity);
            }
            if(rayFlag4)
            {
                Instantiate(Line4, transform.position, Quaternion.identity);
            }
            cnt += 1;
        }
    }
}
