using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;



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

    protected CEnemy m_Enemy = null;
    public CEnemy Enemy { get { return m_Enemy; } }

    protected List<CDragHero> m_AllDragHero = new List<CDragHero>();
    public void AddAllDragHero(CDragHero AddDragHero) { m_AllDragHero.Add(AddDragHero); }
    public void RemoveAllDragHero(CDragHero AddDragHero) { m_AllDragHero.Remove(AddDragHero); }

    private EState m_eCurState = EState.eReady;
    public EState CurState { get { return m_eCurState; } }
    protected float m_StateTime = 0.0f;
    protected float m_StateUnscaledTime = 0.0f;
    protected int m_StateCount = 0;
    protected Vector3 m_OldInput;
    protected float m_HalfScreenWidth = 600.0f;

    protected CDragHeroFloor m_MyDragHeroFloor = null;
    public CDragHeroFloor MyDragHeroFloor { get { return m_MyDragHeroFloor; } }



    void Awake()
    {
        Application.targetFrameRate = 60;
        const float HWRatioPototype = StaticGlobalDel.g_fcbaseHeight / StaticGlobalDel.g_fcbaseWidth;
        float lTempNewHWRatio = ((float)Screen.height / (float)Screen.width);
        m_HalfScreenWidth = (StaticGlobalDel.g_fcbaseWidth / 2.0f) * (lTempNewHWRatio / HWRatioPototype);

        m_MyResultUI = gameObject.GetComponentInChildren<ResultUI>();
        m_MyDragHeroFloor = gameObject.GetComponentInChildren<CDragHeroFloor>();

       // this.GetComponentsInChildren<>();

    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            GameObject lTempCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
            if (lTempCameraObj != null)
                m_Camera = lTempCameraObj.GetComponent<Camera>();

            if (m_Player == null)
                m_Player = gameObject.GetComponentInChildren<CPlayer>();

            if (m_Enemy == null)
                m_Enemy = gameObject.GetComponentInChildren<CEnemy>();

            if (m_Camera != null && m_Player != null && m_Enemy != null)
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
                    if (lTempGameSceneWindow != null)
                        lTempGameSceneWindow.SetGoButton(CGameSceneWindow.EButtonState.eHide);

                    Player.PlayStart();
                    MyDragHeroFloor.ShowSECollider(false);
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

                    m_MyResultUI.ShowSuccessUI(1.0f);
                }
                break;
            case EState.eGameOver:
                {
                    if (lTempGameSceneWindow)
                        lTempGameSceneWindow.ShowObj(false);

                    m_MyResultUI.ShowFailedUI(4.5f);
                }
                break;
        }
    }

    public void UsePlayTick()
    {
#if UNITY_EDITOR
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

#else
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            bool isTouchUIElement = EventSystem.current.IsPointerOverGameObject(touch.fingerId);

            if (!isTouchUIElement)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        {
                            m_bDown = true;
                            m_OldInput = Input.mousePosition;
                        }
                        break;
                    case TouchPhase.Moved:
                        {
                            m_OldInput = Input.mousePosition;
                        }
                        break;
                    case TouchPhase.Ended:
                        {
                            if (m_bDown)
                            {
                                m_bDown = false;
                            }
                        }
                        break;

                }
            }
        }
#endif


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
                {
                    lTempGameSceneWindow.ShowObj(true);
                }
            }
        }

    }

    public void InputRay()
    {
        
    }


    public void AllDragHeroDie()
    {
        for (int i = 0; i < m_AllDragHero.Count; i++)
            m_AllDragHero[i].ChangState = StaticGlobalDel.EMovableState.eDeath;
    }

    public void OnNext()
    {
        m_ChangeScenes.LoadGameScenes();
    }

    public void OnReset()
    {
        m_ChangeScenes.ResetScene();
    }


    public void CheckDragHeroGroupJumpOK()
    {
        if (MyDragHeroFloor.CheckDragHeroGroupJumpOK())
            SetState(EState.ePlay);
    }

    public void GoPlayPerformance()
    {
        SetState( EState.ePlayOKPerformance);
    }
}
