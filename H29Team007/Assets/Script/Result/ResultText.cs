using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour {

    private static readonly float NameAndPriceTextHeight = 80;
    private static readonly float CountTextHeight = 25;

    public int m_Number = 1;

    private Text countText;
    private Text priceText;

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
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public float SetTexts(string name,int count,int price)
    {
        float height = NameAndPriceTextHeight;
        transform.Find("name").GetComponent<Text>().text = name;
        transform.Find("price").GetComponent<Text>().text = (price < 10)?"":(price * count).ToString();
        Transform c = transform.Find("count");

        if (count == 1)
        {
            c.gameObject.SetActive(false);
        }
        else
        {
            c.GetComponent<Text>().text = "@" + price.ToString() + " x " + count.ToString();
            height += CountTextHeight;
        }
        return height;
    }
}
