using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CImageText : MonoBehaviour
{
    Image m_Image = null;
    Text m_text = null;
    int m_textNumber = 0;
    public int TextNumber
    {
        get { return m_textNumber; }
        set
        {
            m_textNumber = value;
            m_text.text = m_textNumber.ToString();
        }
    }

    private void Awake()
    {
        m_Image = GetComponentInChildren<Image>();
        m_text  = GetComponentInChildren<Text>();
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
