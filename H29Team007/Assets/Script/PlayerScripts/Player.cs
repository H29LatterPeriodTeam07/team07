using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        NoCart, // カート無し
        OnCart, // カート持ち
        Gliding, // 滑走中
        Takeover, // カートジャック
        Outside, //追い出される
        Entry,  //入店
        Exit,  //退店
        Throw //投げ
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
    //private GameObject nextCart;

    //2回押し
    private bool push = false;           //　最初に移動ボタンを押したかどうか
    private bool stickBack = false;
    private float nextButtonDownTime = 0.5f;    //　次に移動ボタンが押されるまでの時間
    private float nowTime = 0f;         //　最初に移動ボタンが押されてからの経過時間

    private float limitAngle = 30.0f;            //　最初に押した方向との違いの限度角度
    private Vector2 direction = Vector2.zero;			//　移動キーの押した方向
    //

    private float nikuspeed = 1;
    private float nikuTimeLimit = 10.0f;
    private float nikuTime = 0.0f;

    private ShoppingCount scScript;

    private Transform exitPoint;  //ExitPoint
    private Transform entrancePoint;  //EntrancePoint
    private Transform playerExitPoint; //PlayerExitPoint

    private bool enemyHit = false; //敵につかまったか

    private bool flyHit = false; //投げた籠が当たったか

    //private Transform cartRotatePoint; 

    //private GameObject havedCart;

    private PlayerSE seScript;  //プレイヤーSEのスクリプト

    private PlayerCamera cameraScript;  //playerカメラのスクリプト

    private Fade fade;  //フェードのスクリプト

    private ResultBackCamera resultCamera; //リザルトの背景を映すカメラ

    private GameManager m_gmScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //myCartStatus = GetComponent<CartStatusWithPlayer>();
        scScript = GetComponent<ShoppingCount>();
        myState = PlayerState.Entry;
        //myBaggege = new List<Transform>();
        myCC = GetComponent<CapsuleCollider>();
        m_Animator = GetComponent<Animator>();
        myNav = GetComponent<NavMeshAgent>();
        myNav.enabled = true;
        exitPoint = GameObject.Find("ExitPoint").transform;
        entrancePoint = GameObject.Find("EntrancePoint").transform;
        playerExitPoint = GameObject.Find("PlayerExitPoint").transform;
        myNav.destination = entrancePoint.position;
        //cartRotatePoint = transform.Find("cartrotatepoint");
        seScript = GetComponent<PlayerSE>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.GetComponent<PlayerCamera>();
        fade = GameObject.Find("fade").GetComponent<Fade>();
        m_gmScript = GameObject.FindGameObjectWithTag("GameManager").transform.GetComponent<GameManager>();
        if (GameObject.Find("ResultBackCamera") != null) resultCamera = GameObject.Find("ResultBackCamera").GetComponent<ResultBackCamera>();
    }

    void Update()
    {
        inputVertical = 0;
        if (!MainGameDate.IsStart())
        {
            //m_Animator.SetFloat("Speed", 0);
            rb.velocity = Vector3.zero;
            return;
        }
        if(transform.parent != null && myState != PlayerState.Outside)
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

        NikuManager();

        inputHorizontal = (Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") : Input.GetAxisRaw("Horizontal");
        inputVertical = (Input.GetAxisRaw("XboxLeftVertical") != 0) ? Input.GetAxisRaw("XboxLeftVertical") : Input.GetAxisRaw("Vertical");

        if (myState == PlayerState.NoCart)
        {
            Vector3 inputVec = new Vector3(inputHorizontal, 0, inputVertical);
            inputVec = Vector3.Normalize(inputVec);

            inputHorizontal = inputVec.x;
            inputVertical = inputVec.z;
        }

        

        //if (!scScript.IsCatchBasket()) return;

        if (GetState() != PlayerState.NoCart &&
            GetState() != PlayerState.Takeover &&
            GetState() != PlayerState.Throw)
        {

            if (Input.GetButtonUp("XboxA") || Input.GetKeyDown(KeyCode.R)|| !scScript.IsCatchBasket())
            {
                ReleaseCart();
                //Debug.Log("b");
            }
        }
        else if (GetState() == PlayerState.Throw)
        {
            if (!scScript.IsCatchBasket())
            {
                ReleaseCart(false);
                //Debug.Log("v");
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
            //BreakCart2();
            //BreakCart();
            ReleaseCart();
            if (IsCart()) ReleaseCart();
            fade.FadeOut(1.0f);
            m_Animator.Play("BBAButtobi");
            ChangeState(4);
            enemyHit = false;
        }

    }

    void FixedUpdate()
    {
        m_Animator.SetBool("OnCart", IsCart());
        m_Animator.SetBool("HavingBasket", scScript.IsCatchBasket());
        if (!MainGameDate.IsStart() || transform.parent != null)
        {
            m_Animator.SetFloat("Speed",  myNav.velocity.sqrMagnitude);
            if (myState == PlayerState.Outside) OutSide();
            if (myState == PlayerState.Entry) Entry();
            if (myState == PlayerState.Exit) Exit();
            return;
        }

        switch (myState)
        {
            case PlayerState.NoCart: CartOffMove(); break;
            case PlayerState.OnCart: CartOnMove(); break;
            case PlayerState.Gliding: CartGliding(); break;
            case PlayerState.Takeover: PlayerHacking(); break;
            case PlayerState.Outside: OutSide();break;
            case PlayerState.Entry: Entry();break;
            case PlayerState.Exit: Exit(); break;
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
        if(myState == PlayerState.Gliding)cameraScript.GlidingRotation(0.0f,true);
        if(state == 6 && myState == PlayerState.Outside)m_Animator.Play("NoCart");
        //Debug.Log(state);
        if(state != 0) scScript.SetBasketColliderActive(false);
        switch (state)
        {
            case 0: myState = PlayerState.NoCart; scScript.SetBasketColliderActive(true); break;
            case 1: myState = PlayerState.OnCart;  break;
            case 2: myState = PlayerState.Gliding; myCC.material = glidingPhysiMat;
                m_Animator.SetBool("Gliding", true); break;
            case 3: myState = PlayerState.Takeover; break;
            case 4: myState = PlayerState.Outside;
                myNav.enabled = false; break;
            case 5: myState = PlayerState.Entry; break;
            case 6: myState = PlayerState.Exit;
                fade.FadeOut(1.0f);
                myNav.enabled = false;
                seScript.SEPlay(6); break;
            case 7: myState = PlayerState.Throw;
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
        
        
        if (Input.GetButtonDown("XboxR") || Input.GetKeyDown(KeyCode.L))
        {
            m_Animator.Play("hippari");
        }
        else if (Input.GetButtonDown("XboxL") || Input.GetKeyDown(KeyCode.K))
        {
            m_Animator.Play("moti");
        }

        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O)
            //IsKick()
            )
        {
            //rb.velocity = transform.forward * kickSpeed;
            rb.AddForce(transform.forward * kickSpeed * nikuspeed, ForceMode.VelocityChange);
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
        //Debug.Log(rb.velocity);
        cameraScript.GlidingRotation(inputHorizontal);
        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O)
            //IsKick()
            )
        {
            rb.velocity = transform.forward * kickSpeed * nikuspeed;
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
        rb.velocity = Vector3.zero;
        m_Animator.SetBool("HavingBasket", false);
        ChangeCartCheck();
        Vector3 dis = canGetCart.transform.position;// + canGetCart.transform.forward * (CartRelatedData.cartNavPoint);
        myNav.destination = new Vector3(dis.x, -0.8f, dis.z);
        if (Vector3.Distance(myNav.destination, transform.position) < 1.5f)
        {
            //transform.position = myNav.destination;
            //transform.rotation = canGetCart.transform.rotation;
            myNav.enabled = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            //canGetCart = nextCart;
            scScript.SetBasketParent(transform);
            CatchCart();
        }
    }

    /// <summary>追い出され</summary>
    private void OutSide()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("BBAButtobiLoop") && fade.IsFadeEnd())
        {
            if(transform.parent != null)
            {
                GameObject bull = transform.root.gameObject;
                transform.parent = null;
                Destroy(bull);
            }
            transform.position = exitPoint.position;
            transform.LookAt(exitPoint.position+exitPoint.forward);
            cameraScript.Oidashi(exitPoint);
            fade.FadeIn(1.0f);

            m_Animator.Play("BBAButtobiFukki");

        }
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("NoCart") && myNav.enabled == false && fade.IsFadeEnd())
        {
            cameraScript.CameraReset();
            myNav.enabled = true;
            myNav.destination = entrancePoint.position;
        }
        if (Vector3.Distance(myNav.destination, transform.position) < 1.0f)
        {
            seScript.OnePlay(4);
            //int rand = Random.Range(10,12);
            //seScript.OnePlay2(rand);

            transform.position = myNav.destination;
            myNav.enabled = false;
            myCC.enabled = true;
            //rb.velocity = Vector3.one;
            rb.isKinematic = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            scScript.BasketActive(true);
            scScript.SetBasketParent(transform);
            scScript.BasketOut();
            flyHit = false;
            ChangeState(0);
        }
    }

    /// <summary>入店</summary>
    private void Entry()
    {
        if (Vector3.Distance(myNav.destination, transform.position) < 1.0f)
        {
            seScript.OnePlay(4);
            int rand = Random.Range(10, 13);
            seScript.OnePlay2(rand);
            transform.position = myNav.destination;
            myNav.enabled = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            cameraScript.Entry();
            scScript.BasketActive(true);
            ChangeState(0);
        }
    }

    /// <summary>退店</summary>
    private void Exit()
    {
        if (myNav.enabled == false && fade.IsFadeEnd())
        {
            if (transform.parent != null)
            {
                GameObject bull = transform.root.gameObject;
                transform.parent = null;
                Destroy(bull);
            }
            transform.position = entrancePoint.position;
            transform.LookAt(playerExitPoint);
            cameraScript.ChangeState(3);
            fade.FadeIn(1.0f);
            myNav.enabled = true;
            myNav.destination = playerExitPoint.position;
            int rand = Random.Range(7, 9);
            seScript.OnePlay2(rand);

        }

        if (Vector3.Distance(myNav.destination, transform.position) < 1.0f)
        {
            if (!fade.IsFadeOutOrIn())
            {
                fade.FadeOut(1.0f);
                if(resultCamera != null) resultCamera.ScreenCapture();

            }
            if (fade.IsFadeEnd())
            {
                if (resultCamera != null) resultCamera.Stop();

                SceneManager.LoadScene("Result");
            }
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

    /// <summary>上方向に2回連続で押されたか</summary>
    private bool IsKick()
    {//　移動キーを押した
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            //　最初に1回押していない時は押した事にする
            if (!push)
            {
                push = true;
                //　最初に移動キーを押した時にその方向ベクトルを取得
                direction = new Vector2(horizontal, vertical);
                nowTime = 0f;
            }
            else
            {
                //　2回目に移動キーを押した時の方向ベクトルを取得
                var nowDirection = new Vector2(horizontal, vertical);

                //　押した方向がリミットの角度を越えていない　かつ　制限時間内に移動キーが押されていれば走る
                if (Vector2.Angle(nowDirection, direction) < limitAngle
                && nowTime <= nextButtonDownTime)
                {
                    nowTime = 0f;
                    push = false;
                    return true;
                }
            }
        }

        if (Input.GetAxisRaw("XboxLeftVertical") > 0)
        {
            float horizontal = Input.GetAxisRaw("XboxLeftHorizontal") ;
            float vertical = Input.GetAxisRaw("XboxLeftVertical");

            //　最初に1回押していない時は押した事にする
            if (!push)
            {
                push = true;
                //　最初に移動キーを押した時にその方向ベクトルを取得
                direction = new Vector2(horizontal, vertical);
                nowTime = 0f;
            }
            else if(stickBack)
            {
                //　2回目に移動キーを押した時の方向ベクトルを取得
                var nowDirection = new Vector2(horizontal, vertical);

                //　押した方向がリミットの角度を越えていない　かつ　制限時間内に移動キーが押されていれば走る
                if (Vector2.Angle(nowDirection, direction) < limitAngle
                && nowTime <= nextButtonDownTime)
                {
                    nowTime = 0f;
                    push = false;
                    stickBack = false;
                    return true;
                }
            }

        }

        //　最初の移動キーを押していれば時間計測
        if (push)
        {
            if (Input.GetAxisRaw("XboxLeftVertical") <= 0) stickBack = true;

            //　時間計測
            nowTime += Time.deltaTime;

            if (nowTime > nextButtonDownTime)
            {
                push = false;
                stickBack = false;
            }

        }
        return false;
    }

    /// <summary>カゴを投げ当てたカートに乗り移る </summary>
    /// <param name="nextcart">乗り移るカート</param>
    public void ChangeCart(GameObject nextcart)
    {
        if (myState == PlayerState.Exit) return;
        flyHit = true;
        scScript.BasketActive(true);
        ReleaseCart(false);
        canGetCart = nextcart;
        myNav.enabled = true;
        //canGetCart = nextCart.transform.Find("BackHitArea").gameObject;
        Vector3 basPos = canGetCart.transform.position - canGetCart.transform.forward * 0.1f;
        basPos.y = CartRelatedData.cartInBagLocalPosY;
        scScript.SetBasketGlobalPos(basPos);
        scScript.SetBasketAngle(canGetCart.transform.rotation);
        scScript.SetBasketParent(nextcart.transform);
        //myNav.destination = nextCart.transform.position + nextCart.transform.forward * (-1.5f);
        //ChangeState(3);
    }

    /// <summary>かごを当てたカートが店の外にないかのチェック</summary>
    private void ChangeCartCheck()
    {
        if ((canGetCart.transform.position + canGetCart.transform.forward * (CartRelatedData.cartNavPoint)).x < m_gmScript.StagePos1().transform.position.x) //xが外(左,-)
        {
            canGetCart.transform.position = new Vector3(m_gmScript.StagePos1().transform.position.x + (-CartRelatedData.cartNavPoint), 0, canGetCart.transform.position.z);
            //canGetCart.transform.position += Vector3.right * (-CartRelatedData.cartNavPoint);
        }
        if ((canGetCart.transform.position + canGetCart.transform.forward * (CartRelatedData.cartNavPoint)).x > m_gmScript.StagePos2().transform.position.x) //xが外(右,+)
        {
            canGetCart.transform.position = new Vector3(m_gmScript.StagePos2().transform.position.x - (-CartRelatedData.cartNavPoint), 0, canGetCart.transform.position.z);
        }
        if ((canGetCart.transform.position + canGetCart.transform.forward * (CartRelatedData.cartNavPoint)).z < m_gmScript.StagePos1().transform.position.z) //zが外(後,-)
        {
            canGetCart.transform.position = new Vector3(canGetCart.transform.position.x, 0, m_gmScript.StagePos1().transform.position.z + (-CartRelatedData.cartNavPoint));
        }
        if ((canGetCart.transform.position + canGetCart.transform.forward * (CartRelatedData.cartNavPoint)).z > m_gmScript.StagePos2().transform.position.z) //zが外(前,+)
        {
            canGetCart.transform.position = new Vector3(canGetCart.transform.position.x, 0, m_gmScript.StagePos2().transform.position.z - (-CartRelatedData.cartNavPoint));
        }
    }

    /// <summary>カート壊れる</summary>
    public void BreakCart(bool change = true)
    {
        //cartRotatePoint.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 0, 0));
        scScript.SetBasketParent(transform);
        scScript.BasketOut();
        Destroy(myCart);
        myCart = null;
        if(change)ChangeState(0);
        
    }

    /// <summary>カート壊れる</summary>
    public void BreakCart2()
    {
        //cartRotatePoint.transform.localRotation = Quaternion.AngleAxis(0, new Vector3(1, 0, 0));
        //scScript.SetBasketParent(transform);
        myCart.transform.localPosition = Vector3.forward * CartRelatedData.cartLocalPosZ;
        scScript.BasketIn();
        Destroy(mySecondCart);
        mySecondCart = null;
        //ChangeState(0);
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
            myCart.GetComponent<CartBody>().SetCart(cart.GetComponent<CartStatusWithCart>());

            cart.transform.position = cartPos + transform.forward * CartRelatedData.cartLocalPosZ;
            Vector3 relativePos = myCart.transform.position - transform.position;
            relativePos.y = 0; //上下方向の回転はしないように制御
            transform.rotation = Quaternion.LookRotation(relativePos);
            cart.transform.rotation = Quaternion.LookRotation(relativePos);
            //havedCart = cart;
            BreakCart(change);
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
            BreakCart2();
            //Destroy(mySecondCart);
            //myCart.transform.localPosition = Vector3.forward * CartRelatedData.cartLocalPosZ;
            //scScript.BasketIn();
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

    public float GetFowardSpeed()
    {
        Vector3 result = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //Debug.Log(rb.velocity.sqrMagnitude);
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

    private void NikuManager()
    {
        if(nikuspeed == 2)
        {
            nikuTime += Time.deltaTime;
            if(nikuTime > nikuTimeLimit)
            {
                nikuspeed = 1;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy" )
        {
            if (!collision.gameObject.GetComponent<SecurityGuard>().StateChasing()) return;
            enemyHit = true;
            scScript.BasketActive(false);
            transform.LookAt(collision.transform);
        }
        if (collision.transform.tag == "Cart"
            //&& havedCart == null 
            && mySecondCart == null
            && GetState() < PlayerState.Takeover
            && scScript.IsCatchBasket())
        {
            canGetCart = collision.gameObject;
            canGet = true;
        }
        if(collision.transform.tag == "Carts"
            && myCart == null
            && GetState() < PlayerState.Takeover
            && scScript.IsCatchBasket())
        {
            if(collision.gameObject.GetComponent<CartSpown>().IsCartGet())
            {
                canGetCart = Instantiate(cartRigidPrefab);
                canGet = true;
            }
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
        if (collision.transform.name.Contains("Yakiniku"))
        {
            nikuspeed = 2;
            nikuTime = 0.0f;
            Destroy(collision.gameObject);
        }
    }
    
}
