using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text m_Text = null;
        public string Text
        {
            get
            {
                return m_Text.text;
            }
            set
            {
                m_Text.text = value;
            }
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}
