using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwitch : MonoBehaviour
{
    public AudioSource audioSourceIntro;
    public AudioSource audioSourceLoop;

    private void Start()
    {
        audioSourceIntro.Play();
        audioSourceLoop.PlayDelayed(audioSourceIntro.clip.length);
    }


}
