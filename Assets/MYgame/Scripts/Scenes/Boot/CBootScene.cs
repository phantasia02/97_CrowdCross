using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CBootScene : MonoBehaviour
{
    [SerializeField] protected int m_InitLevelIndex = 0;
    CChangeScenes m_ChangeScenes = new CChangeScenes();

    // Start is called before the first frame update
    void Start()
    {
       
        //if (Application.internetReachability <= NetworkReachability.NotReachable)
        //{
        //    CMessageBoxUIWindow.SharedInstance.ShowObj(true);
        //    return;
        //}

        // Debug.Log("CExample_Internet.SharedInstance.ConnectionStatus() = " + CExample_Internet.SharedInstance.ConnectionStatus().ToString());

        CSaveManager.m_status.m_LevelIndex = 0;
#if UNITY_EDITOR
        CSaveManager.m_status.m_LevelIndex = m_InitLevelIndex;
#endif

        m_ChangeScenes.LoadGameScenes();
        //m_ChangeScenes.LoadTestScenes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
