using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBody : MonoBehaviour
{

    public float wallDamage = 5.0f;

    private CartStatusWithPlayer cs;
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

    // Use this for initialization
    void Start()
    {
        cs = transform.root.GetComponent<CartStatusWithPlayer>();
        rotatepoint = transform.root.Find("cartrotatepoint").gameObject;
        AllHP = transform.Find("AllHPCart").gameObject;
        wheel1 = transform.Find("WheelHP1").gameObject;
        wheel2 = transform.Find("WheelHP2").gameObject;
        bagUnder = transform.Find("BagUnderHP").gameObject;
        handle = transform.Find("HandleHP").gameObject;
        //pointScr = transform.root.Find("PlayerBasket(Clone)").Find("nimotuParent").GetComponent<InclinationOfLuggage>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if(cs.HPNow() <= 30)
        {
            AllHP.SetActive(true);
        }
        wheel1.SetActive(cs.WheelHP0());
        wheel2.SetActive(cs.WheelHP0());
        bagUnder.SetActive(cs.BagUnderHP0());
        handle.SetActive(cs.HandleHP0());
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
            nowTime = 0.0f;
        }
        nowTime += Time.deltaTime;
    }

    /// <summary>カート傾け</summary>
    /// <param name="angle">どのくらい傾けるか</param>
    private void SlopeCart(float angle)
    {
        cs.SetBasketParent(rotatepoint.transform);
        transform.parent = rotatepoint.transform;
        rotatepoint.transform.localRotation = Quaternion.AngleAxis(angle, new Vector3(1, 0, 0));
        isWilly = true;
        //pointScr.PlusSlope(angle);
        //moderuAngle = angle;
    }

    /// <summary>カート傾けない</summary>
    private void NoSlopeCart()
    {
        rotatepoint.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 0, 0));
        cs.SetBasketParent(transform.root);
        transform.parent = transform.root;
        isWilly = false;
        //pointScr.PlusSlope(moderuAngle);
    }

    private void CartHPColor()
    {
        myColor.color = new Color(myColor.color.r, myColor.color.g, myColor.color.b, alphaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject effect = effects[0];
        switch (other.transform.tag)
        {
            case "Wall":
                cs.DamageCart(wallDamage);
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
