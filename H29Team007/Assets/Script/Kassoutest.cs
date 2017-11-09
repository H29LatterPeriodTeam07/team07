using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kassoutest : MonoBehaviour {

    private Rigidbody rb;
    //public PhysicMaterial mat;
    [SerializeField, Header("方向転換する速さ、カート本体の回転")]
    private float angleRotateSpeed = 90.0f;

    [SerializeField, Header("速度回転の速さ、ドリフト的な奴")]
    private float velocityRotateSpeed = 2.0f;

    [SerializeField, Header("ボタン押したときの最高スピード")]
    private float kickSpeed = 10.0f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        //rb.velocity = Vector3.forward * 3.0f;
        //GetComponent<CapsuleCollider>().material = mat;
	}
	
	// Update is called once per frame
	void Update () {
        float inputHorizontal = (Input.GetAxisRaw("PS4LeftHorizontal") != 0) ? Input.GetAxisRaw("PS4LeftHorizontal") : Input.GetAxisRaw("Horizontal");
        

        transform.Rotate(new Vector3(0, inputHorizontal * 90 * Time.deltaTime, 0));

        Quaternion a = new Quaternion(rb.velocity.x, 0, rb.velocity.z, 0);
        a *= Quaternion.AngleAxis(inputHorizontal * -2, Vector3.up);
        rb.velocity = new Vector3(a.x, 0, a.z);
        //transform.rotation = a;

        //transform.LookAt(new Vector3(rb.velocity.x,1,rb.velocity.z));

        if (Input.GetKeyDown(KeyCode.O))
        {
            rb.velocity = transform.forward * 10.0f;
            //rb.AddForce(transform.forward * 3, ForceMode.Impulse);
        }
	}
}
