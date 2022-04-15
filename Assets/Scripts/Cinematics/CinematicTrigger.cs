using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool m_HasPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!m_HasPlayed && other.gameObject.tag == "Player")
            {
                m_HasPlayed = true;
                GetComponent<PlayableDirector>().Play();
            }
            
        }
    }
}