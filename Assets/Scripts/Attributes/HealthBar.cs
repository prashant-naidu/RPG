using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Canvas m_Canvas = null;
        [SerializeField] private RectTransform m_RectTransform = null;
        [SerializeField] private Health m_Health = null;

        void Update()
        {
            float fraction = m_Health.GetFraction();

            if (Mathf.Approximately(fraction, 0) || Mathf.Approximately(fraction, 1))
            {
                m_Canvas.enabled = false;
                return;
            }

            m_Canvas.enabled = true;

            m_RectTransform.localScale = new Vector3(fraction, 1f, 1f);
        }
    }
}
