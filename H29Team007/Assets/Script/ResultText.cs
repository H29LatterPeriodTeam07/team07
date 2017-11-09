using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour {

    public int m_Number = 1;

    private Text countText;
    private Text priceText;

	// Use this for initialization
	void Start () {
        countText = transform.Find("count").GetComponent<Text>();
        priceText = transform.Find("price").GetComponent<Text>();

        int enemyCount = ScoreManager.GetCount(m_Number);
        string ct = enemyCount.ToString();
        int enemysPrice = ScoreManager.EnemyPrice(m_Number) * enemyCount;
        string pt = enemysPrice.ToString();

        countText.text = ct;
        priceText.text = pt;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
