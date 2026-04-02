using System;
using NewWeaponSystem;
using UnityEngine;
using XNode;

public class StatNode : Node {
	
	// xNode에서는 input, output을 구분하여 다양한 작업을 하는 것을 의도하고 있음
	// 하지만 여기에서는 Input = 왼쪽 노드, Output = 오른쪽 노드 로 정의하여 단순 연결만 진행
	[Input] public bool Input;
	[Output] public bool Output;
	
	// 불러올 노드 데이터의 ID
	[SerializeField] protected int _nodeId;
	// getter
	public int GetID() => _nodeId;
	
	// 노드 정보를 불러올 SO
	[SerializeField] protected NodeDataSO _nodeData;
	
	// 현재 노드의 상태
	protected StatNodeState _state;
	
	// setter
	public void SetStatNodeState(StatNodeState changeState) { _state =  changeState; }
	
	// 현재 노드가 중심 노드인지를 판별
	[SerializeField] protected bool _mainNode;
	
	// 현재 노드가 시작 노드인지를 판별
	[SerializeField] protected bool _isFirst;
	
	// 현재 노드가 실제로 사용할 데이터
	protected NodeInfo _info = new();
	
	// 노드 타입
	protected StatNodeType _nodeType;
	
	// 연결된 왼쪽, 오른쪽 노드가 활성화 가능한지 여부를 저장함 / 가능하면 true, 불가능하면 false
	protected bool _isActiveLeft;
	protected bool _isActiveRight;

	// 자신 스스로가 활성화 가능한지 여부를 저장 / 가능하면 true, 불가능하면 false
	protected bool _canActive;
	// getter
	protected bool CanActive => _canActive;
	
	// 실제 활성화 되어 있는지 여부
	public bool IsActive() { return _state == StatNodeState.Active; }
	public bool IsLocked() { return _state == StatNodeState.Locked; }


	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	// 외부에서 Init을 해야 할 상황이 생겨 public으로 설정
	public virtual void InitData()
	{
		if (_nodeData == null) return;
		
		_state = StatNodeState.Locked;

		_canActive = false;
		_isActiveLeft = false;
		_isActiveRight = false;
		
		// SO 리스트에서 내 ID와 일치하는 데이터 찾기
		_info = _nodeData.NodeInfos.Find(match => match.Id == _nodeId);

		SetNodeType();
		
		// 메인 노드가 Active 가능하게 하기 위해 _canActive = true
		if (_mainNode) _canActive = true;
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

	// 노드UI에서 버튼을 클릭하면 발생할 메서드
	// Inactive -> Active 로 전환 후 증가할 능력치를 플레이어에게 전달
	public void OnClick()
	{
		// 이미 활성화 상태이면 return
		if (IsActive())
		{
			Debug.Log($"노드 ID : {_nodeId} / 이미 활성화 완료 되었습니다!");
			return;
		}

		if (StatNodeManager.Instance.NodePoint < _info.NodeCostPoint)
		{
			Debug.Log($"노드 코스트 부족!");
			return;
		}
		
		// 노드 상태 변경
		SetNodeIsCanActive();
		
		if (_info != null && _canActive)
		{
			WeaponUpgradeMsg data = new();
			PlayerStatMSG playerData = new();
			
			switch (_nodeType)
			{
				case StatNodeType.MovementSpeed :
					playerData.moveSpeed = _info.NodeIncrValue;
					PostManager.Instance.Post(PostMessageKey.PlayerStat,playerData);
					break;
				
				case StatNodeType.MaxHP :
					playerData.PlayerHp = (int)_info.NodeIncrValue;
					PostManager.Instance.Post(PostMessageKey.PlayerStat,playerData);
					break;
				
				case StatNodeType.Damage:
					data.damage = _info.NodeIncrValue;
					PostManager.Instance.Post(PostMessageKey.UpgradeWeapon, data);
					break;
				
				case StatNodeType.AttackSpeed:
					data.attackSpeed = _info.NodeIncrValue;
					PostManager.Instance.Post(PostMessageKey.UpgradeWeapon, data);
					break;
				
				case StatNodeType.CriticalRate:
					data.critRate = _info.NodeIncrValue;
					PostManager.Instance.Post(PostMessageKey.UpgradeWeapon, data);
					break;
				
				case StatNodeType.CriticalDamage:
					data.critMultiplier = _info.NodeIncrValue;
					PostManager.Instance.Post(PostMessageKey.UpgradeWeapon, data);
					break;
			}
			// 요구 코스트 만큼 노드 포인트 감소
			StatNodeManager.Instance.SetNodePoint(-_info.NodeCostPoint);
			// 레벨업 시도
			PostManager.Instance.Post(PostMessageKey.NodeLevelUp, 1);
		}
	}

	// 노드의 3가지 상태인 Active,InActive,Locked에 따라 상태 여부를 문자열로 반환
	public string GetNodeState() => NodeStateDescription();
	
	// 노드 활성화 조건에 맞지 않으면 _canActive가 false, 맞으면 true
	private void SetNodeIsCanActive()
	{
		// 메인 노드이고 inactive 상태이면 
		if (_mainNode && _state == StatNodeState.Inactive)
		{
			_state = StatNodeState.Active;
			var specialNodeNameTmp = ExtractSpecialNodeName();
			
			// 메인 노드가 첫 시작 노드이면
			if(_isFirst) StatNodeManager.Instance.SelectFirstMainNode(specialNodeNameTmp);
		}
		
		// 양 옆 노드 상태 검사
		HasActiveNeighbor();
		
		// 서브노드이고 inactive 상태이고, 왼쪽, 오른쪽 노드 둘 중 하나라도 active 상태이면
		if (_mainNode == false && _state == StatNodeState.Inactive && ( _isActiveLeft || _isActiveRight ))
		{
			_state = StatNodeState.Active;
		}
		
		// 양 옆 노드 중 하나가 Active 상태이면 해당 노드 활성화 가능
		if (_isActiveLeft || _isActiveRight)
		{
			_canActive = true;
		}
		
		// 그 후 양 옆 노드 상태 변경
		ChangeSideNodeState();
	}

	// 자기가 메인 노드이거나 Locked 상태이면 메서드 실행 X
	// 왼쪽 노드가 이미 활성화 상태이면 _isActiveLeft = true, 아니면 false
	// 오른쪽 노드도 동일하게 진행
	private void HasActiveNeighbor()
	{
		if (_mainNode || _state == StatNodeState.Locked) return;
		
		var leftPort = GetPort("Input");
		if (leftPort.IsConnected)
		{
			var leftNode = leftPort.Connection.node as StatNode;
			if (leftNode != null && leftNode.IsActive())
			{
				_isActiveLeft = true;
			}
		}
		
		NodePort rightPort = GetPort("Output");
		if (rightPort.IsConnected)
		{
			var rightNode = rightPort.Connection.node as StatNode;
			if (rightNode != null && rightNode.IsActive())
			{
				_isActiveRight = true;
			}
		}
	}
	
	// 양 옆 노드 검사 후 해당 노드의 상태가 Locked이면 Inactive로 변경하는 메서드
	private void ChangeSideNodeState()
	{
		if (_state != StatNodeState.Active ) return;
		
		var leftPort = GetPort("Input");
		if (leftPort.IsConnected)
		{
			var leftNode = leftPort.Connection.node as StatNode;
			if (leftNode != null && leftNode.GetNodeState() == nameof(StatNodeState.Locked) && _canActive)
			{
				leftNode.SetStatNodeState(StatNodeState.Inactive);
			}
		}
		
		NodePort rightPort = GetPort("Output");
		if (rightPort.IsConnected)
		{
			var rightNode = rightPort.Connection.node as StatNode;
			if (rightNode != null && rightNode.GetNodeState() == nameof(StatNodeState.Locked) && _canActive)
			{
				rightNode.SetStatNodeState(StatNodeState.Inactive);
			}
		}
	}
	
	private string NodeStateDescription()
	{
		switch (_state)
		{
			case StatNodeState.Active:
				return "Active";
			
			case StatNodeState.Inactive:
				return "Inactive";
			
			case StatNodeState.Locked:
				return "Locked";
		}
		return "";
	}

	private void SetNodeType()
	{
		if(_info == null) return;
		
		switch (_info.NodeStatType)
		{
			case("Damage") : _nodeType = StatNodeType.Damage;
				break;
			
			case("Attack_Speed") : _nodeType = StatNodeType.AttackSpeed;
				break;
			
			case("Critical_Rate") : _nodeType = StatNodeType.CriticalRate;
				break;
			
			case("Critical_Damage") : _nodeType = StatNodeType.CriticalDamage;
				break;
			
			case("Movement_Speed") : _nodeType = StatNodeType.MovementSpeed;
				break;
			
			case("Max_HP") : _nodeType = StatNodeType.MaxHP;
				break;
			
			case("Projectile_Count") : _nodeType = StatNodeType.ProjectileCount;
				break;
		}
	}

	// 선택한 메인노드의 이름 중 _LV 뒷 부분부터 잘라 다시 전달하기 위한 메서드
	private string ExtractSpecialNodeName()
	{
		string nodeName = name;
		int targetIndex = nodeName.LastIndexOf("_LV", StringComparison.Ordinal);

		// _LV 뒷 내용이 있을 경우
		if (targetIndex != -1)
		{
			return nodeName.Substring(0,targetIndex);
		}
		
		return "Unknown";
	}
}
