using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringManagerArrange : MonoBehaviour {
    
    private List<SpringBoneArrange> springBones;

    private Transform youngestChild;

    // Use this for initialization
    void Start () {
        springBones = new List<SpringBoneArrange>();
        youngestChild = transform.Find("YoungestChild");

        springBones.Clear();
    }

    private void LateUpdate()
    {
        
        for (int i = 0; i < springBones.Count; i++)
        {
            springBones[i].UpdateSpring();
        }
    }

    public void SetChildren(Transform newChild)
    {
        springBones.Add(newChild.GetComponent<SpringBoneArrange>());
        if(springBones.Count == 1)
        {
            newChild.parent = transform;
            springBones[0].ChildSet(youngestChild);
            
        }
        else
        {
            springBones[springBones.Count - 2].ChildSet(newChild);
            springBones[springBones.Count - 1].ChildSet(youngestChild);
        }
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
}
