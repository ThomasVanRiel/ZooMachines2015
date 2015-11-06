using UnityEngine;
using System.Collections.Generic;

public class TrainDispatcher : MonoBehaviour
{
    public Train Train;
    public Vector3 TrainDimentions = new Vector3(2.5f, 5f, 14f);
    public int PoolSize = 1;

    public bool DispatchTrainAtStart = false;
    public float DispatchTimeInterval = 10;
    private float _timer = 0;

    public float TrainSpeed = 40;
    public float TrainDeactivationTime = 5;

    private List<Train> _poolList = new List<Train>();
    private int _poolListIndex = 0;

    void Start()
    {
        if (PoolSize <= 0 || Train == null)
            DestroyImmediate(this);

        // Create pool
        GameObject poolObj = new GameObject("ObjectPool");
        poolObj.transform.SetParent(gameObject.transform);

        for (int i = 0; i < PoolSize; ++i)
        {
            Train t = Instantiate(Train, poolObj.transform.position, poolObj.transform.rotation) as Train;
            t.transform.SetParent(poolObj.transform);
            _poolList.Add(t);
            t.gameObject.SetActive(false);
        }

        if (!DispatchTrainAtStart)
            _timer = Time.time + DispatchTimeInterval;
    }

    void Update()
    {
        if (_timer <= Time.time)
        {
            _timer = Time.time + DispatchTimeInterval;

            // dispatch new train
            Train train = _poolList[_poolListIndex++];
            if (_poolListIndex >= PoolSize)
                _poolListIndex = 0;

            train.transform.position = transform.position;
            train.transform.rotation = transform.rotation;
            train.SetSpeed(TrainSpeed);
            train.SetDeactivationTimer(TrainDeactivationTime);
            train.enabled = true;
            train.gameObject.SetActive(true);
        }
    }


}
