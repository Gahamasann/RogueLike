using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//MovingObject継承
public class Player : MovingObject
{
    public int pointsPerHeal = 20; //HPの回復量
    public float restartlevelDelay = 1f; //次レベルへ行く時の時間差

    private int hp; //プレイヤーの体力

    //MovingObjectのStartメソッドを継承　baseで呼び出し
    protected override void Start()
    {
        //シングルトンであるGameManagerのplayerHpを使うことに
        //よって、レベルを跨いでも値を保持しておける
        hp = GameManager.instance.playerHp;
        //MovingObjectのStartメソッド呼び出し
        base.Start();
    }
    //Playerスクリプトが無効になる前に、体力をGameManagerへ保存
    //UnityのAPIメソッド(Unityに標準で用意された機能)
    private void OnDisable()
    {
        GameManager.instance.playerHp = hp;
    }

    void Update()
    {
        //プレイヤーの順番じゃない時Updateは実行しない
        if (!GameManager.instance.playersTurn)
            return;

        int horizontal = 0; //-1: 左移動, 1: 右移動
        int vertical = 0; //-1: 下移動, 1: 上移動

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        //上下もしくは左右に移動を制限
        if (horizontal != 0)
        {
            vertical = 0;
        }
        //上下左右どれかに移動する時
        if (horizontal != 0 || vertical != 0)
        {
            //Playerの場合はWall以外判定する必要はない
            AttemptMove(horizontal, vertical);
        }
    }

    //移動時の処理
    protected override void AttemptMove(int xDir, int yDir)
    {
        //MovingObjectのAttemptMove呼び出し
        base.AttemptMove(xDir, yDir);

        CheckIfGameOver();
        //プレイヤーの順番終了
        GameManager.instance.playersTurn = false;
    }

    //MovingObjectの抽象メソッドのため必ず必要
    protected override void OnCantMove()
    {
        //移動先にcomponentがあった場合の処理（例文）

        ////Wall型を定義 Wallスクリプトを表す
        //Wall hitWall = component as Wall;
        ////WallスクリプトのDamageWallメソッド呼び出し
        //hitWall.DamageWall(wallDamage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            //Invoke: 引数分遅れてメソッドを実行する
            Invoke("Restart", restartlevelDelay);
            enabled = false; //Playerを無効にする
        }
        else if (other.tag == "Item")
        {
            //体力を回復しotherオブジェクトを削除
            hp += pointsPerHeal;
            other.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        //同じシーンを読み込む
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //敵キャラがプレイヤーを攻撃した時のメソッド
    public void LoseHp(int loss)
    {
        hp -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (hp <= 0)
        {
            //GameManagerのGameOverメソッド実行
            //public staticな変数なのでこのような簡単な形でメソッドを呼び出せる
            GameManager.instance.GameOver();
        }
    }
}
