using System.Collections.Generic;
using System;
using UnityEngine;

public class StatNodeManager : Singleton<StatNodeManager>
{
    [Header("스탯 노드그래프 연결")]
    [SerializeField] private StatNodeGraph _statNodeGraph;
    
    [Header("Node Loader")]
    [SerializeField] private List<NodeLoader> _nodeLoaderList;
    
    [Header("정령의 가호 노드 ID 리스트")]
    [SerializeField] private List<int> _spiritBlessingNodeId;

    [Header("심장 추적자 노드 ID 리스트")] 
    [SerializeField] private List<int> _heartSeekerNodeId;
    
    [Header("황금 사냥꾼 노드 ID 리스트")] 
    [SerializeField] private List<int> _goldHunterNodeId;
    
    // 보유 노드 포인트
    private int _nodePoint;
    // Getter
    public int NodePoint => _nodePoint;
    // Setter
    public void SetNodePoint(int nodePoint){ _nodePoint +=  nodePoint; }
    // 최대 노드 포인트
    private int _maxNodePoint;
    
    [SerializeField] UnityEngine.UI.Text _nodePointText;
    [SerializeField] UnityEngine.UI.Text _curnodePointText;
    
    private void Update()
    {
        _nodePointText.text = _maxNodePoint.ToString();
        _curnodePointText.text = _nodePoint.ToString();
        Debug.Log(_nodePoint);
    }
    
    // 현재 위치한 특수 노드
    private SpecialStatNode _currentSpecialNode;
    // 첫 노드 선택 시 고른 메인 테크 노드
    private List<SpecialStatNode> _currentSpecialNodeList = new();
    // 고른 메인 테크 노드에 접근하기 위한 인덱스
    private int _currentSpecialNodeIndex;
    // 고른 메인 노드의 해당 계층에 있는 스탯 노드들
    private List<StatNode> _currentStatNodeList = new();
    // 메인 테크 노드 이름
    private string _selectSpecialNodeName;
    // 노드 계층 레벨업 도달까지 남은 횟수를 세는 변수
    // 3으로 초기화 후 1씩 감소 시켜 0이면 레벨업할 예정
    private int _levelUpCount;
    
    // 레벨업 하기 전 해당 계층에 아직 active 되지 않는 노드들을 탐색
    private NodeScanner _nodeScanner;
    
    // 그래프에 있는 모든 특수 노드들을 저장
    private Dictionary<int, SpecialStatNode> _specialNodeDict = new ();

    private void Awake()
    {
        base.Awake();
        _nodePoint = 9;
        _maxNodePoint = _nodePoint;
        _nodeScanner = gameObject.AddComponent<NodeScanner>();
    }
    
    private async Awaitable Start()
    {
        // 각 노드 로더에서 데이터 불러오기
        foreach (var nodeLoader in _nodeLoaderList)
        {
            await nodeLoader.InitDataSO();
        }
        
        // 그래프 내 모든 특수 노드들 dict에 저장
        _specialNodeDict.Clear();
        foreach (var node in _statNodeGraph.nodes)
        {
            if (node == null) continue;
            
            if (node is SpecialStatNode specialTmp)
            {
                _specialNodeDict.Add(specialTmp.SpecialNodeId, specialTmp);
            }
        }
        
        ResetNodes();
    }
    
    private void OnEnable()
    {
        // 노드 ui에서 버튼을 누를때마다 해당 메서드를 불러 3번 부를 경우 노드 계층 레벨업하게 처리
        PostManager.Instance.Subscribe<int>(PostMessageKey.NodeLevelUp, NodesLevelUp);
    }

    private void InitNodes()
    {
        foreach (var node in _statNodeGraph.nodes)
        {
            if (node is StatNode statNodeTmp)
            {
                // 각 노드들 초기화
                statNodeTmp.InitData();
                // 각 테크트리의 Lv1의 첫번째, 두번째, 세번째 노드의 상태는 inactive로 변경
                // 나머지 노드들은 Locked로 변경
                var setInactiveNode = node.name;
                if(setInactiveNode.EndsWith("_LV1_1") ||
                   setInactiveNode.EndsWith("_LV1_2") ||
                   setInactiveNode.EndsWith("_LV1_4"))
                {
                    statNodeTmp.SetStatNodeState(StatNodeState.Inactive);
                }
            }
        }
        RequestUiUpdate();
    }

    public void ResetNodes()
    {
        InitManagerData();
        InitNodes();
    }

    private void InitManagerData()
    {
        StatNodeManager.Instance._nodePoint = StatNodeManager.Instance._maxNodePoint;
        
        _selectSpecialNodeName = string.Empty;
        _currentSpecialNodeList.Clear();
        _currentStatNodeList.Clear();
        _currentSpecialNode = null;
        _currentSpecialNodeIndex = 0;
        _levelUpCount = 3;
        
    }

    // 처음 특수 노드를 고를 시 실행될 메서드
    public void SelectFirstMainNode(string specialNodeName)
    {
        InitManagerData();
        
        _selectSpecialNodeName = specialNodeName;
        
        // 맨 첫번째(LV 1) 특수 노드 저장 후 미선택 메인노드들 잠금
        switch (_selectSpecialNodeName)
        {
            case "Spirit_blessing":
                // 현재 특수 노드를 정령의 가호의 첫번째 특수 노드로 저장
                _currentSpecialNode = _specialNodeDict[_spiritBlessingNodeId[_currentSpecialNodeIndex]];
                // 선택된 특수 노드들을 현재 특수 노드 리스트에 추가
                for (int i = 0; i < _spiritBlessingNodeId.Count; i++)
                {
                    _currentSpecialNodeList.Add(_specialNodeDict[_spiritBlessingNodeId[i]]);
                }
                // 미선택된 lv1 특수 노드 잠금처리 1
                var tmpHeartSeeker = _specialNodeDict[_heartSeekerNodeId[_currentSpecialNodeIndex]];
                tmpHeartSeeker.LockMainNodes();
                // 미선택된 lv1 특수 노드 잠금처리 2
                var tmpGoldHunter = _specialNodeDict[_goldHunterNodeId[_currentSpecialNodeIndex]];
                tmpGoldHunter.LockMainNodes();
                break;
            
            case "Heart_seeker":
                _currentSpecialNode = _specialNodeDict[_heartSeekerNodeId[_currentSpecialNodeIndex]];
                for (int i = 0; i < _heartSeekerNodeId.Count; i++)
                {
                    _currentSpecialNodeList.Add(_specialNodeDict[_heartSeekerNodeId[i]]);
                }
                var tmpSpiritBlessing = _specialNodeDict[_spiritBlessingNodeId[_currentSpecialNodeIndex]];
                tmpSpiritBlessing.LockMainNodes();
                var tmp2GoldHunter = _specialNodeDict[_goldHunterNodeId[_currentSpecialNodeIndex]];
                tmp2GoldHunter.LockMainNodes();
                break;
            
            case "Gold_hunter":
                _currentSpecialNode = _specialNodeDict[_goldHunterNodeId[_currentSpecialNodeIndex]];
                for (int i = 0; i < _goldHunterNodeId.Count; i++)
                {
                    _currentSpecialNodeList.Add(_specialNodeDict[_goldHunterNodeId[i]]);
                }
                var tmp2SpiritBlessing = _specialNodeDict[_spiritBlessingNodeId[_currentSpecialNodeIndex]];
                tmp2SpiritBlessing.LockMainNodes();
                var tmp2HeartSeeker = _specialNodeDict[_heartSeekerNodeId[_currentSpecialNodeIndex]];
                tmp2HeartSeeker.LockMainNodes();
                break;
        }
        
        // 선택된 특수 노드를 첫번째 현재 스탯 노드 리스트에 저장 
        // 그 후 선택된 특수 노드가 연결되어 있는 좌우 노드 탐색 후 현재 스탯 노드 리스트에 저장
        _currentStatNodeList.Add(_currentSpecialNode);
        var tmp = _nodeScanner.ScanSides(_currentSpecialNode);
        _currentStatNodeList.AddRange(tmp);
        
        // 상태 변경 후 노드 Ui 갱신
        RequestUiUpdate();
    }

    // 현재 노드 LV(계층)에서 포인트를 투자하였을 경우 처리될 메서드
    // levelUpCount는 1씩 감소하여 0이 되면 레벨업 하게끔 구성
    private void NodesLevelUp(int count)
    {
        // 인덱스 값이 튀었을 경우 일단 실행 안되게 함
        if(_currentSpecialNodeIndex < 0 ||  _currentSpecialNodeIndex >= _currentSpecialNodeList.Count)
            return;
        
        _levelUpCount -= count;
        
        if (_levelUpCount <= 0)
        {
            // 현재 특수 노드의 다음 레벨 상태 변경
            _currentSpecialNode.UnlockUpSpecialNode();
            // 현재 특수 노드와 연결되어 있는 일반 노드들의 상태 점검 후 active가 아니면 locked로 일괄 변경
            foreach (var node in _currentStatNodeList)
            {
                if (node.GetNodeState() != nameof(StatNodeState.Active))
                {
                    node.SetStatNodeState(StatNodeState.Locked);
                }
            }
            // 그 후 특수 노드 인덱스 1 증가 (다음 특수 노드 인덱스 가리키게끔)
            _currentSpecialNodeIndex++;
            // 레벨업을 하여 전 레벨 노드들 저장 정보 초기화
            _currentStatNodeList.Clear();
            // 다음 특수 노드로 갱신
            if (_currentSpecialNodeIndex < _currentSpecialNodeList.Count)
            {
                _currentSpecialNode = _currentSpecialNodeList[_currentSpecialNodeIndex];
                // 다음 특수 노드와 연결되어 있는 좌 우 노드들 추가
                var tmp = _nodeScanner.ScanSides(_currentSpecialNode);
                _currentStatNodeList.AddRange(tmp);
                // 그 후 levelUpCount 초기화
                _levelUpCount = 3;
            }
        }
        // UI 갱신
        RequestUiUpdate();
    }

    // 노드 UI를 갱신해야 할 경우 요청
    private void RequestUiUpdate()
    {
        Action tmp = null;
        PostManager.Instance.Post(PostMessageKey.NodeUIIconUpdate,tmp);
    }

    private void OnDisable()
    {
        if (PostManager.Instance != null)
        {
            PostManager.Instance.Unsubscribe<int>(PostMessageKey.NodeLevelUp, NodesLevelUp);
        }
    }
}
