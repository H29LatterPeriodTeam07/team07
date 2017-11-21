using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBoneArrange : MonoBehaviour {

    //次のボーン
    private Transform child;

    //ボーンの向き
    public Vector3 boneAxis = new Vector3(0.0f, 1.0f, 0.0f);
    
    //バネが戻る力
    public float stiffnessForce = 1.0f;

    //力の減衰力
    public float dragForce = 1.0f;

    public Vector3 springForce = new Vector3(0.0f, -0.05f, 0.0f);
    

    private float springLength;
    private Quaternion localRotation;
    private Transform trs;
    private Vector3 currTipPos;
    private Vector3 prevTipPos;

    private void Awake()
    {
        trs = transform;
        localRotation = transform.localRotation;
    }

    private void Start()
    {
        //springLength = Vector3.Distance(trs.position, child.position);
        //currTipPos = child.position;
        //prevTipPos = child.position;
    }

    public void UpdateSpring()
    {
        if (child == null) return;
        //回転をリセット
        trs.localRotation = Quaternion.identity * localRotation;

        float sqrDt = Time.deltaTime * Time.deltaTime;

        //stiffness
        Vector3 force = trs.rotation * (boneAxis * stiffnessForce) / sqrDt;

        //drag
        force += (prevTipPos - currTipPos) * dragForce / sqrDt;

        force += springForce / sqrDt;

        //前フレームと値が同じにならないように
        Vector3 temp = currTipPos;

        //verlet
        currTipPos = (currTipPos - prevTipPos) + currTipPos + (force * sqrDt);

        //長さを元に戻す
        currTipPos = ((currTipPos - trs.position).normalized * springLength) + trs.position;
        

        prevTipPos = temp;

        //回転を適用；
        Vector3 aimVector = trs.TransformDirection(boneAxis);
        Quaternion aimRotation = Quaternion.FromToRotation(aimVector, currTipPos - trs.position);
        trs.rotation = aimRotation * trs.rotation;
    }

    public void ChildSet(Transform newChild, float height = 1.0f)
    {
        newChild.parent = transform;
        newChild.localPosition = Vector3.up * height;
        springLength = Vector3.Distance(trs.position, newChild.position);
        currTipPos = newChild.position;
        prevTipPos = newChild.position;
        child = newChild;
    }

    public void ChildNull()
    {
        child = null;
    }

    public void RotateReset()
    {
        trs.localRotation = Quaternion.identity * localRotation;
    }
}
