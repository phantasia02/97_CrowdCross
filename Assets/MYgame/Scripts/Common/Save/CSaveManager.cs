using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public int m_LevelIndex     = 0;
    public int m_Money          = 0;
    public int m_SceneIndex     = 0;
}


public class Config
{
    public int m_Sound          = 1;
    public int m_Vibrate        = 1;
}


public class CSaveManager : CSingletonMonoBehaviour<CSaveManager>
{
    const string SaveKey_status = "GameData.Status";
    const string SaveKey_Config = "GameData.Config";
    

    public static Status m_status;
    public static Config m_config;

    private void Awake()
    {
        string lTempDataStr;

        m_status = new Status();
        lTempDataStr = PlayerPrefs.GetString(SaveKey_status);
        if (lTempDataStr.Length != 0)
            m_status = JsonUtility.FromJson<Status>(lTempDataStr);

        m_config = new Config();
        lTempDataStr = PlayerPrefs.GetString(SaveKey_Config);
        if (lTempDataStr.Length != 0)
            m_config = JsonUtility.FromJson<Config>(lTempDataStr);

        //PlayerPrefs.GetString("GameData");
        // PrefsSerialize.Load("savedata_status", status, false);

        // Save();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        PlayerPrefs.SetString(SaveKey_status, JsonUtility.ToJson(m_status));
        PlayerPrefs.SetString(SaveKey_Config, JsonUtility.ToJson(m_config));
        //PrefsSerialize.Save("savedata_status", status);
        //PrefsSerialize.Save("savedata_config", config);
        //PrefsSerialize.Save("savedata_design", design);
    }
}
