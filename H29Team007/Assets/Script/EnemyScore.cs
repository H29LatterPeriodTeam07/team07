using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScore : MonoBehaviour {

    private int m_price;

	// Use this for initialization
	void Start () {
        //ここでタグによって金額変えればいいんでない？
        switch (transform.tag)
        {
            case "Untagged": m_price = 100;break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPrice(int price)
    {
        m_price = price;
    }

    public int GetPrice()
    {
        return m_price;
    }
}
