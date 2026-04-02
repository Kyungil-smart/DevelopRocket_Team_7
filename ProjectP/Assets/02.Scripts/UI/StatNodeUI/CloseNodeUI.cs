using UnityEngine;

public class CloseNodeUI : MonoBehaviour
{
    private GameObject _nodeUI;

    private void Awake()
    {
        _nodeUI = gameObject;
    }

    public void OnClickCloseUI()
    {
        _nodeUI.SetActive(false);
    }
}
