using UnityEngine;

public class CloseNodeUI : MonoBehaviour
{
    private GameObject _nodeUI;

    private void Awake()
    {
        _nodeUI = gameObject;
    }
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
    public void OnClickCloseUI()
    {
        _nodeUI.SetActive(false);
    }
}
