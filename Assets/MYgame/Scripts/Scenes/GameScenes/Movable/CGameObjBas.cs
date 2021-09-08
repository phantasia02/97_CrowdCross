using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CGameObjBas : MonoBehaviour
{
    public enum EObjType
    {
        eMovable            = 0,
        eMax
    }

    abstract public EObjType ObjType();
    protected Transform m_OriginalParent = null;
    protected CGameManager m_MyGameManager = null;

    protected virtual void Awake()
    {
        m_MyGameManager = GetComponentInParent<CGameManager>();
        m_OriginalParent = gameObject.transform.parent;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    public virtual void Init()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
