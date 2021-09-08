using UnityEngine;

public class VarRename : PropertyAttribute
{
    public string g_VarName;
    public int g_Index;
    public string[] g_StrList;

    public VarRename(string elementTitleVar)
    {
        Init();
        g_VarName = elementTitleVar;
    }

    public VarRename(int index)
    {
        Init();
        g_Index = index;
    }

    public VarRename(string[] strList)
    {
        Init();
        g_StrList = strList;
    }

    private void Init()
    {
        g_VarName = "";
        g_StrList = new string[0];
        g_Index = -1;
    }
}