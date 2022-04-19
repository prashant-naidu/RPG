using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;

        public void FadeOutImmediate()
        {
            m_CanvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (m_CanvasGroup.alpha < 1)
            {
                m_CanvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (m_CanvasGroup.alpha > 0)
            {
                m_CanvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}

