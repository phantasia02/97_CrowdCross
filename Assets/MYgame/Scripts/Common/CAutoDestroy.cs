using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAutoDestroy : MonoBehaviour
{
    [SerializeField] protected float m_DestroyTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, m_DestroyTime);
    }


}
