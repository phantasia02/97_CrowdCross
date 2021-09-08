using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CBtnCtrlNumber : MonoBehaviour
{
    [SerializeField] Text m_UINumberText = null;
    int m_Number = 0;
    public int Number{get { return m_Number; }}

    int m_NumberMax = 100;
    public int NumberMax
    {
        set { m_NumberMax = value; }
        get { return m_NumberMax; }
    }
    int m_NumberMin = 0;
    public int NumberMin
    {
        set { m_NumberMin = value; }
        get { return m_NumberMin; }
    }

    private void Awake()
    {
        m_Number = m_NumberMin;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdataText()
    {
        m_UINumberText.text = m_Number.ToString();
    }

    public void SetNumber(int lpNumber)
    {
        if (m_Number < m_NumberMin || m_Number > m_NumberMax)
            return;

        m_Number = lpNumber;
        UpdataText();
    }

    public void OnAdd()
    {
        m_Number++;
        if (m_Number > m_NumberMax)
            m_Number = m_NumberMin;

        UpdataText();
    }

    public void OnSub()
    {
        m_Number--;
        if (m_Number < m_NumberMin)
            m_Number = m_NumberMax;

        UpdataText();
    }
}
