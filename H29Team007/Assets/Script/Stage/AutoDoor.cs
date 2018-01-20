using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour {
    private Sensor Sencer;
    private Transform[] DoorTransforms;
    private Vector3[] MoveStartLocalPosition;
    private Vector3[] MoveEndLocalPosition;
    private float LerpValue;
    
	// Use this for initialization
	void Start () {
        const float MoveValue = 0.35f;
        Sencer = transform.Find("sensor").GetComponent<Sensor>();
        DoorTransforms = new Transform[2];
        MoveEndLocalPosition = new Vector3[2];
        MoveStartLocalPosition = new Vector3[2];
        for (int i =0; i < DoorTransforms.Length; ++i)
        {
            DoorTransforms[i] = transform.Find("Door0" + i.ToString());
            MoveStartLocalPosition[i] = DoorTransforms[i].localPosition;
            if(i == 0)
            {
                MoveEndLocalPosition[i] = MoveStartLocalPosition[i] - Vector3.right * MoveValue;
            }
            else
            {
                MoveEndLocalPosition[i] = MoveStartLocalPosition[i] + Vector3.right * MoveValue;
            }
        }
        LerpValue = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateDoorPosition();

    }

    void UpdateDoorPosition()
    {
        const float Speed = 2.0f;
        if (Sencer.IsStay())
        {
            LerpValue += Time.deltaTime * Speed;
        }
        else
        {
            LerpValue -=Time.deltaTime * Speed;
        }
        LerpValue = Mathf.Clamp(LerpValue, 0.0f, 1.0f);

        
        for(int i = 0; i < DoorTransforms.Length; ++i)
        {
            DoorTransforms[i].localPosition = Vector3.Lerp(MoveStartLocalPosition[i], MoveEndLocalPosition[i], LerpValue);
        }
    }
}
