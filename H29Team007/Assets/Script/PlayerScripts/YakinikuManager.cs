using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YakinikuManager : MonoBehaviour
{
    
    private Player ps;
    public GameObject sparkPrefub;
    private GameObject spark;
    private bool isAboveRoof = false;
    private Yakiniku yaki;

    // Use this for initialization
    void Start()
    {

        ps = transform.root.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.root.tag != "Player" || ps.GetState() > Player.PlayerState.Takeover || transform.position.y <= MainGameDate.ROOFSHEIGHT)
        {
            if (isAboveRoof)
            {
                Destroy(spark);
                isAboveRoof = false;
                yaki = null;
            }
            return;
        }
        if (transform.position.y > MainGameDate.ROOFSHEIGHT)
        {
            if (isAboveRoof)
            {
                yaki = transform.parent.GetComponent<Yakiniku>();
                if (yaki == null) return;
                Debug.Log("熱いぜ");
                spark.transform.position = new Vector3(transform.position.x, MainGameDate.ROOFSHEIGHT, transform.position.z);
                if (ps.GetFowardSpeed() > 0.1f * 0.1f)yaki.Fire();
            }
            else
            {
                yaki = transform.parent.GetComponent<Yakiniku>();
                if (yaki == null) return;
                Debug.Log("天井に着いた");
                isAboveRoof = true;
                spark = Instantiate(sparkPrefub,new Vector3(transform.position.x, MainGameDate.ROOFSHEIGHT, transform.position.z), Quaternion.identity, transform.root);
                spark.transform.eulerAngles = new Vector3(90, 0, 0);
            }
        }
    }
    
}
