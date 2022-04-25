using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffectCompleted : MonoBehaviour
    {
        [SerializeField] private GameObject m_GameObjectToDestroy = null;
        [SerializeField] private ParticleSystem m_ParticleSystem = null;

        private void Update()
        {
            if (!m_ParticleSystem.IsAlive())
            {
                if (m_GameObjectToDestroy != null)
                {
                    Destroy(m_GameObjectToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}