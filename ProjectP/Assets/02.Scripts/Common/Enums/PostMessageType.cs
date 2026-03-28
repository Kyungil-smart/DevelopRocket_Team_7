public enum PostMessageKey
{
    // 필요시 아래 Key 를 추가 후 사용 바랍니다. 
    // 아래 정의된 Key 이외의 예외처리는 안 받습니다.
    PlayerPosition,  // Player 의 위치정보를 받기 위한 Position 데이터
    EnemyDespawned,  // Enemy 사망 후 Despawn 
    BatterySpawned,  // Enemy 사망 후 배터리 스폰
    BatteryDespawned,  // Enemy 사망 후 배터리 디스폰
}