using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance = null;
    public static bool IsInstance => _instance != null;
    public static T TryGetInstance() => IsInstance ? _instance : null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameObject = GameObject.Find("@Managers");
                if (gameObject == null)
                {
                    gameObject = new GameObject("@Managers");
                    DontDestroyOnLoad(gameObject);
                }
                _instance = GameObject.FindAnyObjectByType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    T component = obj.AddComponent<T>();
                    obj.transform.parent = gameObject.transform;

                    _instance = component;
                }
            }
            return _instance;
        }
    }

    private void OnApplicationQuit()
    {
        GameObject gameObject = GameObject.Find("@Managers");
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        Initialize();
    }
    protected virtual void Initialize()
    {
        SceneManager.sceneLoaded += OnsceneChanged;
    }

    protected virtual void OnsceneChanged(Scene sncne,LoadSceneMode mode)
    {
        Clear();
    }

    protected virtual void Clear()
    {

    }
}