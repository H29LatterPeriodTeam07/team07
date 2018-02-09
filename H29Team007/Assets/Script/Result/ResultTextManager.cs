using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTextManager : MonoBehaviour {

    public struct CommentData
    {
        public int borderValue;
        public int CommentIndex;
    };

    private List<string> commentTexts;
    private List<List<CommentData>> stageCommentDatas;

    private static readonly float DefHeight = 100;

    public RectTransform receiptback;
    public GameObject resultItemTextPrefab;
    public GameObject resultPatternTextPrefab;
    public GameObject resultTextPrefab;

    private float nextheight = 0.0f;
    private RectTransform myTransform;
    public RectTransform logoTransform;

    private Dictionary<string, string> EnglishNameToJap;
    private List<string> RegisterNameList;
    private List<string> Coment;

    // Use this for initialization
    void Start() {
        commentTexts = new List<string>();
        stageCommentDatas = new List<List<CommentData>>();
        CreateCommentData();
        RegisterNameList = new List<string>();
        RegisterNameList.Add("店長　ナカガワ");
        RegisterNameList.Add("ヤスタケ");
        RegisterNameList.Add("サイトウ");
        RegisterNameList.Add("シムラ");
        RegisterNameList.Add("ヨコハシ");
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
        str[0] = "担当者：" + SelectRegister();
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

        str[0] = "------ボーナスポイント------";
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

        GameObject oazukari = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        oazukari.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        str[0] = "お預り";
        nextheight += oazukari.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_L, str);
        GameObject money = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        money.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        int amountmoney = AmountMoney(pricegoukei);
        str[0] = "\\" + amountmoney.ToString();
        nextheight += money.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_R, str);


        GameObject otsuri = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        otsuri.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        str[0] = "お釣";
        nextheight += otsuri.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_L, str);
        GameObject resultprice = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        resultprice.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        str[0] = "\\" + (amountmoney - pricegoukei).ToString();
        nextheight += resultprice.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_R, str);

        str[0] = "--------今回ポイント--------";
        GameObject nowPoint = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        nowPoint.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        nextheight += nowPoint.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_C, str);


        GameObject totalpoint = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        totalpoint.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        str[0] =  pointgoukei.ToString() + "Pt";
        nextheight += totalpoint.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_R, str);
        //商品と合計の間の点線
        GameObject ari = Instantiate(resultTextPrefab, transform.position, Quaternion.identity, transform);
        ari.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * nextheight;
        str[0] = Comment(pricegoukei, pointgoukei);
        nextheight += ari.GetComponent<ResultText>().SetTexts(ResultText.TextType.DefaultText_C, str);


        receiptback.sizeDelta = new Vector2(receiptback.sizeDelta.x, nextheight + DefHeight);

        transform.parent.GetComponent<Receipt>().SetParameter(nextheight + DefHeight);
	}

    private string SelectRegister()
    {
        int random = Random.Range(0, RegisterNameList.Count - 1);
        return RegisterNameList[random];
    }

    private int AmountMoney(int price)
    {
        if (price <= 0) return price;
        string priceStr = price.ToString();
        char[] priceChar = priceStr.ToCharArray();
        string[] priceStr_ZeroClear =  priceStr.Trim().Split('0');
        char[] pricechar = {};
        for (int i = 0; i < priceStr_ZeroClear.Length; ++i)
        {
            if(priceStr_ZeroClear[i] != "")
            {
                pricechar = priceStr_ZeroClear[i].ToCharArray();
            }
        }
         
        char minNumber = pricechar[pricechar.Length - 1];
        int minNumberdigit = 0;
        for(int i = 0; i < priceChar.Length; ++i)
        {
            if(priceChar[i] == minNumber)
            {
                minNumberdigit = priceChar.Length - i;
            }
        }

        float AddValue = 0;
        if(int.Parse(minNumber.ToString()) >= 6 && minNumberdigit != 0)
        {
            AddValue = Mathf.Pow(10.0f, (float)minNumberdigit);
            AddValue -= (int)(float.Parse(minNumber.ToString()) * AddValue * 0.1f);
        }
        else if(int.Parse(minNumber.ToString()) < 6 && minNumberdigit != 0)
        {
            AddValue = Mathf.Pow(10.0f, (float)minNumberdigit - 1) * 5.0f;
            AddValue -= (int)(float.Parse(minNumber.ToString()) * AddValue * 0.1f);
        }
        else if(int.Parse(minNumber.ToString()) <= 5 && minNumberdigit == 0)
        {

        }
        return price + (int)AddValue;
    }
    private string Comment(int price, int point)
    {
        if(price <= 0)
        {
            return "何か買ってください";
        }
        int stageIndex = ScoreManager.GetStageNumber() - 1;
        foreach(var i in stageCommentDatas[stageIndex])
        {
            if(point >= i.borderValue)
            {
                return commentTexts[i.CommentIndex];
            }
        }

        return "ありがとうございました。";
    }

    private void CreateCommentData()
    {
        //resourcesフォルダ内にあるsampleTextファイルをロード
        TextAsset textAsset = Resources.Load("CommentsData") as TextAsset;
        //ロードした中身を
        //1行ずつに分割
        string[] row = textAsset.text.Split('\n');
        string[] data;
        string[] animal;
        string[] price;
        for (int i = 0; i < row.Length; i++)
        {
            List<CommentData> l_Stagedata = new List<CommentData>();
            

            string l_line = row[i].Replace("\r", "");
            if (l_line == "") continue;
            // [//]がある場合無視
            if (l_line.Substring(0, 2) == "//") continue;
            string[] typeline = l_line.Split(';');
            if (typeline[0] == "c")
            {
                string[] commentText = typeline[typeline.Length - 1].Split('n');
                string result = "";
                foreach (var j in commentText)
                {
                    result += j;
                    result += "\n";
                }
                commentTexts.Add(result);
                continue;
            }
            else if(typeline[0] == "d")
            {
                CommentData l_data;
                string[] dataStr = typeline[2].Split('/');
                foreach(var j in dataStr)
                {
                    string[] valueAndcomment = j.Split('_');
                    l_data.borderValue = int.Parse(valueAndcomment[0]);
                    l_data.CommentIndex = int.Parse(valueAndcomment[1]);

                    l_Stagedata.Add(l_data);
                }
            }
            stageCommentDatas.Add(l_Stagedata);
        }
    }
}
