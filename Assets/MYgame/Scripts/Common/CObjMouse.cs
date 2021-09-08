using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CObjMouse : MonoBehaviour
{
    protected CMovableBase m_ParentMovable = null;

    private void Awake()
    {
        m_ParentMovable = this.GetComponentInParent<CMovableBase>();
    }

    public void OnMouseDown()
    {
        m_ParentMovable.OnMouseDown();
    }

    public void OnMouseDrag()
    {
        m_ParentMovable.OnMouseDrag();
    }

    public void OnMouseUp()
    {
        m_ParentMovable.OnMouseUp();
    }
}
