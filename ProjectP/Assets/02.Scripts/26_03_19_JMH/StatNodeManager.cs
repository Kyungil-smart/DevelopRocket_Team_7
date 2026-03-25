using XNode;
using UnityEngine;

public class StatNodeManager : MonoBehaviour
{
    [SerializeField] private StatNodeGraph _statNodeGraph;
    
    /*
    void Start() {
        // 그래프 안에 있는 모든 노드를 순회하며 초기화하거나 UI를 생성합니다.
        foreach (Node node in _statNodeGraph.nodes) {
            StatNode statNode = node as StatNode;
            if (statNode != null) {
                Debug.Log($"찾은 스킬: {statNode.GetNodeName()}, 수치: {statNode.GetValue()}");
                
                // 여기서 실제 게임 캐릭터의 스탯에 적용하거나 
                // UI 버튼을 생성하는 로직을 넣습니다.
            }
        }
    }
    */
}
