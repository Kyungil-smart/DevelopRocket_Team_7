using System;
using UnityEngine;

[Serializable]
public class SpecialNodeInfo : ISheetParsable, IIdentifiable
{
    // 구글 시트에서 가져온 데이터를 저장할 변수
    private string _nodeStatType;
    private string _nameVariable;
    private int _id;
    private int _nodeLevel;
    private int _currentNodePoints;
    private int _requiredNodeID;
    private string _description;
    
    // getter
    public string NodeStatType => _nodeStatType;
    public string NameVariable => _nameVariable;
    public int Id => _id;
    public int NodeLevel => _nodeLevel;
    public int CurrentNodePoints => _currentNodePoints;
    public int RequiredNodeID => _requiredNodeID;
    public string Description => _description;
    
    [field: SerializeField] public string Name { get; set; }
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
        int.TryParse(data[4], out _currentNodePoints); // E열 노드 코스트 포인트
        int.TryParse(data[5], out _requiredNodeID); // F열 요구 노드 ID
        _description = data[6];            // G열 설명
    }
}
