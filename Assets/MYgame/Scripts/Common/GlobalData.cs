using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalData : CSingletonMonoBehaviour<GlobalData>
{
    //public const string g_scLevelPrefix = "Level_";
    public const string g_GameScenesName = "GameScenes";
    public const string g_testScenesName = "test";
    public const string g_ShowCurLevelNamePrefix = "Stage";




    public const float g_CameraSwitchTime           = 2.0f;
    public const float g_CameraRange                = 10.0f;
    //public const float g_fPlayerMaxSpeed        = 180.0f;
    // public const float g_fAccelerationSpeed     = 60.0f;
    //public const float g_fPlayerSlowDownSpeed   = 180.0f;
    //public const float g_PlayerToCameraMax      = -25.0f;




    static public string   g_CurSceneName    = "Boot";
    static public int      g_CurSceneIndex   = -1;
    // static public int      g_LevelIndex      = 0;


    [SerializeField] GameObject[] m_AllLevelGameObj = null;
    public GameObject[] LevelGameObj { get { return m_AllLevelGameObj; } }
    // public void SetSc
}
