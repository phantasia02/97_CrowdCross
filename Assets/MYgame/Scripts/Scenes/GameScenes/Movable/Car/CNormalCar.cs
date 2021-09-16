using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNormalCar : CCarBase
{
    public override EMovableType MyMovableType() { return EMovableType.eNormalCar; }

    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStateNormalCar(this));
    }

    protected override void CreateMemoryShare()
    {
        //m_MyPlayerRogueMemoryShare = new CPlayerRogueMemoryShare();
        //m_MyMemoryShare = m_MyPlayerRogueMemoryShare;
        //m_MyPlayerRogueMemoryShare.m_MyPlayerRogue = this;

        //SetBaseMemoryShare();

        base.CreateMemoryShare();
    }

    protected override void Start()
    {
        base.Start();
         SetCurState(StaticGlobalDel.EMovableState.eMove);
    }
}
