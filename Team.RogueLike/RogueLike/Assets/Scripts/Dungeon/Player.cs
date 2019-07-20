using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//MovingObject継承
public class Player : MovingObject
{ 

    //プレイヤー内で統一の貨幣単位を決めてしまっているので後で修正
    public int coin = 20; //お金
    public float restartlevelDelay = 1f; //次レベルへ行く時の時間差
    public Text moneyText;
    public Text hpText;

    public GameObject cursor;

    private int money;
    private int hp; //プレイヤーの体力
    private int hpMax;//プレイヤーの体力上限
    private int steps;//歩行カウント用変数
    private int age;//年齢
    private GameObject rogWindow;
    private GameObject rog;
    private Text rogText;//ダメージ・アイテム拾得表記用テキスト
    private Vector3 latestPos;
    private GameObject damage;
    Random rnd;
    private float dame;
    private float rndnum;
    private Text damagetext;


    private BoxCollider2D boxCollider;
    int horizontal = 0;
    int vertical = 0;
    int dirX, dirY;

    //MovingObjectのStartメソッドを継承　baseで呼び出し
    protected override void Start()
    {
        //シングルトンであるGameManagerのplayerHpを使うことに
        //よって、レベルを跨いでも値を保持しておける
        hp = GameManager.instance.playerHp;
        hpMax = hp;
        steps = 0;
        age = 20;
        money = GameManager.instance.moneydebt;
        moneyText.text = "Money:" + money;

        //MovingObjectのStartメソッド呼び出し
        base.Start();
        rogWindow = GameObject.Find("RogWindow");
        rog = GameObject.Find("RogText");
        rogText = rog.GetComponent<Text>();

        boxCollider = GetComponent<BoxCollider2D>();
    }

    //Playerスクリプトが無効になる前に、体力をGameManagerへ保存
    //UnityのAPIメソッド(Unityに標準で用意された機能)
    private void OnDisable()
    {
        GameManager.instance.playerHp = hp;
        GameManager.instance.moneydebt = money;
    }

    void Update()
    {
        //プレイヤーの順番じゃない時Updateは実行しない
        if (!GameManager.instance.playersTurn)
            return;

        //ウィンドウズ表示時はUpdateを実行しない
        if (cursor.activeSelf)
            return;

        //以前の入力された方向を取得、移動キー入力がないなら方向は変わらない
        if(horizontal!=0||vertical!=0)
        {
            dirY = vertical;
            dirX = horizontal;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            RaycastHit2D hit;
            Attack(dirX, dirY, out hit);
        }

        horizontal = 0; //-1: 左移動, 1: 右移動
        vertical = 0; //-1: 下移動, 1: 上移動

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        //歩行数5以上で体力上限減少
        if(steps > 4)
        {
            //体力上限減少
            hpMax -= 1;
            age += 1;
            //体力が上限より大きいなら体力を上限までさげる
            if(hp > hpMax)
            {
                hp = hpMax;
            }
            steps = 0;
        }
        
        if(!Input.GetKey(KeyCode.Space))
        {
            //上下もしくは左右に移動を制限
            if (horizontal != 0)
            {
                vertical = 0;
            }
        }
        else
        {
            //スペースキーを押している間斜め移動可能(縦横移動不可)
            if(horizontal== 0||vertical == 0)
            {
                horizontal = 0;
                vertical = 0;
            }
        }

        //上下左右どれかに移動する時
        if (horizontal != 0 || vertical != 0)
        {
            //歩行数+1
            steps += 1;
            //Playerの場合はWall以外判定する必要はない
            AttemptMove<Enemy>(horizontal, vertical);
        }
    }

    //移動時の処理
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //MovingObjectのAttemptMove呼び出し
        base.AttemptMove<T>(xDir, yDir);

        CheckIfGameOver();

        //プレイヤーの順番終了
        GameManager.instance.playersTurn = false;
    }


    //MovingObjectの抽象メソッドのため必ず必要
    protected override void OnCantMove<T>(T component)
    {
        Enemy hitenemy = component as Enemy;
        hitenemy.DamageEnemy(attack);
        rogWindow.SetActive(true);
        rog.SetActive(true);
        rogText.text += hitenemy.name + "に" + (int)dame + "のダメージを与えた\n";
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
            //借金を減らしotherオブジェクトを削除
            money += coin;
            moneyText.text = "Money:" + money;
            rogWindow.SetActive(true);
            rog.SetActive(true);
            rogText.text += other.gameObject.name + "を拾った\n";
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

        rndnum = Random.Range(100, 110);

        dame = (loss - (float)defence) * (rndnum / 100);
        if (dame < 0)
        {
            dame = 1;
        }
        hp -= (int)dame;
        hpText.text = "HP:" + hp;
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

    public int GetHp()
    {
        return hp;
    }

    public int GetHpMax()
    {
        return hpMax;
    }

    public int GetAge()
    {
        return age;
    }

    private void Attack(int xDir, int yDir, out RaycastHit2D hit)
    {
        //現在地を取得
        Vector2 start = transform.position;
        //目的地を取得
        Vector2 end = start + new Vector2(xDir, yDir);
        //自身のColliderを無効にし、Linecastｄ自分自身を判定しないようにする
        boxCollider.enabled = false;
        //現在地と目的地との間にblockingLayerのついたオブジェクトが無いか判定
        hit = Physics2D.Linecast(start, end);
        //Colliderを有効に戻す
        boxCollider.enabled = true;
        if(hit.transform == null)
        {
            GameManager.instance.playersTurn = false;
        }
        else
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<Enemy>().DamageEnemy(10);
                rogWindow.SetActive(true);
                rog.SetActive(true);
                rogText.text += hit.transform.name + "に" + "10" + "のダメージを与えた\n";
                GameManager.instance.playersTurn = false;
            }
            else
            {
                GameManager.instance.playersTurn = false;
            }
        }
        
        
    }
}
