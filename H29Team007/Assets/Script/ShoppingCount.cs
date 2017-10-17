using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCount : MonoBehaviour {

    private Player playerScript;
    private float onPosition;

    private List<Transform> myBaggege;
    private float price = 0;

    public GameObject bag;
    public GameObject basket;
    private int aaaa = 0;//一回だけ呼びたいからのやつ、boolでいいゾ

    // Use this for initialization 
    void Start ()
    {
        playerScript = GetComponent<Player>();
        myBaggege = new List<Transform>();
        onPosition = 0.6f;
    }
	
	// Update is called once per frame
	void Update () {
        if (playerScript.GetCart() == null && aaaa != 0)
        {
            basket.transform.localPosition = new Vector3(-0.09f, 0.4f, 0.65f);
            basket.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
            aaaa--;
        }
        else if(playerScript.GetCart() != null && aaaa == 0)
        {
            basket.transform.localPosition = new Vector3(0, 0.6f, 1.5f);
            basket.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
            aaaa++;
        }
    }

    public void PlusY(float y)
    {
        onPosition += y;
    }

    public void Reset()
    {
        myBaggege.Clear();
        onPosition = 0.6f;
    }

    public float GetY()
    {
        return onPosition;
    }

    /// <summary>荷物の追加</summary>
    /// <param name="baggege">荷物のTransform</param>
    public void AddBaggege(Transform baggege)
    {
        baggege.parent = transform;
        myBaggege.Add(baggege);
    }

    /// <summary>荷物落とすときの処理</summary>
    public void BaggegeFall()
    {
        for (int i = 0; i < myBaggege.Count; i++)
        {
            float x = Random.Range(-3.0f, 3.0f);
            float z = Random.Range(-3.0f, 3.0f);
            float sp = Random.Range(5.0f, 10.0f);

            Vector3 pos = new Vector3(transform.position.x + x, 0, transform.position.z + z);

            FallDown fall = myBaggege[i].GetComponent<FallDown>();
            fall.enabled = true;
            fall.SetPoint(pos, sp);

            myBaggege[i].parent = null;

        }
        Reset();
    }

    /// <summary>レジを通した時の処理</summary>
    /// <param name="cart">袋を入れるカート</param>
    public void PassTheRegister(GameObject cart)
    {
        List<Transform> mybags = new List<Transform>();
        List<Transform> kesumono = new List<Transform>();
        //ここでエネミーからの値段をもらう
        for (int i = 0; i < myBaggege.Count; i++)
        {
            if(myBaggege[i].tag == "Plasticbag")
            {
                mybags.Add(myBaggege[i]);
            }
            else
            {
                kesumono.Add(myBaggege[i]);
            }
        }

        if(kesumono.Count != 0)
        {
          
            for (int i = 0; i < kesumono.Count; i++)
            {
                Destroy(kesumono[i].gameObject);
            }
            Reset();
            for (int i = 0; i < mybags.Count; i++)
            {
                AddBaggege(mybags[i]);
                Vector3 nimotuPos = mybags[i].position;
                nimotuPos.y = GetY();
                mybags[i].position = nimotuPos;
                PlusY(mybags[i].GetComponent<RunOverObject>().GetHeight());
            }
            GameObject newbag = Instantiate(bag);

            newbag.GetComponent<RunOverObject>().SetCartPos(cart);  
        }
    }
}
