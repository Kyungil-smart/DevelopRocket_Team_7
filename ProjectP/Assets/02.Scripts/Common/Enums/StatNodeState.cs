public enum StatNodeState
{ 
    Active,	// 실제 활성화가 진행된 상태
    Inactive,	// 특정 조건들을 만족하여 활성화 준비중인 상태
    Locked	// 조건을 만족하지 못하여 활성 불가능 상태
}
