using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private SavingSystem m_SavingSystem;

        const string defaultSaveFile = "save";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                m_SavingSystem.Save(defaultSaveFile);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                m_SavingSystem.Load(defaultSaveFile);
            }
        }
    }
}


