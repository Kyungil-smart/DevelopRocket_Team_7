using UnityEngine;

public class NodeInfo: ISheetParsable, IIdentifiable
{
    // 구글 시트에서 가져온 데이터를 저장할 변수
    private string _nodeStatType;
    private string _nameVariable;
    private int _id;
    private int _nodeLevel;
    private int _nodeCostPoint;
    private int _nodeIncrValue;
    
    // getter
    public string NodeStatType => _nodeStatType;
    public string NameVariable => _nameVariable;
    public int Id => _id;
    public int NodeLevel => _nodeLevel;
    public int NodeCostPoint => _nodeCostPoint;
    public int NodeIncrValue => _nodeIncrValue;
    
    [field: SerializeField] public string Name { get; set; }
    public void ApplyRowData(string[] data)
    {
        // 데이터가 없으면 무시
        if (data == null || data.Length == 0) return;

        // 1. 인스펙터 리스트에 표시될 '이름' 설정 (A열 값 사용)
        Name = data[0];

        // 2. 순서대로 꽂아넣기 (형변환 실패해도 0으로 들어가서 에러 안 남)
        _nodeStatType = data[0];           // A열
        _nameVariable = data[1];           // B열
        int.TryParse(data[2], out _id);    // C열 (ID)
        int.TryParse(data[3], out _nodeLevel); // D열
        int.TryParse(data[4], out _nodeCostPoint); // E열
        int.TryParse(data[5], out _nodeIncrValue); // F열
    }
}
