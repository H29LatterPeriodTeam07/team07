using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScore : MonoBehaviour {

    private int m_price;
    private int m_Number = 1;

    private string[] plasticbagNames;

	// Use this for initialization
	void Start () {
        //ここでタグによって金額変えればいいんでない？
        switch (transform.tag)
        {
            case "Untagged": m_price = 100;break;
            case "Enemy": m_price = 100; break;
            case "BBA": m_price = 100; break;
            case "Parent": m_price = 100; break;
            case "Animal":m_Number = 2;
                if (transform.name.Contains("Pig")) { m_Number = 3; }
                if (transform.name.Contains("Cow")) { m_Number = 4; }
                if (transform.name.Contains("Fish")) { m_Number = 5; }
                break;
            case "Bull": m_Number = 2; break;
            case "Plasticbag":break;
            default:  break;
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
            return ScoreManager.EnemyPrice(transform.name);
        }
        else
        {
            return m_price;
        }
    }

    public void SetNames(List<string> names)
    {
        string[] a = new string[names.Count];
        for(int i = 0; i < names.Count; i++)
        {
            a[i] = names[i];
        }
        plasticbagNames = a;
    }

    public int GetNumber()
    {
        return m_Number;
    }

    public string[] GetNames()
    {
        return plasticbagNames;
    }

    //public int GetPlasticbagCount()
    //{
    //    return plasticbagNumbers.Length;
    //}
}
