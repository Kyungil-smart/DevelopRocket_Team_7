using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeDataSO", menuName = "Scriptable Objects/NodeDataSO")]
public class NodeDataSO : ScriptableObject
{
    // 구글 스프레드 시트 파일 내 데이터를 담을 변수
    [SerializeField] public List<NodeInfo> NodeInfos = new List<NodeInfo>();
}
