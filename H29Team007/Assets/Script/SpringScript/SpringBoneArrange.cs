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
        if(newChild.parent.tag == "Bull")
        {
            newChild.localPosition = Vector3.up * height + Vector3.right * -0.3f;
        }

        float x = Random.Range(-0.1f, 0.1f);
        float z = Random.Range(-0.1f, 0.1f);

        newChild.localPosition += new Vector3(x, 0, z);

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

    public void SetLocalRotation()
    {
        if (transform.tag == "Bull")
        {
            transform.eulerAngles = transform.root.eulerAngles + new Vector3(0, -90, 0);
            Vector3 pos = transform.localPosition;
            pos += transform.forward * -0.3f;
            transform.localPosition = pos;

            //SpringManagerArrange mySMA = transform.root.Find("SecondBaggage").GetComponent<SpringManagerArrange>();
            //SpringManagerArrange otherSMA = transform.root.Find("PlayerBasket(Clone)").Find("nimotuParent").GetComponent<SpringManagerArrange>();

            //float dx = otherSMA.YoungestChild().localPosition.x - mySMA.YoungestChild().localPosition.x;
            //float dy = otherSMA.YoungestChild().localPosition.y - mySMA.YoungestChild().localPosition.y;
            //float rad = Mathf.Atan2(dy, dx);
            //transform.Find("tougyu").RotateAround(mySMA.YoungestChild().position, transform.right, -rad * Mathf.Rad2Deg);
             //rad * Mathf.Rad2Deg;
        }
        localRotation = transform.localRotation;
        
    }
}
