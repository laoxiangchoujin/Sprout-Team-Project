using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 音量设置 : MonoBehaviour
{
    public void setVolume(float volume)
    {
        GetComponent<AudioSource>().volume = volume;
    }
}
