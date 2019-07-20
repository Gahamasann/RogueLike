using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//MovingObject継承
public class Enemy : MovingObject
{
    private Transform target;//プレイヤーの位置情報
    public int skipMove = 1;//敵が動くかどうかの判定
    private int movecount = 1;//敵が動くかどうかをカウントする
    public int enemyHp = 30;
    private float dame;
    Random rnd;
    private float rndnum;

    private GameObject rogWindow;
    private GameObject rog;
    private Text rogText;//ダメージ表記用テキスト

    // Start is called before the first frame update
    protected override void Start()
    {
        //GameManagerスクリプトのEnemyの配列に格納
        GameManager.instance.AddEnemyToList(this);
        //Playerの位置情報を取得
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //MovingObjectのStartメソッド呼び出し
        base.Start();
        rogWindow = GameObject.Find("RogWindow");
        rog = GameObject.Find("RogText");
        rogText = rog.GetComponent<Text>();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //比較して行動ターン数になっていれば動く
        if(movecount >= skipMove)
        {
            base.AttemptMove<T>(xDir, yDir);
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
        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        if(gameObject.activeSelf)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>().LoseHp(attack);
            rogWindow.SetActive(true);
            rog.SetActive(true);
            rogText.text += this.name + "が" + player.name + "に" + (int)dame + "ダメージ\n";
        }
    }

    //エネミーがダメージを受けたときに呼び出されるメソッド
    public void DamageEnemy(int loss)
    {
        rndnum = Random.Range(100, 110);

        dame = (loss - (float)defence) * (rndnum / 100);
        if (dame < 0)
        {
            dame = 1;
        }
        enemyHp -= (int)dame;


        if (enemyHp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
