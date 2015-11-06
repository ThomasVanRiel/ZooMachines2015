using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour
{
    private float _speed = 40;
    private float _deativateDelay = 5;

    void Update()
    {
        if (_deativateDelay > 0)
        {
            _deativateDelay -= Time.deltaTime;
            if (_deativateDelay <= 0)
                gameObject.SetActive(false);

            transform.position += transform.forward * _speed * Time.deltaTime;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public void SetDeactivationTimer(float time)
    {
        _deativateDelay = time;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c != null)
        {
            var scr = c.gameObject.GetComponent<PlayerController>();
            if (scr != null)
            {
                scr.TakeDamage(10);
            }
        }
    }

}
