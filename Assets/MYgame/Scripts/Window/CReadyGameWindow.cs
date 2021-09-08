using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//using GameAnalyticsSDK;

public class CReadyGameWindow : CSingletonMonoBehaviour<CReadyGameWindow>
{

    //[SerializeField] Button m_OptionButton;
    //[SerializeField] Button m_SkinButton;

    [SerializeField] TextMeshProUGUI m_CurLevelText = null;
    [SerializeField] TextMeshProUGUI m_CurLevelTextShadow = null;
    [SerializeField] TextMeshProUGUI m_PlayerNumberText = null;
    [SerializeField] TextMeshProUGUI m_PlayerNumberTextShadow = null;
    [SerializeField] GameObject m_ShowObj = null;
    [SerializeField] GameObject m_ReadyBack = null;
    //[SerializeField] GameObject m_OptionShowObj;
    //[SerializeField] GameObject m_SkinShowObj;
    bool m_CloseShowUI = false;

    protected Text[] m_AllText = null;
    float m_Time = 0.0f;

    private void Awake()
    {
        m_ShowObj.SetActive(true);

        m_ReadyBack.SetActive(true);

        
        CSaveManager lTempCSaveManager = CSaveManager.SharedInstance;
        if (lTempCSaveManager)
            m_CurLevelTextShadow.text = m_CurLevelText.text = (SceneManager.GetActiveScene().buildIndex).ToString() + "\n" + GlobalData.g_ShowCurLevelNamePrefix;

            //m_AllText = this.GetComponentsInChildren<Text>();
            //  GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, m_CurLevelText.text);

            //m_OptionButton.onClick.AddListener(() => {
            //    m_OptionShowObj.SetActive(true);
            //});

            //m_SkinButton.onClick.AddListener(() => {
            //    m_SkinShowObj.SetActive(true);
            //});

        }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //m_Time += Time.deltaTime;

        //Color lTempcolor;
        //for (int i = 0; i < m_AllText.Length; i++)
        //{
        //    lTempcolor = m_AllText[i].color;
        //    lTempcolor.r = lTempcolor.b  = lTempcolor.g = Mathf.Sin(m_Time * GlobalData.g_TUA) * 0.3f + 0.7f;
        //    m_AllText[i].color = lTempcolor;
        //}
        //float timeRoit = 
    }

    public void SetPlayerNumber(int Number)
    {
        m_PlayerNumberTextShadow.text = m_PlayerNumberText.text = Number.ToString();
    }

    public bool GetShow() { return m_ShowObj.activeSelf; }

    public void CloseShowUI()
    {
        if (m_CloseShowUI)
            return;

        m_CloseShowUI = true;
        m_ShowObj.SetActive(false);
    //    CGameSceneWindow.SharedInstance.SetCurState(CGameSceneWindow.EState.eStart);
    }
}
