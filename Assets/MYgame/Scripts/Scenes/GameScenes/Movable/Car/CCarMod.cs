using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class CCarMod : MonoBehaviour
{
    [SerializeField] protected float m_CarLong = 1.0f;
    public float CarLong { get { return m_CarLong; } }

    [SerializeField] protected float m_DefSpeed = StaticGlobalDel.g_DefMovableTotleSpeed;
    public float DefSpeed { get { return m_DefSpeed; } }

    [SerializeField] protected Transform m_ForwardObject = null;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 a = m_ForwardObject.position;
        Vector3 b = m_ForwardObject.position + (-m_ForwardObject.forward * m_CarLong);

        Handles.DrawAAPolyLine(a, b);

        // �e�T�w�j�p���y  ���|�]�� ��v������ �v�T���y
        void DrawSphere(Vector3 p)
        {
            Gizmos.DrawSphere(p, HandleUtility.GetHandleSize(p));
        }

        DrawSphere(a);
        DrawSphere(b);
    }
#endif
}
