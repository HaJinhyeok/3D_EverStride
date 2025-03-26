using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    Transform _transform;
    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        _transform.eulerAngles += new Vector3(0, Time.deltaTime * 100, 0);
    }
}
