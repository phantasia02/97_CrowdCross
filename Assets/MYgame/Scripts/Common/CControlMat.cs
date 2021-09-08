using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CControlMat : MonoBehaviour
{

    [SerializeField] protected string m_ColorStr = "_BaseColor";

    protected Renderer m_MyObjRenderer = null;
    public Renderer MyObjRenderer { get { return m_MyObjRenderer; } }

    protected int m_BaseColorId = 0; 
    public int BaseColorId { get { return m_BaseColorId; } }

    protected Color m_MainObjRendererColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public Color MainObjRendererColor
    {
        set { m_MainObjRendererColor = value; }
        get { return m_MainObjRendererColor; }
    }

    protected MaterialPropertyBlock m_MaterialPropertyBlock = null;

    protected void Awake()
    {
        m_MyObjRenderer = gameObject.GetComponent<Renderer>();


        m_BaseColorId = Shader.PropertyToID(m_ColorStr);

        m_MainObjRendererColor = m_MyObjRenderer.material.GetColor(BaseColorId);

        m_MaterialPropertyBlock = new MaterialPropertyBlock();
        //m_MainObjRendererColor = m_MaterialPropertyBlock.GetColor(BaseColorId);
        m_MyObjRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMatFloat(string shadername, float setdata)
    {
        m_MyObjRenderer.material.SetFloat(shadername, setdata);
    }

    public void SetColor(Color setcolor)
    {
        if (m_MaterialPropertyBlock == null)
            return;

        MainObjRendererColor = setcolor;
        m_MaterialPropertyBlock.SetColor(BaseColorId, m_MainObjRendererColor);
        m_MyObjRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
    }
}
