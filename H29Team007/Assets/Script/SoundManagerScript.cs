using UnityEngine;
using System.Collections;
using System;

public class SoundManagerScript : MonoBehaviour {
    protected static SoundManagerScript instance;

    public static SoundManagerScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SoundManagerScript)FindObjectOfType(typeof(SoundManagerScript));
                if (instance == null)
                {
                    Debug.LogError("SoundManager Instance Error");
                }
            }

            return instance;
        }
    }
    //音量
    public SoundVolime volume = new SoundVolime();

    [SerializeField, Range(0, 3)]
    float _BGMVolume;
    [SerializeField, Range(0, 3)]
    float _SEVolume;


    //AudioSource

    //BGM
    private AudioSource BGMsource;
    //SE
    private AudioSource[] SEsources = new AudioSource[16];
    //Voice
    private AudioSource[] VoiceSources = new AudioSource[16];

    //AudioClip

    //BGM
    public AudioClip[] BGM;
    //SE
    public AudioClip[] SE;
    //Voice
    public AudioClip[] Voice;

    //BGMのFadeOutFlag
    public bool BGMFadeFlag;


    void Awake()
    {
        //全てのオーディオコンポーネントを追加する

        //BGM AudioSource
        BGMsource = gameObject.AddComponent<AudioSource>();
        //BGMはループを有効にする
        BGMsource.loop = true;

        //SEsource
        for (int i = 0; i < SEsources.Length; i++)
        {
            SEsources[i] = gameObject.AddComponent<AudioSource>();
        }

        for (int i = 0; i < VoiceSources.Length; i++)
        {
            VoiceSources[i] = gameObject.AddComponent<AudioSource>();
        }

        //音声source
        for (int i = 0; i < VoiceSources.Length; i++)
        {
            VoiceSources[i] = gameObject.AddComponent<AudioSource>();
        }

        //Fadeを実行しないようにfalseにする
        BGMFadeFlag = false;
    }

    void Update()
    {
        //FadeがtrueになったらFadeOutを実行させる
        if (BGMFadeFlag == true &&
            volume._BGM > 0)
        {
            volume._BGM -= 0.1f;
        }
        else if (BGMFadeFlag == false && volume._BGM <= 1.0f)
        {
            volume._BGM += 0.1f;
        }
        //音が聞こえなくなったらBGMを止める
        if (volume._BGM <= 0)
        {
            StopBGM();
            BGMFadeFlag = false;
        }

        //ミュート設定
        BGMsource.mute = volume._Mute;
        foreach (AudioSource source in SEsources)
        {
            source.mute = volume._Mute;
        }
        foreach (AudioSource source in VoiceSources)
        {
            source.mute = volume._Mute;
        }

        //ボリューム設定
        BGMsource.volume = volume._BGM;
        foreach (AudioSource source in SEsources)
        {
            source.volume = volume._SE;
        }
        foreach (AudioSource source in VoiceSources)
        {
            source.volume = volume._Voce;
        }

    /*    if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChangeScript.Instance.FadeOut("Title");
        }*/
    }


    //BGM再生

    //BGM再生
    public void PlayBGM(int index)
    {
        if (0 > index || BGM.Length <= index)
        {
            return;
        }

        //同じBGMの場合何もしない
        if (BGMsource == BGM[index])
        {
            return;
        }
        BGMsource.Stop();
        BGMsource.clip = BGM[index];
        BGMsource.Play();
        BGMFadeFlag = false;
    }

    public void StopBGM()
    {

        BGMsource.Stop();
        BGMsource.clip = null;

    }

    public void FadeOutBGM()
    {
        BGMFadeFlag = true;
    }
    //SE再生

    //SE再生
    public void PlaySE(int index)
    {
        if (0 > index || SE.Length <= index)
        {
            return;
        }

        //再生中で無いAudioSourceで鳴らす
        foreach (AudioSource source in SEsources)
        {
            if (false == source.isPlaying)
            {
                source.clip = SE[index];
                source.Play();
                return;
            }
        }
    }

    //SE停止
    public void StopSE()
    {
        //全てのSE用のAudioSourceを停止する
        foreach (AudioSource source in SEsources)
        {
            source.Stop();
            source.clip = null;
        }
    }


    //音声再生

    //音声再生
    public void PlayVoice(int index)
    {
        if (0 > index || Voice.Length <= index)
        {
            return;
        }

        //再生中で無いAudioSourceで鳴らす
        foreach (AudioSource source in SEsources)
        {
            if (false == source.isPlaying)
            {
                source.clip = Voice[index];
                source.Play();
                return;
            }
        }
    }

    //音声停止
    public void StopVoice()
    {
        //全ての音声用のAudioSourceを停止する
        foreach (AudioSource source in VoiceSources)
        {
            source.Stop();
            source.clip = null;
        }
    }
}

//音量クラス
[SerializeField]
public class SoundVolime
{
    public float _BGM  = 1.0f;
    public float _SE   = 1.0f;
    public float _Voce = 1.0f;
    public bool _Mute  = false;

    public void Init()
    {
        _BGM  = 1.0f;
        _SE   = 1.0f;
        _Voce = 1.0f;
        _Mute = false;
    }
}
