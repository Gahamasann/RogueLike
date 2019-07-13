using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject wall;
    GameObject player;
    Vector3 playerPos;
    float cnt = 0;//関数実行回数指定用の変数
    void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update()
    {
        playerPos = player.transform.position;
        float dis = Vector3.Distance(playerPos, transform.position);//Playerととの距離判定
        //距離によってミニマップに描画(disの値によって距離変更可能)
        if(dis<10&&cnt <=0)
        {
            Instantiate(wall, transform.position, Quaternion.identity);
            cnt += 1;
        }
    }
}
