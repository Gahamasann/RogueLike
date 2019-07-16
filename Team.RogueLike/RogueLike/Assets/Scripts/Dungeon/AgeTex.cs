using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgeTex : MonoBehaviour
{
    GameObject player;
    Player pl;//スクリプト用
    Text age;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pl = player.GetComponent<Player>();
        age = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        age.text = "年齢:" + pl.GetAge().ToString();
    }
}
