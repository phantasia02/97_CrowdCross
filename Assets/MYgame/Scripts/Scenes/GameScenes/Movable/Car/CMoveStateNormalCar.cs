using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CMoveStateNormalCar : CMoveStateBase
{
    public CMoveStateNormalCar(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
    }

    protected override void InState()
    {
        base.InState();
    }

    protected override void updataState()
    {
        base.updataState();

        m_MyMemoryShare.m_MyMovable.transform.Translate(0.0f, 0.0f, Time.deltaTime * m_MyMemoryShare.m_TotleSpeed);
    }

    protected override void OutState()
    {
        base.OutState();
    }
}
