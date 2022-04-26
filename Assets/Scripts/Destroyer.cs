using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private GameObject m_TargetToDestroy = null;

    public void DestroyTarget()
    {
        Destroy(m_TargetToDestroy);
    }
}
