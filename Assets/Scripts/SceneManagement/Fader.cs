using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;

        private Coroutine m_CurrentlyActiveFade = null;

        public void FadeOutImmediate()
        {
            m_CanvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1f, time);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0f, time);
        }

        public Coroutine Fade(float target, float time)
        {
            if (m_CurrentlyActiveFade != null)
            {
                StopCoroutine(m_CurrentlyActiveFade);
            }
            m_CurrentlyActiveFade = StartCoroutine(FadeRoutine(target, time));
            return m_CurrentlyActiveFade;
        }

        private IEnumerator FadeRoutine(float targetAlpha, float time)
        {
            while (!Mathf.Approximately(m_CanvasGroup.alpha, targetAlpha))
            {
                m_CanvasGroup.alpha = Mathf.MoveTowards(m_CanvasGroup.alpha, targetAlpha, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}

