using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour {

    CanvasGroup _canvasGroup;

    [SerializeField]
    private AnimationCurve flash = null;
    private float _curveRate = 0;
    private float _fadingSpeed = 0.05f;

    // Use this for initialization
    void Awake() {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update() {
        //アニメーション
        _curveRate = Mathf.Clamp(_curveRate + _fadingSpeed, 0f, 1f);
        _canvasGroup.alpha = flash.Evaluate(_curveRate);

        //アニメーション反転
        if (_curveRate == 0 || _curveRate == 1f)
        {
            _fadingSpeed *= -1;
        }
    }

    public void FadeStop()
    {
        _fadingSpeed = 0;
    }

    public void FadeStart()
    {
        _fadingSpeed = 0.05f;
    }

}
