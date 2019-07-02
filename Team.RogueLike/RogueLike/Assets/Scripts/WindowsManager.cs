using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowsManager : MonoBehaviour
{
    //ウィンドウズの大本のゲームオブジェクト取得
    private GameObject windows;
    private GameObject cursor;
    //パネルとテキストを格納するリスト
    private GameObject[] windowsList;
    private Text[] windowsText;

    //カーソル管理系(クソ汚いので後で治そう)
    private int cursornumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        windows = GameObject.Find("Windows");
        cursor = GameObject.Find("Cursor");
        windowsList = new GameObject[windows.transform.childCount];
        windowsText = new Text[windows.transform.childCount];

        for (int i = 0; i < windowsList.Length; i++)
        {
            windowsList[i] = windows.transform.GetChild(i).gameObject;
            windowsText[i] = windowsList[i].transform.GetComponentInChildren<Text>();
            windowsList[i].SetActive(false);
        }
        cursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        #region メニュー
        if(Input.GetKeyDown(KeyCode.X) && !windowsList[4].activeSelf)
        {
            cursornumber = 0;
            cursor.SetActive(true);
            windowsList[4].SetActive(true);
            windowsList[0].SetActive(true);
            windowsList[8].SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.X) && windowsList[4].activeSelf && !windowsList[5].activeSelf)
        {
            cursor.SetActive(false);
            windowsList[4].SetActive(false);
            windowsList[0].SetActive(false);
            windowsList[8].SetActive(false);
        }
        #endregion

        #region カーソル関連(汚すぎる)
        //カーソル位置を数字で管理
        if (cursornumber == 0)
        {
            cursor.transform.localPosition = new Vector2(-270, 85);
        }
        else if(cursornumber == 1)
        {
            cursor.transform.localPosition = new Vector2(-270, 55);
        }
        else if (cursornumber == 2)
        {
            cursor.transform.localPosition = new Vector2(-270, 25);
        }
        #endregion

        #region カーソル上下で移動
        if (Input.GetKeyDown(KeyCode.UpArrow) && windowsList[4].activeSelf && !windowsList[5].activeSelf)
        {
            if(cursornumber ==  0)
            {
                cursornumber = 2;
            }
            else
            {
                cursornumber -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && windowsList[4].activeSelf && !windowsList[5].activeSelf)
        {
            if (cursornumber == 2)
            {
                cursornumber = 0;
            }
            else
            {
                cursornumber += 1;
            }
        }
        #endregion

        #region メニュー二段階目
        if (Input.GetKeyDown(KeyCode.Z) && windowsList[4].activeSelf)
        {
            if(cursornumber == 0)
            {
                windowsText[5].text = " 　武器\n 　武器\n 　武器\n 　武器\n 　武器\n 　武器\n 　武器\n 　武器\n 　武器\n";
                windowsList[5].SetActive(true);
            }
            else if (cursornumber == 1)
            {
                windowsText[5].text = "　 アイテム\n　 アイテム\n　 アイテム\n　 アイテム\n　 アイテム\n　 アイテム\n　 アイテム\n　 アイテム\n　 アイテム\n";
                windowsList[5].SetActive(true);
            }
            else if (cursornumber == 2)
            {
                windowsText[5].text = "　 宝石\n　 宝石\n　 宝石\n　 宝石\n　 宝石\n　 宝石\n　 宝石\n　 宝石\n　 宝石\n";
                windowsList[5].SetActive(true);
            }
        }
        else if(Input.GetKeyDown(KeyCode.X) && windowsList[5].activeSelf)
        {
            windowsList[5].SetActive(false);
        }
        #endregion
    }
}
