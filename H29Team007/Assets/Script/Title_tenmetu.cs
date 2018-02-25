using UnityEngine;
using UnityEngine.UI;

public class Title_tenmetu : MonoBehaviour
{
    public GameObject _Button;

    private Image _image;
    private Text _text;

    //どのようにフェードするかのカーブ
    [SerializeField]
    private AnimationCurve _fadeCurve = null;

    //カーブのどこにいるかの割合(0~1)
    private float _curveRate = 0;

    //フェードする速度
    private float _fadingSpeed = 0.05f;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        //アニメーション
        _curveRate = Mathf.Clamp(_curveRate + _fadingSpeed, 0f, 1f);

        //Color textColor = _text.color;
        //textColor.a = _fadeCurve.Evaluate(_curveRate);
        //_text.color = textColor;

        Color imageColor = _image.color;
        imageColor.a = _fadeCurve.Evaluate(_curveRate);
        _image.color = imageColor;

        //アニメーション反転
        if (_curveRate == 0 || _curveRate == 1f)
        {
            _fadingSpeed *= -1;
        }

        if (Input.GetButtonDown("XboxStart"))
        {
            this.gameObject.SetActive(false);
            _Button.SetActive(true);
        }
    }

}