using System.Collections;
using UnityEngine;

public class MainSceneCameraController : MonoBehaviour
{
    public Camera[] Cameras;

    Vector3[] _positions = { new Vector3(10f, 6f, 60f), new Vector3(35f, 10f, -75f), new Vector3(85f, 18f, 45f) };
    Vector3[] _directions = { new Vector3(1f, 1f, -2f), new Vector3(1f, 1f, -1f), new Vector3(0f, 0f, -1f) };

    Camera _currentCamera;
    Vector3 _currentDirection;
    int i;

    void Start()
    {
        i = 0;
        StartCoroutine(CoCameraMoveSequence());
    }

    void Update()
    {
        _currentCamera.transform.Translate(_currentDirection.normalized * Time.deltaTime * 2f);
    }

    IEnumerator CoCameraMoveSequence()
    {
        while(true)
        {
            _currentCamera = Cameras[i];
            _currentCamera.transform.position = _positions[i];
            _currentDirection = _directions[i];
            _currentCamera.gameObject.SetActive(true);
            yield return new WaitForSeconds(4f);
            _currentCamera.gameObject.SetActive(false);
            i = (i + 1) % 3;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
