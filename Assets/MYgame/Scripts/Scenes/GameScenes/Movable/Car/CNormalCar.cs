using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CNormalCar : CCarBase
{
    public override EMovableType MyMovableType() { return EMovableType.eNormalCar; }



    protected override void CreateMemoryShare()
    {
        base.CreateMemoryShare();
    }

    protected override void Start()
    {
        base.Start();
        SetCurState(StaticGlobalDel.EMovableState.eMove);
    }
}
