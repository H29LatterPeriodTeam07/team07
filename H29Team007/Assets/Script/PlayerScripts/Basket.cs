﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    private GameObject nearCart; //近いカート
    private float searchTime = 0; //経過時間
    private Player playerScr;

    // Use this for initialization
    void Start()
    {
        nearCart = SerchTag("Cart");
        playerScr = transform.root.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(nearCart == null)
        {
            nearCart = SerchTag("Cart");
        }
        if (nearCart == null)
        {
            Debug.Log("カートないやんけ！このハゲー！");
            return;
        }
        if (playerScr.GetState() == Player.PlayerState.Takeover) return;
        searchTime += Time.deltaTime;

        if (searchTime >= 1.0f)
        {
            nearCart = SerchTag("Cart");
            
            searchTime = 0;
        }

        Vector3 myXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 cartXZ = new Vector3(nearCart.transform.position.x, 0, nearCart.transform.position.z);
        float distancce = Vector3.Distance(cartXZ, myXZ);

        if(distancce < 5.0f)
        {
            SetBasketLocalPosition(new Vector3(CartRelatedData.cartOutBagLocalPosX, CartRelatedData.cartOutBagLocalPosY, CartRelatedData.cartOutBagLocalPosZ));
        }
        else
        {
            SetBasketLocalPosition(new Vector3(CartRelatedData.cartOutBagLocalPosX, CartRelatedData.cartOutBagLocalPosY - 1.0f, CartRelatedData.cartOutBagLocalPosZ));
        }

    }

    private GameObject SerchTag(string tagName)
    {
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
        GameObject targetObj = null; //オブジェクト
        Vector3 myXZ = new Vector3(transform.position.x, 0, transform.position.z);

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {

            Vector3 obsXZ = new Vector3(obs.transform.position.x, 0, obs.transform.position.z);
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obsXZ, myXZ);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;
            }

        }
        return targetObj;
    }

    public Vector3 GetCartPosition()
    {
        return transform.localPosition;
    }

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
    
    public void SetBasketGlobalPosition(Vector3 pos)
    {
        //Debug.Log("iei"+transform.position);

        transform.position = pos;
        //Debug.Log("fe" + transform.position);
    }

    public void SetBasketGlobalRotation(Quaternion angle)
    {
        transform.rotation = angle;
    }

    public void SetBasketLocalPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    public void SetBasketLocalRotation(float angle)
    {
        transform.localRotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
    }
}
