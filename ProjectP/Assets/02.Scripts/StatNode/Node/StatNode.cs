using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XNode;

enum StatNodeState
{
	Active,
	Inactive,
	Locked
}

public class StatNode : Node {
	
	[Input] public bool Input;
	[Output] public bool Output;
	
	// 노드의 고유 ID
	[SerializeField] private int _nodeId;
	
	// 노드 정보를 불러올 SO
	[SerializeField] private NodeDataSO _nodeData;
	
	// 현재 노드의 상태
	[SerializeField] private StatNodeState _state;
	
	// 현재 노드가 시작점 노드인지를 검산하는 상태
	[SerializeField] private bool _mainNode;
	
	// 현재 노드가 실제로 사용할 데이터
	private NodeInfo _info;
	private bool _canActiveLeft;
	private bool _canActiveRight;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	// 외부에서 Init을 해야 할 상황이 생겨 public으로 설정
	public void InitData()
	{
		if (_nodeData == null) return;
		
		// SO 리스트에서 내 ID와 일치하는 데이터 찾기
		_info = _nodeData.NodeInfos.Find(x => x.Id == _nodeId);
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

	public void OnClick()
	{
		// CanActiveNode 판정 결과 false면 노드 활성화 안됨
		if (!CanActiveNode())
		{
			Debug.Log("노드 활성화 조건 불만족");
			return;
		}
		
		if(_state == StatNodeState.Inactive) _state = StatNodeState.Active;
		Debug.Log($"스킬 노드 type: {GetNodeStatType()} / 노드 ID : {_nodeId}");
	}

	//public int GetID() => _info.Id;
	//public string GetNodeName() => _info?.NameVariable;
	public string GetNodeStatType() => _info?.NodeStatType;
	//public int GetNodeLevel() => _info.NodeLevel;

	// 노드의 3가지 상태인 Active,InActive,Locked에 따라 상태 여부를 문자열로 반환
	// 추후 노드 전용 UI에서 해당 상태에 따라 노드 버튼 위에 자물쇠 아이콘,노드 버튼 비활성화 시 어떻게 보일지 등을 처리할 예정
	public string GetNodeState() => NodeStateDescription();
	
	// 노드 활성화 조건에 맞지 않으면 false, 맞으면 true
	public bool CanActiveNode()
	{
		// 메인 노드 이면 활성화 가능
		if(_mainNode) return true;
		
		// 양 옆 노드 중 하나가 Active 상태이면 해당 노드 활성화 가능
		if (HasActiveNeighbor()) return true;
		
		// 현재 보유한 노드 포인트가 해당 노드가 Active되기 위한 요구 노드 포인트 이상 있으면 활성화 가능
		// 기능 구현 예정
		
		return false;
	}

	// 자기가 시작 노드이거나, 양 옆에 노드중 하나가 Active 상태인 노드가 있을 경우 true, 아니면 false 반환
	private bool HasActiveNeighbor()
	{
		if (_mainNode) return true;
		
		NodePort leftPort = GetPort("Input");
		if (leftPort.IsConnected)
		{
			var leftNode = leftPort.Connection.node as StatNode;
			if (leftNode != null && leftNode.CanActiveNode()) return true;
		}
		
		NodePort rightPort = GetPort("Output");
		if (rightPort.IsConnected)
		{
			var rightNode = rightPort.Connection.node as StatNode;
			if (rightNode != null && rightNode.CanActiveNode()) return true;
		}
		
		return false;
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
	
}
