using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCarCreatePos : CGameObjBas
{
    protected CAllCarCreatePos m_MyAllCarCreatePos = null;

    protected List<CCarBase> m_MyAllCar = new List<CCarBase>();

    [SerializeField] protected Collider m_EndCollider = null;

    public override EObjType ObjType() { return EObjType.eCarCreatePos; }

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void AddCar(CCarBase car)
    {
        int Addindex = m_MyAllCar.Count;
        m_MyAllCar.Add(car);
        car.transform.parent = this.transform;
        car.transform.localPosition = Vector3.zero;
        car.transform.rotation = this.transform.rotation;
        car.AddToList(Addindex);
    }

    public void RemoveCar(CCarBase car)
    {
        m_MyAllCarCreatePos.AllCarBasePool.RemoveObj(car);
    }

    private void OnEnable()
    {
        m_MyAllCarCreatePos = this.gameObject.GetComponentInParent<CAllCarCreatePos>();
        m_MyAllCarCreatePos.AddAllCarCreatePos(this);

      //  Vector3 lTempv3pos2 = Vector3.zero;
        Vector3 lTempv3pos = Vector3.Lerp(transform.position, m_EndCollider.transform.position, 0.6f);
        lTempv3pos.y = 0.0f;

        Vector3 lTempv3Dir = transform.position - m_EndCollider.transform.position;
        lTempv3Dir.y = 0.0f;
        lTempv3Dir.Normalize();

        CCarBase lTempCarBase = null;

        void AddCarFunc(Vector3 lpos)
        {
            lTempCarBase = m_MyAllCarCreatePos.AllCarBasePool.AddObj();
            AddCar(lTempCarBase);
            lTempCarBase.transform.position = lpos;
        }

        AddCarFunc(lTempv3pos);

        for (int i = 0; i < 10; i++)
        {
            lTempv3pos = Vector3.MoveTowards(lTempCarBase.transform.position, this.transform.position, lTempCarBase.CarLong + Random.Range(2.0f, 5.0f));
            lTempv3pos.y = 0.0f;
            if (Vector3.SqrMagnitude(lTempv3pos - this.transform.position) < 1.0f)
                break;

            AddCarFunc(lTempv3pos);
        }
    }

    private void OnDisable()
    {
        m_MyAllCarCreatePos.RemoveAllCarCreatePos(this);

        for (int i = m_MyAllCar.Count - 1; i >= 0; i--)
            m_MyAllCarCreatePos.AllCarBasePool.RemoveObj(m_MyAllCar[i]); 
    }
}
