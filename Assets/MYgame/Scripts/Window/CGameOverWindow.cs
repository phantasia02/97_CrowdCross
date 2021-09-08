using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using GameAnalyticsSDK;

public class CGameOverWindow : CSingletonMonoBehaviour<CGameOverWindow>
{
    [SerializeField] Button m_ResetButton = null;
    [SerializeField] GameObject m_ShowObj = null;
    CChangeScenes m_ChangeScenes = new CChangeScenes();

    private void Awake()
    {
        m_ResetButton.onClick.AddListener(() => {
            m_ChangeScenes.ResetScene();
        });
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
      
        }
        m_ShowObj.SetActive(showObj);
       // CGameSceneWindow.SharedInstance.SetCurState(CGameSceneWindow.EState.eEndStop);
    }
}
