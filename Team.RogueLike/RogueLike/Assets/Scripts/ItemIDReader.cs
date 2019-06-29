using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIDReader: MonoBehaviour
{

    private string[] sideLine;//生データの横一行を入れるやつ
    private string[,] textWords;//加工後のデータが入るやつ

    private int rowLength;//行数
    private int colnumLength;//列数

    [SerializeField]
    private int iD;
    [SerializeField]
    private string type;
    [SerializeField]
    private int rare;
    [SerializeField]                     
    private int attack;
    [SerializeField]
    private int defence;
    [SerializeField]
    private int heal;
    [SerializeField]
    private int buy;
    [SerializeField]
    private float rate;
    [SerializeField]
    private int sale;
    [SerializeField]
    private string text;

    void Start()
    {
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

        Insertion();
    }

    void Insertion()
    {
        for (int i = 1; i < rowLength; i++)
        {
            if(textWords[i, 1] != "" &&  int.Parse(textWords[i, 1]) == iD)//testWords[i, 1]が空じゃないかつ、IDが一致したら
            {
                if (textWords[i, 0] != "")  type               = textWords[i, 0]; //空じゃなければ
                if (textWords[i, 2] != "")  gameObject.name    = textWords[i, 2]; 
                if (textWords[i, 3] != "")  rare               = int.Parse(textWords[i, 3]); 
                if (textWords[i, 4] != "")  attack             = int.Parse(textWords[i, 4]); 
                if (textWords[i, 5] != "")  defence            = int.Parse(textWords[i, 5]); 
                if (textWords[i, 6] != "")  heal               = int.Parse(textWords[i, 6]); 
                if (textWords[i, 7] != "")  buy                = int.Parse(textWords[i, 7]); 
                if (textWords[i, 8] != "")  rate               = float.Parse(textWords[i, 8]); 
                if (textWords[i, 9] != "")  sale               = int.Parse(textWords[i, 9]); 
                if (textWords[i, 10] != "") text               = textWords[i, 10]; 
            }
        }
    }

}
