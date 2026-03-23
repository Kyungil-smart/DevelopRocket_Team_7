using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using XNode;

public class StatTreeView : MonoBehaviour
{
    public StatNodeGraph statGraph;     // xNode 그래프 에셋
    public VisualTreeAsset nodeTemplate; // 화면에 출력할 node 자체 ui

    private VisualElement _container;
    private VisualElement _root;
    private bool _isOpened = false; // 현재 창이 열려있는지 확인

    void OnEnable()
    {
        // UI Document에서 'Container'라는 이름의 요소를 찾기
        _root = GetComponent<UIDocument>().rootVisualElement;
        _container = _root.Q<VisualElement>("Container");
        
        // 처음 시작할 때는 화면에서 숨김
        _root.style.display = DisplayStyle.None;

        if (statGraph != null) GenerateTree();
    }
    
    void Update()
    {
        // 키 입력 감지
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

    void GenerateTree()
    {
        _container.Clear(); // 기존 UI 삭제

        foreach (Node node in statGraph.nodes)
        {
            if (node is StatNode statNode)
            {
                // 템플릿(Label + Button)을 새로 만듬
                VisualElement nodeUI = nodeTemplate.Instantiate();
                
                // 데이터 연결 (UXML 내부의 이름과 일치해야 함)
                nodeUI.Q<Label>("StatName").text = statNode.GetNodeName(); 
                // 만약 Label에 이름을 지어줬다면 .Q<Label>("MyLabelName") 로 작성
                
                // 버튼 찾아서 로그 찍기 (Hierarchy에 있는 이름 기준)
                Button btn = nodeUI.Q<Button>("UnlockBtn");
                if (btn != null)
                {
                    btn.clicked += () => {
                        // 임시로 클릭시 로그로 노드 이름과 값 출력
                        Debug.Log($"스킬 클릭: {statNode.GetNodeName()} / 수치: {statNode.GetValue()}");
                    };
                }

                // xnode Editor position을 Scene에 설정 
                nodeUI.style.position = Position.Absolute;
                nodeUI.style.left = node.position.x;
                nodeUI.style.top = node.position.y;

                _container.Add(nodeUI);
            }
        }
    }
}

