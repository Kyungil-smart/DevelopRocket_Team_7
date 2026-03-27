using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SpecialStatNode : StatNode {
	
	// 특수 노드랑 연결이 되어야 할 메인 스텟 노드
	[Output] public bool Output2;
	
	// 특수 노드의 고유 ID
	[SerializeField] private int _specialNodeId;

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	public new void InitData()
	{
		if (_nodeData == null) return;

		_canActive = false;
		_isActiveLeft = false;
		_isActiveRight = false;
		
		// SO 리스트에서 내 ID와 일치하는 데이터 찾기
		_info = _nodeData.NodeInfos.Find(x => x.Id == _nodeId);
		// 메인 노드가 Active 가능하게 하기 위해 _canActive = true
		if (_mainNode) _canActive = true;
		
		Debug.Log($"노드 ID : {_nodeId} / _CanActive : {_canActive}");
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}