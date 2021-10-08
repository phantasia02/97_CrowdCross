using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CMoveStateNormalCar : CMoveStateBase
{
    CCarBaseMemoryShare m_MyCCarBaseMemoryShare = null;

    public CMoveStateNormalCar(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyCCarBaseMemoryShare = (CCarBaseMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
    }

    protected override void updataState()
    {
        base.updataState();

        m_MyMemoryShare.m_MyMovable.transform.Translate(0.0f, 0.0f, Time.deltaTime * m_MyMemoryShare.m_TotleSpeed);
        m_MyCCarBaseMemoryShare.m_CarBase.ForwardObjCheck();
        m_MyCCarBaseMemoryShare.m_CarBase.UpdateSpeed();
    }

    protected override void OutState()
    {
        base.OutState();
    }
}
