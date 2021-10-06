using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CAnimatorStateCtl : MonoBehaviour
{
    public const float Jump1Speed = 0.5f;


    [System.Serializable]
    public class cAnimatorData
    {
        [SerializeField] public string m_flagName = "";
        [SerializeField] public string m_AnimationName = "";
        [SerializeField] public string m_AnimationStateName = "";
        [SerializeField] public float m_Speed = 1.0f;
        [HideInInspector] public AnimationClip m_AnimationClip = null;
        [HideInInspector] public float m_AnimationTime = 0.0f;
    }

    public enum EState
    {
        eIdle        = 0,
        eRun         = 1,
        eDeath       = 2,
        eWin         = 3,
        eHit         = 4,
        eJump        = 5,
        eAtk         = 6,
        eMax
    }

    public class cAnimationCallBackPar
    {
        public string AnimationName     = "";
        public EState eAnimationState   = EState.eMax;
        public int StateIndividualIndex = 0;
        public int iIndex = 0;
    }

    public delegate void ReturnAnimationCall(cAnimationCallBackPar Paramete);

    public Animator m_ThisAnimator = null;
    public float AnimatorSpeed
    {
        set { m_ThisAnimator.speed = value; }
        get { return m_ThisAnimator.speed; }
    }

    [SerializeField] CMovableBase m_MyMovableBase = null;

    
    [VarRename(new string[] { "Idle", "Run", "Death", "Win", "Hit"})]
    cAnimatorData[][] m_AllAnimatorData = new cAnimatorData[(int)EState.eMax][];

    public ReturnAnimationCall m_EndCallBack = null;
    public ReturnAnimationCall m_KeyFramMessageCallBack = null;

    [VarRename(new string[] { "Idle0", "Idle1", "Idle2"})]
    [SerializeField] public cAnimatorData[] m_AllIdleAnima = new cAnimatorData[1];

    [VarRename(new string[] { "Run0", "Run1", "Run2" })]
    [SerializeField] public cAnimatorData[] m_AllRunAnima = new cAnimatorData[1];

    [VarRename(new string[] { "Death0", "Death1", "Death2" })]
    [SerializeField] public cAnimatorData[] m_AllDeathAnima = new cAnimatorData[1];

    [VarRename(new string[] { "Win0", "Win1", "Win2" })]
    [SerializeField] public cAnimatorData[] m_AllWinAnima = new cAnimatorData[1];

    [VarRename(new string[] { "Hit0", "Hit1", "Hit2" })]
    [SerializeField] public cAnimatorData[] m_AllHitAnima = new cAnimatorData[1];

    [VarRename(new string[] { "jump0", "jump1", "jump2" })]
    [SerializeField] public cAnimatorData[] m_AllJumpAnima = new cAnimatorData[1];

    [VarRename(new string[] { "Atk0", "Atk1", "Atk2" })]
    [SerializeField] public cAnimatorData[] m_AllAtkAnima = new cAnimatorData[1];

    bool m_PlayingEnd = false;
    public bool PlayingEnd{get { return m_PlayingEnd; }}

    EState m_CurState = EState.eIdle;
    public EState CurState { get { return m_CurState; } }

    EState m_LockState = EState.eMax;
    public EState LockState
    {
        set { m_LockState = value; }
        get { return m_LockState; }
    }

    int[] m_StateIndividualIndex = new int[(int)EState.eMax];
    public int GetStateIndex(EState parstate) { return m_StateIndividualIndex[(int)CurState]; }

    protected bool m_ResetForward = true;
    public bool ResetForward
    {
        set { m_ResetForward = value; }
        get { return m_ResetForward; }
    }

    protected void Awake()
    {
        m_ThisAnimator = gameObject.GetComponent<Animator>();
        m_MyMovableBase = gameObject.GetComponentInParent<CMovableBase>();

        if (m_MyMovableBase)
            m_MyMovableBase.AnimatorStateCtl = this;

        if (m_ThisAnimator)
        {
            m_AllAnimatorData[(int)EState.eIdle]        = m_AllIdleAnima;
            m_AllAnimatorData[(int)EState.eRun]         = m_AllRunAnima;
            m_AllAnimatorData[(int)EState.eDeath]       = m_AllDeathAnima;
            m_AllAnimatorData[(int)EState.eWin]         = m_AllWinAnima;
            m_AllAnimatorData[(int)EState.eHit]         = m_AllHitAnima;
            m_AllAnimatorData[(int)EState.eJump]        = m_AllJumpAnima;
            m_AllAnimatorData[(int)EState.eAtk]         = m_AllAtkAnima;

            for (int i = 0; i < m_AllAnimatorData.Length; i++)
            {
                for (int x = 0; x < m_AllAnimatorData[i].Length; x++)
                    InitAnimatorData(ref m_AllAnimatorData[i][x]);

                m_StateIndividualIndex[i] = 0;
            }

        }
    }

    public void OnEnable()
    {

    }

    protected void InitAnimatorData(ref cAnimatorData rAnidata)
    {
        if (rAnidata.m_AnimationName.Length == 0)
            return;

        RuntimeAnimatorController ac = m_ThisAnimator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
            if (ac.animationClips[i].name == rAnidata.m_AnimationName)
            {
                rAnidata.m_AnimationClip = ac.animationClips[i];
                rAnidata.m_AnimationTime = rAnidata.m_AnimationClip.length;
                
            }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (!m_ThisAnimator)
        //    return;

        //int lTempCurState = (int)m_CurState;
        //cAnimatorData lTempAnimatorData = m_AllAnimatorData[lTempCurState][m_StateIndividualIndex[lTempCurState]];
        //if (lTempAnimatorData.m_AnimationName.Length == 0)
        //    return;

        //AnimatorStateInfo info = m_ThisAnimator.GetCurrentAnimatorStateInfo(0);

        //if (info.speed == 0)
        //    return;


        //if (ResetForward)
        //{
        //    float lTempsqr = Vector3.SqrMagnitude(m_MyMovableBase.transform.forward - this.transform.forward);
        //    if (lTempsqr >= 0.01f)
        //        this.transform.forward = Vector3.Lerp(this.transform.forward, m_MyMovableBase.transform.forward, 5.0f * Time.deltaTime);
        //    else
        //    {
        //        this.transform.forward = m_MyMovableBase.transform.forward;
        //        ResetForward = false;
        //    }
        //}

        //if (info.normalizedTime >= (1.0f / info.speed) && info.IsName(lTempAnimatorData.m_AnimationStateName) && !m_PlayingEnd)
        //{
        //    if (m_EndCallBack != null)
        //    {
        //        cAnimationCallBackPar lTempAnimationCallBackPar = new cAnimationCallBackPar();
        //        lTempAnimationCallBackPar.eAnimationState = m_CurState;
        //        m_EndCallBack(lTempAnimationCallBackPar);
        //        m_EndCallBack = null;
        //    }

        //    m_PlayingEnd = true;
        //}
    }

    //public float GetCurrentNormalizedTime()
    //{
    //    Debug.Log(m_ThisAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));
    //    return m_ThisAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    //}

    public string GetCurCurrentAnimator(){return m_ThisAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name; }

    public float GetAnimationStateTime(EState pGetstateTime)
    {
        cAnimatorData lTempAnimatorData = m_AllAnimatorData[(int)pGetstateTime][m_StateIndividualIndex[(int)pGetstateTime]];
        if (lTempAnimatorData.m_Speed == 0.0f || AnimatorSpeed == 0.0f)
            return 0.0f;

        //return m_AllAnimatorData[(int)pGetstateTime].m_AnimationTime;
        return (lTempAnimatorData.m_AnimationTime / lTempAnimatorData.m_Speed) / AnimatorSpeed;
    }

    public float GetAnimatiotStateTime(EState setstate, int index)
    {
        //return 0.0f;
        int iCurStatIndex = (int)setstate;

        if (iCurStatIndex < 0 || iCurStatIndex >= (int)EState.eMax)
            return 0.0f;

       return m_AllAnimatorData[iCurStatIndex][index].m_AnimationTime;
    }

    public float CurActionTimeRatio()
    {
        int lTempCurState = (int)m_CurState;
        cAnimatorData lTempAnimatorData = m_AllAnimatorData[lTempCurState][m_StateIndividualIndex[lTempCurState]];
        if (lTempAnimatorData.m_AnimationName.Length == 0)
            return 0.0f;

        AnimatorStateInfo info = m_ThisAnimator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(lTempAnimatorData.m_AnimationStateName))
            return info.normalizedTime;

        return 0.0f;
    }

    public void OpenAnimatorComponent(bool open)
    {
        if (!m_ThisAnimator || m_ThisAnimator.enabled == open)
            return;

        m_ThisAnimator.enabled = open;
    }

    public void SetStateIndividualIndex(EState state, int SetIdelIndex)
    {
        if (!m_ThisAnimator)
            return;

        int iCurIdelIndex = SetIdelIndex;


        if (iCurIdelIndex < 0 || iCurIdelIndex >= m_AllAnimatorData[(int)state].Length)
            return;

        if (m_StateIndividualIndex[(int)state] == SetIdelIndex)
            return;

        int ioldIdleIndex = m_StateIndividualIndex[(int)state];
        m_StateIndividualIndex[(int)state] = iCurIdelIndex;

        if (state == CurState)
        {
            cAnimatorData lTempAnimatorData = m_AllAnimatorData[(int)state][iCurIdelIndex];
            cAnimatorData lTempOldAnimatorData = m_AllAnimatorData[ioldIdleIndex][m_StateIndividualIndex[ioldIdleIndex]];

            if (lTempOldAnimatorData.m_flagName.Length != 0)
                m_ThisAnimator.ResetTrigger(lTempOldAnimatorData.m_flagName);

            if (lTempAnimatorData.m_flagName.Length != 0)
            {
              //  this.transform.forward = m_OriginForward;
                m_ThisAnimator.SetTrigger(lTempAnimatorData.m_flagName);
                m_PlayingEnd = false;
                ResetForward = true;
            }
        }

        //if (m_AllBaseIdleAnima[ioldIdleIndex].m_flagName.Length != 0)
        //    m_ThisAnimator.SetInteger(m_AllBaseIdleAnima[ioldIdleIndex].m_flagName, false);


        //if (lTempAnimatorData.m_flagName.Length != 0)
        //    m_ThisAnimator.SetInteger(lTempAnimatorData.m_flagName, iCurIdelIndex);
    }

    public void SetCurState(EState SetState, int index = -1)
    {
        if (!m_ThisAnimator)
            return;

        int iCurStatIndex = (int)SetState;

        if (iCurStatIndex < 0 || iCurStatIndex >= (int)EState.eMax)
            return;

        if (m_CurState == SetState)
            return;

        if (LockState != EState.eMax || (LockState != EState.eMax && LockState != SetState))
            return;

        EState oldState = m_CurState;
        m_CurState = SetState;
        int ioldStateIndex = (int)oldState;
        int lCurStateIndividualIndex = (int)SetState;

        if (index > -1)
            m_StateIndividualIndex[(int)SetState] = index;

        cAnimatorData lTempCurAnimatorData = m_AllAnimatorData[(int)SetState][m_StateIndividualIndex[lCurStateIndividualIndex]];
        cAnimatorData lTempOldAnimatorData = m_AllAnimatorData[ioldStateIndex][m_StateIndividualIndex[ioldStateIndex]];

        if (lTempOldAnimatorData.m_flagName.Length != 0)
            m_ThisAnimator.ResetTrigger(lTempOldAnimatorData.m_flagName);

        if (lTempCurAnimatorData.m_flagName.Length != 0)
        {
           // this.transform.forward = m_OriginForward;
            //m_ThisAnimator.gameObject.transform.eulerAngles = Vector3.zero;
            m_ThisAnimator.SetTrigger(lTempCurAnimatorData.m_flagName);
            m_PlayingEnd = false;
            ResetForward = true;
        }
    }

    public void KeyFrameCall(int setmessageIndex)
    {
       // Debug.Log("setmessageIndex = " + setmessageIndex.ToString());
        if (m_KeyFramMessageCallBack != null)
        {
            cAnimationCallBackPar lTempAnimationCallBackPar = new cAnimationCallBackPar();
            lTempAnimationCallBackPar.eAnimationState = m_CurState;
            lTempAnimationCallBackPar.iIndex = setmessageIndex;
            lTempAnimationCallBackPar.StateIndividualIndex = m_StateIndividualIndex[(int)m_CurState];
            lTempAnimationCallBackPar.AnimationName = m_AllAnimatorData[(int)m_CurState][m_StateIndividualIndex[(int)m_CurState]].m_AnimationStateName;
            m_KeyFramMessageCallBack(lTempAnimationCallBackPar);
            
        }
    }
}
