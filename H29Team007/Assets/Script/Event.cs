using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour {

    public enum _Event
    {
        Cow,
        Pig,
        Fish,
        NULL
    }
    public _Event _event = _Event.NULL;
    public int eventnumber;

    public GameObject[] _obj;
     
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_event != _Event.NULL)
        {
            if (_event == _Event.Cow)
            {
                _obj[0].SetActive(true);
                _obj[1].SetActive(false);
                _obj[2].SetActive(false);
            }
            else if (_event == _Event.Pig)
            {
                _obj[0].SetActive(false);
                _obj[1].SetActive(true);
                _obj[2].SetActive(false);
            }
            else if (_event == _Event.Fish)
            {
                _obj[0].SetActive(false);
                _obj[1].SetActive(false);
                _obj[2].SetActive(true);
            }
        }
        else
        {
            _obj[0].SetActive(false);
            _obj[1].SetActive(false);
            _obj[2].SetActive(false);
        }

        //switch (_event)
        //{
        //    case _Event.Cow:
        //        _obj[0].SetActive(true);
        //        break;
        //    case _Event.Pig:
        //        _obj[1].SetActive(true);
        //        break;
        //    case _Event.Fish:
        //        _obj[2].SetActive(true);
        //        break;
        //    case _Event.NULL:
        //        break;
        //}
    }

    //public void SelectEvent(int number)
    //{
    //    if(number >= 3)
    //    {
    //        return;
    //    }
    //    switch (number)
    //    {
    //        case 0:
    //            eventnumber = number;
    //            break;
    //        case 1:
    //            eventnumber = number;
    //            break;
    //        case 2:
    //            eventnumber = number;
    //            break;
    //    }
    //}

    //public void EventActive()
    //{
    //    GameObject child = _obj[eventnumber];
    //    child.SetActive(true);
    //}
}
