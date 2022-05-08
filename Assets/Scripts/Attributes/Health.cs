using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    /// <summary>
    /// Represents the amount of health an actor has.
    /// </summary>
    public class Health : MonoBehaviour, ISaveable
    {
        [Header("Dependencies")]
        [SerializeField] private Animator m_Animator;
        [SerializeField] private ActionScheduler m_ActionScheduler;
        [SerializeField] private BaseStats m_BaseStats;

        [Header("Other")]
        [SerializeField] private CapsuleCollider m_Collider;
        [SerializeField] private TakeDamageEvent m_OnTakeDamage;    // Occurs when an actor receives damage
        [SerializeField] private UnityEvent m_OnDie;                // Occurs when an actor's health is at 0

        [Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        private float m_HealthPoints;                               // The current amount of health.
        private const float RegenerationPercentage = 70f;           // The percentage of health regenerated.

        /// <summary>
        /// Gets or sets the amount of remaining health.
        /// </summary>
        public float HealthPoints
        {
            get => m_HealthPoints;
            set
            {
                // Set the new value
                m_HealthPoints = value;

                // We ded?
                if (IsDead)
                {
                    // Yes.  Invoke the death event
                    m_OnDie.Invoke();

                    // Trigger the death animation
                    m_Animator.SetTrigger("die");

                    // Cancel any current action
                    m_ActionScheduler.CancelCurrentAction();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the actor is dead.
        /// </summary>
        public bool IsDead => HealthPoints <= 0;

        /// <summary>
        /// Gets the maximum possible health points.
        /// </summary>
        public float MaxHealthPoints => m_BaseStats.GetStat(Stat.Health);

        /// <summary>
        /// Occurs when the scene (?) starts
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// Occurs when the actor is enabled.
        /// </summary>
        private void OnEnable()
        {
            m_BaseStats.OnLevelUp += RegenerateHealth;
        }

        /// <summary>
        /// Occurs when the actor is disabled.
        /// </summary>
        private void OnDisable()
        {
            m_BaseStats.OnLevelUp -= RegenerateHealth;
        }

        /// <summary>
        /// Occurs when a frame is updated.
        /// </summary>
        private void Update()
        {
            m_Collider.enabled = !IsDead;
        }

        /// <summary>
        /// Gets the current health as a percentage between 0 and 100.
        /// </summary>
        /// <returns></returns>
        public float GetPercentage() => GetFraction() * 100f;

        /// <summary>
        /// Gets the current health as a percentage.
        /// </summary>
        /// <returns></returns>
        public float GetFraction() => HealthPoints / m_BaseStats.GetStat(Stat.Health);

        /// <summary>
        /// Decreases the health by the specified amount.
        /// </summary>
        /// <param name="instigator">A <strong>GameObject</strong> object.  The character which caused damage.</param>
        /// <param name="damage">A <strong>float</strong> value.  The amount of health to lose.</param>
        public void TakeDamage(GameObject instigator, float damage)
        {
            // Show that we took damage
            print(gameObject.name + " took damage: " + damage);

            // Decrease the current health
            HealthPoints = Mathf.Max(HealthPoints - damage, 0f);

            // Are we ded?
            if (IsDead)
            {
                // /yes.  Give the attacker experience
                AwardExperience(instigator);
            }
            else
            {
                // No.  Reduce health
                m_OnTakeDamage.Invoke(damage);
            }
        }

        /// <summary>
        /// Restores health by the specified amount.
        /// </summary>
        /// <param name="healthToRestore"></param>
        public void Heal(float healthToRestore)
        {
            HealthPoints = Mathf.Min(HealthPoints + healthToRestore, MaxHealthPoints);
        }

        /// <summary>
        /// Increases health by the specified amount.
        /// </summary>
        private void RegenerateHealth()
        {
            HealthPoints = Mathf.Max(HealthPoints, m_BaseStats.GetStat(Stat.Health) * (RegenerationPercentage / 100f));
        }

        /// <summary>
        /// Increases the experience by the specified actor's experiance reward.
        /// </summary>
        /// <param name="instigator"></param>
        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience != null)
            {
                experience.GainExperience(m_BaseStats.GetStat(Stat.ExperienceReward));
            }
        }

        /// <summary>
        /// Saves the current health.
        /// </summary>
        /// <returns></returns>
        public object CaptureState()
        {
            return HealthPoints;
        }

        /// <summary>
        /// Deserializes the current health.
        /// </summary>
        /// <param name="state"></param>
        public void RestoreState(object state)
        {
            HealthPoints = (float)state;
        }
    }
}