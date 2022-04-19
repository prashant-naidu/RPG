using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private SavingSystem m_SavingSystem;

        const string defaultSaveFile = "save";
        const float FadeInTime = 0.3f;

        private IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return m_SavingSystem.LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(FadeInTime);
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


