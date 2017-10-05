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

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.root.name);

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
