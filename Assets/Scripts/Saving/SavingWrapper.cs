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

        private void Start()
        {
            Load();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }

        public void Save()
        {
            m_SavingSystem.Save(defaultSaveFile);
        }

        public void Load()
        {
            m_SavingSystem.Load(defaultSaveFile);
        }
    }
}


