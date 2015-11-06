using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class CameraHandler : MonoBehaviour
{
    public Vector3 Rotation = new Vector3(60, 315, 0);
    public float Offset = 25;

    private List<PlayerController> PlayerList = new List<PlayerController>();

    private DepthOfField _dof;

    void Awake()
    {
        _dof = GetComponent<DepthOfField>();
    }

    void Start()
    {
        PlayerController[] ps = GameObject.FindObjectsOfType<PlayerController>();
        foreach (var item in ps)
        {
            PlayerList.Add(item);
        }
    }

    void Update()
    {

        // set camera location and offset
        Vector3 avrPos = Vector3.zero;
        int liveCount = 0;
        if (PlayerList.Count > 0)
        {
            foreach (var item in PlayerList)
            {
                if (item.Health > 0)
                {
                    ++liveCount;
                    avrPos += item.transform.position;
                }
            }
            avrPos = avrPos / liveCount;
        }

        // calc distance between furthest players
        float largestDist = 25;
        if (liveCount >= 2)
        {
            float dist = 0;
            for (int x = 0; x < PlayerList.Count; ++x)
            {
                for (int y = 0; y < PlayerList.Count; ++y)
                {
                    if (x != y)
                    {
                        float temp = Vector3.Distance(PlayerList[x].transform.position, PlayerList[y].transform.position);
                        if (temp > dist)
                        {
                            dist = temp;
                        }
                    }
                }
            }
            if (dist > 0)
                largestDist = dist;
        }

        bool cq = (largestDist < Offset) ? true : false;
        float newOffset = Offset + ((largestDist - Offset) * ((cq) ? 0.8f : 0.35f));

        _dof.focalLength = Mathf.Abs(Offset - 1);

        transform.position = avrPos + (transform.forward * (-1) * newOffset);


        // set camera rotation
        Vector3 newRotation = Rotation;
        if (largestDist < Offset)
        {
            const float tilt = 45;
            newRotation.x = Mathf.Lerp(Rotation.x, tilt, 1 - (largestDist / Offset));
        }

        transform.rotation = Quaternion.Euler(newRotation);

    }
}
