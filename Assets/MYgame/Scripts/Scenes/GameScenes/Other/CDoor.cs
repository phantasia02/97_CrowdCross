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
        eSubtract   = 1,
        eMultiply   = 2,
        eDivide     = 3,
        eMax
    };
    public override EObjType ObjType() { return EObjType.eDoor; }
    // ==================== SerializeField ===========================================
    [SerializeField] protected EMathematicsSymbol m_MyMathematicsSymbol = EMathematicsSymbol.eAdd;
    [SerializeField] protected Text m_ShowText = null;
    [SerializeField] protected int m_Number = 1;
    // ==================== SerializeField ===========================================

    readonly string[] CntMathematicsSymbolStr = { "+", "-", "x", "÷" };

    protected CDoorGroup m_MyDoorGroup = null;

    private void Awake()
    {
        UpdateShowText();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateShowText()
    {
        m_ShowText.text = $"{CntMathematicsSymbolStr[(int)m_MyMathematicsSymbol]}{m_Number}" ;
    }

    private void OnValidate() { UpdateShowText(); }
}
