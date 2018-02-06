using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunOverObject : MonoBehaviour
{
    public AudioClip m_se;
    [SerializeField, Header("元のモデル")]
    public GameObject m_model;
    [SerializeField, Header("ぐてモデル")]
    public GameObject m_gutemodel;

    private AudioSource m_AS;
    [SerializeField, Header("カートに乗った後の高さ")]
    private float runOverAfterHeight = 1.0f;

    private NavMeshAgent myNav;
    private Collider myCollider;
    Rigidbody rb;
    Animator m_Anime;

    // Use this for initialization
    void Start()
    {
        myNav = GetComponent<NavMeshAgent>();
        myCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        m_AS = GetComponent<AudioSource>();
        m_Anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NavReStart()
    {
        myNav.enabled = true;
        myCollider.enabled = true;
        if (m_Anime != null)
        {
            m_Anime.SetTrigger("VO");
        }
        //ここにアニメ再開入れるかも
        if (transform.tag == "Bull")
        {
            transform.Find("BullHitArea").gameObject.SetActive(true);
        }
        if (m_model != null && m_gutemodel != null)
        {
            m_model.SetActive(true);
            m_gutemodel.SetActive(false);
        }
    }

    /// <summary>navmeshのポジション移動</summary>
    /// <param name="pos">移動先のポイント</param>
    public void NavPosition(Vector3 pos)
    {
        myNav.Warp(pos); //navmeshのポジション移動
    }

    /// <summary>レジを通したレジ袋用の関数</summary>
    /// <param name="basket">袋を入れるカゴ</param>
    public void SetPlasticBagPos(GameObject basket)
    {
        //myNav.enabled = false;

        var sc = basket.transform.root.GetComponent<ShoppingCount>();
        //myCollider = GetComponent<BoxCollider>();
        //myCollider.enabled = false;
        Vector3 v = basket.transform.position;
        Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
        transform.position = nimotuPos;
        //Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
        //transform.position = nimotuPos;

        sc.AddBaggege(transform);

        sc.PlusY(runOverAfterHeight);
    }

    public float GetHeight()
    {
        return runOverAfterHeight;
    }

    /// <summary>エネミーのプレイヤーが見えてるかのパクリのパクリ</summary>
    private bool CanGetEnemy(Transform cart)
    {
        if(transform.tag == "Enemy")
        {
            SecurityGuard sg = gameObject.GetComponent<SecurityGuard>();
            if (sg.Guard()) return false;
        }
        //カートからエネミーへの方向ベクトル(ワールド座標系)
        Vector3 directionToEnemy = transform.position - cart.position;
        // エネミーの正面向きベクトルとエネミーへの方向ベクトルの差分角度
        float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

        // 引ける角度の範囲内にエネミーがいるかどうかを返却する
        return (Mathf.Abs(angleToEnemy) <= 90);
    }

    private void BullOver(Transform player)
    {
        var ps = player.GetComponent<Player>();
        if (!ps.CanGetBull(transform)) return;
        var sc = player.GetComponent<ShoppingCount>();
        if (!sc.IsCatchBasket() || sc.IsBaggegeMax(ps.MySecondCart())) return;
        myNav.enabled = false;
        myCollider.enabled = false;
        sc.AddBaggege(transform, ps.MySecondCart(), 2);
        transform.Find("BullHitArea").gameObject.SetActive(false);
        if (m_model != null)
        {
            m_model.SetActive(false);
            m_gutemodel.SetActive(true);
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        if (transform.parent != null) return;
        if (other.name == "FrontHitArea")//プレイヤーババア用　敵ババアが特売品を轢く処理は頑張って
        {
            if (other.transform.root.GetComponent<Player>().GetFowardSpeed() <= 0.1f * 0.1f) return;
            if (transform.tag == "Bull")
            {
                BullOver(other.transform.root);
                return;
            }
            if (transform.tag == "Enemy" && !CanGetEnemy(other.transform)
                || transform.tag == "BBA" && !CanGetEnemy(other.transform)) { return; }
            var sc = other.transform.root.GetComponent<ShoppingCount>();
            if (transform.tag == "Animal" && !sc.IsHumanMoreThanAnimal()) return;
            //if (!sc.IsCatchBasket() || sc.IsBaggegeMax(other.transform.parent.gameObject)) return;
            if (m_Anime != null)
            {
                m_Anime.SetTrigger("Kago");
            }
            myNav.enabled = false;
            myCollider.enabled = false;
            //ここにアニメ停止や変更入れるかも
            Vector3 v = other.transform.parent.position;
            //bool a = sc.IsHumanMoreThanAnimal();
            Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
            transform.position = nimotuPos;
            //Vector3 v = other.transform.parent.position;
            //Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
            //transform.position = nimotuPos;

            sc.AddBaggege(transform,other.transform.parent.gameObject);
            sc.PlusY(runOverAfterHeight);
            if (transform.tag == "Animal")
            {
                if (m_model != null)
                {
                    m_model.SetActive(false);
                    m_gutemodel.SetActive(true);
                }
            }
            m_AS.clip = m_se;
            m_AS.Play();
        }

        if (other.name == "EnemyFrontHitArea")//敵ババア用　敵ババアが特売品を轢く処理は頑張って
        {

            if (transform.tag == "Animal")
            {
                var sc = other.transform.root.GetComponent<BBACartCount>();
                if (transform.tag == "Animal" && !sc.IsHumanMoreThanAnimal()) return;
                myNav.enabled = false;
                myCollider.enabled = false;
                //ここにアニメ停止や変更入れるかも
                Vector3 v = other.transform.parent.transform.position;
                Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
                transform.position = nimotuPos;
                sc.AddBaggege(transform);
                //transform.parent = other.transform.root;
                if (m_Anime != null)
                {
                    m_Anime.SetTrigger("Kago");
                }
                sc.PlusY(runOverAfterHeight);
                if (m_model != null)
                {
                    m_model.SetActive(false);
                    m_gutemodel.SetActive(true);
                }
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.isKinematic = true;
                }
            }
        }
        if (other.name == "BullHitArea")//闘牛用
        {
            var sc = other.transform.root.GetComponent<BullCount>();
            if (transform.tag == "Animal" && !sc.IsHumanMoreThanAnimal()) return;
            myCollider.enabled = false;
            //ここにアニメ停止や変更入れるかも
            Vector3 v = other.transform.parent.transform.position;
            Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
            transform.position = nimotuPos;
            sc.AddBaggege(transform);
            //transform.parent = other.transform.root;
            if (m_Anime != null)
            {
                m_Anime.SetTrigger("Kago");
            }
            if (m_model != null)
            {
                m_model.SetActive(false);
                m_gutemodel.SetActive(true);
            }
            sc.PlusY(runOverAfterHeight);
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
            if(myNav != null) myNav.enabled = false;

        }
    }
    //public void OnTriggerStay(Collider other)
    //{
    //  // if (m_model == null) return;
    //    if (other.name == "Plane")
    //    {
    //        if (m_model != null && m_gutemodel !=null)
    //        {
    //            m_model.SetActive(true);
    //            m_gutemodel.SetActive(false);
    //        }
    //    }

    //}

}
