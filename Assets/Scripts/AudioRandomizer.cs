using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource = null;
    [SerializeField] private AudioClip[] m_AudioClips;

    public void Play()
    {
        m_AudioSource.PlayOneShot(m_AudioClips[Random.Range(0, m_AudioClips.Length)]);
    }
}
