using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using XNode;
using UnityEngine;

public class StatNodeManager : MonoBehaviour
{
    [Header("스탯 노드그래프 연결")]
    [SerializeField] private StatNodeGraph _statNodeGraph;

    [Header("보유 노드 포인트")] 
    [SerializeField] private int _openNodePoint;
    
    private Dictionary<int, SpecialStatNode> _specialNodeDict = new Dictionary<int, SpecialStatNode>();

    private void Awake()
    {
        foreach (var node in _statNodeGraph.nodes)
        {
            if (node is StatNode statNodeTmp)
            {
                // 각 테크트리의 첫번째, 두번째, 세번째 노드의 상태는 inactive로 변경
                // 나머지 노드들은 Locked로 변경
                var setInactiveNode = node.name;
                if(setInactiveNode.EndsWith("_LV1_1") ||
                   setInactiveNode.EndsWith("_LV1_2") ||
                   setInactiveNode.EndsWith("_LV1_4"))
                {
                    statNodeTmp.SetStatNodeState(StatNodeState.Inactive);
                }
                else
                {
                    statNodeTmp.SetStatNodeState(StatNodeState.Locked);
                }
            }
        }
    }
    
    private void OnEnable()
    {
        _specialNodeDict.Clear();
        
        foreach (var node in _statNodeGraph.nodes)
        {
            if (node == null) continue;
            
            if (node is SpecialStatNode specialTmp)
            {
                _specialNodeDict.Add(specialTmp.SpecialNodeId, specialTmp);
            }
        }
    }

    private void NodeLevelUpgrade()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
