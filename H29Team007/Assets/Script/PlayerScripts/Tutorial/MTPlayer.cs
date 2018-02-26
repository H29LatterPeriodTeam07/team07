using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MTPlayer : MonoBehaviour {

    public enum PlayerState
    {
        NoCart, // カート無し
        OnCart, // カート持ち
        Gliding, // 滑走中
        Takeover, // カートジャック
        Throw //投げ
    }

    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody rb;

    private GameObject myCart;
    private PlayerState myState;

    private GameObject mySecondCart;

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
    
    private Animator m_Animator;

    private bool canGet = false; //カートを持てるか
    private GameObject canGetCart; //もてるカート

    private NavMeshAgent myNav;
    

    private float nikuspeed = 1;
    private float nikuTimeLimit = 10.0f;
    private float nikuTime = 0.0f;

    private TutorialShopping scScript;
    

    private bool enemyHit = false; //敵につかまったか

    private bool flyHit = false; //投げた籠が当たったか
    

    private PlayerSE seScript;  //プレイヤーSEのスクリプト

    private TutorialCamera cameraScript;  //playerカメラのスクリプト

    private Fade fade;  //フェードのスクリプト


    private MTManager tm;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //myCartStatus = GetComponent<CartStatusWithPlayer>();
        scScript = GetComponent<TutorialShopping>();
        //myBaggege = new List<Transform>();
        myCC = GetComponent<CapsuleCollider>();
        m_Animator = GetComponent<Animator>();
        myNav = GetComponent<NavMeshAgent>();
        //cartRotatePoint = transform.Find("cartrotatepoint");
        seScript = GetComponent<PlayerSE>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.GetComponent<TutorialCamera>();
        fade = GameObject.Find("fade").GetComponent<Fade>();
        tm = GameObject.Find("tutorialmanager").GetComponent<MTManager>();
    }

    void Update()
    {
        inputVertical = 0;
        if (!tm.FadeEnd())
        {
            rb.velocity = Vector3.zero;
            return;
        }
        if (transform.parent != null)
        {
            rb.velocity = Vector3.zero;
            if (myCart != null)
            {
                BreakCart2();
                BreakCart();
            }
            scScript.BaggegeFall(transform.position);
            scScript.BasketActive(false);
            fade.FadeOut(5.0f);
            m_Animator.Play("BBAButtobi");
            ChangeState(4);
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

        if (!CanMove())
        {
            inputHorizontal = 0;
            inputVertical = 0;
        }


        if (GetState() != PlayerState.NoCart &&
            GetState() != PlayerState.Takeover &&
            GetState() != PlayerState.Throw)
        {

            if (
                //Input.GetButtonUp("XboxA") || Input.GetKeyDown(KeyCode.R) || 
                !scScript.IsCatchBasket())
            {
                ReleaseCart();
            }
        }
        else if (GetState() == PlayerState.Throw)
        {
            if (!scScript.IsCatchBasket())
            {
                ReleaseCart(false);
            }
        }
        if (canGet && GetState() < PlayerState.Takeover && canGetCart != null)
        {
            CatchCart();
            canGet = false;
        }
        if (enemyHit)
        {
            scScript.BaggegeFall(transform.position);
            //ReleaseCart();
            //if (IsCart()) ReleaseCart();
            //fade.FadeOut(1.0f);
            m_Animator.Play("BBAButtobi");
            ChangeState(4);
            enemyHit = false;
            tm.Index10Reset();
        }

    }

    void FixedUpdate()
    {
        m_Animator.SetBool("OnCart", IsCart());
        m_Animator.SetBool("HavingBasket", scScript.IsCatchBasket());
        if (!tm.FadeEnd() || transform.parent != null)
        {
            m_Animator.SetFloat("Speed", myNav.velocity.sqrMagnitude);
            return;
        }

        switch (myState)
        {
            case PlayerState.NoCart: CartOffMove(); break;
            case PlayerState.OnCart: CartOnMove(); break;
            case PlayerState.Gliding: CartGliding(); break;
            case PlayerState.Takeover: PlayerHacking(); break;
            case PlayerState.Throw: Throw(); break;
        }

        float playerSpeed = rb.velocity.sqrMagnitude;
        if (myState != PlayerState.NoCart && myState != PlayerState.Gliding && inputVertical < 0) playerSpeed *= -1;
        if (myState >= PlayerState.Takeover) playerSpeed = myNav.velocity.sqrMagnitude;
        m_Animator.SetFloat("Speed", playerSpeed);
        //m_Animator.SetBool("OnCart", IsCart());

        if (playerSpeed == 0
            || myState != PlayerState.Gliding && myState != PlayerState.OnCart)
        {
            seScript.SEPlay(6);
        }
        else if (myState == PlayerState.Gliding && inputHorizontal != 0)
        {
            seScript.SEPlay(1);
        }
        else if (myState == PlayerState.OnCart || myState == PlayerState.Gliding)
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
            case 2:
                myState = PlayerState.Gliding; myCC.material = glidingPhysiMat;
                m_Animator.SetBool("Gliding", true); break;
            case 3: myState = PlayerState.Takeover; break;
            case 7:
                myState = PlayerState.Throw;
                m_Animator.Play("BBAThrow"); break;
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

        rb.velocity = moveForward * onCartMoveSpeed * nikuspeed + new Vector3(0, rb.velocity.y, 0);
        
        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O)
            )
        {
            if (!CanMove()) return;
            rb.AddForce(transform.forward * kickSpeed * nikuspeed, ForceMode.VelocityChange);
            ChangeState(2);
        }

        if (!tm.FadeEnd() || tm.TutorialIndex() != 7) return;
        if (Input.GetButtonDown("XboxR") || Input.GetKeyDown(KeyCode.L))
        {
            m_Animator.Play("hippari");
        }
        else if (Input.GetButtonDown("XboxL") || Input.GetKeyDown(KeyCode.K))
        {
            m_Animator.Play("moti");
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
        rb.velocity = moveForward * offCartMoveSpeed * nikuspeed + new Vector3(0, rb.velocity.y, 0);

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
        //投げたかごがカートに当たったら
        if (flyHit)
        {
            ChangeState(3);
            flyHit = false;
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
        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O)
            )
        {
            if (!CanMove()) return;
            rb.velocity = transform.forward * kickSpeed * nikuspeed;
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
        rb.velocity = Vector3.zero;
        m_Animator.SetBool("HavingBasket", false);
        Vector3 dis = canGetCart.transform.position;// + canGetCart.transform.forward * (CartRelatedData.cartNavPoint);
        myNav.destination = new Vector3(dis.x, -0.8f, dis.z);
        if (Vector3.Distance(myNav.destination, transform.position) < 1.5f)
        {
            //transform.position = myNav.destination;
            //transform.rotation = canGetCart.transform.rotation;
            myNav.enabled = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            scScript.SetBasketParent(transform);
            CatchCart();
        }
    }
    
    
    

    /// <summary>投げ</summary>
    private void Throw()
    {
        m_Animator.SetBool("HavingBasket", false);
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("NoCart"))
        {
            ChangeState(0);
        }
    }
    

    /// <summary>カゴを投げ当てたカートに乗り移る </summary>
    /// <param name="nextcart">乗り移るカート</param>
    public void ChangeCart(GameObject nextcart)
    {
        scScript.BasketActive(true);
        ReleaseCart(false);
        canGetCart = nextcart;
        myNav.enabled = true;
        Vector3 basPos = canGetCart.transform.position - canGetCart.transform.forward * 0.1f;
        basPos.y = CartRelatedData.cartInBagLocalPosY;
        scScript.SetBasketGlobalPos(basPos);
        scScript.SetBasketAngle(canGetCart.transform.rotation);
        scScript.SetBasketParent(null);
        flyHit = true;
    }

    /// <summary>カート壊れる</summary>
    public void BreakCart(bool change = true)
    {
        scScript.SetBasketParent(transform);
        scScript.BasketOut();
        Destroy(myCart);
        myCart = null;
        if (change) ChangeState(0);

    }

    /// <summary>カート壊れる</summary>
    public void BreakCart2()
    {
        myCart.transform.localPosition = Vector3.forward * CartRelatedData.cartLocalPosZ;
        scScript.BasketIn();
        Destroy(mySecondCart);
        mySecondCart = null;
    }


    /// <summary>カートを離す </summary>
    public void ReleaseCart(bool change = true)
    {
        if (myCart == null) return;

        Vector3 cartPos = new Vector3(transform.position.x, 0, transform.position.z);
        if (mySecondCart == null)
        {

            GameObject cart = Instantiate(cartRigidPrefab);

            //離したカートに現在の耐久値を渡す
            //myCart.GetComponent<TutorialCart>().SetCart(cart.GetComponent<CartStatusWithCart>());

            cart.transform.position = cartPos + transform.forward * CartRelatedData.cartLocalPosZ;
            Vector3 relativePos = myCart.transform.position - transform.position;
            relativePos.y = 0; //上下方向の回転はしないように制御
            transform.rotation = Quaternion.LookRotation(relativePos);
            cart.transform.rotation = Quaternion.LookRotation(relativePos);
            BreakCart(change);

            canGetCart = cart;
        }
        else
        {
            TutorialCart cartScript = myCart.GetComponent<TutorialCart>();
            if (cartScript.IsWilly())
            {
                cartScript.NoSlopeCart();
                mySecondCart.GetComponent<TutorialCart>().NoSlopeCart();
            }
            scScript.BaggegeFall2(mySecondCart.transform.position);
            GameObject cart2 = Instantiate(cartRigidPrefab);

            //離したカートに現在の耐久値を渡す
            //mySecondCart.GetComponent<TutorialCart>().SetCart(cart2.GetComponent<CartStatusWithCart>());

            cart2.transform.position = cartPos + transform.forward * CartRelatedData.cartLocalPosZ - transform.right * 1.0f;
            Vector3 center = (mySecondCart.transform.position + myCart.transform.position) / 2;
            Vector3 relativePos = center - transform.position;
            relativePos.y = 0; //上下方向の回転はしないように制御
            transform.rotation = Quaternion.LookRotation(relativePos);
            cart2.transform.rotation = Quaternion.LookRotation(relativePos);
            BreakCart2();
        }
    }

    /// <summary>カートを持つ</summary>
    public void CatchCart()
    {
        //持つカートの耐久値をもらう
        if (myCart == null)
        {
            ChangeState(1);
            myCart = Instantiate(cartBodyPrefab);
            //持つカートの耐久値をもらう
            //myCart.GetComponent<TutorialCart>().GetCart(canGetCart.transform.gameObject.GetComponent<CartStatusWithCart>());

            myCart.transform.parent = transform;
            myCart.transform.localPosition = Vector3.forward * CartRelatedData.cartLocalPosZ;
            Vector3 relativePos = myCart.transform.position - transform.position;
            relativePos.y = 0; //上下方向の回転はしないように制御
            transform.rotation = Quaternion.LookRotation(relativePos);
            myCart.transform.rotation = Quaternion.LookRotation(relativePos);
            scScript.BasketIn();
        }
        else if (mySecondCart == null)
        {
            TutorialCart cartScript = myCart.GetComponent<TutorialCart>();
            if (cartScript.IsWilly())
            {
                cartScript.NoSlopeCart();
            }
            mySecondCart = Instantiate(cartBodyPrefab);
            //持つカートの耐久値をもらう
            //mySecondCart.GetComponent<TutorialCart>().GetCart(canGetCart.transform.gameObject.GetComponent<CartStatusWithCart>());

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
        if(canGetCart != null)Destroy(canGetCart.transform.gameObject);
    }

    public PlayerState GetState()
    {
        return myState;
    }

    public float GetFowardSpeed()
    {
        Vector3 result = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        return result.sqrMagnitude;//inputVertical;//rb.velocity.z;
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

    /// <summary>投げたかごが当たったかどうか</summary>
    public bool Flyhit()
    {
        return flyHit;
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

    /// <summary>動けるか</summary
    public bool CanMove()
    {
        bool result = true;
        if (9 == tm.TutorialIndex()
            || 11 == tm.TutorialIndex()
            || 14 == tm.TutorialIndex()
            || 0 <= tm.TutorialIndex() && tm.TutorialIndex() < 3) result = false;
        return result;
    }

    /// <summary>投げれるか</summary>
    public bool CanThrow()
    {
        bool result = false;
        if (tm.TutorialIndex() == 12) result = true;
        return result;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            if (!collision.gameObject.GetComponent<MTSecurity>().StateChasing()) return;
            enemyHit = true;
            scScript.BasketActive(false);
            transform.LookAt(collision.transform);
        }
        if (collision.transform.tag == "Cart"
            && mySecondCart == null
            && GetState() < PlayerState.Takeover
            && scScript.IsCatchBasket())
        {
            canGetCart = collision.gameObject;
            canGet = true;
        }
        if (collision.transform.tag == "Carts"
            && myCart == null
            && GetState() < PlayerState.Takeover
            && scScript.IsCatchBasket())
        {
            if (collision.gameObject.GetComponent<CartSpown>().IsCartGet())
            {
                canGetCart = Instantiate(cartRigidPrefab);
                canGet = true;
            }
        }
        if (collision.transform.name.Contains("Yakiniku"))
        {
            nikuspeed = 2;
            nikuTime = 0.0f;
            Destroy(collision.gameObject);
        }
    }

}
