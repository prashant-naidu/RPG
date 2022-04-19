using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string m_Id = "";

        //private void Awake()
        //{
        //    m_Id = System.Guid.NewGuid().ToString();
        //}

        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty idProperty = serializedObject.FindProperty("m_Id");

            if(string.IsNullOrEmpty(idProperty.stringValue))
            {
                idProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }

        public string GetUniqueIdentifier()
        {
            return m_Id;
        }

        public object CaptureState()
        {
            print("Capturing state for " + GetUniqueIdentifier());
            return null;
        }

        public void RestoreState(object state)
        {
            print("Restoring state for " + GetUniqueIdentifier());
        }
    }
}
