using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTextManager : MonoBehaviour {

    private static readonly float DefHeight = 100;

    public RectTransform receiptback;
    public GameObject resultTextPrefab;
    private float nextheight = 0.0f;
    private RectTransform myTransform;
    public RectTransform logoTransform;

	// Use this for initialization
	void Start () {
        myTransform = GetComponent<RectTransform>();
        int pricegoukei = 0;

        nextheight += logoTransform.sizeDelta.y;
        //取った敵たち
		for(int i = 0; i < ScoreManager.EnemyTypeCount(); i++)
        {
            int enemycount = ScoreManager.GetCount(i);
            if (enemycount == 0) continue;
            string enemyname = ScoreManager.enemysname[i];
            string enemynamejapan = "";
            int price = ScoreManager.EnemyPrice(enemyname);
            switch (enemyname)
            {
                case "human": enemynamejapan = "人間"; break;
                case "Pig": enemynamejapan = "豚";break;
                case "Cow": enemynamejapan = "牛";break;
                case "Fish": enemynamejapan = "魚"; break;
                case "Lamborghini": enemynamejapan = "闘牛"; break;
                default: enemynamejapan = enemyname; break;
            }

            GameObject resultText = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
            //resultText.transform.SetParent(transform);
            resultText.GetComponent<RectTransform>().anchoredPosition -=   Vector2.up * nextheight;
            nextheight += resultText.GetComponent<ResultText>().SetTexts(enemynamejapan, enemycount, price);

            pricegoukei += price * enemycount;
        }
        //商品と合計の間の点線
        GameObject tensen = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        tensen.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        nextheight += tensen.GetComponent<ResultText>().SetTexts("一一一一一一一一一一", 1, 1);
        //合計
        GameObject goukei = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        goukei.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        nextheight += goukei.GetComponent<ResultText>().SetTexts("合計", 1, pricegoukei);
        //商品と合計の間の点線
        GameObject ari = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        ari.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        nextheight += ari.GetComponent<ResultText>().SetTexts("ありがとうございました。", 1, 1);
        Text arigatou = ari.transform.Find("name").GetComponent<Text>();
        arigatou.fontSize = 30;
        arigatou.alignment = TextAnchor.MiddleCenter;


        receiptback.sizeDelta = new Vector2(receiptback.sizeDelta.x, nextheight + DefHeight);

        transform.parent.GetComponent<Receipt>().SetParameter(nextheight + DefHeight);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
