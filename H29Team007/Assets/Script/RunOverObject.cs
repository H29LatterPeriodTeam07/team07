using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunOverObject : MonoBehaviour
{
    [SerializeField, Header("カートに乗った後の高さ")]
    private float runOverAfterHeight = 1.0f;

    private NavMeshAgent myNav;

    // Use this for initialization
    void Start()
    {
        myNav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NavReStart()
    {
        myNav.enabled = true;
        //ここにアニメ再開入れるかも
    }

    /// <summary>navmeshのポジション移動</summary>
    /// <param name="pos">移動先のポイント</param>
    public void NavPosition(Vector3 pos)
    {
        myNav.Warp(pos); //navmeshのポジション移動
    }

    /// <summary>レジを通したレジ袋用の関数</summary>
    /// <param name="cart">袋を入れるカート</param>
    public void SetCartPos(GameObject cart)
    {
        //myNav.enabled = false;

        var sc = cart.transform.root.GetComponent<ShoppingCount>();
        Vector3 v = cart.transform.position;
        Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
        transform.position = nimotuPos;
        sc.AddBaggege(transform);

        sc.PlusY(runOverAfterHeight);
    }

    public float GetHeight()
    {
        return runOverAfterHeight;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "FrontHitArea")
        {
            myNav.enabled = false;
            //ここにアニメ停止や変更入れるかも
            var sc = other.transform.root.GetComponent<ShoppingCount>();
            Vector3 v = other.transform.parent.transform.position;
            Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
            transform.position = nimotuPos;
            sc.AddBaggege(transform);
            //transform.parent = other.transform.root;

            sc.PlusY(runOverAfterHeight);
        }
    }
}
