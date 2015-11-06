using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof(DepthOfField))]
public class CameraHandler : MonoBehaviour
{
    public Vector3 Rotation = new Vector3(60, 315, 0);
    public float Offset = 25;

    private List<PlayerController> PlayerList = new List<PlayerController>();
    private Vector3 _lastPosition = Vector3.zero;

    private DepthOfField _dof;

    void Awake()
    {
        _dof = GetComponent<DepthOfField>();
    }

    void Update()
    {

        // set camera location and offset
        Vector3 avrPos = Vector3.zero;
        List<PlayerController> livePlayers = new List<PlayerController>();
        if (PlayerList.Count > 0)
        {
            foreach (var item in PlayerList)
            {
                if (item.Health > 0)
                {
                    avrPos += item.transform.position;
                    livePlayers.Add(item);
                }
            }
            avrPos = avrPos / livePlayers.Count;
        }

        // calc distance between furthest players
        float largestDist = 25;
        if (livePlayers.Count >= 2)
        {
            float dist = 0;
            for (int x = 0; x < livePlayers.Count; ++x)
            {
                for (int y = 0; y < livePlayers.Count; ++y)
                {
                    if (x != y)
                    {
                        float temp = Vector3.Distance(livePlayers[x].transform.position, livePlayers[y].transform.position);
                        if (temp > dist)
                        {
                            dist = temp;
                        }
                    }
                }
            }
            largestDist = Mathf.Clamp(dist, 10, float.MaxValue); ;
        }

        bool cq = (largestDist < Offset) ? true : false;
        float newOffset = Offset + ((largestDist - Offset) * ((cq) ? 0.3f : 0.50f));

        _dof.focalLength = Mathf.Abs(Offset - 1);


        _lastPosition = transform.position;
        Vector3 newPosition = avrPos + (transform.forward * (-1) * newOffset);
        transform.position = Vector3.Lerp(_lastPosition, newPosition, Time.deltaTime);


        // set camera rotation
        Vector3 newRotation = Rotation;
        if (largestDist < Offset)
        {
            const float tilt = 45;
            newRotation.x = Mathf.Lerp(Rotation.x, tilt, 1 - (largestDist / Offset));
        }

        transform.rotation = Quaternion.Euler(newRotation);
    }

    public void SetPlayerList(List<PlayerController> pl)
    {
        PlayerList = new List<PlayerController>();
        PlayerList = pl;
    }

}
