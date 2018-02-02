using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTextManager : MonoBehaviour {

    private static readonly float DefHeight = 100;

    public RectTransform receiptback;
    public GameObject resultItemTextPrefab;
    public GameObject resultPatternTextPrefab;
    public GameObject resultTextPrefab;

    private float nextheight = 0.0f;
    private RectTransform myTransform;
    public RectTransform logoTransform;

    private Dictionary<string, string> EnglishNameToJap;

    // Use this for initialization
    void Start() {
        EnglishNameToJap = new Dictionary<string, string>();
        EnglishNameToJap["human"] = "人間";
        EnglishNameToJap["Arai"] = "アライグマ";
        EnglishNameToJap["Pig"] = "豚";
        EnglishNameToJap["Cow"] = "牛";
        EnglishNameToJap["Fish"] = "魚";
        EnglishNameToJap["Herazika"] = "ヘラジカ";
        EnglishNameToJap["Sheep"] = "羊";
        EnglishNameToJap["Kaziki"] = "カジキマグロ";
        EnglishNameToJap["Chickin"] = "鶏";
        EnglishNameToJap["Lamborghini"] = "闘牛";
        EnglishNameToJap["Shark"] = "サメ";
        myTransform = GetComponent<RectTransform>();
        int pricegoukei = 0;
        int pointgoukei = 0;

        nextheight += logoTransform.sizeDelta.y;

        string[] str = { "本店　Tel(XOO)XXX-OOO"};
        // 
        GameObject shop = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        shop.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        nextheight += shop.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_L, str);
        // 日時
        str[0] = 
            System.DateTime.Now.Year + "年" +
            System.DateTime.Now.Month + "月" +
            System.DateTime.Now.Day + "日" +
            "（" + System.DateTime.Now.DayOfWeek.ToString().Remove(3) + "）" +
            System.DateTime.Now.Hour + "：" +
            System.DateTime.Now.Minute;
        GameObject day = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        day.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        nextheight += day.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_L, str);
        // レジ担当
        str[0] = "担当者：名無し";
        GameObject men = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        men.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        nextheight += men.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_L, str);

        //取った敵たち
        for (int i = 0; i < ScoreManager.EnemyTypeCount(); i++)
        {
            int enemycount = ScoreManager.GetCount(i);
            if (enemycount == 0) continue;
            string enemyname = ScoreManager.enemysname[i];
            int price = ScoreManager.EnemyPrice(enemyname);
            int point = ScoreManager.EnemyPoint(enemyname);
            

            GameObject resultText = Instantiate(resultItemTextPrefab, transform.position, Quaternion.identity, transform);
            //resultText.transform.SetParent(transform);
            resultText.GetComponent<RectTransform>().anchoredPosition -=   Vector2.up * nextheight;
            string[] l_texts = { EnglishNameToJap[enemyname], price.ToString(), point.ToString(), enemycount.ToString(), (price* enemycount).ToString(), (point * enemycount).ToString()};
            nextheight += resultText.GetComponent<ResultText>().SetTexts(ResultText.TextType.ItemText, l_texts);

            pricegoukei += price * enemycount;
            pointgoukei += point * enemycount;
        }

        str[0] = "--------ボーナスポイント--------";
        GameObject ad = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        ad.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        nextheight += ad.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_C, str);

        foreach (var i in ScoreManager.GetResultPatternDatas())
        {
            GameObject resultText = Instantiate(resultPatternTextPrefab, transform.position, Quaternion.identity, transform);
            //resultText.transform.SetParent(transform);
            resultText.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
            string[] l_texts = { i.PatternName, EnglishNameToJap[i.nameList[0]] + "&" + EnglishNameToJap[i.nameList[1]] + "&" + EnglishNameToJap[i.nameList[2]], i.point.ToString()};
            nextheight += resultText.GetComponent<ResultText>().SetTexts( ResultText.TextType.PatternText, l_texts);
            pointgoukei += i.point;
        }

        //商品と合計の間の点線
        GameObject tensen = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        tensen.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        str[0] = "----------------------------";
        nextheight += tensen.GetComponent<ResultText>().SetTexts( ResultText.TextType.DefaultText_C, str);
        //合計
        GameObject goukei = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        goukei.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        str[0] = "合計";
        nextheight += goukei.GetComponent<ResultText>().SetTexts( ResultText.TextType.DefaultText_L,str);
        GameObject totalprice = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        totalprice.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        str[0] = "\\" + pricegoukei.ToString();
        nextheight += totalprice.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_R, str);
        GameObject totalpoint = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        totalpoint.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        str[0] =  pointgoukei.ToString() + "Pt";
        nextheight += totalpoint.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_R, str);
        //商品と合計の間の点線
        GameObject ari = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        ari.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        str[0] = "ありがとうございました。";
        nextheight += ari.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_C, str);


        receiptback.sizeDelta = new Vector2(receiptback.sizeDelta.x, nextheight + DefHeight);

        transform.parent.GetComponent<Receipt>().SetParameter(nextheight + DefHeight);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
