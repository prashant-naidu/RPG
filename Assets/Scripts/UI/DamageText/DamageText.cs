using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text m_Text = null;
        
        public void SetText(float value)
        {
            m_Text.text = string.Format("{0:0}", value);
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}
