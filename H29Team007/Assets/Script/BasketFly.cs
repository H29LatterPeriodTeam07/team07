using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketFly : MonoBehaviour
{

    private Rigidbody m_rigid;
    private GameObject player;

    public GameObject cartRigidPrefab;

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
        if(transform.position.y < -1) //デバッグ中に下に落ちた('ω')
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
        if (collision.transform.tag == "Cart")
        {
            //Debug.Log(gameObject.name);
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //transform.position = collision.transform.position + collision.transform.
            player.GetComponent<Player>().ChangeCart(collision.gameObject);
            player.SendMessage("BaggegeParentPlayer", SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "EnemyCart")
        {
            //Debug.Log(collision.gameObject.name);
            EnemyCart ec = collision.gameObject.GetComponent<EnemyCart>();
            ec.Independence();


            GameObject newcart = ec.NewCart();

            newcart.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            player.GetComponent<Player>().ChangeCart(newcart.gameObject);
            player.SendMessage("BaggegeParentPlayer", SendMessageOptions.DontRequireReceiver);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.transform.tag != "Player")
        {
            ShoppingCount sc = player.GetComponent<ShoppingCount>();
            sc.BaggegeFall(transform.position);
        }


    }
}
