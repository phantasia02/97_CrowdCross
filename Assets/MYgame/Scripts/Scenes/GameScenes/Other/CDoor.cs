using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[ExecuteAlways]         // 不管編輯模式 還是 執行中都執行
#endif

public class CDoor : CGameObjBas
{
    public enum EMathematicsSymbol
    {
        eAdd        = 0,
        eMultiply   = 1,
        eSubtract   = 2,
        eDivide     = 3,
        eMax
    };
    public override EObjType ObjType() { return EObjType.eDoor; }
    // ==================== SerializeField ===========================================
    [SerializeField] protected EMathematicsSymbol m_MyMathematicsSymbol = EMathematicsSymbol.eAdd;
    public EMathematicsSymbol MyMathematicsSymbol { get { return m_MyMathematicsSymbol; } }
    [SerializeField] protected Text m_ShowText = null;
    [SerializeField] protected int m_Number = 1;
    public int Number { get { return m_Number; } }
    // ==================== SerializeField ===========================================

    readonly string[] CntMathematicsSymbolStr = { "+", "x", "-", "÷" };

    protected CDoorGroup m_MyDoorGroup = null;

    protected override void Awake()
    {
        

        m_MyMathematicsSymbol = (EMathematicsSymbol)Random.Range(0, 2);
        if (m_MyMathematicsSymbol == EMathematicsSymbol.eAdd)
            m_Number = Random.Range(5, 30);
        else
            m_Number = Random.Range(2, 5);

        UpdateShowText();
        base.Awake();
    }

    // Start is called before the first frame update
    //protected override void Start()
    //{
    //    base.Start();
    //}

    //// Update is called once per frame
    //protected override void Update()
    //{
    //    base.Update();
    //}

    public void UpdateShowText()
    {
        m_ShowText.text = $"{CntMathematicsSymbolStr[(int)m_MyMathematicsSymbol]}{m_Number}" ;
    }

    private void OnValidate() { UpdateShowText(); }
}
