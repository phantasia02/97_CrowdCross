using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCarCollisionPlayerRogue 
{
    public CPlayerRogueMemoryShare m_MyPlayerRogueMemoryShare = null;

    public CCarCollisionPlayerRogue(CPlayerRogueMemoryShare MemoryShare)
    {
        m_MyPlayerRogueMemoryShare = MemoryShare;
    }

    public void CollisionEnter(Collision other)
    {
        if (other.gameObject.layer == (int)StaticGlobalDel.ELayerIndex.eCarCollider)
        {
            m_MyPlayerRogueMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            m_MyPlayerRogueMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eDeath;
            m_MyPlayerRogueMemoryShare.m_MyMovable.LockChangState = StaticGlobalDel.EMovableState.eDeath;
            m_MyPlayerRogueMemoryShare.m_MyGroup.SetBoolRearrangement(true, 2.0f);
            m_MyPlayerRogueMemoryShare.m_GroupIndex = -1;

            Vector3 lTempV3 = other.contacts[0].normal + (Vector3.up * 10.0f);
            Vector3 lTemppointV3 = other.contacts[0].point + (Vector3.down * 5.0f);
            lTemppointV3.Normalize();
            m_MyPlayerRogueMemoryShare.m_MyRigidbody.AddForceAtPosition(lTempV3 * Random.Range(50.0f, 100.0f), lTemppointV3);

            //m_MyPlayerRogueMemoryShare.m_MyArms.transform.parent = null;
            //Rigidbody lTempRigidbody = m_MyPlayerRogueMemoryShare.m_MyArms.GetComponent<Rigidbody>();
            //lTempRigidbody.useGravity = true;
            //lTempRigidbody.constraints = RigidbodyConstraints.None;
            //lTempRigidbody.AddForceAtPosition(lTempV3 * Random.Range(50.0f, 100.0f), Random.insideUnitSphere);
        }
    }
}
