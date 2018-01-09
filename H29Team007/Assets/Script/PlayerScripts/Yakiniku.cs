using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yakiniku : MonoBehaviour {

    private float[] r, g, b;
    private float[] rd, gd, bd;
    public GameObject changeObject;
    private Renderer chengeMat;

    public GameObject nikuPrefab;

	// Use this for initialization
	void Start () {
        chengeMat = changeObject.GetComponent<Renderer>();
        //Debug.Log(chengeMat.materials[0]);
        r = new float[chengeMat.materials.Length];
        g = new float[chengeMat.materials.Length];
        b = new float[chengeMat.materials.Length];
        rd = new float[chengeMat.materials.Length];
        gd = new float[chengeMat.materials.Length];
        bd = new float[chengeMat.materials.Length];
        for (int i = 0; i < chengeMat.materials.Length; i++)
        {
            r[i] = chengeMat.materials[i].color.r;
            g[i] = chengeMat.materials[i].color.g;
            b[i] = chengeMat.materials[i].color.b;
            rd[i] = chengeMat.materials[i].color.r;
            gd[i] = chengeMat.materials[i].color.g;
            bd[i] = chengeMat.materials[i].color.b;
        }

    }
	
	// Update is called once per frame
	void Update () {
        //if(transform.parent != null)Fire();

    }

    public void Fire()
    {

        for (int i = 0; i < chengeMat.materials.Length; i++)
        {
            chengeMat.materials[i].color = new Color(r[i], g[i], b[i], chengeMat.material.color.a);
            r[i] -= (rd[i] / 10.0f) * Time.deltaTime;
            g[i] -= (gd[i] / 10.0f) * Time.deltaTime;
            b[i] -= (bd[i] / 10.0f) * Time.deltaTime;
        }
        if(r[0] < 0)
        {
            GameObject niku = Instantiate(nikuPrefab);
            niku.transform.position = transform.position;
            transform.root.GetComponent<ShoppingCount>().DeleteBaggege(transform);
        }
    }
}
