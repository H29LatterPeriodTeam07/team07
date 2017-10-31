using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCount : MonoBehaviour
{

    private Player playerScript;
    private float onPosition;

    private List<Transform> myBaggege;
    private float price = 0;

    public GameObject bagPrefab;
    public GameObject basketPrefab;
    public GameObject flyBasketPrefab;
    private GameObject basket;

    private Basket basketScript;

    public int maxCountDefault = 10;
    private int maxCount;

    public Text score;

    // Use this for initialization 
    void Start()
    {
        basket = Instantiate(basketPrefab);
        basket.transform.parent = transform;
        playerScript = GetComponent<Player>();
        myBaggege = new List<Transform>();
        onPosition = 0.0f;
        maxCount = maxCountDefault;
        basketScript = basket.GetComponent<Basket>();
        BasketOut();
        SetScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.GetState() == Player.PlayerState.Takeover) return;
        if (basket.activeSelf && Input.GetButtonDown("XboxB") ||
            basket.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            GameObject flyBasket = Instantiate(flyBasketPrefab);

            flyBasket.transform.rotation = basket.transform.rotation;
            if (myBaggege.Count != 0)
            {
                flyBasket.transform.position = basket.transform.position;
                for (int i = 0; i < myBaggege.Count; i++)
                {
                    myBaggege[i].parent = flyBasket.transform;
                }
            }
            Vector3 baspos = basket.transform.position;
            baspos.y = 1.6f;
            flyBasket.transform.position = baspos;

            basket.SetActive(false);
        }
    }

    public void BasketIn()
    {
        basketScript.SetBasketLocalPosition(new Vector3(0, 0.6f, 1.5f));
        basketScript.SetBasketLocalRotation(0);
        basketScript.enabled = false;
    }

    public void BasketOut()
    {
        basketScript.enabled = true;
        basketScript.SetBasketLocalPosition(new Vector3(-0.09f, 1.4f, 0.65f));
        basketScript.SetBasketLocalRotation(90);
    }

    public void PlusY(float y)
    {
        onPosition += y;
    }

    public void Reset()
    {
        myBaggege.Clear();
        onPosition = 0.0f;
        SetScore();
    }

    public float GetY()
    {
        return basket.transform.position.y + onPosition;
    }

    public void BasketActive(bool active)
    {
        basket.SetActive(active);
    }

    public void SetBasketParent(Transform parent)
    {
        basketScript.SetParent(parent);
    }

    public void SetBasketPos(Vector3 pos)
    {
        basketScript.SetBasketGlobalPosition(pos);
    }

    public void SetBasketAngle(Quaternion angle)
    {
        basketScript.SetBasketGlobalRotation(angle);
    }

    public bool IsCatchBasket()
    {
        return basket.activeSelf;
    }

    public bool IsBaggegeMax()
    {
        return (myBaggege.Count > maxCount - 1);
    }

    public bool IsBaggegeinHuman()
    {
        //List<Transform> mybags = new List<Transform>();
        List<Transform> kesumono = new List<Transform>();
        for (int i = 0; i < myBaggege.Count; i++)
        {
            if (myBaggege[i].tag == "Plasticbag" || myBaggege[i].tag == "Animal")
            {
                //mybags.Add(myBaggege[i]);
            }
            else
            {
                kesumono.Add(myBaggege[i]);
            }
        }
        return (kesumono.Count > 0);
    }

    /// <summary>荷物の追加</summary>
    /// <param name="baggege">荷物のTransform</param>
    public void AddBaggege(Transform baggege)
    {
        baggege.parent = basket.transform;
        myBaggege.Add(baggege);
        SetScore();
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
            float x = Random.Range(-3.0f, 3.0f);
            float z = Random.Range(-3.0f, 3.0f);
            float sp = Random.Range(5.0f, 10.0f);

            //Vector3 pos = new Vector3(transform.position.x + x, 0, transform.position.z + z);
            Vector3 pos = new Vector3(startPos.x + x, 0, startPos.z + z);

            FallDown fall = myBaggege[i].GetComponent<FallDown>();
            fall.enabled = true;
            fall.SetPoint(pos, sp);

            myBaggege[i].parent = null;

        }
        Reset();
    }

    /// <summary>レジを通した時の処理</summary>
    public void PassTheRegister()
    {
        List<Transform> mybags = new List<Transform>();
        List<Transform> kesumono = new List<Transform>();
        int bagprice = 0;

        for (int i = 0; i < myBaggege.Count; i++)
        {
            if (myBaggege[i].tag == "Plasticbag")
            {
                mybags.Add(myBaggege[i]);
            }
            else
            {
                kesumono.Add(myBaggege[i]);
                bagprice += myBaggege[i].GetComponent<EnemyScore>().GetPrice();
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
            GameObject newbag = Instantiate(bagPrefab);

            newbag.GetComponent<EnemyScore>().SetPrice(bagprice);
            newbag.GetComponent<RunOverObject>().SetPlasticBagPos(basket);
        }
    }

    private void SetScore()
    {
        int goukei = 0;
        //ここでエネミーからの値段をもらう
        for (int i = 0; i < myBaggege.Count; i++)
        {
            goukei += myBaggege[i].GetComponent<EnemyScore>().GetPrice();
        }
        string printscore = goukei.ToString();
        score.text = "￥" + printscore;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "FlyBasket(Clone)")
        {
            other.transform.position = basket.transform.position;
            other.transform.rotation = basket.transform.rotation;
            Destroy(other.gameObject);
            basket.SetActive(true);
        }
    }
}
