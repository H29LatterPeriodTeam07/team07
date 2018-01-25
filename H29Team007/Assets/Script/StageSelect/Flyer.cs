using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flyer : MonoBehaviour {
    // 広告の商品の親
    private GameObject m_Goods;

	// Use this for initialization
	void Start () {
        m_Goods = transform.Find("Goods").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPrice(string goodsName, int price)
    {
        // 広告の品の名前と一致するものがあれば価格を設定する
        for(int i = 0; i < m_Goods.transform.childCount; ++i)
        {
            // 商品表示管理者の子のImagePrefabが商品名を持っている
            Transform l_GoodsHolder = m_Goods.transform.GetChild(i);
            if (l_GoodsHolder.name == "Frame") continue;
            string l_name = l_GoodsHolder.Find("Image").GetComponent<Image>().sprite.name;

            if (goodsName == l_name)
            {
                // 価格設定
                l_GoodsHolder.Find("Price").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = price.ToString();
            }
        }
    }
    public void SetPoint(string goodsName, int point)
    {
        // 広告の品の名前と一致するものがあれば価格を設定する
        for (int i = 0; i < m_Goods.transform.childCount; ++i)
        {
            // 商品表示管理者の子のImagePrefabが商品名を持っている
            Transform l_GoodsHolder = m_Goods.transform.GetChild(i);
            if (l_GoodsHolder.name == "Frame") continue;
            string l_name = l_GoodsHolder.Find("Image").GetComponent<Image>().sprite.name;

            if (goodsName == l_name)
            {
                // ポイント設定
                l_GoodsHolder.Find("Point").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = point.ToString() + "%Pt";
            }
        }
    }

    public void SetImageGoodslocalPosition(GameObject image, string goodsName)
    {
        // 広告の品の名前と一致するものがあれば価格を設定する
        for (int i = 0; i < m_Goods.transform.childCount; ++i)
        {
            // 商品表示管理者の子のPricePrefabのTextが商品名を持っている
            Transform l_GoodsHolder = m_Goods.transform.GetChild(i);
            if (l_GoodsHolder.name == "Frame") continue;
            GameObject l_GoodsImage = l_GoodsHolder.Find("Image").gameObject;
            GameObject l_Price = l_GoodsHolder.Find("Price").Find("Text").gameObject;
            string l_name = l_GoodsImage.GetComponent<Image>().sprite.name;

            if (goodsName == l_name)
            {
                image.transform.parent = transform.parent;
                image.GetComponent<RectTransform>().position = l_Price.GetComponent<RectTransform>().position;
            }
        }
    }
}
