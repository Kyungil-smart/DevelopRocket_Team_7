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
			}
		}
	}

	// 메인노드가 선택된 후에 선택되지 않은 노드들 잠금 처리
	// 맨 처음에 각 메인 Lv1 노드와 그 좌우 노드만 inactive 처리를 함
	// 선택되지 않은 노드들만 다시 locked로 바꿈
	public void LockMainNodes()
	{
		LockSideNodes();
		
		// 좌우 잠근 후에 자기 자신 잠금
		_canActive = false;
		_state = StatNodeState.Locked;
	}

	// 특수 노드의 좌 우 노드들 잠금처리
	public void LockSideNodes()
	{
		var leftPort = GetPort("Input");
		if (leftPort.IsConnected)
		{
			var leftNode = leftPort.Connection.node as StatNode;
			if (leftNode != null && leftNode.GetNodeState() == nameof(StatNodeState.Inactive))
			{
				leftNode.SetStatNodeState(StatNodeState.Locked);
			}
		}
		
		NodePort rightPort = GetPort("Output");
		if (rightPort.IsConnected)
		{
			var rightNode = rightPort.Connection.node as StatNode;
			if (rightNode != null && rightNode.GetNodeState() == nameof(StatNodeState.Inactive))
			{
				rightNode.SetStatNodeState(StatNodeState.Locked);
			}
		}
	}
}