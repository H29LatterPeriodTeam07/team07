using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cursor : MonoBehaviour {

    private RectTransform m_RectTranslate;

    [SerializeField]
    private AnimationCurve _fadeCurve = null;
    private float _curveRate = 0;
    private float _fadingSpeed = 0.05f;

    public SoundManagerScript _smScript;

    //Color selectedColor;

    // Use this for initialization
    void Start () {
        m_RectTranslate = GetComponent<RectTransform>();
        /////selectedColor = .GetComponent<Image>().color;

    }
	
	// Update is called once per frame
	void LateUpdate () {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        Color selectedColor = selectedObject.GetComponent<Image>().color;

        if (selectedObject == null)
        {
            return;
        }

        Vector2 cursorPosition = new Vector2(-270, 0);

        m_RectTranslate.anchoredPosition = selectedObject.GetComponent<RectTransform>().anchoredPosition + cursorPosition;

        Color alpha = selectedColor;
        alpha.a = _fadeCurve.Evaluate(_curveRate);
        selectedColor = alpha;

        if (_curveRate == 0 || _curveRate == 1f)
        {
            _fadingSpeed *= -1;
        }

    }
}
