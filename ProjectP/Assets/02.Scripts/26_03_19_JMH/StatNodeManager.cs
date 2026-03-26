using XNode;
using UnityEngine;

public class StatNodeManager : MonoBehaviour
{
    [Header("스탯 노드그래프 연결")]
    [SerializeField] private StatNodeGraph _statNodeGraph;

    [Header("보유 노드 포인트")] 
    [SerializeField] private int _openNodePoint;

}
