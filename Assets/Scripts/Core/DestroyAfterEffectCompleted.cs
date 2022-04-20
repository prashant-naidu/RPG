using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffectCompleted : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_ParticleSystem = null;

        private void Update()
        {
            if (!m_ParticleSystem.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}