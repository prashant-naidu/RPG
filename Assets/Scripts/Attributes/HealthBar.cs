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

            m_Canvas.enabled = fraction > 0 && fraction < 1;

            m_RectTransform.localScale = new Vector3(fraction, 1f, 1f);
        }
    }
}
