﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBasketFly : MonoBehaviour {

    private Rigidbody m_rigid;
    private GameObject player;

    public GameObject cartRigidPrefab;

    private bool oneHit = false;
    private bool punch = false;
    private bool onthewall = true;


    [SerializeField, Header("爆発のプレハブ")]
    private GameObject explosionPrefub;
    public LayerMask mask;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //enabled = false;
        m_rigid = GetComponent<Rigidbody>();
        m_rigid.AddForce(player.transform.up * CartRelatedData.flyBasketUpPower, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (!punch && transform.position.y < CartRelatedData.flyBasketStartPosY)
        {
            m_rigid.velocity = player.transform.forward * CartRelatedData.flyBasketPunchPower;
            //m_rigid.AddForce(player.transform.forward * 20.0f, ForceMode.VelocityChange);
            punch = true;
        }
        if (m_rigid.velocity == Vector3.zero)
        {
            if (onthewall) //壁（障害物の上にいるかどうか）
            {
                //mapstageレイヤーに当たるレイを飛ばす
                RaycastHit[] hitInfo
                    = Physics.RaycastAll(transform.position + transform.up * 0.5f, -Vector3.up, 1.0f, mask);

                onthewall = false;
                //当たったオブジェクトの中にwallタグのやつがいるか探す
                if (hitInfo.Length != 0)
                {
                    for (int i = 0; i < hitInfo.Length; i++)
                    {
                        if (onthewall) continue;
                        onthewall = (hitInfo[i].collider.tag == "Wall");
                        //Debug.Log(hitInfo[i].collider.name);
                    }
                }

                if (onthewall) //障害物の上にいるなら籠を動かす
                {
                    GameObject explosion = Instantiate(explosionPrefub);
                    explosion.transform.position = transform.position;
                    float x = Random.Range(-5.0f, 5.0f);
                    float z = Random.Range(-5.0f, 5.0f);

                    Vector3 moveVec = new Vector3(x, 5.0f, z);
                    m_rigid.velocity = moveVec;
                }


            }
            else
            {
                //Debug.DrawRay(transform.position + transform.up * 0.5f, -Vector3.up, Color.red, 1.0f);
                m_rigid.constraints = RigidbodyConstraints.FreezePositionY;
                GetComponent<BoxCollider>().isTrigger = true;
            }

        }
        if (transform.position.y < -1) //デバッグ中に下に落ちた('ω')
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            transform.rotation = new Quaternion(0, 0, 0, 0);
            m_rigid.velocity = Vector3.zero;
            m_rigid.constraints = RigidbodyConstraints.FreezePositionY;
            GetComponent<BoxCollider>().isTrigger = true;

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (oneHit) return;
        if (collision.transform.tag == "Cart")
        {
            oneHit = true;
            //Debug.Log(gameObject.name);
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //transform.position = collision.transform.position + collision.transform.
            player.GetComponent<MTPlayer>().ChangeCart(collision.gameObject);
            player.GetComponent<TutorialShopping>().BaggegeParentPlayer();
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "BBA")
        {
            oneHit = true;
            GameObject enemyCart = collision.transform.Find("EnemyCart").gameObject;
            //Debug.Log(collision.gameObject.name);
            //collision.gameObject.GetComponent<BBACartCount>().BaggegeFall(collision.transform.position);
            EnemyCart ec = enemyCart.GetComponent<EnemyCart>();
            ec.Independence();

            GameObject newcart = ec.NewCart();

            newcart.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            player.GetComponent<MTPlayer>().ChangeCart(newcart.gameObject);
            player.GetComponent<TutorialShopping>().BaggegeParentPlayer();
            collision.transform.tag = "Cutomer";
            Destroy(enemyCart.gameObject);
            Destroy(gameObject);
        }
        else if (collision.transform.tag != "Player")
        {
            TutorialShopping sc = player.GetComponent<TutorialShopping>();
            sc.BaggegeFall(transform.position);
        }


    }
}
