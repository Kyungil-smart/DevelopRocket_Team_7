public enum PostMessageKey
{
    // 필요시 아래 Key 를 추가 후 사용 바랍니다. 
    // 아래 정의된 Key 이외의 예외처리는 안 받습니다.
    PlayerPosition,  // Player 의 위치정보를 받기 위한 Position 데이터
    EnemyDespawned,  // Enemy 사망 후 Despawn 
    BatterySpawned,  // Enemy 사망 후 배터리 스폰
    BatteryDespawned,  // Enemy 사망 후 배터리 디스폰
    BossBulletDespawned,  // 보스의 Bullet 이 사라지기 위함.
    PostExp,  // 보스 사망 후 경험치 추가
    InitPlayerPosition,  // 처음 플레이어 등장시 시작 위치 보내기
    ProjectileSpawned,  // 플레이어 탄환체 꺼내기
    ProjectileDespawned,  // 플레이어 탄환체 반납하기
}