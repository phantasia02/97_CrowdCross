using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTrafficLightObj : MonoBehaviour
{

    [VarRename(new string[] { "Red", "Yellow", "Green" })]
    [SerializeField] protected GameObject[] m_AllLightObj = null;

    private void Awake()
    {
        
    }

    public void ShowLightIndex(CTrafficLightGroup.ELightIndex LightIndex)
    {
        int index = (int)LightIndex;

        if (index < 0 || index >= m_AllLightObj.Length)
            return;

        for (int i = 0; i < m_AllLightObj.Length; i++)
            m_AllLightObj[i].SetActive(false);

        m_AllLightObj[index].SetActive(true);
    }

}
