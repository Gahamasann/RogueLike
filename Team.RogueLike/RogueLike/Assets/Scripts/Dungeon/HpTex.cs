using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpTex : MonoBehaviour
{
    GameObject player;
    Player pl;//スクリプト用
    Text hp;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pl = player.GetComponent<Player>();
        hp = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        hp.text = "HP:" + pl.GetHp().ToString() + "/" + pl.GetHpMax().ToString();
    }
}
