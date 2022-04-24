using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RPG.Attributes
{
    class Experience : MonoBehaviour
    {
        [SerializeField] private float m_ExperiencePoints = 0;

        public void GainExperience(float experience)
        {
            m_ExperiencePoints += experience;
        }
    }
}
