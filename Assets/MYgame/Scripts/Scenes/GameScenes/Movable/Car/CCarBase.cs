using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CCarBase : CMovableBase
{
    [SerializeField] protected float m_DefSpeed = StaticGlobalDel.g_DefMovableTotleSpeed;
    public override float DefSpeed { get { return m_DefSpeed; } }
}
