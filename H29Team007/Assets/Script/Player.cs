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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        if (myCart != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

                myCart.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                myCart.GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezeRotationX |
                    RigidbodyConstraints.FreezeRotationZ |
                    RigidbodyConstraints.FreezePositionY;

                myCart.transform.parent = null;

                myCart = null;
            }
        }
    }

    void FixedUpdate()
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

    public void OnTriggerStay(Collider other)
    {
        if(other.name == "BackHitArea")
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                myCart = other.transform.parent.transform.gameObject;
               // Debug.Log(myCart);
                //transform.LookAt(myCart.transform.forward);
                Vector3 relativePos = myCart.transform.position - transform.position;
                relativePos.y = 0; //上下方向の回転はしないように制御
                transform.rotation = Quaternion.LookRotation(relativePos);
                myCart.transform.parent = transform;
                myCart.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
