using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunOverObject : MonoBehaviour
{
    [SerializeField, Header("カートに乗った後の高さ")]
    private float runOverAfterHeight = 1.0f;

    private NavMeshAgent myNav;
    private BoxCollider myCollider;

    // Use this for initialization
    void Start()
    {
        myNav = GetComponent<NavMeshAgent>();
        myCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NavReStart()
    {
        myNav.enabled = true;
        myCollider.enabled = true;
        //ここにアニメ再開入れるかも
    }

    /// <summary>navmeshのポジション移動</summary>
    /// <param name="pos">移動先のポイント</param>
    public void NavPosition(Vector3 pos)
    {
        myNav.Warp(pos); //navmeshのポジション移動
    }

    /// <summary>レジを通したレジ袋用の関数</summary>
    /// <param name="basket">袋を入れるカゴ</param>
    public void SetPlasticBagPos(GameObject basket)
    {
        //myNav.enabled = false;

        var sc = basket.transform.root.GetComponent<ShoppingCount>();
        //myCollider = GetComponent<BoxCollider>();
        //myCollider.enabled = false;
        Vector3 v = basket.transform.position;
        Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
        transform.position = nimotuPos;
        sc.AddBaggege(transform);

        sc.PlusY(runOverAfterHeight);
    }

    public float GetHeight()
    {
        return runOverAfterHeight;
    }

    /// <summary>エネミーのプレイヤーが見えてるかのパクリのパクリ</summary>
    private bool CanGetEnemy(Transform cart)
    {
        //カートからエネミーへの方向ベクトル(ワールド座標系)
        Vector3 directionToEnemy = transform.position - cart.position;
        // エネミーの正面向きベクトルとエネミーへの方向ベクトルの差分角度
        float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

        // 引ける角度の範囲内にエネミーがいるかどうかを返却する
        return (Mathf.Abs(angleToEnemy) <= 90);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "FrontHitArea")//プレイヤーババア用　敵ババアが特売品を轢く処理は頑張って
        {
            if (transform.tag == "Enemy" && !CanGetEnemy(other.transform)) return;
            var sc = other.transform.root.GetComponent<ShoppingCount>();
            if (!sc.IsCatchBasket() || sc.IsBaggegeMax()) return;
            myNav.enabled = false;
            myCollider.enabled = false;  //荷物のあたり判定のせいでカート増えてたあばばばばば 敵全部ボックスコライダーでありがと
            //ここにアニメ停止や変更入れるかも
            Vector3 v = other.transform.parent.transform.position;
            Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
            transform.position = nimotuPos;
            sc.AddBaggege(transform);
            //transform.parent = other.transform.root;

            sc.PlusY(runOverAfterHeight);
        }
        if (other.name == "EnemyFrontHitArea")//敵ババア用
        {
            if (transform.tag == "Animal" && !CanGetEnemy(other.transform)) return;
            var sc = other.transform.root.GetComponent<ShoppingCount>();
            if (!sc.IsCatchBasket() || sc.IsBaggegeMax()) return;
            myNav.enabled = false;
            myCollider.enabled = false;
            Vector3 v = other.transform.parent.transform.position;
            Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
            transform.position = nimotuPos;
            sc.AddBaggege(transform);

            sc.PlusY(runOverAfterHeight);
        }
    }
}
