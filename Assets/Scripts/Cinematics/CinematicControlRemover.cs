using RPG.Control;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        [SerializeField] private PlayableDirector m_PlayableDirector = null;

        private GameObject m_PlayerGO;

        private void Awake()
        {
            m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnEnable()
        {
            m_PlayableDirector.played += OnPlayed;
            m_PlayableDirector.stopped += OnStopped;
        }

        private void OnDisable()
        {
            m_PlayableDirector.played -= OnPlayed;
            m_PlayableDirector.stopped -= OnStopped;
        }

        private void OnPlayed(PlayableDirector obj)
        {
            m_PlayerGO.GetComponent<Mover>().Cancel();
            m_PlayerGO.GetComponent<PlayerController>().enabled = false;
            Cursor.visible = false;
        }

        private void OnStopped(PlayableDirector obj)
        {
            m_PlayerGO.GetComponent<PlayerController>().enabled = true;
            Cursor.visible = true;
        }
    }
}