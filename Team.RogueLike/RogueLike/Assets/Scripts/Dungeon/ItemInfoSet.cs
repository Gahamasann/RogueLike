using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoSet : MonoBehaviour
{
    private string[] sideLine;//生データの横一行を入れるやつ
    private string[,] textWords;//加工後のデータが入るやつ

    private int rowLength;//行数
    private int colnumLength;//列数

    SpriteRenderer image;

    [SerializeField]
    Category category;

    private int iD;
    private string itemName;
    private int rare;
    private int attack;
    private int defence;
    private int heal;
    private int buy;
    private float rate;
    private int sale;
    private string text;

    public int floor_RARE1;
    public int floor_RARE2;
    public int floor_RARE3;
    public int floor_RARE4;


    private enum Category
    {
        WEAPON,
        AROMOUR,
        FOOD,
        GOODS,
        TREASURE,
    }

    // Start is called before the first frame update
    void Start()
    {


        //image = gameObject.GetComponent<SpriteRenderer>();

        TextAsset textAsset = new TextAsset();//テキストファイルのデータを取得

        textAsset = Resources.Load("ItemID", typeof(TextAsset)) as TextAsset; //対象のテキストを読み込んでTextAssetにキャスト

        string textLines = textAsset.text;//string型にしてtextLineに入れる


        sideLine = textLines.Split('\n');//一行づつに分けてsideLineに入れる


        colnumLength = 11;//見出し？の数

        rowLength = sideLine.Length - 1;//改行の数で判断しているので-1しないとコレクション外になりエラー吐く

        textWords = new string[rowLength, colnumLength];

        for (int i = 0; i < rowLength; i++)
        {
            string[] tempWprds = sideLine[i].Split(',');//コンマ毎に分けたものを入れる

            for (int j = 0; j < colnumLength; j++)
            {
                textWords[i, j] = tempWprds[j];//区切ったものを入れる
            }
        }

        switch (category.ToString())
        {
            case "WEAPON":
                iD = Random.Range(101, 105 + 1);
                break;

            case "AROMOUR":
                iD = Random.Range(201, 203 + 1);
                break;

            case "FOOD":
                iD = Random.Range(301, 301 + 1);
                break;

            case "GOODS":
                iD = Random.Range(401, 401 + 1);
                break;

            case "TREASURE":
                iD = Random.Range(1001, 1006 + 1);
                break;
        }

        Insertion();

        Debug.Log(itemName);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Insertion()
    {
        for (int i = 1; i < rowLength; i++)
        {
            if (textWords[i, 1] != "" && int.Parse(textWords[i, 1]) == iD)//testWords[i, 1]が空じゃないかつ、IDが一致したら
            {
                if (textWords[i, 2] != "") itemName = textWords[i, 2]; //空じゃなければ入れる
                if (textWords[i, 3] != "") rare = int.Parse(textWords[i, 3]);
                if (textWords[i, 4] != "") attack = int.Parse(textWords[i, 4]);
                if (textWords[i, 5] != "") defence = int.Parse(textWords[i, 5]);
                if (textWords[i, 6] != "") heal = int.Parse(textWords[i, 6]);
                if (textWords[i, 7] != "") buy = int.Parse(textWords[i, 7]);
                if (textWords[i, 8] != "") rate = float.Parse(textWords[i, 8]);
                if (textWords[i, 9] != "") sale = int.Parse(textWords[i, 9]);
                if (textWords[i, 10] != "") text = textWords[i, 10];
            }
        }
    }
}
