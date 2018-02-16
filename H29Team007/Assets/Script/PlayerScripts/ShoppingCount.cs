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
    private int humanCount = 1;
    private int animalCount = 0;

    //[SerializeField, Header("スコアUI")]
    private Text score;
    private GameObject scoreUI;
    [SerializeField, Header("コイン管理プレハブ")]
    private GameObject coinManagerPrefab;
    [SerializeField, Header("ポプアップスコアプレハブ")]
    private GameObject popscorePrefab;

    private int childCount = 0;

    private Transform baggageParent;
    private SpringManagerArrange baggageScript;

    //private float flyWaitTime = 0;

    private List<Transform> myBaggage2;
    private SpringManagerArrange baggage2Script;

    private float baggageLimitAngle = 90.0f;

    private PlayerSE seScript;

    private bool rightpush = false;           //　最初に移動ボタンを押したかどうか
    private bool leftpush = false;           //　最初に移動ボタンを押したかどうか
    private float nextButtonDownTime = 0.5f;    //　次に移動ボタンが押されるまでの時間
    private float rightnowTime = 0f;			//　最初に移動ボタンが押されてからの経過時間
    private float leftnowTime = 0f;			//　最初に移動ボタンが押されてからの経過時間
    private bool canpushLR2 = true;  //LR2を反応させていいかどうか

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
        scoreUI = GameObject.Find("Score");
        score = scoreUI.GetComponent<Text>();
        baggageParent = basket.transform.Find("nimotuParent");
        baggageScript = baggageParent.GetComponent<SpringManagerArrange>();
        seScript = GetComponent<PlayerSE>();

        //2個目
        baggage2Script = transform.Find("SecondBaggage").GetComponent<SpringManagerArrange>();
        myBaggage2 = new List<Transform>();

        BasketOut();
        SetScore();
        BasketActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.GetState() >= Player.PlayerState.Takeover
            || transform.parent != null
            || !MainGameDate.IsStart()
            ) return;
        if (basket.activeSelf && Input.GetButtonDown("XboxX") ||
            basket.activeSelf && Input.GetKey(KeyCode.F))
        {
            if (playerScript.IsCart2()) return;
            //flyWaitTime += Time.deltaTime;
            //if (flyWaitTime < 1.0f) return;
            //flyWaitTime = 0.0f;
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
            baspos.y = CartRelatedData.flyBasketStartPosY;
            flyBasket.transform.position = baspos;
            playerScript.ChangeState(7);

            basket.SetActive(false);
        }
        //else
        //{
        //    flyWaitTime = 0.0f;
        //}

        TopFallInput();
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
        if (cart == playerScript.MyCart())
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
            if (myBaggage[i].tag == "Plasticbag" || myBaggage[i].tag == "Animal" || myBaggage[i].tag == "Bull")
            { }
            else
            {
                hc++;
            }
        }
        for (int i = 0; i < myBaggage2.Count; i++)
        {
            if (myBaggage2[i].tag == "Plasticbag" || myBaggage2[i].tag == "Animal" || myBaggage2[i].tag == "Bull")
            { }
            else
            {
                hc++;
            }
        }
        return (hc > 0);
    }

    public bool IsHumanMoreThanAnimal()
    {

        return (GetHumanCount() > animalCount);
    }

    /// <summary>荷物の追加</summary>
    /// <param name="baggege">荷物のTransform</param>
    public void AddBaggege(Transform baggege)
    {
        myBaggage.Add(baggege);
        //baggege.eulerAngles = playerScript.MyCart().transform.eulerAngles;
        baggege.eulerAngles = basket.transform.eulerAngles;
        baggageScript.SetChildren(baggege, baggege.GetComponent<RunOverObject>().GetHeight());
        SetScore();
    }

    public void AddBaggege(Transform baggege, GameObject cart)
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

    public void AddBaggege(Transform baggege, GameObject cart, float height)
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

    private void TopBaggegeFall(int n = 1)
    {
        List<Transform> mybags = new List<Transform>();
        List<Transform> mybags2 = new List<Transform>();
        if (n == 1)
        {
            if (myBaggage.Count < 1) return;
            for (int i = 0; i < myBaggage.Count - 1; i++)
            {
                mybags.Add(myBaggage[i]);
            }
            for (int i = 0; i < myBaggage2.Count; i++)
            {
                mybags2.Add(myBaggage2[i]);
            }

            float x = Random.Range(1.0f, 3.0f);
            float z = Random.Range(-3.0f, -1.0f);
            float sp = Random.Range(5.0f, 10.0f);

            Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z) + transform.right * x + transform.forward * z;

            FallDown fall = myBaggage[myBaggage.Count - 1].GetComponent<FallDown>();
            fall.enabled = true;
            fall.SetPoint(pos, sp);

            myBaggage[myBaggage.Count - 1].parent = null;
        }
        else
        {
            if (myBaggage2.Count < 1) return;
            for (int i = 0; i < myBaggage.Count; i++)
            {
                mybags.Add(myBaggage[i]);
            }
            for (int i = 0; i < myBaggage2.Count - 1; i++)
            {
                mybags2.Add(myBaggage2[i]);
            }

            float x = Random.Range(-1.0f, -3.0f);
            float z = Random.Range(-3.0f, -1.0f);
            float sp = Random.Range(5.0f, 10.0f);

            Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z) + transform.right * x + transform.forward * z;

            FallDown fall = myBaggage2[myBaggage2.Count - 1].GetComponent<FallDown>();
            fall.enabled = true;
            fall.SetPoint(pos, sp);

            myBaggage2[myBaggage2.Count - 1].parent = null;
        }




        Reset();
        for (int i = 0; i < mybags.Count; i++)
        {
            AddBaggege(mybags[i]);
            PlusY(mybags[i].GetComponent<RunOverObject>().GetHeight());
        }
        for (int i = 0; i < mybags2.Count; i++)
        {
            AddBaggege(mybags2[i], playerScript.MySecondCart());
        }

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
        int bagPoint = 0;
        List<string> bagnames = new List<string>();

        int scorecount = 1;

        //一個目のカートのスコア計算
        for (int i = 0; i < myBaggage.Count; i++)
        {
            if (myBaggage[i].tag == "Plasticbag")
            {
                mybags.Add(myBaggage[i]);
            }
            else
            {
                kesumono.Add(myBaggage[i]);
                int enemyscore = myBaggage[i].GetComponent<EnemyScore>().GetPrice();
                bagprice += enemyscore;
                int enemyPoint = myBaggage[i].GetComponent<EnemyScore>().GetPoint();
                bagPoint += enemyPoint;
                bagnames.Add(myBaggage[i].name);

                GameObject popscore = Instantiate(popscorePrefab);
                PopupScore2D popscoreScript = popscore.GetComponent<PopupScore2D>();
                //popscoreScript.SetPositionAndRotation(myBaggage[i].position + transform.right * 2, Camera.main.transform.eulerAngles.y);
                popscoreScript.SetText("＋" + StringWidthConverter.ConvertToFullWidth(enemyPoint.ToString() + "pt"));
                popscoreScript.transform.SetParent(score.transform);
                popscoreScript.SetTarget(scorecount);
                scorecount++;
            }
        }
        //2個目のカートのスコア計算
        for (int i = 0; i < myBaggage2.Count; i++)
        {
            if (myBaggage2[i].tag == "Plasticbag")
            {
                mybags.Add(myBaggage2[i]);
            }
            else
            {
                kesumono.Add(myBaggage2[i]);
                int enemyscore = myBaggage2[i].GetComponent<EnemyScore>().GetPrice();
                bagprice += enemyscore;
                int enemyPoint = myBaggage2[i].GetComponent<EnemyScore>().GetPoint();
                bagPoint += enemyPoint;
                bagnames.Add(myBaggage2[i].name);

                GameObject popscore = Instantiate(popscorePrefab);
                PopupScore2D popscoreScript = popscore.GetComponent<PopupScore2D>();
                //popscoreScript.SetPositionAndRotation(myBaggage2[i].position + transform.right * 2, Camera.main.transform.eulerAngles.y);
                popscoreScript.SetText("＋" + StringWidthConverter.ConvertToFullWidth(enemyPoint.ToString()) + "pt");
                popscoreScript.transform.SetParent(score.transform);
                popscoreScript.SetTarget(scorecount);
                scorecount++;
            }
        }

        //1個目のカートのパターン
        if (myBaggage.Count >= 3)
        {
            for (int i = 0; i < myBaggage.Count - 2; i++)
            {
                //int num = Pattern.PatternNumber(myBaggage[i], myBaggage[i + 1], myBaggage[i + 2]);
                string[] patternNames = { myBaggage[i].name, myBaggage[i + 1].name, myBaggage[i + 2].name };
                ScoreManager.PatternData l_data = ScoreManager.GetEnemyPatternData(patternNames);
                if (l_data.PatternName != "None")
                {
                    string patternname = l_data.PatternName;
                    int patternpoint = ScoreManager.GetPatternPoint(l_data);
                    bagPoint += patternpoint;
                    bagnames.Add(patternname);
                    GameObject popscore = Instantiate(popscorePrefab);
                    PopupScore2D popscoreScript = popscore.GetComponent<PopupScore2D>();
                    //popscoreScript.SetPositionAndRotation(myBaggage[i + 1].position + transform.right * 2, Camera.main.transform.eulerAngles.y);
                    popscoreScript.SetOutColorOrange();
                    popscoreScript.SetText(patternname + "＋" + StringWidthConverter.ConvertToFullWidth(patternpoint.ToString() + "Pt"));
                    popscoreScript.transform.SetParent(score.transform);
                    popscoreScript.SetTarget(scorecount);

                    // resultで使うデータ追加
                    ScoreManager.PatternResultData l_resultData;
                    l_resultData.PatternName = l_data.PatternName;
                    l_resultData.nameList = l_data.PatternList;
                    l_resultData.point = patternpoint;
                    ScoreManager.AddResultPatternData(l_resultData);
                    scorecount++;
                    i += 2;
                }
            }
        }

        //2個目のスコアのパターン
        if (myBaggage2.Count >= 3)
        {
            for (int i = 0; i < myBaggage2.Count - 2; i++)
            {
                //int num = Pattern.PatternNumber(myBaggage2[i], myBaggage2[i + 1], myBaggage2[i + 2]);
                int num = Pattern.PatternNumber(myBaggage2[i], myBaggage2[i + 1], myBaggage2[i + 2]);
                string[] patternNames = { myBaggage2[i].name, myBaggage2[i + 1].name, myBaggage2[i + 2].name };
                ScoreManager.PatternData l_data = ScoreManager.GetEnemyPatternData(patternNames);
                if (l_data.PatternName != "None")
                {

                    string patternname = l_data.PatternName;
                    int patternpoint = ScoreManager.GetPatternPoint(l_data);
                    bagPoint += patternpoint;
                    bagnames.Add(patternname);
                    GameObject popscore = Instantiate(popscorePrefab);
                    PopupScore2D popscoreScript = popscore.GetComponent<PopupScore2D>();
                    //popscoreScript.SetPositionAndRotation(myBaggage[i + 1].position + transform.right * 2, Camera.main.transform.eulerAngles.y);
                    popscoreScript.SetOutColorOrange();
                    popscoreScript.SetText(patternname + "＋" + StringWidthConverter.ConvertToFullWidth(patternpoint.ToString() + "Pt"));
                    popscoreScript.transform.SetParent(score.transform);
                    popscoreScript.SetTarget(scorecount);
                    scorecount++;
                    i += 2;
                }
            }
            //if (num != 0)
            //    {
            //        string patternname = PatternScore.PatternText(num);
            //        int patternpoint = ScoreManager.EnemyPrice(patternname);
            //        bagPoint += patternpoint;
            //        bagnames.Add(patternname);
            //        GameObject popscore = Instantiate(popscorePrefab);
            //        PopupScore2D popscoreScript = popscore.GetComponent<PopupScore2D>();
            //        //popscoreScript.SetPositionAndRotation(myBaggage[i + 1].position + transform.right * 2, Camera.main.transform.eulerAngles.y);
            //        popscoreScript.SetOutColorOrange();
            //        popscoreScript.SetText(patternname + "＋" + StringWidthConverter.ConvertToFullWidth(patternpoint.ToString() + "Pt"));
            //        popscoreScript.transform.SetParent(score.transform);
            //        popscoreScript.SetTarget(scorecount);
            //        scorecount++;
            //        i += 2;
            //    }
            //}
        }

        if (kesumono.Count != 0)
        {


            seScript.OnePlay(5);
            seScript.OnePlay2(9);
            for (int i = 0; i < kesumono.Count; i++)
            {
                if (kesumono[i].Find("YoungestChild") != null) { Transform a = kesumono[i].Find("YoungestChild"); a.parent = transform; a.localPosition = Vector3.zero; }
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

            Vector2 l_initPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            coinManagerPrefab.GetComponent<CoinManager>().SetInitPosition(l_initPosition);
            coinManagerPrefab.GetComponent<CoinManager>().SetCreateCoinCount(bagPoint);
            coinManagerPrefab.GetComponent<CoinManager>().CreateCoin();

            newbag.GetComponent<EnemyScore>().SetPrice(bagprice);
            newbag.GetComponent<EnemyScore>().SetPoint(bagPoint);
            newbag.GetComponent<EnemyScore>().SetNames(bagnames);
            newbag.GetComponent<RunOverObject>().SetPlasticBagPos(basket);
            childCount = 0;
        }
    }

    private string ScoreString()
    {
        string smoji = "";
        return smoji;
    }

    public void DeleteBaggege(Transform kesumono)
    {
        List<Transform> mybags = new List<Transform>();
        List<Transform> mybags2 = new List<Transform>();

        for (int i = 0; i < myBaggage.Count; i++)
        {
            if (myBaggage[i] != kesumono)
            {
                mybags.Add(myBaggage[i]);
            }
        }

        for (int i = 0; i < myBaggage2.Count; i++)
        {
            if (myBaggage2[i] != kesumono)
            {
                mybags2.Add(myBaggage2[i]);
            }
        }

        if (kesumono.tag == "Parent") childCount--;

        Reset();
        Destroy(kesumono.gameObject);
        for (int i = 0; i < mybags.Count; i++)
        {
            AddBaggege(mybags[i]);
            PlusY(mybags[i].GetComponent<RunOverObject>().GetHeight());
        }
        for (int i = 0; i < mybags2.Count; i++)
        {
            AddBaggege(mybags2[i], playerScript.MySecondCart());
        }

    }

    private void TopFallInput()
    {
        if (!canpushLR2 && -0.5f <= Input.GetAxisRaw("XboxLR2") && Input.GetAxisRaw("XboxLR2") <= 0.5f) canpushLR2 = true;

        if (Input.GetAxisRaw("XboxLR2") < -0.5f || Input.GetKeyDown(KeyCode.L))
        {
            if (!rightpush)
            {
                rightpush = true;
                rightnowTime = 0.0f;
            }
            else if (canpushLR2)
            {
                if (Input.GetAxisRaw("XboxLR2") < -0.5f) canpushLR2 = false;
                rightpush = false;
                TopBaggegeFall();
            }
        }
        if (rightpush)
        {
            rightnowTime += Time.deltaTime;

            if (rightnowTime > nextButtonDownTime)
            {
                rightpush = false;
            }
        }

        if (Input.GetAxisRaw("XboxLR2") > 0.5f || Input.GetKeyDown(KeyCode.K))
        {
            if (!leftpush)
            {
                leftpush = true;
                leftnowTime = 0.0f;
            }
            else if(canpushLR2)
            {
                if (Input.GetAxisRaw("XboxLR2") > 0.5f) canpushLR2 = false;
                leftpush = false;
                TopBaggegeFall(2);
            }
        }
        if (leftpush)
        {
            leftnowTime += Time.deltaTime;

            if (leftnowTime > nextButtonDownTime)
            {
                leftpush = false;
            }
        }
    }

    /// <summary>スコアの決定</summary>
    private void SetScore()
    {
        InCount();

        int goukei = 0;
        ScoreManager.Reset();
        //ここでエネミーからの値段をもらう
        for (int i = 0; i < myBaggage.Count; i++)
        {
            EnemyScore es = myBaggage[i].GetComponent<EnemyScore>();
            if (myBaggage[i].tag == "Plasticbag")
            {
                ScoreManager.AddCount(es.GetNames());
            }
            else
            {
                //ScoreManager.AddCount(es.GetNumber());
                ScoreManager.AddCount(myBaggage[i].name);
            }

            goukei += es.GetPoint();
        }
        for (int i = 0; i < myBaggage2.Count; i++)
        {
            EnemyScore es = myBaggage2[i].GetComponent<EnemyScore>();
            if (myBaggage2[i].tag == "Plasticbag")
            {
                ScoreManager.AddCount(es.GetNames());
            }
            else
            {
                //ScoreManager.AddCount(es.GetNumber());
                ScoreManager.AddCount(myBaggage2[i].name);
            }

            goukei += es.GetPoint();
        }
        string printscore = goukei.ToString() + "pt";
        score.text = printscore;
    }

    /// <summary>籠に入っているもののカウント</summary>
    private void InCount()
    {
        humanCount = 1;
        animalCount = 0;
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
            else if (myBaggage2[i].tag == "Animal" || myBaggage2[i].tag == "Bull")
            {
                animalCount++;
            }
            else
            {
                humanCount++;
            }
        }
    }

    public void PlusChild()
    {
        childCount++;
    }

    public void MinusChild()
    {
        childCount--;
    }

    public int GetHumanCount()
    {
        return (humanCount + childCount);
    }

    public void SetBaggageLimitAngle(float angle)
    {
        baggageLimitAngle = angle;
    }

    public float GetBaggageLimitAngle()
    {
        return baggageLimitAngle;
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

