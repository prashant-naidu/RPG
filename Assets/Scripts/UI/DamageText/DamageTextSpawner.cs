using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [Header("Prefab References")]
        [SerializeField] private DamageText m_DamageTextPrefabReference;

        public void Spawn(float damageTaken)
        {
            DamageText instance = Instantiate(m_DamageTextPrefabReference, transform);
            instance.Text = damageTaken.ToString();
        }
    }
}
