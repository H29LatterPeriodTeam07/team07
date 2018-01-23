using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yakiniku : MonoBehaviour {

    private float[] r, g, b;
    private float[] rd, gd, bd;
    public GameObject changeObject;
    private Renderer changeMat;
    [Header("ぺちゃモデルがあるやつは、ここに元のモデル(歩く方)を入れる")]
    public GameObject changeObject2;
    private Renderer changeMat2;

    public GameObject nikuPrefab;

	// Use this for initialization
	void Start () {
        changeMat = changeObject.GetComponent<Renderer>();
        //Debug.Log(chengeMat.materials[0]);
        r = new float[changeMat.materials.Length];
        g = new float[changeMat.materials.Length];
        b = new float[changeMat.materials.Length];
        rd = new float[changeMat.materials.Length];
        gd = new float[changeMat.materials.Length];
        bd = new float[changeMat.materials.Length];
        for (int i = 0; i < changeMat.materials.Length; i++)
        {
            r[i] = changeMat.materials[i].color.r;
            g[i] = changeMat.materials[i].color.g;
            b[i] = changeMat.materials[i].color.b;
            rd[i] = changeMat.materials[i].color.r;
            gd[i] = changeMat.materials[i].color.g;
            bd[i] = changeMat.materials[i].color.b;
        }
        if(changeObject2 != null) changeMat2 = changeObject2.GetComponent<Renderer>();

    }
	
	// Update is called once per frame
	void Update () {
        //if(transform.parent != null)Fire();

    }

    public void Fire()
    {

        for (int i = 0; i < changeMat.materials.Length; i++)
        {
            changeMat.materials[i].color = new Color(r[i], g[i], b[i], changeMat.material.color.a);
            r[i] -= (rd[i] / 10.0f) * Time.deltaTime;
            g[i] -= (gd[i] / 10.0f) * Time.deltaTime;
            b[i] -= (bd[i] / 10.0f) * Time.deltaTime;
            if (changeObject2 != null) changeMat2.materials[i].color = changeMat.materials[i].color;
        }
        if(r[0] < 0)
        {
            GameObject niku = Instantiate(nikuPrefab);
            niku.transform.position = transform.position;
            transform.root.GetComponent<ShoppingCount>().DeleteBaggege(transform);
        }
    }
}
