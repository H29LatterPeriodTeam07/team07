using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody rb;

    private GameObject myCart;

    private float moveSpeed = 3f;
    private float rotateSpeed = 60.0f;

    public GameObject cartBodyPrefab;
    public GameObject cartRigidPrefab;

    private CartStatusWithPlayer myCartStatus;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCartStatus = GetComponent<CartStatusWithPlayer>();
    }

    void Update()
    {
        

        inputHorizontal = (Input.GetAxisRaw("PS4LeftHorizontal") != 0)?Input.GetAxisRaw("PS4LeftHorizontal"):Input.GetAxisRaw("Horizontal");
        inputVertical = (Input.GetAxisRaw("PS4LeftVertical") != 0) ? Input.GetAxisRaw("PS4LeftVertical") : Input.GetAxisRaw("Vertical");
        
        if (myCart != null)
        {
            if (Input.GetButtonDown("PS4Cross") ||Input.GetKeyDown(KeyCode.E))
            {
                BreakCart();

                GameObject cart = Instantiate(cartRigidPrefab);

                //離したカートに現在の耐久値を渡す
                myCartStatus.SetCart(cart.GetComponent<CartStatusWithCart>());

                Vector3 cartPos = new Vector3(transform.position.x, 0, transform.position.z);
                cart.transform.position = cartPos + transform.forward * 2.0f;
                Vector3 relativePos = myCart.transform.position - transform.position;
                relativePos.y = 0; //上下方向の回転はしないように制御
                transform.rotation = Quaternion.LookRotation(relativePos);
                cart.transform.rotation = Quaternion.LookRotation(relativePos);
            }
        }
        
    }

    void FixedUpdate()
    {
        if(myCart == null)
        {
            CartOffMove();
        }
        else
        {
            CartOnMove();
        }
    }

    private void CartOnMove()
    {
        transform.Rotate(new Vector3(0, inputHorizontal * rotateSpeed * Time.deltaTime, 0));

        Vector3 moveForward = transform.forward * inputVertical * 3.0f;

        rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);
    }

    private void CartOffMove()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }

    public bool IsCart()
    {
        return (myCart != null);
    }

    public void BreakCart()
    {
        Destroy(myCart);
    }

    public void BaggegeFall()
    {
        List<Transform> myList = new List<Transform>();

        Transform[] nimotu = new Transform[10];
        int a = 0;
        foreach (Transform chird in transform)
        {
            if (chird.tag == "Enemy")
            {
                float x = Random.Range(-3.0f, 3.0f);
                float z = Random.Range(-3.0f, 3.0f);
                float sp = Random.Range(2.0f, 5.0f);

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
                myCart = Instantiate(cartBodyPrefab);
                Vector3 cartPos = new Vector3(transform.position.x, 0, transform.position.z);
                myCart.transform.position = cartPos + transform.forward * 2.0f;
                Vector3 relativePos = myCart.transform.position - transform.position;
                relativePos.y = 0; //上下方向の回転はしないように制御
                transform.rotation = Quaternion.LookRotation(relativePos);
                myCart.transform.rotation = Quaternion.LookRotation(relativePos);
                myCart.transform.parent = transform;
            }
        }
    }
}
