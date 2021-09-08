using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStandPoint : MonoBehaviour
{
    [SerializeField] protected int m_HighNumber = 0;
    public int HighNumber
    {
        get { return m_HighNumber; }
        set {  m_HighNumber = value; }
    }

    [SerializeField] protected bool m_StandOK = false;
    public bool StandOK
    {
        get { return m_StandOK; }
        set { m_StandOK = value; }
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
