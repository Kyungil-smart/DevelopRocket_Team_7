using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using XNode;

// 현재 노드 UI 관련하여 디자인이나 기타 사항들이 정해진 사항이 없어서 임시로 화면에 출력만 되게끔 만든 TempUI
// 변경될 여지가 아주 많음

public class StatTreeView : MonoBehaviour
{
    public StatNodeGraph statGraph;     // xNode 그래프 에셋
    public VisualTreeAsset nodeTemplate; // 화면에 출력할 node 자체 ui

    // 노드와 생성된 UI를 매칭해서 저장해둘 Dictionary
    private Dictionary<StatNode, VisualElement> _nodeDictionary = new Dictionary<StatNode, VisualElement>();
    
    private VisualElement _container;
    private VisualElement _root;
    private bool _isOpened = false; // 현재 창이 열려있는지 확인, 처음 시작시에는 비활성화

    void OnEnable()
    {
        // UI Document에서 'Container'라는 이름의 요소를 찾기
        _root = GetComponent<UIDocument>().rootVisualElement;
        _container = _root.Q<VisualElement>("Container");
        
        // 처음 시작할 때는 화면에서 숨김
        _root.style.display = DisplayStyle.None;
        
    }
    
    void Update()
    {
        // 테스트를 위하여 K키 입력 감지시 ui 열리게끔 설계, 추후 변경 예정
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            ToggleSkillTree();
        }
    }
    
    void ToggleSkillTree()
    {
        _isOpened = !_isOpened;
        _root.style.display = _isOpened ? DisplayStyle.Flex : DisplayStyle.None;

        // 마우스 커서 제어
        UnityEngine.Cursor.visible = _isOpened;
        UnityEngine.Cursor.lockState = _isOpened ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void GenerateTree()
    {
        _container.Clear(); // 기존 UI 삭제
        _nodeDictionary.Clear(); // 매칭해둔 Dictionary 삭제

        foreach (Node node in statGraph.nodes)
        {
            if (node is StatNode movementNode)
            {
                // 템플릿(Label + Button)을 새로 만듬
                VisualElement nodeUI = nodeTemplate.Instantiate();
                
                // 데이터 연결 (UXML 내부의 이름과 일치해야 함)
                nodeUI.Q<Label>("StatName").text = movementNode.GetNodeStatType();
                nodeUI.Q<Label>("StatNodeState").text = movementNode.GetNodeState(); 
                // 만약 Label에 이름을 지어줬다면 .Q<Label>("MyLabelName") 로 작성
                
                // 버튼 찾아서 로그 찍기 (Hierarchy에 있는 이름 기준)
                Button btn = nodeUI.Q<Button>("UnlockBtn");
                if (btn != null)
                {
                    btn.clicked += () => {
                        // 버튼 클릭 시 실행할 메서드
                        movementNode.OnClick();
                        RefreshAllNodes();
                    };
                }

                // xnode Editor position을 Scene과 동기화하여 보여주게끔 설정 
                nodeUI.style.position = Position.Absolute;
                nodeUI.style.left = node.position.x;
                nodeUI.style.top = node.position.y;

                _container.Add(nodeUI);
                _nodeDictionary.Add(movementNode, nodeUI); // 사전에 등록
            }
            
        }
        
        // 생성 후 한번 갱신
        RefreshAllNodes();
    }
    
    // 데이터 값만 업데이트하는 함수
    public void RefreshAllNodes()
    {
        foreach (var item in _nodeDictionary)
        {
            StatNode node = item.Key;
            VisualElement ui = item.Value;

            // UI 요소들의 텍스트나 스타일만 변경 (새로 만들지 않음)
            ui.Q<Label>("StatName").text = node.GetNodeStatType();
            ui.Q<Label>("StatNodeState").text = node.GetNodeState();
            
        }
    }
    
}

