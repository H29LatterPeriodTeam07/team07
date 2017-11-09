using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScore : MonoBehaviour {

    private int m_price;
    private int m_Number = 1;

    private int[] plasticbagNumbers;

	// Use this for initialization
	void Start () {
        //ここでタグによって金額変えればいいんでない？
        switch (transform.tag)
        {
            case "Untagged": m_price = 100;break;
            case "Enemy": m_price = 100; break;
            case "BBA": m_price = 100; break;
            case "Parent": m_price = 100; break;
            case "Animal":m_Number = 2; m_price = 100; break;
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
        if (transform.tag != "Plasticbag")
        {
            return ScoreManager.EnemyPrice(m_Number);
        }
        else
        {
            return m_price;
        }
    }

    public void SetNumber(List<int> nums)
    {
        int[] a = new int[nums.Count];
        for(int i = 0; i < nums.Count; i++)
        {
            a[i] = nums[i];
        }
        plasticbagNumbers = a;
    }

    public int GetNumber()
    {
        return m_Number;
    }

    public int[] GetNumbers()
    {
        return plasticbagNumbers;
    }

    //public int GetPlasticbagCount()
    //{
    //    return plasticbagNumbers.Length;
    //}
}
