public enum PostMessageKey
{
    // 필요시 아래 Key 를 추가 후 사용 바랍니다. 
    // 아래 정의된 Key 이외의 예외처리는 안 받습니다.
    PlayerPosition,  // Player 의 위치정보를 받기 위한 Position 데이터
    EnemySpawned,  // Enemy 사망 후 Despawn
    EnemyDespawned,  // Enemy 사망 후 Despawn 
    BatterySpawned,  // Enemy 사망 후 배터리 스폰
    BatteryDespawned,  // Enemy 사망 후 배터리 디스폰
    BossBulletDespawned,  // 보스의 Bullet 이 사라지기 위함.
    PostExp,  // 적 사망 후 경험치 추가
    InitPlayerPosition,  // 처음 플레이어 등장시 시작 위치 보내기
    ProjectileSpawned,  // 플레이어 탄환체 꺼내기
    ProjectileDespawned,  // 플레이어 탄환체 반납하기
    PlayerStat, // 노드에서 플레이어에게 스탯을 전달하는 이벤트
    NodeUIIconUpdate, // 노드 UI 갱신 이벤트
    NodeLevelUp, // 노드 Level 증가 이벤트
    EnemyRangeBulletSpawned,  // 몬스터 탄환체 꺼내기
    EnemyRangeBulletDespawned,  // 몬스터 탄환체 반납하기
    EnemyDeadAlarm,  // 몬스터가 죽어서 신호를 보냄.
    UpgradeWeapon,  // 무기 업그레이드 하기
    SelectWeapon,  // 무기 선택
    PlayerLevelUp,  // 플레이어 Level Up 정보 전달
    ProjectileSelection,  // 무기 선택 시 동시에 탄환 선택
    UITextReqeust,  // UI Text Data Request
    ChangeLanguage,  // 언어 설정 변경
    RequestChangeText,  // 언어 설정 변경 후 Text 일괄 변화 관련
    NodeReset,  // 노드 초기화
}