using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum PlayerState
    {
        NoCart,
        OnCart,
        Gliding
    }

    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody rb;

    private GameObject myCart;
    private PlayerState myState;

    //private List<Transform> myBaggege;

    [SerializeField, Header("カート持っていない時の速さ")]
    private float offCartMoveSpeed = 3f;

    [SerializeField, Header("カート持った時の速さ")]
    private float onCartMoveSpeed = 3f;
    [SerializeField, Header("カート持った時の方向転換する速さ")]
    private float onCartRotateSpeed = 90.0f;


    [Header("下３つ滑走関係")]
    [SerializeField, Header("方向転換する速さ、カート本体の回転")]
    private float angleRotateSpeed = 90.0f;
    [SerializeField, Header("速度回転の速さ、ドリフト的な奴")]
    private float velocityRotateSpeed = 2.0f;
    [SerializeField, Header("ボタン押したときの最高スピード")]
    private float kickSpeed = 10.0f;


    [SerializeField, Header("その他")]
    public GameObject cartBodyPrefab;
    public GameObject cartRigidPrefab;
    public PhysicMaterial glidingPhysiMat;
    private CapsuleCollider myCC;

    private CartStatusWithPlayer myCartStatus;
    private Animator m_Animator;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCartStatus = GetComponent<CartStatusWithPlayer>();
        myState = PlayerState.NoCart;
        //myBaggege = new List<Transform>();
        myCC = GetComponent<CapsuleCollider>();
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        

        inputHorizontal = (Input.GetAxisRaw("PS4LeftHorizontal") != 0)?Input.GetAxisRaw("PS4LeftHorizontal"):Input.GetAxisRaw("Horizontal");
        inputVertical = (Input.GetAxisRaw("PS4LeftVertical") != 0) ? Input.GetAxisRaw("PS4LeftVertical") : Input.GetAxisRaw("Vertical");
        
        if (IsCart())
        {
            if (Input.GetButtonDown("PS4Cross") ||Input.GetKeyDown(KeyCode.E))
            {
                BreakCart();

                GameObject cart = Instantiate(cartRigidPrefab);

                //離したカートに現在の耐久値を渡す
                myCartStatus.SetCart(cart.GetComponent<CartStatusWithCart>());

                Vector3 cartPos = new Vector3(transform.position.x, 0, transform.position.z);
                cart.transform.position = cartPos + transform.forward * 1.5f;
                Vector3 relativePos = myCart.transform.position - transform.position;
                relativePos.y = 0; //上下方向の回転はしないように制御
                transform.rotation = Quaternion.LookRotation(relativePos);
                cart.transform.rotation = Quaternion.LookRotation(relativePos);
            }
        }
        
    }

    void FixedUpdate()
    {
        switch (myState)
        {
            case PlayerState.NoCart:CartOffMove();break;
            case PlayerState.OnCart:CartOnMove();break;
            case PlayerState.Gliding: CartGliding(); break;
        }
        float playerSpeed = rb.velocity.sqrMagnitude;
        if (myState != PlayerState.NoCart&&inputVertical < 0) playerSpeed *= -1;
        m_Animator.SetFloat("Speed", playerSpeed);
    }

    /// <summary> 状態変化 </summary>
    /// <param name="state">0:カート無し 1:カートあり 2:滑走</param>
    public void ChangeState(int state)
    {
        switch (state)
        {
            case 0:myState = PlayerState.NoCart;break;
            case 1:myState = PlayerState.OnCart;break;
            case 2:myState = PlayerState.Gliding; myCC.material = glidingPhysiMat; break;
        }
        if (myCC.material != null && state != 2)
        {
            myCC.material = null;
        }
    }

    /// <summary>カートを持っているときの動き</summary>
    private void CartOnMove()
    {
        transform.Rotate(new Vector3(0, inputHorizontal * onCartRotateSpeed * Time.deltaTime, 0));

        Vector3 moveForward = transform.forward * inputVertical * 3.0f;

        rb.velocity = moveForward * onCartMoveSpeed + new Vector3(0, rb.velocity.y, 0);

        if (Input.GetButtonDown("PS4R1") || Input.GetKeyDown(KeyCode.O))
        {
            rb.velocity = transform.forward * kickSpeed;
            ChangeState(2);
        }
    }

    /// <summary>カートを持っていないときの動き </summary>
    private void CartOffMove()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = moveForward * offCartMoveSpeed + new Vector3(0, rb.velocity.y, 0);

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }

    /// <summary>滑走中の動き </summary>
    private void CartGliding()
    {
        transform.Rotate(new Vector3(0, inputHorizontal * angleRotateSpeed * Time.deltaTime, 0));

        Quaternion a = new Quaternion(rb.velocity.x, 0, rb.velocity.z, 0);
        a *= Quaternion.AngleAxis(inputHorizontal * -velocityRotateSpeed, Vector3.up);
        rb.velocity = new Vector3(a.x, 0, a.z);

        if (Input.GetButtonDown("PS4R1") || Input.GetKeyDown(KeyCode.O))
        {
            rb.velocity = transform.forward * kickSpeed;
        }
        if(rb.velocity == Vector3.zero)
        {
            ChangeState(1);
        }
    }

    public bool IsCart()
    {
        return (myCart != null);
    }

    public void BreakCart()
    {
        Destroy(myCart);
        ChangeState(0);
    }

    //ShoppingCount に引っ越しました
    /* public void AddBaggege(Transform baggege)
     {
         baggege.parent = transform;
         myBaggege.Add(baggege);
     }

     /// <summary>荷物落とすときの処理</summary>
     public void BaggegeFall()
     {
         List<Transform> myList = new List<Transform>();

         int a = 0;
         foreach (Transform chird in transform)
         {
             if (chird.tag == "Enemy")
             {
                 float x = Random.Range(-3.0f, 3.0f);
                 float z = Random.Range(-3.0f, 3.0f);
                 float sp = Random.Range(5.0f, 10.0f);

                 Vector3 pos = new Vector3(transform.position.x + x, 0,transform.position.z + z);

                 FallDown fall = chird.GetComponent<FallDown>();
                 fall.enabled = true;
                 fall.SetPoint(pos, sp);

                 myList.Add(chird);
                 a++;
             }
         }
         for (int i = 0; i < a; i++)
         {
             myList[i].parent = null;
         }
         for(int i = 0; i < myBaggege.Count; i++)
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
         myBaggege.Clear();
         GetComponent<ShoppingCount>().Reset();
     }*/

    public GameObject GetCart()
    {
        return myCart;
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.name == "BackHitArea")
        {
            if (Input.GetButtonDown("PS4Circle") || Input.GetKeyDown(KeyCode.R))
            {
                //持つカートの耐久値をもらう
                myCartStatus.GetCart(other.transform.parent.gameObject.GetComponent<CartStatusWithCart>());

                Destroy(other.transform.parent.gameObject);
                ChangeState(1);
                myCart = Instantiate(cartBodyPrefab);
                Vector3 cartPos = new Vector3(transform.position.x, 0, transform.position.z);
                myCart.transform.position = cartPos + transform.forward * 1.5f;
                Vector3 relativePos = myCart.transform.position - transform.position;
                relativePos.y = 0; //上下方向の回転はしないように制御
                transform.rotation = Quaternion.LookRotation(relativePos);
                myCart.transform.rotation = Quaternion.LookRotation(relativePos);
                myCart.transform.parent = transform;
            }
        }
    }
}
