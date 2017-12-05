using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCount : MonoBehaviour
{

    private Player playerScript;
    private float onPosition;//いらない可能性大（12/03）

    private List<Transform> myBaggage;
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

    private float flyWaitTime = 0;

    private List<Transform> myBaggage2;
    private SpringManagerArrange baggage2Script;
    

    private PlayerSE seScript;

    // Use this for initialization 
    void Start()
    {
        basket = Instantiate(basketPrefab);
        basket.transform.parent = transform;
        playerScript = GetComponent<Player>();
        myBaggage = new List<Transform>();
        onPosition = 0.0f;
        maxCount = maxCountDefault;
        basketScript = basket.GetComponent<Basket>();
        baggageParent = basket.transform.Find("nimotuParent");
        baggageScript = baggageParent.GetComponent<SpringManagerArrange>();
        seScript = GetComponent<PlayerSE>();

        //2個目
        baggage2Script = transform.Find("SecondBaggage").GetComponent<SpringManagerArrange>();
        myBaggage2 = new List<Transform>();

        BasketOut();
        SetScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.GetState() == Player.PlayerState.Takeover
            || transform.parent != null
            ) return;
        if (basket.activeSelf && Input.GetButton("XboxA") ||
            basket.activeSelf && Input.GetKey(KeyCode.F))
        {
            if (playerScript.IsCart2()) return;
            flyWaitTime += Time.deltaTime;
            if (flyWaitTime < 1.0f) return;
            flyWaitTime = 0.0f;
            GameObject flyBasket = Instantiate(flyBasketPrefab);

            flyBasket.transform.rotation = basket.transform.rotation;
            if (myBaggage.Count != 0)
            {
                flyBasket.transform.position = basket.transform.position;
                //for (int i = 0; i < myBaggege.Count; i++)
                //{
                    myBaggage[0].parent = flyBasket.transform;
                //}
            }
            Vector3 baspos = basket.transform.position;
            baspos.y = CartRelatedData.cartFlyStartPosY;
            flyBasket.transform.position = baspos;

            basket.SetActive(false);
        }
        else
        {
            flyWaitTime = 0.0f;
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
        myBaggage.Clear();
        Reset2();
        onPosition = 0.0f;
        SetScore();
    }

    public void Reset2()
    {
        baggage2Script.NullChildren();
        myBaggage2.Clear();
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
        int result = (myBaggage2.Count > myBaggage.Count) ? myBaggage2.Count : myBaggage.Count;
        return result;
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
    public bool IsBaggegeMax(GameObject cart)
    {
        bool result = false;
        if(cart == playerScript.MyCart())
        {
            result = (myBaggage.Count > maxCount - 1);
        }
        else
        {
            result = (myBaggage2.Count > maxCount - 1);
        }
        return result;
    }

    /// <summary>カゴの中に人が入っているか</summary>
    /// <returns>入っていたらtrue</returns>
    public bool IsBaggegeinHuman()
    {

        int hc = 0;
        for (int i = 0; i < myBaggage.Count; i++)
        {
            if (myBaggage[i].tag == "Plasticbag" || myBaggage[i].tag == "Animal")
            {}
            else
            {
                hc++;
            }
        }
        for (int i = 0; i < myBaggage2.Count; i++)
        {
            if (myBaggage2[i].tag == "Plasticbag" || myBaggage2[i].tag == "Animal")
            {}
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
        for (int i = 0; i < myBaggage.Count; i++)
        {
            if (myBaggage[i].tag == "Plasticbag")
            {
                //mybags.Add(myBaggege[i]);
            }
            else if (myBaggage[i].tag == "Animal")
            {
                animalCount++;
            }
            else
            {
                humanCount++;
            }
        }
        for (int i = 0; i < myBaggage2.Count; i++)
        {
            if (myBaggage2[i].tag == "Plasticbag")
            {
                //mybags.Add(myBaggege[i]);
            }
            else if (myBaggage2[i].tag == "Animal")
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
        myBaggage.Add(baggege);
        baggageScript.SetChildren(baggege,baggege.GetComponent<RunOverObject>().GetHeight());
        SetScore();
    }

    public void AddBaggege(Transform baggege,GameObject cart)
    {
        if (playerScript.MyCart() == cart)
        {
            myBaggage.Add(baggege);
            baggageScript.SetChildren(baggege, baggege.GetComponent<RunOverObject>().GetHeight());
        }
        else
        {
            myBaggage2.Add(baggege);
            baggage2Script.SetChildren(baggege, baggege.GetComponent<RunOverObject>().GetHeight());
        }
        SetScore();
    }

    public void AddBaggege(Transform baggege, GameObject cart,float height)
    {
        if (playerScript.MyCart() == cart)
        {
            myBaggage.Add(baggege);
            baggageScript.SetChildren(baggege, height);
        }
        else
        {
            myBaggage2.Add(baggege);
            baggage2Script.SetChildren(baggege, height);
        }
        SetScore();
    }

    public void BaggegeParentPlayer()
    {
        List<Transform> mybags = new List<Transform>();
        for (int i = 0; i < myBaggage.Count; i++)
        {
            mybags.Add(myBaggage[i]);

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
        for (int i = 0; i < myBaggage.Count; i++)
        {
            float x = Random.Range(-3.0f, 3.0f);
            float z = Random.Range(-3.0f, 3.0f);
            float sp = Random.Range(5.0f, 10.0f);

            //Vector3 pos = new Vector3(transform.position.x + x, 0, transform.position.z + z);
            Vector3 pos = new Vector3(startPos.x + x, 0, startPos.z + z);

            FallDown fall = myBaggage[i].GetComponent<FallDown>();
            fall.enabled = true;
            fall.SetPoint(pos, sp);

            myBaggage[i].parent = null;

        }
        BaggegeFall2(startPos);
        Reset();
    }

    /// <summary>2つ目の荷物落とすときの処理</summary>
    public void BaggegeFall2(Vector3 startPos)
    {
        for (int i = 0; i < myBaggage2.Count; i++)
        {
            float x = Random.Range(-3.0f, 3.0f);
            float z = Random.Range(-3.0f, 3.0f);
            float sp = Random.Range(5.0f, 10.0f);

            //Vector3 pos = new Vector3(transform.position.x + x, 0, transform.position.z + z);
            Vector3 pos = new Vector3(startPos.x + x, 0, startPos.z + z);

            FallDown fall = myBaggage2[i].GetComponent<FallDown>();
            fall.enabled = true;
            fall.SetPoint(pos, sp);

            myBaggage2[i].parent = null;

        }
        Reset2();
    }

    /// <summary>レジを通した時の処理</summary>
    public void PassTheRegister()
    {
        List<Transform> mybags = new List<Transform>();
        List<Transform> kesumono = new List<Transform>();
        int bagprice = 0;
        List<int> bagnums = new List<int>();

        for (int i = 0; i < myBaggage.Count; i++)
        {
            if (myBaggage[i].tag == "Plasticbag")
            {
                mybags.Add(myBaggage[i]);
            }
            else
            {
                kesumono.Add(myBaggage[i]);
                bagprice += myBaggage[i].GetComponent<EnemyScore>().GetPrice();
                bagnums.Add(myBaggage[i].GetComponent<EnemyScore>().GetNumber());
            }
        }

        for (int i = 0; i < myBaggage2.Count; i++)
        {
            if (myBaggage2[i].tag == "Plasticbag")
            {
                mybags.Add(myBaggage2[i]);
            }
            else
            {
                kesumono.Add(myBaggage2[i]);
                bagprice += myBaggage2[i].GetComponent<EnemyScore>().GetPrice();
                bagnums.Add(myBaggage2[i].GetComponent<EnemyScore>().GetNumber());
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
        for (int i = 0; i < myBaggage.Count; i++)
        {
            EnemyScore es = myBaggage[i].GetComponent<EnemyScore>();
            if (myBaggage[i].tag == "Plasticbag")
            {
                ScoreManager.AddCount(es.GetNumbers());
            }
            else
            {
                ScoreManager.AddCount(es.GetNumber());
            }

            goukei += es.GetPrice();
        }
        for (int i = 0; i < myBaggage2.Count; i++)
        {
            EnemyScore es = myBaggage2[i].GetComponent<EnemyScore>();
            if (myBaggage2[i].tag == "Plasticbag")
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
        if (other.tag == "Register")
        {
            PassTheRegister();
        }
    }
}
