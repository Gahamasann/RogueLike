﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// ダンジョンの自動生成モジュール
/// </summary>
public class DgGenerator : MonoBehaviour
{
    /// <summary>
    /// マップ全体の幅
    /// </summary>
    public int WIDTH = 30;
    /// <summary>
    /// マップ全体の高さ
    /// </summary>
    public int HEIGHT = 30;

    /// <summary>
    /// 区画と部屋の余白サイズ
    /// </summary>
    public int OUTER_MERGIN = 3;
    /// <summary>
    /// 部屋配置の余白サイズ
    /// </summary>
    public int POS_MERGIN = 2;
    /// <summary>
    /// 最小の部屋サイズ
    /// </summary>
    public int MIN_ROOM = 3;
    /// <summary>
    /// 最大の部屋サイズ
    /// </summary>
    public int MAX_ROOM = 5;

    /// <summary>
    /// 通路
    /// </summary>
    const int CHIP_NONE = 1;
    /// <summary>
    /// 壁
    /// </summary>
    const int CHIP_WALL = 0;

    /// <summary>
    /// 2次元配列情報
    /// </summary>
    Layer2D _layer = null;
    /// <summary>
    /// 区画リスト
    /// </summary>
    List<DgDivision> _divList = null;


    //*****************************************
    //**********外部から追加した分*************
    //*****************************************

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
    //アイテムが出現する個数指定
    public Count itemCount = new Count(1, 5);
    //敵が出現する個数設定
    public Count enemyCount = new Count(1, 5);
    //Exitオブジェクト
    public GameObject exit;
    //Playerオブジェクト
    GameObject player;
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

    void Start()
    {
        // ■1. 初期化
        // 2次元配列初期化
        _layer = new Layer2D(WIDTH, HEIGHT);

        // 区画リスト作成
        _divList = new List<DgDivision>();

        // ■2. すべてを壁にする
        _layer.Fill(CHIP_WALL);

        // ■3. 最初の区画を作る
        CreateDivision(0, 0, WIDTH - 1, HEIGHT - 1);

        // ■4. 区画を分割する
        // 垂直 or 水平分割フラグの決定
        bool bVertical = (Random.Range(0, 2) == 0);
        SplitDivison(bVertical);

        // ■5. 区画に部屋を作る
        CreateRoom();

        // ■6. 部屋同士をつなぐ
        ConnectRooms();
    }

    /// <summary>
    /// 最初の区画を作る
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="top">上</param>
    /// <param name="right">右</param>
    /// <param name="bottom">下</param>
    void CreateDivision(int left, int top, int right, int bottom)
    {
        DgDivision div = new DgDivision();
        div.Outer.Set(left, top, right, bottom);
        _divList.Add(div);
    }

    /// <summary>
    /// 区画を分割する
    /// </summary>
    /// <param name="bVertical">垂直分割するかどうか</param>
    void SplitDivison(bool bVertical)
    {
        // 末尾の要素を取り出し
        DgDivision parent = _divList[_divList.Count - 1];
        _divList.Remove(parent);

        // 子となる区画を生成
        DgDivision child = new DgDivision();

        if (bVertical)
        {
            // ▼縦方向に分割する
            if (CheckDivisionSize(parent.Outer.Height) == false)
            {
                // 縦の高さが足りない
                // 親区画を戻しておしまい
                _divList.Add(parent);
                return;
            }

            // 分割ポイントを求める
            int a = parent.Outer.Top    + (MIN_ROOM + OUTER_MERGIN);
            int b = parent.Outer.Bottom - (MIN_ROOM + OUTER_MERGIN);
            // AB間の距離を求める
            int ab = b - a;
            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, MAX_ROOM);

            // 分割点を決める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            child.Outer.Set(
                parent.Outer.Left, p, parent.Outer.Right, parent.Outer.Bottom);

            // 親の下側をp地点まで縮める
            parent.Outer.Bottom = child.Outer.Top;
        }
        else
        {
            // ▼横方向に分割する
            if (CheckDivisionSize(parent.Outer.Width) == false)
            {
                // 横幅が足りない
                // 親区画を戻しておしまい
                _divList.Add(parent);
                return;
            }

            // 分割ポイントを求める
            int a = parent.Outer.Left  + (MIN_ROOM + OUTER_MERGIN);
            int b = parent.Outer.Right - (MIN_ROOM + OUTER_MERGIN);
            // AB間の距離を求める
            int ab = b - a;
            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, MAX_ROOM);

            // 分割点を求める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            child.Outer.Set(
                p, parent.Outer.Top, parent.Outer.Right, parent.Outer.Bottom);

            // 親の右側をp地点まで縮める
            parent.Outer.Right = child.Outer.Left;
        }

        // 次に分割する区画をランダムで決める
        if (Random.Range(0, 2) == 0)
        {
            // 子を分割する
            _divList.Add(parent);
            _divList.Add(child);
        }
        else
        {
            // 親を分割する
            _divList.Add(child);
            _divList.Add(parent);
        }

        // 分割処理を再帰呼び出し (分割方向は縦横交互にする)
        SplitDivison(!bVertical);
    }

    /// <summary>
    /// 指定のサイズを持つ区画を分割できるかどうか
    /// </summary>
    /// <param name="size">チェックする区画のサイズ</param>
    /// <returns>分割できればtrue</returns>
    bool CheckDivisionSize(int size)
    {
        // (最小の部屋サイズ + 余白)
        // 2分割なので x2 する
        // +1 して連絡通路用のサイズも残す
        int min = (MIN_ROOM + OUTER_MERGIN) * 2 + 1;

        return size >= min;
    }

    /// <summary>
    /// 区画に部屋を作る
    /// </summary>
    void CreateRoom()
    {
        foreach (DgDivision div in _divList)
        {
            // 基準サイズを決める
            int dw = div.Outer.Width - OUTER_MERGIN;
            int dh = div.Outer.Height - OUTER_MERGIN;

            // 大きさをランダムに決める
            int sw = Random.Range(MIN_ROOM, dw);
            int sh = Random.Range(MIN_ROOM, dh);

            // 最大サイズを超えないようにする
            sw = Mathf.Min(sw, MAX_ROOM);
            sh = Mathf.Min(sh, MAX_ROOM);

            // 空きサイズを計算 (区画 - 部屋)
            int rw = (dw - sw);
            int rh = (dh - sh);
            
            // 部屋の左上位置を決める
            int rx = Random.Range(0, rw) + POS_MERGIN;
            int ry = Random.Range(0, rh) + POS_MERGIN;

            int left   = div.Outer.Left + rx;
            int right  = left + sw;
            int top    = div.Outer.Top + ry;
            int bottom = top + sh;

            // 部屋のサイズを設定
            div.Room.Set(left, top, right, bottom);

            // 部屋を通路にする
            FillDgRect(div.Room);
        }
    }

    /// <summary>
	/// DgRectの範囲を塗りつぶす
    /// </summary>
    /// <param name="rect">矩形情報</param>
    void FillDgRect(DgDivision.DgRect r)
    {
        _layer.FillRectLTRB(r.Left, r.Top, r.Right, r.Bottom, CHIP_NONE);
    }

    /// <summary>
    /// 部屋同士を通路でつなぐ
    /// </summary>
    void ConnectRooms()
    {
        for (int i = 0; i < _divList.Count - 1; i++)
        {
            // リストの前後の区画は必ず接続できる
            DgDivision a = _divList[i];
            DgDivision b = _divList[i + 1];

            // 2つの部屋をつなぐ通路を作成
            CreateRoad(a, b);

			// 孫にも接続する
			for(int j = i + 2; j < _divList.Count; j++)
			{
				DgDivision c = _divList[j];
				if(CreateRoad(a, c, true))
				{
					// 孫に接続できたらおしまい
					break;
				}
			}
        }
    }

    /// <summary>
    /// 指定した部屋の間を通路でつなぐ
    /// </summary>
    /// <param name="divA">部屋1</param>
    /// <param name="divB">部屋2</param>
	/// <param name="bGrandChild">孫チェックするかどうか</param>
    /// <returns>つなぐことができたらtrue</returns>
	bool CreateRoad(DgDivision divA, DgDivision divB, bool bGrandChild=false)
    {
        if (divA.Outer.Bottom == divB.Outer.Top || divA.Outer.Top == divB.Outer.Bottom)
        {
            // 上下でつながっている
            // 部屋から伸ばす通路の開始位置を決める
            int x1 = Random.Range(divA.Room.Left, divA.Room.Right);
            int x2 = Random.Range(divB.Room.Left, divB.Room.Right);
            int y  = 0;

			if(bGrandChild)
			{
				// すでに通路が存在していたらその情報を使用する
				if(divA.HasRoad()) { x1 = divA.Road.Left; }
				if(divB.HasRoad()) { x2 = divB.Road.Left; }
			}

            if (divA.Outer.Top > divB.Outer.Top)
            {
                // B - A (Bが上側)
                y = divA.Outer.Top;
                // 通路を作成
				divA.CreateRoad(x1, y + 1, x1 + 1, divA.Room.Top);
				divB.CreateRoad(x2, divB.Room.Bottom, x2 + 1, y);
            }
            else
            {
                // A - B (Aが上側)
                y = divB.Outer.Top;
                // 通路を作成
				divA.CreateRoad(x1, divA.Room.Bottom, x1 + 1, y);
				divB.CreateRoad(x2, y, x2 + 1, divB.Room.Top);
            }
			FillDgRect(divA.Road);
			FillDgRect(divB.Road);

            // 通路同士を接続する
            FillHLine(x1, x2, y);

            // 通路を作れた
            return true;
        }

        if (divA.Outer.Left == divB.Outer.Right || divA.Outer.Right == divB.Outer.Left)
        {
            // 左右でつながっている
            // 部屋から伸ばす通路の開始位置を決める
            int y1 = Random.Range(divA.Room.Top, divA.Room.Bottom);
            int y2 = Random.Range(divB.Room.Top, divB.Room.Bottom);
            int x  = 0;

			if(bGrandChild)
			{
				// すでに通路が存在していたらその情報を使う
				if(divA.HasRoad()) { y1 = divA.Road.Top; }
				if(divB.HasRoad()) { y2 = divB.Road.Top; }
			}

            if (divA.Outer.Left > divB.Outer.Left)
            {
                // B - A (Bが左側)
                x = divA.Outer.Left;
                // 通路を作成
				divB.CreateRoad(divB.Room.Right, y2, x, y2 + 1);
				divA.CreateRoad(x + 1, y1, divA.Room.Left, y1 + 1);
            }
            else
            {
                // A - B (Aが左側)
                x = divB.Outer.Left;
				divA.CreateRoad(divA.Room.Right, y1, x, y1 + 1);
				divB.CreateRoad(x, y2, divB.Room.Left, y2 + 1);
            }
			FillDgRect(divA.Road);
			FillDgRect(divB.Road);

            // 通路同士を接続する
            FillVLine(y1, y2, x);

            // 通路を作れた
            return true;
        }


        // つなげなかった
        return false;
    }

    /// <summary>
    /// 水平方向に線を引く (左と右の位置は自動で反転する)
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="right">右</param>
    /// <param name="y">Y座標</param>
    void FillHLine(int left, int right, int y)
    {
        if (left > right)
        {
            // 左右の位置関係が逆なので値をスワップする
            int tmp = left;
            left = right;
            right = tmp;
        }
        _layer.FillRectLTRB(left, y, right + 1, y + 1, CHIP_NONE);
    }

    /// <summary>
    /// 垂直方向に線を引く (上と下の位置は自動で反転する)
    /// </summary>
    /// <param name="top">上</param>
    /// <param name="bottom">下</param>
    /// <param name="x">X座標</param>
    void FillVLine(int top, int bottom, int x)
    {
        if (top > bottom)
        {
            // 上下の位置関係が逆なので値をスワップする
            int tmp = top;
            top = bottom;
            bottom = tmp;
        }
        _layer.FillRectLTRB(x, top, x + 1, bottom + 1, CHIP_NONE);
    }

    //*****************************************
    //**********外部から追加した分*************
    //*****************************************

    //任意の位置から八方向のレイヤーに埋め込まれている数の合計
    int LayerSum(int x,int y)
    {
        int result = _layer.Get(x + 1, y) + _layer.Get(x + 1, y + 1) + _layer.Get(x + 1, y - 1)
                    + _layer.Get(x - 1, y) + _layer.Get(x - 1, y + 1) + _layer.Get(x - 1, y - 1)
                    + _layer.Get(x, y + 1) + _layer.Get(x, y - 1);
        return result;
    }

    //外壁、床を配置
    void BoardSetup()
    {
        //Boardオブジェクトを作成し、transform情報をboardHolderに保存
        boardHolder = new GameObject("Board").transform;
        //widthとheightで指定した分ループ
        for (int x = 0; x < WIDTH/* + 1*/; x++)
        {
            for (int y = 0; y < HEIGHT/* + 1*/; y++)
            {
                GameObject toInstantiate = null;
                if (_layer.Get(x,y) == CHIP_NONE)
                {
                    //床をランダムで生成
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                }
                else if(LayerSum(x,y) > 0)//周囲に１マスでも床があれば
                {
                    if (_layer.Get(x, y) == CHIP_WALL)
                    {
                        //外壁をランダムで生成
                        toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    }
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                }
                else
                {
                    //床をランダムで生成
                    //toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }

                //床or外壁を生成し、instance変数に格納
                //GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                //生成したinstanceをBoardオブジェクトの子オブジェクトとする
                //instance.transform.SetParent(boardHolder);
            }
        }
    }

    //アイテム等を配置できる範囲を決定
    void InitialiseList()
    {
        //gridPositionを一旦クリア
        gridPositions.Clear();
        //gridPositionにオブジェクト配置可能範囲を指定
        //widthとheightで指定した分ループ
        for (int x = 1; x < WIDTH - 1; x++)
        {
            for (int y = 1; y < HEIGHT - 1; y++)
            {
                if (_layer.Get(x, y) == CHIP_NONE&&LayerSum(x,y)>2
                    &&_layer.Get(x + 1,y)+_layer.Get(x - 1,y)>0
                    &&_layer.Get(x,y + 1)+_layer.Get(x,y - 1)>0)
                {
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }
    }

    Vector3 RandomPosition()
    {
        //gridPositionsからランダムで位置を1つ決定し、位置情報を確定
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        //ランダム決定した数値を削除
        gridPositions.RemoveAt(randomIndex);
        //確定した位置情報を返す
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //最低値～最大値+1のランダム回数分だけループ
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++)
        {
            //gridPositionから位置情報を1つ取得
            Vector3 randomPosition = RandomPosition();
            //引数tileArrayからランダムで1つ選択
            GameObject tileChoise = tileArray[Random.Range(0, tileArray.Length)];
            //ランダムで決定した種類・位置でオブジェクトを生成
            Instantiate(tileChoise, randomPosition, Quaternion.identity);
        }
    }

    void LayoutObjectAtRandom(GameObject tileArray)
    {
            //gridPositionから位置情報を1つ取得
            Vector3 randomPosition = RandomPosition();
            //ランダムで決定した種類・位置でオブジェクトを生成
            Instantiate(tileArray, randomPosition, Quaternion.identity);
    }

    void PlayerInit(GameObject gameObject)
    {
        Vector3 randomPosition = RandomPosition();
        player = GameObject.Find("Player");//生成にしたほうが処理速いかも
        player.transform.position = randomPosition;
    }

    //オブジェクトを配置していくメソッド
    //唯一のpublicメソッド　床生成時に、GameManagerから呼ばれる
    public void SetupScene(int level)
    {
        //0と1の配列でランダムダンジョンを生成
        Start();
        //床と外壁を配置し、
        BoardSetup();
        //アイテム等を配置できる位置を決定し、
        InitialiseList();
        //アイテムをランダムで配置し、
        LayoutObjectAtRandom(itemTiles, itemCount.minimum, itemCount.maximum);
        //エネミーをランダムで配置し、
        LayoutObjectAtRandom(enemyTiles, enemyCount.minimum, enemyCount.maximum);
        //Exitを右上の位置に配置する。
        LayoutObjectAtRandom(exit);
        //Playerをマップに配置
        PlayerInit(player);
    }
}
