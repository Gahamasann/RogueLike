using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//Random用
using Random = UnityEngine.Random;

//ステージ生成（ランダム生成から応用で作っているので改造の余地あり）
public class BoardManager : MonoBehaviour
{
    //カウント用のクラス設定
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min,int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    //10*10のゲームボードを生成
    public int width = 10;
    public int height = 10;
    //アイテムが出現する個数指定
    public Count itemCount = new Count(1, 5);
    //Exitオブジェクト
    public GameObject exit;
    //床、外壁、アイテムオブジェクト(Tileとして受け取る)
    public GameObject[] floorTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] itemTiles;
    public GameObject[] enemyTiles;
    //オブジェクトの位置情報を保存する変数
    private Transform boardHolder;
    //オブジェクトを配置できる範囲を表すリスト
    //Listは可変型の配列
    private List<Vector3> gridPositions = new List<Vector3>();

    //アイテム等を配置できる範囲を決定
    void InitialiseList()
    {
        //gridPositionを一旦クリア
        gridPositions.Clear();
        //gridPositionにオブジェクト配置可能範囲を指定
        //widthとheightで指定した分ループ
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //外壁、床を配置
    void BoardSetup()
    {
        //Boardオブジェクトを作成し、transform情報をboardHolderに保存
        boardHolder = new GameObject("Board").transform;
        //widthとheightで指定した分ループ
        for (int x = -1; x < width + 1; x++)
        {
            for (int y = -1; y < height + 1; y++)
            {
                //床をランダムで生成
                GameObject toInstantiate = floorTiles[Random.Range(0,floorTiles.Length)];
                //左端、右端、最低部、最上部の時 = 外壁を作るとき
                if(x == -1 || x == width || y == -1 || y == height)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                //床or外壁を生成し、instance変数に格納
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                //生成したinstanceをBoardオブジェクトの子オブジェクトとする
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        //0～36からランダムで1つ決定し、位置情報を確定
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        //ランダム決定した数値を削除
        gridPositions.RemoveAt(randomIndex);
        //確定した位置情報を返す
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray,int minimum,int maximum)
    {
        //最低値～最大値+1のランダム回数分だけループ
        int objectCount = Random.Range(minimum, maximum + 1);
        for(int i = 0;i < objectCount;i++)
        {
            //gridPositionから位置情報を1つ取得
            Vector3 randomPosition = RandomPosition();
            //引数tileArrayからランダムで1つ選択
            GameObject tileChoise = tileArray[Random.Range(0, tileArray.Length)];
            //ランダムで決定した種類・位置でオブジェクトを生成
            Instantiate(tileChoise, randomPosition, Quaternion.identity);
        }
    }

    //オブジェクトを配置していくメソッド
    //唯一のpublicメソッド　床生成時に、GameManagerから呼ばれる
    public void SetupScene(int level)
    {
        //床と外壁を配置し、
        BoardSetup();
        //アイテム等を配置できる位置を決定し、
        InitialiseList();
        //アイテムをランダムで配置し、
        LayoutObjectAtRandom(itemTiles, itemCount.minimum, itemCount.maximum);
        //レベル設定して階層ごとに設置数を変更する際に有用なのでコメントアウトしておきます
        ////Mathf.Log : 対数で計算。level=2なら4、level=3なら8
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        //Exitを右上の位置に配置する。
        Instantiate(exit, new Vector3(width - 1, height - 1, 0F), Quaternion.identity);
    }
}
