using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] GameObject m_PersistentObjectsPrefab;

        private static bool m_HasSpawned;

        private void Awake()
        {
            if (m_HasSpawned) return;

            SpawnPersistentObjects();

            m_HasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObjects = Instantiate(m_PersistentObjectsPrefab);
            DontDestroyOnLoad(persistentObjects);
        }
    }
}
