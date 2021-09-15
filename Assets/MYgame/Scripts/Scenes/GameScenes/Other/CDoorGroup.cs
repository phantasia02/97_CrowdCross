using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDoorGroup : CGameObjBas
{
    public enum EAllPosDoorType
    {
        eLPosDoor = 0,
        eRPosDoor = 1,
        eMax
    }

    public override EObjType ObjType() { return EObjType.eDoorGroup; }

    // ==================== SerializeField ===========================================
    [SerializeField] protected CDoor[] m_AllDoor = null;
    // ==================== SerializeField ===========================================

    public CDoor GetDoor(EAllPosDoorType index) { return m_AllDoor[(int)index]; }
    protected Collider m_Myprotected = null;
    protected Renderer[] m_AllRenderer = null;

    private void Awake()
    {
        m_Myprotected = this.GetComponentInChildren<Collider>();
        m_AllRenderer = this.GetComponentsInChildren<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCollider(bool setshow)
    {
        m_Myprotected.gameObject.SetActive(setshow);
    }

    public void Show(bool setshow)
    {
        float lTempAlphaval = 1.0f;
        if (setshow)
            lTempAlphaval = 1.0f;
        else
            lTempAlphaval = 0.5f;

        for (int i = 0; i < m_AllRenderer.Length; i++)
        {
            Color lTemp = m_AllRenderer[i].material.color;
            lTemp.a = lTempAlphaval;
            m_AllRenderer[i].material.color = lTemp;
        }

    }
}
