using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour {


    [SerializeField, Header("コインプレハブ")]
    private GameObject coinPrefab;

    private int m_CreatedCoinCount;
    private int m_CreateCoinCount;

    private Vector2 m_InitPosition;

    bool m_IsCreate;
	// Use this for initialization
	void Start () {
        m_IsCreate = false;
        m_CreateCoinCount = 0;
        m_CreatedCoinCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_IsCreate)
        {
            if (m_CreatedCoinCount < m_CreateCoinCount) {
                GameObject l_coin = Instantiate(coinPrefab);
                l_coin.transform.SetParent(transform);
                l_coin.GetComponent<RectTransform>().localPosition = new Vector2(0.0f, -230.0f);
                l_coin.GetComponent<Coin>().SetInitPosition(new Vector2(0.0f, -230.0f));
                l_coin.GetComponent<Coin>().SetTarget(new Vector3(Random.Range(-640.0f, 640.0f), Random.Range(-50.0f, 200.0f), 0.0f));
                ++m_CreatedCoinCount;
            }
            else
            {
                m_IsCreate = false;
            }
        }
	}
    public void SetInitPosition(Vector2 initPosition)
    {
        m_InitPosition = initPosition;
    }
    public void SetCreateCoinCount(int CoinCount)
    {
        m_CreateCoinCount = CoinCount;
        m_CreatedCoinCount = 0;
    }
    public void CreateCoin()
    {
        m_IsCreate = true;
    }
}
