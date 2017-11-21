using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullBasket : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
}

