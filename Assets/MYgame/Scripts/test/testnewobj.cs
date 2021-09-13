using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testnewobj : MonoBehaviour
{
    public GameObject testPrefabs;

    private void Awake()
    {
        //Vector3 centerBottom = gameObject.transform.position;
        //float radius = 0.0f;
        //float angleDegrees = 10f;
        //float angleRadians = angleDegrees * Mathf.PI / 180f;
        //for (var i = 0; i < 100; i++)
        //{
        //    float x = radius * Mathf.Cos(i * angleRadians);
        //    float z = radius * Mathf.Sin(i * angleRadians);
        //    Vector3 pos = new Vector3(centerBottom.x + x, centerBottom.y, centerBottom.z + z);

        //    GameObject step = Instantiate(testPrefabs, pos, Quaternion.identity);
        //    step.transform.Rotate(0, -i * angleDegrees, 0);
        //    radius += 0.1f;
        //}

        List<Vector3> targetPositionList = GetPositionListAround(this.transform.position, new float[] { 1.5f, 3.0f, 4.5f, 6.0f, 7.5f}, new int[] { 8, 20, 40, 50, 70, 100 });

        for (var i = 0; i < 100; i++)
        {


            GameObject step = Instantiate(testPrefabs, targetPositionList[i], this.transform.rotation, this.transform);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        Vector3 ApplyRotationToVector(Vector3 vec, float angle){return Quaternion.Euler(0, angle, 0) * vec;}

        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0, 0), angle);
            Vector3 position = startPosition + dir * (distance + Random.Range(-0.1f, 0.5f));
            positionList.Add(position);
        }
        return positionList;
    }

}
