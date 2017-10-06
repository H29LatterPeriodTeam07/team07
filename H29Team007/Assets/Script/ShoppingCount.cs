using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCount : MonoBehaviour {

    private float onPosition;

    private List<Transform> myBaggege;

    // Use this for initialization
    void Start ()
    {
        myBaggege = new List<Transform>();
        onPosition = 0.5f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlusY(float y)
    {
        onPosition += y;
    }

    public void Reset()
    {
        onPosition = 0.5f;
    }

    public float GetY()
    {
        return onPosition;
    }

    public void AddBaggege(Transform baggege)
    {
        baggege.parent = transform;
        myBaggege.Add(baggege);
    }

    /// <summary>荷物落とすときの処理</summary>
    public void BaggegeFall()
    {
        for (int i = 0; i < myBaggege.Count; i++)
        {
            float x = Random.Range(-3.0f, 3.0f);
            float z = Random.Range(-3.0f, 3.0f);
            float sp = Random.Range(5.0f, 10.0f);

            Vector3 pos = new Vector3(transform.position.x + x, 0, transform.position.z + z);

            FallDown fall = myBaggege[i].GetComponent<FallDown>();
            fall.enabled = true;
            fall.SetPoint(pos, sp);

            myBaggege[i].parent = null;

        }
        myBaggege.Clear();
        GetComponent<ShoppingCount>().Reset();
    }
}
