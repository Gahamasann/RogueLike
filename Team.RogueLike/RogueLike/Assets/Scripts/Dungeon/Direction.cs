using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Direction : MonoBehaviour
{
    Vector3 pos;
    bool hit;//エネミーとの接触フラグ
    GameObject hitEnemy;
    private GameObject rogWindow;
    private GameObject rog;
    private Text rogText;//ダメージ・アイテム拾得表記用テキスト

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.parent.position - new Vector3(0, 0.8f, 0);
        hit = false;
        rogWindow = GameObject.Find("RogWindow");
        rog = GameObject.Find("RogText");
        rogText = rog.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pos;
    }

    //方向処理
    public Vector3 Dir(float h, float v)
    {
        if (h > 0 && v == 0)
        {
            pos = transform.parent.position+new Vector3(0.8f,0,0);
        }
        if (h < 0 && v == 0)
        {
            pos = transform.parent.position - new Vector3(0.8f, 0, 0);
        }
        if (h == 0 && v > 0)
        {
            pos = transform.parent.position + new Vector3(0, 0.8f, 0);
        }
        if (h == 0 && v < 0)
        {
            pos = transform.parent.position - new Vector3(0, 0.8f, 0);
        }

        return pos;
    }

    public void AtE()
    {
        if (hitEnemy != null)
        {
            hitEnemy.GetComponent<Enemy>().DamageEnemy(10);
            rogWindow.SetActive(true);
            rog.SetActive(true);
            rogText.text += hitEnemy.name + "に" + "10" + "のダメージを与えた\n";
        }
        else
        {

        }
        //プレイヤーの順番終了
        GameManager.instance.playersTurn = false;
    }

    public bool GetHit()
    {
        return hit;
    }

    public GameObject GetHitEnemy()
    {
        return hitEnemy;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            hit = true;
            hitEnemy = other.transform.gameObject;
            Debug.Log("aaaaaaaa");
        }
    }
}
