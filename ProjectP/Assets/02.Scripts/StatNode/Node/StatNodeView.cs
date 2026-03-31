using System;
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

    private void OnEnable()
    {
        // 버튼 클릭시 다른 노드들의 상태가 연쇄적으로 변경되는 로직이 있음
        // 이를 감지하여 활성화, 비활성화시 아이콘 활성화 관련 로직 처리를 위한 아이콘 갱신 이벤트
        PostManager.Instance.Subscribe<Action>(PostMessageKey.NodeUIIconUpdate,UpdateActiveIcon);
    }

    private void InitView()
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
        // 해당 이벤트를 받는 쪽에 UI 갱신 구현
        // levelUp 시도를 세번 하여 3번째에 레벨업 실시
        PostManager.Instance.Post(PostMessageKey.NodeLevelUp, 1);
        Debug.Log("클릭 처리 완료");
    }

    private void UpdateActiveIcon(Action callback)
    {
        if (_statNodeData.IsActive())
        {
            _activeIcon.SetActive(true);
            _lockedIcon.SetActive(false);
        }
        else if (_statNodeData.IsLocked())
        {
            _activeIcon.SetActive(false);
            _lockedIcon.SetActive(true);
        }
        else
        {
            _activeIcon.SetActive(false);
            _lockedIcon.SetActive(false);
        }
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<Action>(PostMessageKey.NodeUIIconUpdate,UpdateActiveIcon);
    }
}
