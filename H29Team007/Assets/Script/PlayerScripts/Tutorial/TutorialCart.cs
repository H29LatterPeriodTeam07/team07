using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCart : MonoBehaviour {

    
    private GameObject rotatepoint;

    private Vector3 willyPoint = CartRelatedData.cartRotatePointBack;
    private Vector3 motiagePoint = CartRelatedData.cartRotatePointFront;

    private bool isR = false;
    private bool isL = false;

    private bool isWilly = false;

    //[SerializeField, Header("カート持っていない時の速さ")]
    private float willTime = 0.3f;
    private float nowTime = 0.0f;

    private float moderuAngle = 0.0f;

    public GameObject[] effects;

    private Transform secondCartBaggageParent;


    //CartStatusWithPlayお引越し
    private TutorialPlayer playerScript;
    private TutorialShopping scScript;

    private float[] cartStatus;


    [SerializeField, Header("カートのタイヤが壊れた時に毎フレーム下げる本体の耐久度")]
    private float minusCartHP = 0.01f;
    [SerializeField, Header("カートのハンドルが壊れた時に下げる回転速度")]
    private float minusRotateSpeedDefault = 60;
    [SerializeField, Header("カートの荷台が壊れた時の荷物の傾きの限界")]
    private float baggageRotateLimit = 60;


    [SerializeField, Header("デフォの荷物の傾きの限界")]
    private float baggageRotateLimitDefault = 90;

    private PlayerSE seScript;

    private TutorialManager tm;

    // Use this for initialization
    void Start()
    {
        rotatepoint = transform.root.Find("cartrotatepoint").gameObject;

        //お引越し
        playerScript = transform.root.GetComponent<TutorialPlayer>();
        scScript = transform.root.GetComponent<TutorialShopping>();

        seScript = transform.root.GetComponent<PlayerSE>();

        tm = GameObject.Find("tutorialmanager").GetComponent<TutorialManager>();

        if (playerScript.MyCart() != gameObject)
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
        //--

        if (isWilly)
        {
            Willy();
        }
        else
        {
            Normal();
        }
    }

    /// <summary>通常時</summary>
    private void Normal()
    {
        if (Input.GetButtonDown("XboxR") || Input.GetKeyDown(KeyCode.L))
        {
            rotatepoint.transform.localPosition = willyPoint;
            SlopeCart(-13);
            tm.RPush();
            //isR = true;
        }
        else if (Input.GetButtonDown("XboxL") || Input.GetKeyDown(KeyCode.K))
        {
            rotatepoint.transform.localPosition = motiagePoint;
            SlopeCart(13);
            tm.LPush();
            //isL = false;
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
        transform.RotateAround(rotatepoint.transform.position, rotatepoint.transform.right, angle);
        isWilly = true;
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

    public bool TutorialRotate()
    {
        return (isL && isR);
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject effect = effects[0];
        switch (other.transform.tag)
        {
            case "Wall":
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
