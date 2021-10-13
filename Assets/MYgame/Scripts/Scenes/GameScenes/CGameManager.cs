using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UniRx;


public class CGameManager : MonoBehaviour
{
    public enum EState
    {
        eReady              = 0,
        ePlay               = 1,
        ePlayHold           = 2,
        ePlayOKPerformance  = 3,
        eReadyEnd           = 4,
        eNextEnd            = 5,
        eGameOver           = 6,
        eWinUI              = 7,
        eMax
    };

    public enum EQAData
    {
        eQ_DataIndex = 0,
        eA_DataIndex = 1,
        eMax
    };

    bool m_bInitOK = false;
    bool m_bDown = false;

    CChangeScenes m_ChangeScenes = new CChangeScenes();
    protected ResultUI m_MyResultUI = null;
    public ResultUI MyResultUI { get { return m_MyResultUI; } }

    protected Camera m_Camera = null;
    public Camera MainCamera { get { return m_Camera; } }

    protected CPlayer m_Player = null;
    public CPlayer Player { get { return m_Player; } }

    protected CEnemyGroup m_EnemyGroup = null;
    public CEnemyGroup EnemyGroup { get { return m_EnemyGroup; } }

    protected CPlayerRogueGroup m_MyPlayerRogueGroup = null;
    public CPlayerRogueGroup MyPlayerRogueGroup { get { return m_MyPlayerRogueGroup; } }

    [SerializeField] Transform m_ResulPos = null;
    public Transform ResulPos { get { return m_ResulPos; } }

    [SerializeField] GameObject m_PlayerNormalCamera = null;
    public GameObject PlayerNormalFollowObj { get { return m_PlayerNormalCamera; } }

    [SerializeField] GameObject m_PlayerResultCamera = null;
    public GameObject PlayerResultCamera { get { return m_PlayerResultCamera; } }

    [SerializeField] GameObject m_ResultEndCamera = null;
    public GameObject ResultEndCamera { get { return m_ResultEndCamera; } }

    [SerializeField] CinemachineTargetGroup m_CinemachineTargetGroup = null;
    public CinemachineTargetGroup CinemachineTargetGroup { get { return m_CinemachineTargetGroup; } }
    

    private EState m_eCurState = EState.eReady;
    public EState CurState { get { return m_eCurState; } }
    protected float m_StateTime = 0.0f;
    protected float m_StateUnscaledTime = 0.0f;
    protected int m_StateCount = 0;
    protected Vector3 m_OldInput;
    protected float m_HalfScreenWidth = 600.0f;


    void Awake()
    {
        Application.targetFrameRate = 60;
        const float HWRatioPototype = StaticGlobalDel.g_fcbaseHeight / StaticGlobalDel.g_fcbaseWidth;
        float lTempNewHWRatio = ((float)Screen.height / (float)Screen.width);
        m_HalfScreenWidth = (StaticGlobalDel.g_fcbaseWidth / 2.0f) * (lTempNewHWRatio / HWRatioPototype);

        m_MyResultUI = gameObject.GetComponentInChildren<ResultUI>();
        m_EnemyGroup = gameObject.GetComponentInChildren<CEnemyGroup>(true);
        m_MyPlayerRogueGroup = gameObject.GetComponentInChildren<CPlayerRogueGroup>(true);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            GameObject lTempCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
            if (lTempCameraObj != null)
                m_Camera = lTempCameraObj.GetComponent<Camera>();

            if (m_Camera != null )
            {
                m_bInitOK = true;
            }

            yield return null;

        } while (!m_bInitOK);

     
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_bInitOK)
            return;

  
        m_StateTime += Time.deltaTime;
        m_StateCount++;
        m_StateUnscaledTime += Time.unscaledDeltaTime;

        switch (m_eCurState)
        {
            case EState.eReady:
                {
                    UsePlayTick();
                }
                break;
            case EState.ePlay:
                {
                    UsePlayTick();
                }
                break;
            case EState.ePlayHold:
                {

                }
                break;
            case EState.ePlayOKPerformance:
                {
                }
                break;

            case EState.eReadyEnd:
                {
                }
                break;


            case EState.eNextEnd:
                {
                }
                break;
            case EState.eGameOver:
                {

                }
                break;
        }
    }

    public void LateUpdate()
    {

        //m_PlayerFollowObj.transform.position = m_Player.transform.position;
        //Vector3 lTempV3 = m_Player.transform.position;
        //lTempV3.y += 15.0f;
        //lTempV3.z += -7.5f;
        //m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, lTempV3, 0.95f);
    }


    public void SetState(EState lsetState)
    {
        if (lsetState == m_eCurState)
            return;

        EState lOldState = m_eCurState;
        m_StateTime = 0.0f;
        m_StateCount = 0;
        m_StateUnscaledTime = 0.0f;
        m_eCurState = lsetState;

        CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;

        switch (m_eCurState)
        {
            case EState.eReady:
                {
                }
                break;
            case EState.ePlay:
                {
                    if (lTempGameSceneWindow != null)
                        lTempGameSceneWindow.SetGoButton(CGameSceneWindow.EButtonState.eNormal);
                }
                break;
            case EState.ePlayHold:
                {
                    if (lTempGameSceneWindow != null)
                        lTempGameSceneWindow.SetGoButton(CGameSceneWindow.EButtonState.eDisable);
                }
                break;
            case EState.ePlayOKPerformance:
                {
                }
                break;
            case EState.eReadyEnd:
                {
                }
                break;
            case EState.eNextEnd:
                {

                }
                break;
            case EState.eWinUI:
                {
                    if (lTempGameSceneWindow)
                        lTempGameSceneWindow.ShowObj(false);

                    m_MyResultUI.ShowSuccessUI(2.0f);
                }
                break;
            case EState.eGameOver:
                {
                    if (lTempGameSceneWindow)
                        lTempGameSceneWindow.ShowObj(false);

                    m_MyResultUI.ShowFailedUI(1.0f);
                }
                break;
        }
    }

    public void UsePlayTick()
    {
//#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            m_bDown = true;
            m_OldInput = Input.mousePosition;
            //InputRay();
        }
        else if (Input.GetMouseButton(0))
        {
            //float moveX = (Input.mousePosition.x - m_OldInput.x) / m_HalfScreenWidth;
            //m_Player.SetXMove(moveX);
            //m_OldInput = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (m_bDown)
            {
                m_OldInput = Vector3.zero;
                m_bDown = false;
            }
        }

//#elif UNITY_ANDROID
//        testbugtext.Add("UsePlayTick 2222222222222222222222");
//        if (Input.touchCount == 1)
//        {
//        testbugtext.Add("UsePlayTick 3333333333333333");
//            Touch touch = Input.GetTouch(0);
//            bool isTouchUIElement = EventSystem.current.IsPointerOverGameObject(touch.fingerId);

//            if (!isTouchUIElement)
//            {
//                switch (touch.phase)
//                {
//                    case TouchPhase.Began:
//                        {
//        testbugtext.Add("UsePlayTick 4444444444444444444");
//                            m_bDown = true;
//                            m_OldInput = Input.mousePosition;
//                        }
//                        break;
//                    case TouchPhase.Moved:
//                        {
//                            m_OldInput = Input.mousePosition;
//                        }
//                        break;
//                    case TouchPhase.Ended:
//                        {
//                            if (m_bDown)
//                            {
//                                m_bDown = false;
//                            }
//                        }
//                        break;

//                }
//            }
//        }
//#endif


        if (m_eCurState == EState.eReady)
        {
            if (m_bDown)
            {
                SetState(EState.ePlay);

                CReadyGameWindow lTempCReadyGameWindow = CReadyGameWindow.SharedInstance;
                if (lTempCReadyGameWindow && lTempCReadyGameWindow.GetShow())
                    lTempCReadyGameWindow.CloseShowUI();

                CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
                if (lTempGameSceneWindow && !lTempGameSceneWindow.GetShow())
                    lTempGameSceneWindow.ShowObj(true);


                CPlayerRogueGroup lTempPlayerRogueGroup = this.GetComponentInChildren<CPlayerRogueGroup>();
                lTempPlayerRogueGroup.ShowHandIcon(true);
            }
        }

    }
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable
    List<string> testbugtext = new List<string>();
    private void OnGUI()
    {
        string test = "";
        for (int i = testbugtext.Count - 1; i >= 0; i--)
            test += $"{testbugtext[i]}\n";

        guiStyle.fontSize = 60; //change the font size
        GUI.Label(new Rect(10, 10, 1000, 2000), test, guiStyle);
    }

    public void InputRay()
    {
        
    }

    public void OnNext()
    {
        m_ChangeScenes.LoadGameScenes();
    }

    public void OnReset()
    {
        m_ChangeScenes.ResetScene();
    }

    public void AddGroup(Transform lTempAddTransform)
    {
        CinemachineTargetGroup.AddMember(lTempAddTransform, 1.0f, 2.0f);
    }

    public void RemoveMemberGroup(Transform lTempAddTransform)
    {CinemachineTargetGroup.RemoveMember(lTempAddTransform);}





}
