using UnityEngine;

public class StatNodeView : MonoBehaviour
{
    [Header("버튼이랑 매칭할 노드의 데이터")]
    [SerializeField] private StatNode _statNodeData;
    
    [Header("버튼이 활성화 되었을 경우 나타낼 아이콘")]
    [SerializeField] private GameObject _activeIcon;
    
    [Header("버튼이 잠겨있을 경우 나타낼 아이콘")]
    [SerializeField] private GameObject _lockedIcon;

    private void Start()
    {
        InitView();
    }

    public void InitView()
    {
        // 활성화 아이콘은 꺼둔 상태로 초기화
        if(_activeIcon != null) 
            _activeIcon.SetActive(false);
        
        // 만약 매칭된 노드 데이터의 상태가 Locked일 경우
        // 자물쇠 아이콘 활성화, 그 외는 비활성화
        if (_lockedIcon != null && _statNodeData.IsLocked())
        {
            _lockedIcon.SetActive(true);
        }
        else if (_lockedIcon != null && !_statNodeData.IsLocked())
        {
            _lockedIcon.SetActive(false);
        }
    }
    
    public void OnNodeClick()
    {
        Debug.Log("OnNodeClick");
        if (_statNodeData == null) return;
        
        _statNodeData.OnClick();
        UpdateActiveIcon();
        Debug.Log("클릭 처리 완료");
    }

    private void UpdateActiveIcon()
    {
        if (_statNodeData.IsActive())
        {
            _activeIcon.SetActive(true);
        }
    }
}
