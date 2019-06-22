using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Enemyの動作時間
    public float turnDelay = 0.1f;
    //static変数：シーン間で変数を共有
    //ゲーム内でユニークな関数
    //オブジェクトに属さずクラスに属す
    public static GameManager instance = null;

    public BoardManager boardScript;

    //シングルトンであるGameManagerに体力を作成することで
    //体力情報を維持する
    public int playerHp = 100;//プレイヤーの体力
    //HideInInspector:public変数だがInspectorで編集できない
    //プレイヤーの順番か判定用
    [HideInInspector] public bool playersTurn = true;

    //テスト用でレベル3
    //この数に応じて出現する敵の数を調整する予定。(未実装)
    public int level = 3;

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
        //BoardManager取得
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        enemies.Clear();//EnemyListを初期化
        //BoardManagerのSetupSceneメソッドを実行
        boardScript.SetupScene(level);
    }
    
    public void GameOver()
    {
        //GameManagerを無効にする
        enabled = false;
    }

    void Update()
    {
        //PlayerのターンかEnemyが動いた後ならUpdateしない
        if(playersTurn || enemiesMoving)
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
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
