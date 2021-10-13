using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CDeathStateActor : CMoveStateBase
{
    CActorMemoryShare m_MyActorMemoryShare = null;
  //  CActor m_TargetActorBuff = null;

    public readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

    public CDeathStateActor(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyActorMemoryShare = (CActorMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        
        SetAnimationState(CAnimatorStateCtl.EState.eDeath);
        m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.LockState = CAnimatorStateCtl.EState.eDeath;

        m_MyActorMemoryShare.m_RendererMesh.material.DOColor(new Color(0.7f, 0.7f, 0.7f), BaseColorID, 3.0f);
        m_MyGameManager.RemoveMemberGroup(m_MyActorMemoryShare.m_MyActor.DummyRef);

        //Vector3 lTempV3 = m_MyActorMemoryShare.m_MyMovable.transform.position;
        //lTempV3.y += 0.05f;
        //CGGameSceneData lTempGameSceneData = CGGameSceneData.SharedInstance;
        //GameObject lTempObj = GameObject.Instantiate(lTempGameSceneData.m_AllOtherObj[(int)CGGameSceneData.EOtherObj.eDidFloor],
        //    lTempV3, m_MyActorMemoryShare.m_MyMovable.transform.rotation);
        
        m_MyActorMemoryShare.m_MyMovable.transform.forward = -m_MyActorMemoryShare.m_DeathImpactDir;
        m_MyActorMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        m_MyActorMemoryShare.m_MyRigidbody.drag = 1.0f;
        m_MyActorMemoryShare.m_MyRigidbody.mass = 1.0f;
        m_MyActorMemoryShare.m_MyRigidbody.useGravity = true;
        Vector3 lTempForce = m_MyActorMemoryShare.m_DeathImpactDir * 2.0f + Vector3.up * 10.0f;
        m_MyActorMemoryShare.m_MyRigidbody.AddForce(lTempForce * Random.Range(200.0f, 400.0f));
        //m_End = false;
        // Time.timeScale = 0.1f;
        for (int i = 0; i < m_MyActorMemoryShare.m_AllChildCollider.Length; i++)
            m_MyActorMemoryShare.m_AllChildCollider[i].gameObject.SetActive(false);

    }

    protected override void updataState()
    {
        base.updataState();

        if (m_MyActorMemoryShare.m_MyMovable.transform.position.y <= -0.01f)
        {
            CloseCollider();
        }
    }

    protected override void OutState()
    {
        base.OutState();
    }

    //public override void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == StaticGlobalDel.TagFloor)
    //    {
    //        CloseCollider();
    //        //  m_MyPlayerRogueMemoryShare.m_MyGroup.OnTriggerEnter(other);
    //    }
    //}

    public void CloseCollider()
    {



        m_MyActorMemoryShare.m_MyRigidbody.useGravity = false;
        m_MyActorMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
