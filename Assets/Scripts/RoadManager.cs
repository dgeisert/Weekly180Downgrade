using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public Transform truck;
    public IslandBuilder roadSegment;

    public int segments = 0;
    public float downY = 0;
    float grade = 20;
    List<IslandBuilder> roadSegments = new List<IslandBuilder>();

    private void Start()
    {
        roadSegments.Add(Instantiate(roadSegment, new Vector3(-40, 0, 0), Quaternion.identity).Init(grade));
    }
    void Update()
    {
        if (truck.position.z > (segments - 1) * 80)
        {
            segments++;
            if (roadSegments.Count > 3)
            {
                IslandBuilder rs = roadSegments[0];
                roadSegments.Remove(rs);
                Destroy(rs.gameObject);
            }
            grade += 3;
            downY -= grade / 100 * 80 - 2.2f;
            roadSegments.Add(Instantiate(roadSegment, new Vector3(-40, downY, 80 * segments), Quaternion.identity).Init(grade));
        }
    }
}