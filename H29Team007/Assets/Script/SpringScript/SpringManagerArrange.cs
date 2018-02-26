using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringManagerArrange : MonoBehaviour {
    
    private List<SpringBoneArrange> springBones;

    private Transform youngestChild;

    private float nextHeight = 0.0f;

    private ShoppingCount scScript;
    //private CartStatusWithPlayer csScript;

    private Player ps;
    private float muteki = 0.0f;

    // Use this for initialization
    void Start ()
    {
        ps = transform.root.GetComponent<Player>();
        springBones = new List<SpringBoneArrange>();
        youngestChild = transform.Find("YoungestChild");
        scScript = transform.root.gameObject.GetComponent<ShoppingCount>();
        //csScript = transform.root.gameObject.GetComponent<CartStatusWithPlayer>();

        springBones.Clear();
    }

    private void LateUpdate()
    {
        if (springBones.Count < 1) return;

        //float allAngle = 0;

        for (int i = 0; i < springBones.Count; i++)
        {
            springBones[i].UpdateSpring();
            //allAngle += Mathf.Abs(springBones[i].transform.localRotation.x);
        }

        if(ps.NikuSpeed() == 2)
        {
            muteki = 1.0f;
            return;
        }
        else if(muteki > 0.0f)
        {
            muteki -= Time.deltaTime;
            return;
        }

        if (!MainGameDate.IsStart() || ps.GetFowardSpeed() < 0.1f * 0.1f) return;

        //Debug.Log(springBones[springBones.Count - 1].transform.eulerAngles.x - 180);
        if (Mathf.Abs(springBones[springBones.Count - 1].transform.eulerAngles.x - 180) <= scScript.GetBaggageLimitAngle()
            || Mathf.Abs(springBones[springBones.Count - 1].transform.eulerAngles.z - 180) <= scScript.GetBaggageLimitAngle())
        {
            scScript.BaggegeFall(transform.root.position);
            Debug.Log("角度オーバー");
        }
    }

    public void SetChildren(Transform newChild,float height)
    {
        springBones.Add(newChild.GetComponent<SpringBoneArrange>());
        if(springBones.Count == 1)
        {
            newChild.parent = transform;
            springBones[0].transform.localPosition = Vector3.zero;
            springBones[0].SetLocalRotation();
            springBones[0].ChildSet(youngestChild);
            
        }
        else
        {
            for(int i = 0; i < springBones.Count; i++)
            {
                springBones[i].RotateReset();
            }
            springBones[springBones.Count - 2].ChildSet(newChild, nextHeight);
            springBones[springBones.Count - 1].SetLocalRotation();
            springBones[springBones.Count - 1].ChildSet(youngestChild);
        }
        nextHeight = height;
    }

    public void NullChildren()
    {
        for (int i = 0; i < springBones.Count; i++)
        {
            springBones[i].ChildNull();
        }
        youngestChild.parent = transform;
        youngestChild.position = transform.position;
        springBones.Clear();
    }

    //闘牛回転用---
    public Transform YoungestChild()
    {
        return youngestChild;
    }

    public float NextHeight()
    {
        return nextHeight;
    }
}
