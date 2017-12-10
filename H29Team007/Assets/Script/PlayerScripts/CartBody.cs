using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBody : MonoBehaviour
{

    public float wallDamage = 5.0f;
    
    private GameObject rotatepoint;
    //private InclinationOfLuggage pointScr;

    private Vector3 willyPoint = CartRelatedData.cartRotatePointBack;
    private Vector3 motiagePoint = CartRelatedData.cartRotatePointFront;

    private bool isR = false;

    private bool isWilly = false;

    //[SerializeField, Header("カート持っていない時の速さ")]
    private float willTime = 0.3f;
    private float nowTime = 0.0f;

    private float moderuAngle = 0.0f;

    public Material myColor;

    private float alphaTime = 0.0f;
    private int alphaPlus = 1;

    private GameObject AllHP;
    private GameObject wheel1;
    private GameObject wheel2;
    private GameObject bagUnder;
    private GameObject handle;


    public GameObject[] effects;

    private Transform secondCartBaggageParent;


    //CartStatusWithPlayお引越し
    private Player playerScript;
    private ShoppingCount scScript;

    private float[] cartStatus;
    //private float[] cartStatus2;


    [SerializeField, Header("カートのタイヤが壊れた時に毎フレーム下げる本体の耐久度")]
    private float minusCartHP = 0.01f;
    [SerializeField, Header("カートのハンドルが壊れた時に下げる回転速度")]
    private float minusRotateSpeedDefault = 60;
    [SerializeField, Header("カートの荷台が壊れた時の荷物の傾きの限界")]
    private float baggageRotateLimit = 60;


    [SerializeField, Header("デフォの荷物の傾きの限界")]
    private float baggageRotateLimitDefault = 90;

    private PlayerSE seScript;

    // Use this for initialization
    void Start()
    {
        //cs = transform.root.GetComponent<CartStatusWithPlayer>();
        rotatepoint = transform.root.Find("cartrotatepoint").gameObject;
        AllHP = transform.Find("AllHPCart").gameObject;
        wheel1 = transform.Find("WheelHP1").gameObject;
        wheel2 = transform.Find("WheelHP2").gameObject;
        bagUnder = transform.Find("BagUnderHP").gameObject;
        handle = transform.Find("HandleHP").gameObject;
        //pointScr = transform.root.Find("PlayerBasket(Clone)").Find("nimotuParent").GetComponent<InclinationOfLuggage>();

        //お引越し
        playerScript = transform.root.GetComponent<Player>();
        scScript = transform.root.GetComponent<ShoppingCount>();

        //cartStatus = new float[4];
        //cartStatus2 = new float[4];

        seScript = transform.root.GetComponent<PlayerSE>();

        if(playerScript.MyCart() != gameObject)
        {
            secondCartBaggageParent = transform.root.Find("SecondBaggage");
        }

    }

    // Update is called once per frame
    void Update()
    {
        //お引越し--
        if (cartStatus[2] <= 0)
        {
            cartStatus[0] -= minusCartHP;
        }
        if(playerScript.MyCart() == gameObject)
        {
            scScript.SetBaggageLimitAngle(BaggageRotateLimit());
        }
        //--

        if (isWilly)
        {
            Willy();
        }
        else
        {
            Normal();
        }
        if(alphaTime > 1 || alphaTime < 0)
        {            
            alphaPlus *= -1;
        }
        alphaTime = Mathf.Clamp(alphaTime, 0, 1);

        CartHPColor();
        alphaTime += Time.deltaTime * alphaPlus;

        if(cartStatus[0] <= 30)
        {
            AllHP.SetActive(true);
        }
        wheel1.SetActive(cartStatus[1] <= 0);
        wheel2.SetActive(cartStatus[1] <= 0);
        bagUnder.SetActive(cartStatus[2] <= 0);
        handle.SetActive(cartStatus[3] <= 0);
    }

    /// <summary>通常時</summary>
    private void Normal()
    {
        if (Input.GetButtonDown("XboxR") || Input.GetKeyDown(KeyCode.L))
        {
            rotatepoint.transform.localPosition = willyPoint;
            SlopeCart(-13);
            isR = true;
        }
        else if (Input.GetButtonDown("XboxL") || Input.GetKeyDown(KeyCode.K))
        {
            rotatepoint.transform.localPosition = motiagePoint;
            SlopeCart(13);
            isR = false; 
        }
    }

    /// <summary>ウィリー時</summary>
    private void Willy()
    {
        if (nowTime >= willTime)
        {
            NoSlopeCart();
        }
        nowTime += Time.deltaTime;
    }

    public bool IsWilly()
    {
        return isWilly;
    }

    /// <summary>カート傾け</summary>
    /// <param name="angle">どのくらい傾けるか</param>
    private void SlopeCart(float angle)
    {
        if (playerScript.MyCart() == gameObject)
        {
            scScript.SetBasketParent(transform);
        }
        else
        {
            secondCartBaggageParent.parent = transform;
        }
        transform.parent = rotatepoint.transform;
        //rotatepoint.transform.localRotation = Quaternion.AngleAxis(angle, new Vector3(1, 0, 0));
        transform.RotateAround(rotatepoint.transform.position, rotatepoint.transform.right, angle);
        isWilly = true;
        //pointScr.PlusSlope(angle);
        moderuAngle = angle;
    }

    /// <summary>カート傾けない</summary>
    public void NoSlopeCart()
    {
        rotatepoint.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 0, 0));
        transform.RotateAround(rotatepoint.transform.position, rotatepoint.transform.right, -moderuAngle);
        if (playerScript.MyCart() == gameObject)
        {
            scScript.SetBasketParent(transform.root);
        }
        else
        {
            secondCartBaggageParent.parent = transform.root;
        }
        transform.parent = transform.root;
        isWilly = false;
        nowTime = 0.0f;
    }

    /// <summary>
    /// カートを持った時にカートのデータをもらう
    /// </summary>
    /// <param name="cart"></param>
    public void GetCart(CartStatusWithCart cart)
    { 
        cartStatus = cart.PassStatus();
        if (cartStatus[3] <= 0)
        {
            playerScript.SetMinusRotateSpeed(minusRotateSpeedDefault);
        }
    }

    /// <summary>
    /// カートを離した時にカートのデータを渡す
    /// </summary>
    /// <param name="cart"></param>
    public void SetCart(CartStatusWithCart cart)
    {
        cart.SetStatus(cartStatus);
        playerScript.SetMinusRotateSpeed(0);
    }

    public void DamageCart(float dm)
    {
        float dame = dm;
        //if (cartStatus[2] <= 0)dame = dame * 2;
        cartStatus[0] -= dame;
        /*ここでランダム部位にダメージを与える*/
        int rand = Random.Range(1, 4);

        switch (rand)
        {
            case 1: cartStatus[1] -= dame; break;
            case 2: cartStatus[2] -= dame; break;
            case 3: cartStatus[3] -= dame; break;
        }




        if (cartStatus[3] <= 0)
        {
            if (playerScript.MyCart() == gameObject)
            {
                playerScript.SetMinusRotateSpeed(minusRotateSpeedDefault);
            }
            else
            {
                playerScript.SetMinusRotateSpeed2(minusRotateSpeedDefault);
            }
        }
        if (cartStatus[0] <= 0)
        {            
            if (playerScript.MyCart() == gameObject)
            {
                if (playerScript.IsCart2())
                {
                    playerScript.ReleaseCart();
                }
                playerScript.BreakCart();
                scScript.BaggegeFall(transform.position);
                playerScript.SetMinusRotateSpeed(0);
            }
            else
            {
                playerScript.BreakCart2();
                scScript.BaggegeFall2(transform.position);
                playerScript.SetMinusRotateSpeed2(0);
            }
            seScript.OnePlay(3);
        }
        else
        {
            seScript.OnePlay(2);
        }
    }

    private void CartHPColor()
    {
        myColor.color = new Color(myColor.color.r, myColor.color.g, myColor.color.b, alphaTime);
    }

    public float BaggageRotateLimit()
    {
        float result = baggageRotateLimitDefault;
        if (cartStatus[1] <= 0) result = baggageRotateLimit;
        return (180 - result);
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject effect = effects[0];
        switch (other.transform.tag)
        {
            case "Wall":
                DamageCart(wallDamage);
                break;
            case "Animal":
            case "Bull":
                effect = effects[1];
                break;
            case "Enemy":
            case "BBA":
            case "Customer":
                effect = effects[2];
                break;
            default:
                effect = null;
                break;
        }
        if (effect == null) return;
        effect = Instantiate(effect);
        //Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
        effect.transform.position = other.ClosestPointOnBounds(this.transform.position) + Vector3.up;
    }
}
