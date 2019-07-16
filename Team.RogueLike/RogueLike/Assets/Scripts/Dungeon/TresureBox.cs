using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;


public class TresureBox : MonoBehaviour
{
    //カウント用のクラス設定
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public Count itemCount = new Count(1, 5);
    public GameObject[] itemTiles;
    GameObject tresure;

    // Start is called before the first frame update
    void Start()
    {
        PutInItem(itemTiles,itemCount.minimum,itemCount.maximum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PutInItem(GameObject[] tileArray, int minimum, int maximum)
    {
        //最低値～最大値+1のランダム回数分だけループ
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++)
        {
            //引数tileArrayからランダムで1つ選択
            GameObject tileChoise = tileArray[Random.Range(0, tileArray.Length)];
            //ランダムで決定した種類・位置でオブジェクトを生成
            tresure = Instantiate(tileChoise, transform.position+new Vector3(0,0,0.1f), Quaternion.identity);
            tresure.transform.parent = this.transform;
        }
    }
}
