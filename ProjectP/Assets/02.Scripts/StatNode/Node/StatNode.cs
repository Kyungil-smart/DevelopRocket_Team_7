using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using XNode;

public enum StatNodeState
{ 
	Active,	// 실제 활성화가 진행된 상태
	Inactive,	// 특정 조건들을 만족하여 활성화 준비중인 상태
	Locked	// 조건을 만족하지 못하여 활성 불가능 상태
}

public enum StatNodeType
{
	Damage = 100,
	AttackSpeed,
	CriticalRate,
	CriticalDamage,
	MovementSpeed,
	MaxHP,
	ProjectileCount,
}

public class StatNode : Node {
	
	// xNode에서는 input, output을 구분하여 다양한 작업을 하는 것을 의도하고 있음
	// 하지만 여기에서는 Input = 왼쪽 노드, Output = 오른쪽 노드 로 정의하여 단순 연결만 진행
	[Input] public bool Input;
	[Output] public bool Output;
	
	// 불러올 노드 데이터의 ID
	[SerializeField] protected int _nodeId;
	
	// 노드 정보를 불러올 SO
	[SerializeField] protected NodeDataSO _nodeData;
	
	// 현재 노드의 상태
	[SerializeField] protected StatNodeState _state;
	
	// setter
	public void SetStatNodeState(StatNodeState changeState) { _state =  changeState; }
	
	// 현재 노드가 시작점 노드인지를 판별
	[SerializeField] protected bool _mainNode;
	
	// 현재 노드가 실제로 사용할 데이터
	protected NodeInfo _info;
	
	// 플레이어에게 넘겨줄 증감수치
	protected PlayerStat _postStat;
	
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

		_canActive = false;
		_isActiveLeft = false;
		_isActiveRight = false;

		_postStat = new PlayerStat();
		_postStat.moveSpeed = 0f;
		_postStat.playerHp = 0;
		
		// SO 리스트에서 내 ID와 일치하는 데이터 찾기
		_info = _nodeData.NodeInfos.Find(x => x.Id == _nodeId);

		SetNodeType();
		
		// 메인 노드가 Active 가능하게 하기 위해 _canActive = true
		if (_mainNode) _canActive = true;
		
		Debug.Log($"노드 ID : {_nodeId} / _CanActive : {_canActive} / 노드 타입 : {_nodeType}");
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
		
		// 노드 상태 변경
		SetNodeIsCanActive();
		
		// 현재 보유한 노드 포인트가 해당 노드가 Active되기 위한 요구 노드 포인트 이상 있으면 활성화 가능
		// 기능 구현 예정
		if (_info != null && _canActive)
		{
			switch (_nodeType)
			{
				case StatNodeType.MovementSpeed :
					_postStat.moveSpeed += _info.NodeIncrValue;
					break;
				
				case StatNodeType.MaxHP :
					_postStat.playerHp = (int)_info.NodeIncrValue;
					break;
			}
			PostManager.Instance.Post<PlayerStat>(PostMessageKey.PlayerStat, _postStat);
		}
		
		Debug.Log($"스킬 노드 type: {_nodeType} / 노드 ID : {_nodeId} ");
	}

	public int GetID() => _info.Id;
	//public string GetNodeName() => _info?.NameVariable;
	public string GetNodeStatType() => _info?.NodeStatType;
	//public int GetNodeLevel() => _info.NodeLevel;

	// 노드의 3가지 상태인 Active,InActive,Locked에 따라 상태 여부를 문자열로 반환
	// 추후 노드 전용 UI에서 해당 상태에 따라 노드 버튼 위에 자물쇠 아이콘,노드 버튼 비활성화 시 어떻게 보일지 등을 처리할 예정
	public string GetNodeState() => NodeStateDescription();
	
	// 노드 활성화 조건에 맞지 않으면 _canActive가 false, 맞으면 true
	// 메인 노드는 일단 바로 활성화 가능하게 설정
	private void SetNodeIsCanActive()
	{
		// 메인 노드이고 inactive 상태이면 
		if (_mainNode && _state == StatNodeState.Inactive)
		{
			_state = StatNodeState.Active;
			Debug.Log($"메인 노드 ID : {_nodeId}  / 노드 상태 : {_state}");
		}
		
		// 양 옆 노드 상태 검사
		HasActiveNeighbor();
		
		// 서브노드이고 inactive 상태이고, 왼쪽, 오른쪽 노드 둘 중 하나라도 active 상태이면
		if (_mainNode == false && _state == StatNodeState.Inactive && ( _isActiveLeft || _isActiveRight ))
		{
			_state = StatNodeState.Active;
			Debug.Log($"서브 노드 ID : {_nodeId}  / 노드 상태 : {_state}");
		}
		
		/*
		// 양 옆 노드 상태 검사
		HasActiveNeighbor();
		*/
		
		// 양 옆 노드 중 하나가 Active 상태이면 해당 노드 활성화 가능
		if (_isActiveLeft || _isActiveRight)
		{
			_canActive = true;
		}
		
		
		// 그 후 양 옆 노드 상태 변경
		ChangeSideNodeState();
		
		
		Debug.Log($"노드 ID : {_nodeId}  / 노드 상태 : {_state}");
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
		
		Debug.Log($"노드 ID : {_nodeId}  / 왼쪽 노드, 오른쪽 노드 활성화 여부 : {_isActiveLeft} , {_isActiveRight}");
	}
	
	// 양 옆 노드 검사 후 해당 노드의 상태가 Locked이면 Inactive로 변경하는 메서드
	private void ChangeSideNodeState()
	{
		if (_state == StatNodeState.Inactive || _state == StatNodeState.Locked ) return;
		
		var leftPort = GetPort("Input");
		if (leftPort.IsConnected)
		{
			var leftNode = leftPort.Connection.node as StatNode;
			if (leftNode != null && leftNode.GetNodeState() == nameof(StatNodeState.Locked) && _canActive)
			{
				leftNode.SetStatNodeState(StatNodeState.Inactive);
				Debug.Log($"왼쪽 노드 {leftNode.GetID()} 의 상태 : {leftNode.GetNodeState()}");
			}
		}
		
		NodePort rightPort = GetPort("Output");
		if (rightPort.IsConnected)
		{
			var rightNode = rightPort.Connection.node as StatNode;
			if (rightNode != null && rightNode.GetNodeState() == nameof(StatNodeState.Locked) && _canActive)
			{
				rightNode.SetStatNodeState(StatNodeState.Inactive);
				Debug.Log($"오른쪽 노드 {rightNode.GetID()} 의 상태 : {rightNode.GetNodeState()}");
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
	
}
