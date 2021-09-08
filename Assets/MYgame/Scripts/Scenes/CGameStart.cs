using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameStart : MonoBehaviour
{


    private void Awake()
    {
        GlobalData lTempGlobalData = GlobalData.SharedInstance;
        if (!lTempGlobalData)
            return;

        int lTempCurLevelIndex = CSaveManager.m_status.m_LevelIndex;
        GameObject[] lTempAllLevelObj = GlobalData.SharedInstance.LevelGameObj;
        GameObject lTempGameObjLevel = GameObject.Instantiate<GameObject>(lTempAllLevelObj[lTempCurLevelIndex]);
       // lTempGameObjLevel.transform.SetParent(this.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
