using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialRunOver : MonoBehaviour {

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
        if (m_Anime != null)
        {
            if (transform.position.y > 0.3f)
            {
                m_Anime.SetTrigger("Kago");
            }
            else
            {
                m_Anime.SetTrigger("Trigger");
            }
        }
    }

    public void NavReStart()
    {
        myNav.enabled = true;
        myCollider.enabled = true;
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

        var sc = basket.transform.root.GetComponent<TutorialShopping>();
        Vector3 v = basket.transform.position;
        Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
        transform.position = nimotuPos;

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
        //カートからエネミーへの方向ベクトル(ワールド座標系)
        Vector3 directionToEnemy = transform.position - cart.position;
        // エネミーの正面向きベクトルとエネミーへの方向ベクトルの差分角度
        float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

        // 引ける角度の範囲内にエネミーがいるかどうかを返却する
        return (Mathf.Abs(angleToEnemy) <= 90);
    }
    

    public void OnTriggerEnter(Collider other)
    {
        if (transform.parent != null) return;
        if (other.name == "FrontHitArea")
        {
            if (transform.tag == "Enemy" && !CanGetEnemy(other.transform)
                || transform.tag == "BBA" && !CanGetEnemy(other.transform)) return;
            var sc = other.transform.root.GetComponent<TutorialShopping>();
            if (transform.tag == "Animal" && !sc.IsHumanMoreThanAnimal()) return;
            if (!sc.IsCatchBasket() || sc.IsBaggegeMax(other.transform.parent.gameObject)) return;
            myNav.enabled = false;
            myCollider.enabled = false;
            //ここにアニメ停止や変更入れるかも
            Vector3 v = other.transform.parent.position;
            bool a = sc.IsHumanMoreThanAnimal();
            Vector3 nimotuPos = new Vector3(v.x, sc.GetY(), v.z);
            transform.position = nimotuPos;

            sc.AddBaggege(transform, other.transform.parent.gameObject);
            sc.PlusY(runOverAfterHeight);
            if (m_model != null)
            {
                m_model.SetActive(false);
                m_gutemodel.SetActive(true);
            }
            m_AS.clip = m_se;
            m_AS.Play();
        }
        
    }
    public void OnTriggerStay(Collider other)
    {
        if (m_model == null) return;
        if (other.name == "Plane")
        {
            m_model.SetActive(true);
            m_gutemodel.SetActive(false);
        }
    }
}
