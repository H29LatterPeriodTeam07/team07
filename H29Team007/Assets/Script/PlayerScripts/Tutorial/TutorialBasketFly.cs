using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBasketFly : MonoBehaviour {

    private Rigidbody m_rigid;
    private GameObject player;

    public GameObject cartRigidPrefab;

    private bool oneHit = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //enabled = false;
        m_rigid = GetComponent<Rigidbody>();
        m_rigid.AddForce(player.transform.forward * 20.0f, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rigid.velocity == Vector3.zero)
        {

            m_rigid.constraints = RigidbodyConstraints.FreezePositionY;
            GetComponent<BoxCollider>().isTrigger = true;
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
            player.GetComponent<TutorialPlayer>().ChangeCart(collision.gameObject);
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
            player.GetComponent<TutorialPlayer>().ChangeCart(newcart.gameObject);
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
