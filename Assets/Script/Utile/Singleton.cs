using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;
    public static bool HasInstance => _instance != null;
    public static T TryGetInstance() => HasInstance ? _instance : null;
    public static T Current => _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    Debug.LogError($"{typeof(T)} 인스턴스가 씬에 없습니다!");
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        InitializeSingleton();
    }
    protected virtual void InitializeSingleton()
    {
        //게임이 실행중이 아니라면 종료합니다.
        if (!Application.isPlaying)
        {
            return;
        }

        _instance = this as T;
    }
}
