using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CGameSceneWindow : CSingletonMonoBehaviour<CGameSceneWindow>
{
    public enum EButtonState
    {
        eNormal     = 0,
        eDisable    = 1,
        eHide       = 2,
    }

    CChangeScenes m_ChangeScenes = new CChangeScenes();
    [SerializeField] GameObject m_ShowObj       = null;
    [SerializeField] Button m_ResetButton       = null;
    [SerializeField] Button m_GoButton          = null;

    public void SetTemptextPos(Vector3 pos)
    {
    }

    private void Awake()
    {
        m_ResetButton.onClick.AddListener(() => {
            m_ChangeScenes.ResetScene();
        });

        m_GoButton.onClick.AddListener(() => {
            SetGoButton( EButtonState.eHide);
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

    public void SetGoButton(EButtonState settype)
    {
        if (settype == EButtonState.eDisable)
        {
            m_GoButton.gameObject.SetActive(true);
            m_GoButton.interactable = false;
        }
        else if (settype == EButtonState.eHide)
        {
            m_GoButton.gameObject.SetActive(false);
        }
        else if (settype == EButtonState.eNormal)
        {
            m_GoButton.gameObject.SetActive(true);
            m_GoButton.interactable = true;
        }
    }
}
