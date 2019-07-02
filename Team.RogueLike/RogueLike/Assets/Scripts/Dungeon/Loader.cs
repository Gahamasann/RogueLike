using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    //GameObjectのプレハブを指定
    public GameObject gameManager;

    void Awake()
    {
        //GameManagerが存在しないとき、GameManagerを生成する
        if(GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
