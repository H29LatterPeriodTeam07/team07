using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTotalText : MonoBehaviour {

    private Text countText;
    private Text priceText;

    private int totalcount = 0;
    private int totalprice = 0;

    // Use this for initialization
    void Start () {
        countText = transform.Find("count").GetComponent<Text>();
        priceText = transform.Find("price").GetComponent<Text>();
        for(int i = 1; i <= ScoreManager.EnemyTypeCount(); i++)
        {
            int count = ScoreManager.GetCount(i);
            int price = ScoreManager.EnemyPrice(ScoreManager.enemysname[i]) * count;

            totalcount += count;
            totalprice += price;
        }

        string ct = totalcount.ToString();
        string pt = totalprice.ToString();

        countText.text = ct;
        priceText.text = pt;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
