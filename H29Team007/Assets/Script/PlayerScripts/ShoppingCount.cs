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

    [SerializeField, Header("買い物袋のプレハブ")]
    private GameObject bagPrefab;
    [SerializeField, Header("プレイヤーが持つカゴのプレハブ")]
    private GameObject basketPrefab;
    [SerializeField, Header("プレイヤーが飛ばすカートのプレハブ")]
    private GameObject flyBasketPrefab;
    private GameObject basket;

    private Basket basketScript;

    [SerializeField, Header("カートに乗れる最大の値")]
    private int maxCountDefault = 10;
    private int maxCount;

    [SerializeField, Header("スコアUI")]
    private Text score;
    private int childCount = 0;

    private Transform baggageParent;
    private SpringManagerArrange baggageScript;

    private PlayerSE seScript;

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
        baggageParent = basket.transform.Find("nimotuParent");
        baggageScript = baggageParent.GetComponent<SpringManagerArrange>();
        seScript = GetComponent<PlayerSE>();

        BasketOut();
        SetScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.GetState() == Player.PlayerState.Takeover
            || transform.parent != null
            ) return;
        if (basket.activeSelf && Input.GetButtonDown("XboxB") ||
            basket.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            GameObject flyBasket = Instantiate(flyBasketPrefab);

            flyBasket.transform.rotation = basket.transform.rotation;
            if (myBaggege.Count != 0)
            {
                flyBasket.transform.position = basket.transform.position;
                //for (int i = 0; i < myBaggege.Count; i++)
                //{
                    myBaggege[0].parent = flyBasket.transform;
                //}
            }
            Vector3 baspos = basket.transform.position;
            baspos.y = CartRelatedData.cartFlyStartPosY;
            flyBasket.transform.position = baspos;

            basket.SetActive(false);
        }
    }

    public void BasketIn()
    {
        basketScript.SetBasketLocalPosition(CartRelatedData.cartInBagLocalPos);
        basketScript.SetBasketLocalRotation(0);
        basketScript.enabled = false;
    }

    public void BasketOut()
    {
        basketScript.enabled = true;
        basketScript.SetBasketLocalPosition(CartRelatedData.cartOutBagLocalPos);
        basketScript.SetBasketLocalRotation(90);
    }

    public void PlusY(float y)
    {
        onPosition += y;
    }

    public void Reset()
    {
        baggageScript.NullChildren();
        myBaggege.Clear();
        onPosition = 0.0f;
        SetScore();
    }

    public float GetY()
    {
        return basket.transform.position.y + onPosition;
    }

    public float GetCameraLookPoiintPluseY()
    {
        return onPosition / 2;
    }

    public int GetBaggageCount()
    {
        return myBaggege.Count;
    }

    public void BasketActive(bool active)
    {
        basket.SetActive(active);
    }

    public void SetBasketParent(Transform parent)
    {
        basketScript.SetParent(parent);
    }

    public void SetBasketGlobalPos(Vector3 pos)
    {
        basketScript.SetBasketGlobalPosition(pos);
    }

    public void SetBasketLocalPos(Vector3 pos)
    {
        basketScript.SetBasketLocalPosition(pos);
    }

    public void SetBasketAngle(Quaternion angle)
    {
        basketScript.SetBasketGlobalRotation(angle);
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
            if (myBaggege[i].tag == "Plasticbag" || myBaggege[i].tag == "Animal")
            {
                //mybags.Add(myBaggege[i]);
            }
            else
            {
                hc++;
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
        return (humanCount + childCount > animalCount);
    }

    /// <summary>荷物の追加</summary>
    /// <param name="baggege">荷物のTransform</param>
    public void AddBaggege(Transform baggege)
    {
        //baggege.parent = nimotuparent;
        myBaggege.Add(baggege);
        baggageScript.SetChildren(baggege,baggege.GetComponent<RunOverObject>().GetHeight());
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
            //Vector3 nimotuPos = basket.transform.position;
            //nimotuPos.y = GetY();
            //mybags[i].position = nimotuPos;
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
        List<int> bagnums = new List<int>();

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
                bagnums.Add(myBaggege[i].GetComponent<EnemyScore>().GetNumber());
            }
        }

        if (kesumono.Count != 0)
        {

            seScript.OnePlay(5);
            for (int i = 0; i < kesumono.Count; i++)
            {
                Destroy(kesumono[i].gameObject);
            }
            Reset();
            for (int i = 0; i < mybags.Count; i++)
            {
                AddBaggege(mybags[i]);
                //Vector3 nimotuPos = mybags[i].position;
                //nimotuPos.y = GetY();
                //mybags[i].position = nimotuPos;
                PlusY(mybags[i].GetComponent<RunOverObject>().GetHeight());
            }
            GameObject newbag = Instantiate(bagPrefab);

            newbag.GetComponent<EnemyScore>().SetPrice(bagprice);
            newbag.GetComponent<EnemyScore>().SetNumber(bagnums);
            newbag.GetComponent<RunOverObject>().SetPlasticBagPos(basket);
            childCount = 0;
        }
    }

    private void SetScore()
    {
        int goukei = 0;
        ScoreManager.Reset();
        //ここでエネミーからの値段をもらう
        for (int i = 0; i < myBaggege.Count; i++)
        {
            EnemyScore es = myBaggege[i].GetComponent<EnemyScore>();
            if (myBaggege[i].tag == "Plasticbag")
            {
                ScoreManager.AddCount(es.GetNumbers());
            }
            else
            {
                ScoreManager.AddCount(es.GetNumber());
            }

            goukei += es.GetPrice();
        }
        string printscore = goukei.ToString();
        score.text = "￥" + printscore;
    }

    public void PlusChild()
    {
        childCount++;
    }

    public void MinusChild()
    {
        childCount--;
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
