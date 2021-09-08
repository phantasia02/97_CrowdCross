using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CDebugWindow : CSingletonMonoBehaviour<CDebugWindow>
{

    [SerializeField] GameObject m_Background    = null;
    [SerializeField] Button     m_Debug         = null;


    [SerializeField] Button m_Reset = null;
    [SerializeField] Button m_Next  = null;

    [SerializeField] CBtnCtrlNumber m_JupmNumber    = null;
    [SerializeField] Button         m_JupmButton    = null;

    [SerializeField] List<Text>     m_AllDebugtext  = new List<Text>();
    [SerializeField] Text           m_DebugTextPre  = null;

    CChangeScenes m_ChangeScenes = new CChangeScenes();

    private void Awake()
    {
        //    OnClickClose();
        //    m_JupmNumber.NumberMax = GlobalData.SharedInstance.LevelGameObj.Length - 1;
        //    m_JupmNumber.NumberMin = 0;
        //    m_JupmNumber.SetNumber(m_JupmNumber.NumberMin);


        Vector3 lTempV3;
        for (int i = 0; i < 10; i++)
        {
            m_AllDebugtext.Add(GameObject.Instantiate<Text>(m_DebugTextPre, m_Background.transform));
            m_AllDebugtext[i].text = i.ToString();

            lTempV3 = m_AllDebugtext[i].transform.localPosition;

            //RectTransform lTempRectTransform = m_AllDebugtext[i].gameObject.GetComponent<RectTransform>();
            //if (lTempRectTransform)
            //    lTempRectTransform.pivot.Set(0.0f, 0.0f);
            


            lTempV3.x = 500.0f;
            lTempV3.y = -800.0f - (i * 50.0f);
            
            m_AllDebugtext[i].transform.localPosition = lTempV3;
        }
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    public void AddDebugText(string Addtext)
    {
        for (int i = m_AllDebugtext.Count - 1; i >= 0; i--)
        {
            if (i + 1 < m_AllDebugtext.Count)
                m_AllDebugtext[i + 1].text = m_AllDebugtext[i].text;
        }

        m_AllDebugtext[0].text = Addtext;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickDebug()
    {
        m_Background.SetActive(true);
        m_Debug.gameObject.SetActive(false);
    }

    public void OnClickClose()
    {
        m_Background.SetActive(false);
        m_Debug.gameObject.SetActive(true);
    }

    public void OnReset()
    {
        m_ChangeScenes.ResetScene();
    }

    public void OnNext()
    {
        m_ChangeScenes.SetNextLevel();
        m_ChangeScenes.LoadGameScenes();
    }

    public void OnJupm()
    {
        CSaveManager.m_status.m_LevelIndex = m_JupmNumber.Number;
        m_ChangeScenes.LoadGameScenes();
    }

}
