using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour {

    public enum TextType
    {
        DefaultText_R,
        DefaultText_C,
        DefaultText_L,
        ItemText,
        PatternText,
    }

    int m_TextHeight;

    private static readonly int TextHeightSize = 30;



	// Use this for initialization
	void Start () {
        //countText = transform.Find("count").GetComponent<Text>();
        //priceText = transform.Find("price").GetComponent<Text>();

        //int enemyCount = ScoreManager.GetCount(m_Number);
        //string ct = enemyCount.ToString();
        //int enemysPrice = ScoreManager.EnemyPrice(m_Number) * enemyCount;
        //string pt = enemysPrice.ToString();

        //countText.text = ct;
        //priceText.text = pt;
        m_TextHeight = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public float SetTexts(TextType type, string[] texts)
    {
        m_TextHeight = 0;
        if(type == TextType.DefaultText_L ||
           type == TextType.DefaultText_C ||
           type == TextType.DefaultText_R)
        {
            DefaultTextCreate(type, texts[0]);
        }
        else if(type == TextType.ItemText)
        {
            ItemText(texts);
        }
        else if(type == TextType.PatternText)
        {
            PatternText(texts);
        }

        return m_TextHeight;
    }

    private void DefaultTextCreate(TextType type, string text)
    {
        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = text;

        if (type == TextType.DefaultText_L)
        {
            transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Left;
        }
        else if (type == TextType.DefaultText_C)
        {
            transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Center;
        }
        else if (type == TextType.DefaultText_R)
        {
            transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Right;
        }
        m_TextHeight += TextHeightSize;
    }

    private void ItemText(string[] texts)
    {
        // パラメターによって変更
        if (texts.Length >= 6)
        {
            transform.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = texts[0];
            transform.Find("unitValue").GetComponent<TMPro.TextMeshProUGUI>().text = texts[1] + "x\n" + texts[2] + "x";
            transform.Find("count").GetComponent<TMPro.TextMeshProUGUI>().text = texts[3] + "\n" + texts[3];
            transform.Find("value").GetComponent<TMPro.TextMeshProUGUI>().text = "\\" + texts[4] + "\n" + texts[5] + "Pt";
            // 名前
            m_TextHeight += TextHeightSize;
            // 金額
            m_TextHeight += TextHeightSize;
            // ポイント
            m_TextHeight += TextHeightSize;
        }
        else
        {
            transform.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = texts[0];
            transform.Find("unitValue").GetComponent<TMPro.TextMeshProUGUI>().text = texts[1] + "x\n ";
            transform.Find("count").GetComponent<TMPro.TextMeshProUGUI>().text = texts[2] + "\n ";
            transform.Find("value").GetComponent<TMPro.TextMeshProUGUI>().text = "\\" + texts[3] + "\n ";
            // 名前
            m_TextHeight += TextHeightSize;
            // 金額
            m_TextHeight += TextHeightSize;
        }
    }
    private void PatternText(string[] texts)
    {
        transform.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = texts[0];
        transform.Find("patternNames").GetComponent<TMPro.TextMeshProUGUI>().text = texts[1];
        transform.Find("value").GetComponent<TMPro.TextMeshProUGUI>().text = texts[2] + "Pt";

        // 名前
        m_TextHeight += TextHeightSize;

        // ポイント
        m_TextHeight += TextHeightSize;
    }
}
