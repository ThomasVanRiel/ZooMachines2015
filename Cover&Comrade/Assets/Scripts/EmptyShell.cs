using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EmptyShell : MonoBehaviour
{
    public float ForceMin;
    public float ForceMax;

    private Rigidbody _rigid = null;
    private float _lifeTime = 5;
    private float _fadeTime = 3;

    void Start()
    {
        float force = Random.Range(ForceMin, ForceMax);
        _rigid = GetComponent<Rigidbody>();
        _rigid.AddForce(transform.right * force);
        _rigid.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(_lifeTime);

        float percent = 0;
        float fadeSpeed = 1 / _fadeTime;

        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            Material mat = rend.material;
            Color initialColor = mat.color;

            while (percent < 1)
            {
                percent += Time.deltaTime * fadeSpeed;
                mat.color = Color.Lerp(initialColor, Color.clear, percent);
                yield return null;
            }
        }

        Destroy(gameObject);
        yield return null;
    }
}
