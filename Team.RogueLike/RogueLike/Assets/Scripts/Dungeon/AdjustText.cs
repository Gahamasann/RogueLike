using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustText : MonoBehaviour
{
    private Text log;
    private string preLog;//調整前のログ保存用
    float cnt = 0;
    GameObject player;
    Player pl;

    [SerializeField, Tooltip("新しいログがUI範囲内に収まるようにテキストを調整する(Truncate限定)")]
    private bool viewInRect = true;

    private void Awake()
    {
        log = this.GetComponent<Text>();
        cnt = 3.0f;
        player = GameObject.Find("Player");
        pl = player.GetComponent<Player>();
    }

    private void Update()
    {
        cnt -= 1 * Time.deltaTime;
        preLog = log.text;

        Adjust(log);//メッセージ欄からはみでるログを削除

        if (preLog != log.text)
        {
            cnt = 3.0f;
        }

        if (cnt <= 0)
        {
            //テキスト内容が変わらない状態が続くと親を非活性化
            transform.parent.gameObject.SetActive(false);
        }

        if(pl.GetHp() <1)
        {
            transform.parent.gameObject.SetActive(false);
        }

    }

    private void Adjust(Text t)
    {
        TextGenerator generator = t.cachedTextGenerator;
        //text.GetGenerationSettingsで書き込める範囲を取得(Vector2)
        var settings = t.GetGenerationSettings(t.rectTransform.rect.size);

        //取得した設定(settings)でテキストを生成
        generator.Populate(t.text, settings);

        //生成されて表示されている行に含まれている文字数(countVisible)
        int countVisible = generator.characterCountVisible;

        //追加される文字がないとき(countVisible=0)または
        //表示する文字数が書き込める範囲をこえているときここで返す
        if (countVisible == 0 || t.text.Length <= countVisible)
            return;

        //はみ出ている文字数(truncatedCount)を取得
        int truncatedCount = t.text.Length - countVisible;

        //行ごとに分けて配列作成
        var lines = t.text.Split('\n');

        foreach (string line in lines)
        {
            //見切れている文字数が0になるまで,テキストの先頭行から消してゆく
            t.text = t.text.Remove(0, line.Length + 1);

            //消した行分の文字数をtruncatedCountから引く
            truncatedCount -= (line.Length + 1);

            //truncatedCountが0未満になるまで続ける
            if (truncatedCount <= 0)
                break;
        }

    }
}
