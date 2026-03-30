using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SpecialStatNode : StatNode
{
	// 메인 테크 트리간 연결을 위한 노드 (위쪽)
	[Input] public bool SpecialToSpecialUp;
	
	// 메인 테크 트리간 연결을 위한 노드 (아래쪽)
	[Output] public bool SpecialToSpecialDown;
	
	// 특수 노드의 고유 ID
	[SerializeField] private int _specialNodeId;
	// getter
	public int SpecialNodeId => _specialNodeId;
	
	// 특수 노드 정보를 불러올 SO
	[SerializeField] private NodeDataSO _specialNodeData;
	
	// 실제로 사용할 특수 노드 정보 데이터
	private SpecialNodeInfo _specialInfo;

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	public override void InitData()
	{
		// 스탯 노드 초기화
		base.InitData();

		if (_specialNodeData == null) return;
		
		// 특수 노드 데이터 불러오기
		_specialInfo = _specialNodeData.SpecialNodeInfos.Find(x => x.Id == _specialNodeId);
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

	// 특수 노드 끼리는 위 아래로 연결 되는데
	// 자신 기준 위 아래 특수 노드 중 아래 노드의 상태를 변경하는 경우는 없음
	// 따라서 위 노드의 상태만 변경
	public void UnlockUpSpecialNode()
	{
		var upPort = GetPort("SpecialToSpecialUp");
		if (upPort.IsConnected)
		{
			var upNode = upPort.Connection.node as SpecialStatNode;
			if (upNode != null && upNode.GetNodeState() == nameof(StatNodeState.Locked) && _canActive)
			{
				upNode.SetStatNodeState(StatNodeState.Inactive);
				Debug.Log($"위 노드 {upNode.GetID()} 의 상태 : {upNode.GetNodeState()}");
			}
		}
	}
}