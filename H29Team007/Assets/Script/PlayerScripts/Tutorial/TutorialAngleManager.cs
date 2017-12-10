using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAngleManager : MonoBehaviour {

    private List<SpringBoneArrange> springBones;

    private Transform youngestChild;

    private float nextHeight = 0.0f;

    private TutorialShopping scScript;
    //private CartStatusWithPlayer csScript;

    // Use this for initialization
    void Start()
    {
        springBones = new List<SpringBoneArrange>();
        youngestChild = transform.Find("YoungestChild");
        scScript = transform.root.gameObject.GetComponent<TutorialShopping>();
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


        //Debug.Log(springBones[springBones.Count - 1].transform.eulerAngles.x - 180);
        if (Mathf.Abs(springBones[springBones.Count - 1].transform.eulerAngles.x - 180) <= scScript.GetBaggageLimitAngle())
        {
            scScript.BaggegeFall(transform.root.position);
        }
    }

    public void SetChildren(Transform newChild, float height)
    {
        springBones.Add(newChild.GetComponent<SpringBoneArrange>());
        if (springBones.Count == 1)
        {
            newChild.parent = transform;
            springBones[0].transform.localPosition = Vector3.zero;
            springBones[0].SetLocalRotation();
            springBones[0].ChildSet(youngestChild);

        }
        else
        {
            for (int i = 0; i < springBones.Count; i++)
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
