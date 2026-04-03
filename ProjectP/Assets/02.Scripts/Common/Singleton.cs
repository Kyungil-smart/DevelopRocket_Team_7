using UnityEngine;

 
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 외부에서 접근할 때 사용하는 Property
    private static T m_Instance;
    [Header("씬 이동시 유지 할것인지")]
    [SerializeField]private bool m_DontDestroyOnLoad = false;

    // private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            // if (_applicationIsQuitting)
            // {
            //     return null;
            // }
            if (m_Instance == null)
            {
                // 씬에서 해당 타입의 객체를 찾습니다.
                m_Instance = FindFirstObjectByType<T>();

                // 씬에 없다면 새로 생성합니다.
                if (m_Instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    m_Instance = obj.AddComponent<T>();
                }
            }
            return m_Instance;
        }
    }

    protected virtual void Awake()
    {
        // 인스턴스가 이미 있는데 내가 그 인스턴스가 아니라면 (중복 생성 방지)
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this as T;
        if(m_DontDestroyOnLoad)
        {
          DontDestroyOnLoad(this.gameObject);
        }
    }
    // protected virtual void OnDestroy()
    // {
    //     // 앱이 종료되어 이 오브젝트가 파괴될 때 플래그를 true로 설정
    //     _applicationIsQuitting = true;
    // }
}