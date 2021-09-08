using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CDragHeroFloor : MonoBehaviour
{
    public class ParPointStandOK
    {
        public int  Xindex      = -1;
        public int  HighNumber  = 0;
        public bool StandOK     = false;
    }

    [SerializeField] protected List<CDragHeroGroup> m_AllDragHeroDragHeroGroup;
    public List<CDragHeroGroup> AllDragHeroDragHeroGroup { get { return m_AllDragHeroDragHeroGroup; } }
    [SerializeField] protected Collider[] m_SECollider = null;
    //protected List<List<CStandPoint>> m_AllStandPoint = new List<List<CStandPoint>>();
    //public List<List<CStandPoint>> AllStandPoint { get { return m_AllStandPoint; } }

    private void Awake()
    {
        CDragHeroGroup[] lTempCDragHeroGroup = this.GetComponentsInChildren<CDragHeroGroup>();

        for (int i = 0; i < lTempCDragHeroGroup.Length; i++)
            m_AllDragHeroDragHeroGroup.Add(lTempCDragHeroGroup[i]);

        m_AllDragHeroDragHeroGroup.Sort((x, y) => x.transform.position.x.CompareTo(y.transform.position.x));

        for (int i = 0; i < lTempCDragHeroGroup.Length; i++)
            m_AllDragHeroDragHeroGroup[i].DragHeroFloorIndex = i;

    }



    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSECollider(bool show)
    {
        //for (int i = 0; i < m_SECollider.Length; i++)
        //    m_SECollider[i].gameObject.SetActive(show);
    }

    public bool CheckDragHeroGroupJumpOK()
    {
        for (int i = 0; i < AllDragHeroDragHeroGroup.Count; i++)
        {
            if (AllDragHeroDragHeroGroup[i].CurState == StaticGlobalDel.EMovableState.eJump)
                return false;
        }

        return true;
    }
}
