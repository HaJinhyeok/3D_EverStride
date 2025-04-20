using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    Transform _transform;

    const float _rotationSpeed = 100f;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        _transform.eulerAngles += new Vector3(0, Time.deltaTime * _rotationSpeed, 0);
    }
}
