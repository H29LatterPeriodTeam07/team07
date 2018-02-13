using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BullCount : MonoBehaviour {

    [SerializeField, Header("入店ポイント入れる")]
    private GameObject m_Entrance;

    private float onPosition;

    private List<Transform> myBaggege;
    private float price = 0;

    public GameObject basket;

    public int maxCountDefault = 1;
    private int maxCount;

    GameObject m_Exitpoint;
    GameObject m_Player;

    // Use this for initialization 
    void Start()
    {
        //  basket = Instantiate(basketPrefab);
        //  basket.transform.parent = transform;
        myBaggege = new List<Transform>();
        onPosition = 0.0f;
        maxCount = maxCountDefault;
        m_Exitpoint = GameObject.FindGameObjectWithTag("ExitPoint");
        m_Player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlusY(float y)
    {
        onPosition += y;
    }

    public void Reset()
    {
        myBaggege.Clear();
        onPosition = 0.0f;
    }

    public float GetY()
    {
        return basket.transform.position.y + onPosition;
    }

    public void BasketActive(bool active)
    {
        basket.SetActive(active);
    }

    /// <summary>プレイヤーが持っているカゴのあたり判定のactive</summary>
    /// <param name="active">あたり判定を有効にするかどうか</param>
    public void SetBasketColliderActive(bool active)
    {
        basket.GetComponent<BoxCollider>().enabled = active;
    }


    /// <summary>プレイヤーが持っているカゴがactiveか</summary>
    /// <returns>activeならtrue</returns>
    public bool IsCatchBasket()
    {
        return basket.activeSelf;
    }

    /// <summary>カゴにのる量が最大以上か</summary>
    /// <returns>ぴったり、または乗りすぎていたらtrue</returns>
    public bool IsBaggegeMax()
    {
        return (myBaggege.Count > maxCount - 1);
    }

    /// <summary>カゴの中に人が入っているか</summary>
    /// <returns>入っていたらtrue</returns>
    public bool IsBaggegeinHuman()
    {
        //List<Transform> mybags = new List<Transform>();
        //List<Transform> kesumono = new List<Transform>();
        int hc = 0;
        for (int i = 0; i < myBaggege.Count; i++)
        {
            if (myBaggege[i].tag == "Player")
            {
                hc++;
            }
            else
            {
                //mybags.Add(myBaggege[i]);
            }
        }
        return (hc > 0);
    }

    public bool IsHumanMoreThanAnimal()
    {
        int humanCount = 1;
        int animalCount = 0;
        for (int i = 0; i < myBaggege.Count; i++)
        {
            if (myBaggege[i].tag == "Plasticbag")
            {
                //mybags.Add(myBaggege[i]);
            }
            else if (myBaggege[i].tag == "Animal")
            {
                animalCount++;
            }
            else
            {
                humanCount++;
            }
        }
        return (humanCount > animalCount);
    }

    /// <summary>荷物の追加</summary>
    /// <param name="baggege">荷物のTransform</param>
    public void AddBaggege(Transform baggege)
    {
        baggege.parent = basket.transform;
        myBaggege.Add(baggege);
    }

    public void BaggegeParentPlayer()
    {
        List<Transform> mybags = new List<Transform>();
        for (int i = 0; i < myBaggege.Count; i++)
        {
            mybags.Add(myBaggege[i]);

        }


        Reset();
        for (int i = 0; i < mybags.Count; i++)
        {
            AddBaggege(mybags[i]);
            Vector3 nimotuPos = basket.transform.position;
            nimotuPos.y = GetY();
            mybags[i].position = nimotuPos;
            PlusY(mybags[i].GetComponent<RunOverObject>().GetHeight());
        }
        //GameObject newbag = Instantiate(bagPrefab);

        //newbag.GetComponent<RunOverObject>().SetPlasticBagPos(basket);

    }

    /// <summary>荷物落とすときの処理</summary>
    public void BaggegeFall(Vector3 startPos) 
    {
        for (int i = 0; i < myBaggege.Count; i++)
        {
            if (myBaggege[i].tag == "Player")
            {

                m_Player.transform.position = new Vector3(startPos.x, 0, startPos.z);
                Rigidbody rb = m_Player.GetComponent<Rigidbody>();
                NavMeshAgent nav = m_Player.GetComponent<NavMeshAgent>();
                Collider m_coll = m_Player.GetComponent<Collider>();
                m_coll.enabled=true;
                rb.velocity = Vector3.one;
                rb.isKinematic = false;
                myBaggege[i].parent = null;
            }
            /*float x = Random.Range(-3.0f, 3.0f);
            float z = Random.Range(-3.0f, 3.0f);
            float sp = Random.Range(5.0f, 10.0f);

            //Vector3 pos = new Vector3(transform.position.x + x, 0, transform.position.z + z);
            Vector3 pos = new Vector3(startPos.x + x, 0, startPos.z + z);

            FallDown fall = myBaggege[i].GetComponent<FallDown>();
            fall.enabled = true;
            fall.SetPoint(pos, sp);

            myBaggege[i].parent.parent = null;*/

        }
        Reset();
    }
    public void BaggegeFall2(Vector3 startPos)
    {
        for (int i = 0; i < myBaggege.Count; i++)
        {
            float x = Random.Range(-3.0f, 3.0f);
            float z = Random.Range(-3.0f, 3.0f);
            float sp = Random.Range(5.0f, 10.0f);

            Vector3 pos = new Vector3(startPos.x + x, 0, startPos.z + z);

            FallDown fall = myBaggege[i].GetComponent<FallDown>();
            fall.enabled = true;
            fall.SetPoint(pos, sp);

            myBaggege[i].parent.parent = null;

        }
        Reset();
    }

    /// <summary>レジを通した時の処理</summary>
    public void PassTheRegister()
    {
        List<Transform> mybags = new List<Transform>();
        List<Transform> kesumono = new List<Transform>();

        for (int i = 0; i < myBaggege.Count; i++)
        {
            if (myBaggege[i].tag == "Plasticbag")
            {
                mybags.Add(myBaggege[i]);
            }
            else
            {
                kesumono.Add(myBaggege[i]);
            }
        }

        if (kesumono.Count != 0)
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
        }
    }
}