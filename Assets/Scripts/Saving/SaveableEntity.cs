using RPG.Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    [ExecuteAlways]
    class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string m_Id = "";
        public string Id { get { return m_Id; } }

        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty idProperty = serializedObject.FindProperty("m_Id");

            if (string.IsNullOrEmpty(idProperty.stringValue) || !IsUnique(idProperty.stringValue))
            {
                idProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[idProperty.stringValue] = this;
        }
#endif

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

        private bool IsUnique(string candidateId)
        {
            // cases where designer duplicates a saveable asset in the scene
            if (!globalLookup.ContainsKey(candidateId)) return true;
            if (globalLookup[candidateId] == this) return true;
            // case where designer switches scenes
            if (globalLookup[candidateId] == null)
            {
                globalLookup.Remove(candidateId);
                return true;
            }
            // case where designer manually changes id in inspector
            if (globalLookup[candidateId].Id != candidateId)
            {
                globalLookup.Remove(candidateId);
                return true;
            }
            return false;
        }
    }
}
