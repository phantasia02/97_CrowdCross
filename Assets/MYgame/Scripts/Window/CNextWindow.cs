using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using GameAnalyticsSDK;

public class CNextWindow : CSingletonMonoBehaviour<CNextWindow>
{
    CChangeScenes m_ChangeScenes = new CChangeScenes();
    [SerializeField] GameObject m_ShowObj = null;
    [SerializeField] Text m_TextScore = null;
    int m_GetMoney = 0;
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

    public bool GetShow() { return m_ShowObj.activeSelf; }

    public void ShowObj(bool showObj)
    {

        if (showObj)
        {
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, GlobalData.g_ShowCurLevelNamePrefix + (CSaveManager.m_status.m_LevelIndex + 1).ToString());
            //CFacebookManager.SharedInstance.LevelEnable(CSaveManager.m_status.m_LevelIndex + 1);

            //CAudioManager.SharedInstance.PlaySE(CAudioManager.ESE.eWin);

            //Vibration.PlayVibrate();
            //m_GetMoney = (CSaveManager.m_status.m_LevelIndex * 2) + 1;
          //  m_ChangeScenes.SetNextLevel();
           
        }

        m_ShowObj.SetActive(showObj);
     //   CGameSceneWindow.SharedInstance.SetCurState( CGameSceneWindow.EState.eEndStop);
    }

    public void SetScore(int Score)
    {
        m_TextScore.text = "Score : " + Score.ToString();
    }

    public void OnNext()
    {
        m_ShowObj.SetActive(false);
        AddMoney(m_GetMoney);
        m_ChangeScenes.LoadGameScenes();
    }

    public void ShowAD()
    {

        //GoogleMobileAdsScript lTempGoogleMobileAdsScript = GoogleMobileAdsScript.SharedInstance;
        //if (!lTempGoogleMobileAdsScript)
        //    return;

        //if (!lTempGoogleMobileAdsScript.CanShow())
        //    return;

        //ShowObj(false);
        //AddMoney((int)Math.Pow((float)m_GetMoney, 2.0f));
        //m_ChangeScenes.LoadGameScenes();
        //lTempGoogleMobileAdsScript.UserChoseToWatchAd();
    }

    public void AddMoney(int money)
    {
        CSaveManager.m_status.m_Money += money;
        CSaveManager.SharedInstance.Save();
    //    CGameSceneWindow.SharedInstance.UpdataDataInfo();
    }
}
