using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MovingObject継承
public class Enemy : MovingObject
{
    public int playerDamage;

    private Transform target;//プレイヤーの位置情報
    public int skipMove = 1;//敵が動くかどうかの判定
    private int movecount = 1;//敵が動くかどうかをカウントする

    // Start is called before the first frame update
    protected override void Start()
    {
        //GameManagerスクリプトのEnemyの配列に格納
        GameManager.instance.AddEnemyToList(this);
        //Playerの位置情報を取得
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //MovingObjectのStartメソッド呼び出し
        base.Start();
    }

    protected override void AttemptMove(int xDir, int yDir)
    {
        //比較して行動ターン数になっていれば動く
        if(movecount >= skipMove)
        {
            base.AttemptMove(xDir, yDir);
            movecount = 1;
            return;
        }
        
        //移動が終了したらtrueにする
        movecount += 1;
    }

    //敵キャラ移動用メソッド(GameManagerから呼ばれる)
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;
        //同じX軸にいる時
        //Math.Absで絶対値を取る
        if(Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            //プレイヤーが上にいれば+1、下にいれば-1する
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            //プレイヤーが右にいれば+1、左にいれば-1する
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        AttemptMove(xDir, yDir);
    }

    protected override void OnCantMove()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().LoseHp(playerDamage);
    }
}
