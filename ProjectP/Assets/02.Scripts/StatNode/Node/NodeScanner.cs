using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NodeScanner : MonoBehaviour
{
    private List<StatNode> _AllNodes = new();
    private List<StatNode> LeftNodes = new();
    private List<StatNode> RightNodes = new();

    public List<StatNode> ScanSides(StatNode center)
    {
        if(center == null) return null;
        
        LeftNodes.Clear();
        RightNodes.Clear();
        _AllNodes.Clear();
        
        ScanNodes(center, "Input", LeftNodes);
        ScanNodes(center, "Output", RightNodes);
        
        // 3-2-1-4-5 순서로 노드가 구성되어 있음
        // 왼쪽 먼저 저장 후 오른쪽 저장
        foreach (var node in LeftNodes)
            _AllNodes.Add(node);
        
        foreach (var node in RightNodes)
            _AllNodes.Add(node);
        
        return _AllNodes;
    }

    private void ScanNodes(StatNode currentNode, string portName, List<StatNode> targetList)
    {
        // 현재 노드의 지정된 포트 가져오기
        NodePort port = currentNode.GetPort(portName);
        
        if (port == null || !port.IsConnected) return;

        // 해당 포트에 연결된 모든 노드 확인
        List<NodePort> connections = port.GetConnections();
        foreach (var connection in connections)
        {
            StatNode nextNode = connection.node as StatNode;
            // 존재하거나 이미 저장되지 않았을 경우에만 저장
            if (nextNode != null && !targetList.Contains(nextNode))
            {
                
                if (nextNode is SpecialStatNode)
                {
                    // 다음 노드가 특수 노드이면 탐색 X
                    continue; 
                }
                
                targetList.Add(nextNode);
                // 다음 일반 노드에 대해서도 같은 방향으로 재귀적 진행
                ScanNodes(nextNode, portName, targetList);
            }
        }
    }
}
