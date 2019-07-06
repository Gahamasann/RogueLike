using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI用

public class GameManager : MonoBehaviour
{
    //階層表示画面で2秒待つ
    public float floorStartDelay = 2f;
    //Enemyの動作時間
    public float turnDelay = 0.01f;
    //static変数：シーン間で変数を共有
    //ゲーム内でユニークな関数
    //オブジェクトに属さずクラスに属す
    public static GameManager instance = null;

    private DgGenerator dungionScript;

    //シングルトンであるGameManagerに体力を作成することで
    //体力情報を維持する
    public int playerHp = 100;//プレイヤーの体力
    public int moneydebt = -2000000;//借金
    //HideInInspector:public変数だがInspectorで編集できない
    //プレイヤーの順番か判定用
    [HideInInspector] public bool playersTurn = true;

    //フロアテキスト
    private Text floorText;
    //フロアイメージ
    private GameObject floorImage;
    //この数に応じて出現する敵の数を調整する予定。(階層)
    private int floor = 1;
    //セットアップ中かどうか
    private bool doingSetup;

    //Enemyクラスの配列
    private List<Enemy> enemies;
    //Enemyのターン中はtrueになる
    private bool enemiesMoving;

    void Awake()
    {
        //ゲーム開始時にGameManagerをinstanceに指定
        if(instance == null)
        {
            instance = this;
        }
        //このオブジェクト以外にGameManagerが存在するとき
        else if(instance != this)
        {
            //このオブジェクトを破壊する
            Destroy(gameObject);
        }
        //シーン遷移時にこのオブジェクトを引き継ぐ
        DontDestroyOnLoad(gameObject);
        //Enemyを格納する配列生成
        enemies = new List<Enemy>();
        //DgGenerator取得
        dungionScript = GetComponent<DgGenerator>();
        InitGame();
    }

    private void OnLevelWasLoaded(int level)
    {
        floor++;
        InitGame();
    }

    void InitGame()
    {
        //trueの間、プレイヤーは身動きを取れない
        doingSetup = true;
        //それぞれのオブジェクト取得
        floorImage = GameObject.Find("FloorImage");
        floorText = GameObject.Find("FloorText").GetComponent<Text>();
        floorText.text = floor + "F";
        floorImage.SetActive(true);
        Invoke("HideFloorImage", floorStartDelay);
        //EnemyListを初期化
        enemies.Clear();
        //DgManegerのSetupSceneメソッドを実行
        dungionScript.SetupScene(floor);
    }

    private void HideFloorImage()
    {
        //非アクティブ化及びプレイヤーを動けるように
        floorImage.SetActive(false);
        doingSetup = false;
    }
    
    public void GameOver()
    {
        //ゲームオーバーメッセージを表示
        floorText.text = "GameOver";
        floorImage.SetActive(true);
        //GameManagerを無効にする
        enabled = false;
    }

    void Update()
    {
        //PlayerのターンかEnemyが動いた後かdoingSetup = trueならUpdateしない
        if(playersTurn || enemiesMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        //Enemyの数だけEnemyスクリプトのMoveEnemyを実行
        for(int i = 0;i < enemies.Count; i++)
        {
            if(enemies[i].isActiveAndEnabled)
            {
                enemies[i].MoveEnemy();
                yield return new WaitForSeconds(enemies[i].moveTime);
            }
            else
            {
                enemies.Remove(enemies[i]);
            }
        }

        yield return new WaitForSeconds(0.1f);

        playersTurn = true;
        enemiesMoving = false;
    }
}
