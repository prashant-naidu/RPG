using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [Header("Dependencies")]
        [SerializeField] private Animator m_Animator;
        [SerializeField] private ActionScheduler m_ActionScheduler;
        [SerializeField] private BaseStats m_BaseStats;

        [Header("Other")]
        [SerializeField] private CapsuleCollider m_Collider;
        [SerializeField] private TakeDamageEvent m_OnTakeDamage;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        private LazyValue<float> m_HealthPoints;
        public float HealthPoints { get { return m_HealthPoints.value; } }

        private bool m_IsDead = false;
        public bool IsDead
        {
            get
            {
                return m_IsDead;
            }
        }

        public float MaxHealthPoints { get { return m_BaseStats.GetStat(Stat.Health); } }

        private const float m_RegenerationPercentage = 70f;

        private void Awake()
        {
            m_HealthPoints = new LazyValue<float>(GetInitialHealthPoints);
        }

        private float GetInitialHealthPoints()
        {
            return m_BaseStats.GetStat(Stat.Health);
        }

        private void Start()
        {
            m_HealthPoints.ForceInit();
        }

        private void OnEnable()
        {
            m_BaseStats.OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            m_BaseStats.OnLevelUp -= RegenerateHealth;
        }

        private void Update()
        {
            m_Collider.enabled = !m_IsDead;
        }

        public float GetPercentage()
        {
            return GetFraction() * 100f;
        }

        public float GetFraction()
        {
            return m_HealthPoints.value / m_BaseStats.GetStat(Stat.Health);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);

            m_HealthPoints.value = Mathf.Max(m_HealthPoints.value - damage, 0f);
            if (m_HealthPoints.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
            else
            {
                m_OnTakeDamage.Invoke(damage);
            }
        }

        private void RegenerateHealth()
        {
            m_HealthPoints.value = Mathf.Max(m_HealthPoints.value, m_BaseStats.GetStat(Stat.Health) * (m_RegenerationPercentage / 100f));
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience != null)
            {
                experience.GainExperience(m_BaseStats.GetStat(Stat.ExperienceReward));
            }
        }

        private void Die()
        {
            if (m_IsDead) return;

            m_IsDead = true;

            m_Animator.SetTrigger("die");
            m_ActionScheduler.CancelCurrentAction();
        }

        public object CaptureState()
        {
            return m_HealthPoints.value;
        }

        public void RestoreState(object state)
        {
            m_HealthPoints.value = (float)state;

            if (m_HealthPoints.value <= 0)
            {
                Die();
            }
        }
    }
}