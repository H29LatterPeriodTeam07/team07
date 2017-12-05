using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        NoCart, // カート無し
        OnCart, // カート持ち
        Gliding, // 滑走中
        Takeover // カートジャック
    }

    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody rb;

    private GameObject myCart;
    private PlayerState myState;

    private GameObject mySecondCart;

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


    private float minusRotateSpeed;
    private float minusRotateSpeed2;


    [SerializeField, Header("その他")]
    public GameObject cartBodyPrefab;
    public GameObject cartRigidPrefab;
    public PhysicMaterial glidingPhysiMat;
    private CapsuleCollider myCC;

    //private CartStatusWithPlayer myCartStatus;
    private Animator m_Animator;

    private bool canGet = false; //カートを持てるか
    private GameObject canGetCart; //もてるカート

    private NavMeshAgent myNav;
    private GameObject nextCart;

    private ShoppingCount scScript;

    public Transform exitPoint;

    private Transform cartRotatePoint;

    private GameObject havedCart;

    private PlayerSE seScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //myCartStatus = GetComponent<CartStatusWithPlayer>();
        scScript = GetComponent<ShoppingCount>();
        myState = PlayerState.NoCart;
        //myBaggege = new List<Transform>();
        myCC = GetComponent<CapsuleCollider>();
        m_Animator = GetComponent<Animator>();
        myNav = GetComponent<NavMeshAgent>();
        myNav.enabled = false;
        cartRotatePoint = transform.Find("cartrotatepoint");
        seScript = GetComponent<PlayerSE>();
    }

    void Update()
    {
        if (!MainGameDate.IsStart())
        {
            m_Animator.SetFloat("Speed", 0);
            rb.velocity = Vector3.zero;
            return;
        }
        if(transform.parent != null)
        {
            rb.velocity = Vector3.zero;
            if (myCart != null)
            {
                BreakCart2();
                BreakCart();
            }
            return;
        }
        if (GetState() == PlayerState.Takeover) return;
        

        inputHorizontal = (Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") : Input.GetAxisRaw("Horizontal");
        inputVertical = (Input.GetAxisRaw("XboxLeftVertical") != 0) ? Input.GetAxisRaw("XboxLeftVertical") : Input.GetAxisRaw("Vertical");

        if (myState == PlayerState.NoCart)
        {
            Vector3 inputVec = new Vector3(inputHorizontal, 0, inputVertical);
            inputVec = Vector3.Normalize(inputVec);

            inputHorizontal = inputVec.x;
            inputVertical = inputVec.z;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            m_Animator.Play("BBAButtobi");
        }

        //if (!scScript.IsCatchBasket()) return;

        if (GetState() != PlayerState.NoCart &&
            GetState() != PlayerState.Takeover)
        {

            if (Input.GetButtonUp("XboxA") || Input.GetKeyDown(KeyCode.R)|| !scScript.IsCatchBasket())
            {
                ReleaseCart();
            }
        }
        if (canGet && GetState() != PlayerState.Takeover && canGetCart != null)
        {
            CatchCart();
            canGet = false;
        }

    }

    void FixedUpdate()
    {
        if (!MainGameDate.IsStart() || transform.parent != null)
        {
            m_Animator.SetFloat("Speed", 0);
            m_Animator.SetBool("OnCart", IsCart());
            return;
        }

        switch (myState)
        {
            case PlayerState.NoCart: CartOffMove(); break;
            case PlayerState.OnCart: CartOnMove(); break;
            case PlayerState.Gliding: CartGliding(); break;
            case PlayerState.Takeover: PlayerHacking(); break;
        }

        float playerSpeed = rb.velocity.sqrMagnitude;
        if (myState != PlayerState.NoCart && myState != PlayerState.Gliding && inputVertical < 0) playerSpeed *= -1;
        if (myState == PlayerState.Takeover) playerSpeed = myNav.velocity.sqrMagnitude;
        m_Animator.SetFloat("Speed", playerSpeed);
        m_Animator.SetBool("OnCart", IsCart());

        if (playerSpeed == 0 
            || myState != PlayerState.Gliding && myState != PlayerState.OnCart)
        {
            seScript.SEPlay(6);
        }
        else if(myState == PlayerState.Gliding && inputHorizontal != 0){
            seScript.SEPlay(1);
        }
        else if(myState == PlayerState.OnCart || myState == PlayerState.Gliding)
        {
            seScript.SEPlay(0);
        }
    }

    /// <summary> 状態変化 </summary>
    /// <param name="state">0:カート無し 1:カートあり 2:滑走 3:カートチェンジ</param>
    public void ChangeState(int state)
    {
        switch (state)
        {
            case 0: myState = PlayerState.NoCart; scScript.SetBasketColliderActive(true); break;
            case 1: myState = PlayerState.OnCart; scScript.SetBasketColliderActive(false); break;
            case 2: myState = PlayerState.Gliding; myCC.material = glidingPhysiMat;
                m_Animator.SetBool("Gliding", true); break;
            case 3: myState = PlayerState.Takeover; break;
        }
        if (myCC.material != null && state != 2)
        {
            myCC.material = null;
            m_Animator.SetBool("Gliding", false);
        }
    }

    /// <summary>カートを持っているときの動き</summary>
    private void CartOnMove()
    {
        float mrs = Mathf.Max(minusRotateSpeed, minusRotateSpeed2);
        transform.Rotate(new Vector3(0, inputHorizontal * (onCartRotateSpeed - mrs) * Time.deltaTime, 0));

        Vector3 moveForward = transform.forward * inputVertical;

        rb.velocity = moveForward * onCartMoveSpeed + new Vector3(0, rb.velocity.y, 0);

        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O))
        {
            //rb.velocity = transform.forward * kickSpeed;
            rb.AddForce(transform.forward * kickSpeed, ForceMode.VelocityChange);
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
        float mrs = Mathf.Max(minusRotateSpeed, minusRotateSpeed2);
        transform.Rotate(new Vector3(0, inputHorizontal * (angleRotateSpeed - mrs) * Time.deltaTime, 0));

        Quaternion a = new Quaternion(rb.velocity.x, 0, rb.velocity.z, 0);
        a *= Quaternion.AngleAxis(inputHorizontal * -velocityRotateSpeed, Vector3.up);
        rb.velocity = new Vector3(a.x, 0, a.z);
        //Debug.Log(rb.velocity);
        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O))
        {
            rb.velocity = transform.forward * kickSpeed;
            //var info = m_Animator.GetCurrentAnimatorStateInfo(0);
            m_Animator.Play("Kick", 0, 0.0f);
        }
        if (rb.velocity == Vector3.zero)
        {
            ChangeState(1);
        }
    }

    /// <summary>カートのジャック</summary>
    private void PlayerHacking()
    {
        Vector3 dis = nextCart.transform.position + nextCart.transform.forward * (CartRelatedData.cartNavPoint);
        myNav.destination = new Vector3(dis.x, -0.8f, dis.z);
        if (Vector3.Distance(myNav.destination, transform.position) < 1.0f)
        {
            transform.position = myNav.destination;
            transform.rotation = nextCart.transform.rotation;
            myNav.enabled = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            canGetCart = nextCart;
            scScript.SetBasketParent(transform);
            CatchCart();
        }
    }

    /// <summary>カゴを投げ当てたカートに乗り移る </summary>
    /// <param name="nextcart">乗り移るカート</param>
    public void ChangeCart(GameObject nextcart)
    {
        scScript.BasketActive(true);
        ReleaseCart();
        nextCart = nextcart;
        myNav.enabled = true;
        canGetCart = nextCart.transform.Find("BackHitArea").gameObject;
        Vector3 basPos = nextCart.transform.position - nextCart.transform.forward * 0.1f;
        basPos.y = CartRelatedData.cartInBagLocalPosY;
        scScript.SetBasketGlobalPos(basPos);
        scScript.SetBasketAngle(nextCart.transform.rotation);
        scScript.SetBasketParent(null);
        //myNav.destination = nextCart.transform.position + nextCart.transform.forward * (-1.5f);
        ChangeState(3);
    }

    /// <summary>カート壊れる</summary>
    public void BreakCart()
    {
        //cartRotatePoint.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 0, 0));
        scScript.SetBasketParent(transform);
        scScript.BasketOut();
        Destroy(myCart);
        ChangeState(0);
    }

    /// <summary>カート壊れる</summary>
    public void BreakCart2()
    {
        //cartRotatePoint.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 0, 0));
        scScript.SetBasketParent(transform);
        myCart.transform.localPosition = Vector3.forward * CartRelatedData.cartLocalPosZ;
        scScript.BasketIn();
        Destroy(mySecondCart);
        //ChangeState(0);
    }


    /// <summary>カートを離す </summary>
    public void ReleaseCart()
    {
        if (myCart == null) return;
        
        Vector3 cartPos = new Vector3(transform.position.x, 0, transform.position.z);
        if (mySecondCart == null)
        {

            GameObject cart = Instantiate(cartRigidPrefab);

            //離したカートに現在の耐久値を渡す
            myCart.GetComponent<CartBody>().SetCart(cart.GetComponent<CartStatusWithCart>());

            cart.transform.position = cartPos + transform.forward * CartRelatedData.cartLocalPosZ;
            Vector3 relativePos = myCart.transform.position - transform.position;
            relativePos.y = 0; //上下方向の回転はしないように制御
            transform.rotation = Quaternion.LookRotation(relativePos);
            cart.transform.rotation = Quaternion.LookRotation(relativePos);
            //havedCart = cart;
            BreakCart();
        }
        else
        {
            CartBody cartScript = myCart.GetComponent<CartBody>();
            if (cartScript.IsWilly())
            {
                cartScript.NoSlopeCart();
                mySecondCart.GetComponent<CartBody>().NoSlopeCart();
            }
            scScript.BaggegeFall2(mySecondCart.transform.position);
            GameObject cart2 = Instantiate(cartRigidPrefab);

            //離したカートに現在の耐久値を渡す
            mySecondCart.GetComponent<CartBody>().SetCart(cart2.GetComponent<CartStatusWithCart>());

           // cart.transform.position = cartPos + transform.forward * CartRelatedData.cartLocalPosZ + transform.right * 0.5f;
            cart2.transform.position = cartPos + transform.forward * CartRelatedData.cartLocalPosZ - transform.right * 1.0f;
            Vector3 center = (mySecondCart.transform.position + myCart.transform.position) / 2;
            Vector3 relativePos = center - transform.position;
            relativePos.y = 0; //上下方向の回転はしないように制御
            transform.rotation = Quaternion.LookRotation(relativePos);
            //cart.transform.rotation = Quaternion.LookRotation(relativePos);
            cart2.transform.rotation = Quaternion.LookRotation(relativePos);
            Destroy(mySecondCart);
            myCart.transform.localPosition = Vector3.forward * CartRelatedData.cartLocalPosZ;
            scScript.BasketIn();
        }
    }

    /// <summary>カートを持つ</summary>
    public void CatchCart()
    {
        //持つカートの耐久値をもらう
        //myCartStatus.GetCart(canGetCart.transform.gameObject.GetComponent<CartStatusWithCart>());

        if (myCart == null)
        {
            ChangeState(1);
            myCart = Instantiate(cartBodyPrefab);
            //持つカートの耐久値をもらう
            myCart.GetComponent<CartBody>().GetCart(canGetCart.transform.gameObject.GetComponent<CartStatusWithCart>());

            myCart.transform.parent = transform;
            //Vector3 cartPos = new Vector3(transform.position.x, 0, transform.position.z);
            myCart.transform.localPosition = Vector3.forward * CartRelatedData.cartLocalPosZ;
            Vector3 relativePos = myCart.transform.position - transform.position;
            relativePos.y = 0; //上下方向の回転はしないように制御
            transform.rotation = Quaternion.LookRotation(relativePos);
            myCart.transform.rotation = Quaternion.LookRotation(relativePos);
            scScript.BasketIn();
        }
        else if(mySecondCart == null)
        {
            CartBody cartScript = myCart.GetComponent<CartBody>();
            if (cartScript.IsWilly())
            {
                cartScript.NoSlopeCart();
            }
            mySecondCart = Instantiate(cartBodyPrefab);
            //持つカートの耐久値をもらう
            mySecondCart.GetComponent<CartBody>().GetCart(canGetCart.transform.gameObject.GetComponent<CartStatusWithCart>());

            mySecondCart.transform.parent = transform;
            myCart.transform.localPosition = Vector3.forward * CartRelatedData.cartLocalPosZ + Vector3.right * 0.5f;
            mySecondCart.transform.localPosition = Vector3.forward * CartRelatedData.cartLocalPosZ - Vector3.right * 0.5f;
            Vector3 center = (mySecondCart.transform.position + myCart.transform.position) / 2;
            Vector3 relativePos = center - transform.position;
            relativePos.y = 0; //上下方向の回転はしないように制御
            transform.rotation = Quaternion.LookRotation(relativePos);
            mySecondCart.transform.rotation = Quaternion.LookRotation(relativePos);
            scScript.SetBasketLocalPos(CartRelatedData.cartInBagLocalPos + Vector3.right * 0.5f);
        }
        Destroy(canGetCart.transform.gameObject);
    }

    public PlayerState GetState()
    {
        return myState;
    }

    /// <summary>プレイヤーがカートを持っているかどうか</summary>
    /// <returns>持っていたらtrue</returns>
    public bool IsCart()
    {
        return (myCart != null);
    }

    public bool IsCart2()
    {
        return (mySecondCart != null);
    }

    public GameObject MyCart()
    {
        return myCart;
    }

    public GameObject MySecondCart()
    {
        return mySecondCart;
    }

    /// <summary>カゴの中に人が入っているか</summary>
    /// <returns>入っていたらtrue</returns>
    public bool IsGetHuman()
    {
        return scScript.IsBaggegeinHuman();
    }

    public void SetMinusRotateSpeed(float speed)
    {
        minusRotateSpeed = speed;
    }

    public void SetMinusRotateSpeed2(float speed)
    {
        minusRotateSpeed2 = speed;
    }

    /// <summary>闘牛を捕まえることができるか</summary>
    public bool CanGetBull(Transform bull)
    {
        //カートが2台なかったら捕獲できない or 人間が動物以下だったら捕獲できない
        if (mySecondCart == null || !scScript.IsHumanMoreThanAnimal()) return false;
        //プレイヤーから闘牛への方向ベクトル(ワールド座標系)
        Vector3 directionToBull = bull.position - transform.position;
        // プレイヤーの正面向きベクトルと闘牛への方向ベクトルの差分角度
        float angleToBull = Vector3.Angle(transform.forward, directionToBull);

        // 捕獲できる角度の範囲内に闘牛がいるかどうかを返却する
        return (Mathf.Abs(angleToBull) <= 90);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy" )
        {
            if (!collision.gameObject.GetComponent<SecurityGuard>().StateChasing()) return;
            scScript.BaggegeFall(transform.position);
            ReleaseCart();
            transform.position = exitPoint.position;
            seScript.OnePlay(4);
            //havedCart = null;
        }
        if (collision.transform.tag == "Cart"
            //&& havedCart == null 
            && mySecondCart == null
            && GetState() != PlayerState.Takeover
            && scScript.IsCatchBasket())
        {
            canGetCart = collision.gameObject;
            canGet = true;
        }
        //if(collision.transform.tag == "Bull")
        //{
        //    if (CanGetBull(collision.transform))
        //    {
        //        collision.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        //        collision.gameObject.GetComponent<Collider>().enabled = false;
        //        scScript.AddBaggege(collision.transform, mySecondCart, 1);
        //    }
        //}
    }

    public void OnCollisionExit(Collision collision)
    {
        //if (collision.transform.tag == "Cart" && havedCart != null)
        //{
        //    havedCart = null;
        //}
    }


    //public void OnTriggerStay(Collider other)
    //{
    //    if (other.name == "BackHitArea")
    //    {
    //        canGetCart = other.gameObject;
    //        if (CanGetCart())
    //        {
    //            canGet = true;
    //        }
    //        else
    //        {
    //            canGet = false;
    //        }
    //    }
    //}

    //public void OnTriggerExit(Collider other)
    //{
    //    if (other.name == "BackHitArea")
    //    {
    //        canGet = false;
    //        canGetCart = null;
    //    }
    //}
}
