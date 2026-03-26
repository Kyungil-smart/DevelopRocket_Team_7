using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class StatNode : Node {
	
	[Input] public bool Input;
	[Output] public bool Output;
	
	// 노드의 고유 ID
	[SerializeField] private int _nodeId;
	
	// 노드 정보를 불러올 SO
	[SerializeField] private NodeDataSO _nodeData;
	
	// 현재 노드가 실제로 사용할 데이터
	private NodeInfo _info;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

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

	public int GetID() => _info.Id;
	public string GetNodeName() => _info?.NameVariable;
	public string GetNodeStatType() => _info?.NodeStatType;
	public int GetNodeLevel() => _info.NodeLevel;
}