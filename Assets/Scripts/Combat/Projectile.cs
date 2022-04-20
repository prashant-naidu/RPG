using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float m_Speed = 5f;
        [SerializeField] private bool m_IsHoming = true;
        [SerializeField] private GameObject m_HitEffect = null;

        private Health m_Target = null;
        private float m_Damage = 0f;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (m_Target != null)
            {
                if (m_IsHoming && !m_Target.IsDead)
                {
                    transform.LookAt(GetAimLocation());
                }
                transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
            }
        }

        public void SetTarget(Health target, float damage)
        {
            m_Target = target;
            m_Damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = m_Target.GetComponent<CapsuleCollider>();
            if (targetCapsule != null)
            {
                return m_Target.transform.position + (Vector3.up * targetCapsule.height / 2);
            }
            return m_Target.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != m_Target) return;
            if (m_Target.IsDead) return;

            m_Target.TakeDamage(m_Damage);

            if (m_HitEffect != null)
            {
                Instantiate(m_HitEffect, GetAimLocation(), transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}