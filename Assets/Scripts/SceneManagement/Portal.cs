using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        public enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        public int SceneToLoad = -1;
        public Transform SpawnPoint;
        public DestinationIdentifier Destination;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (SceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set!");
                yield break;
            }

            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(SceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.Destination != this.Destination) continue;

                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            playerGO.GetComponent<NavMeshAgent>().Warp(otherPortal.SpawnPoint.position);
            playerGO.transform.rotation = otherPortal.SpawnPoint.rotation;
        }
    }
}
