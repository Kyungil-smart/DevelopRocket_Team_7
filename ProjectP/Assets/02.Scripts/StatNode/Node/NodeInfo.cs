using System;
using UnityEngine;

[Serializable]
public class NodeInfo: ISheetParsable, IIdentifiable
{
    // 구글 시트에서 가져온 데이터를 저장할 변수
    [SerializeField] private string _nodeStatType;
    [SerializeField] private string _nameVariable;
    [SerializeField] private int _id;
    [SerializeField] private int _nodeLevel;
    [SerializeField] private int _nodeCostPoint;
    [SerializeField] private float _nodeIncrValue;
    [SerializeField] private string _description;
    
    // getter
    public string NodeStatType => _nodeStatType;
    public string NameVariable => _nameVariable;
    public int Id => _id;
    public int NodeLevel => _nodeLevel;
    public int NodeCostPoint => _nodeCostPoint;
    public float NodeIncrValue => _nodeIncrValue;
    public string Description => _description;
    
    [SerializeField] private string _name;
    public string Name { get => _name; set => _name = value; }
    
    public void ApplyRowData(string[] data)
    {
        // 데이터가 없으면 무시
        if (data == null || data.Length == 0) return;

        // 인스펙터 리스트에 표시될 '이름' 설정 (A열 값 사용)
        Name = data[0];

        // 순서대로 꽂아넣기
        _nodeStatType = data[0];           // A열 노드 이름
        _nameVariable = data[1];           // B열 
        int.TryParse(data[2], out _id);    // C열 (ID)
        int.TryParse(data[3], out _nodeLevel); // D열 노드 레벨
        int.TryParse(data[4], out _nodeCostPoint); // E열 노드 코스트 포인트
        float.TryParse(data[5], out _nodeIncrValue); // F열 증감치
        _description = data[6];            // G열 설명
    }
}
