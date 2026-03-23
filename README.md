# 📌 Team Rule (팀 개발 규칙)



## ✅ 1. Git Commit Message 규칙 (필수)

커밋 메시지는 작업 내용을 한눈에 파악할 수 있도록 작성한다.

### 📍 타입 종류
- feat: 기능 추가
- fix: 버그 수정
- refactor: 구조 개선 (기능 변화 없음)
- style: 코드 스타일 수정 (세미콜론, 정렬 등)
- docs: 문서 수정
- test: 테스트 코드
- chore: 기타 작업 (빌드, 설정 등)

### 📍 작성 예시
- feat: 플레이어 이동 기능 추가
- fix: 적이 공격하지 않는 버그 수정
- refactor: UI 구조 분리

---

## 🌿 2. 브랜치 생성 규칙 (필수)

브랜치는 역할에 맞게 명확하게 분리한다.

### 📍 브랜치 구조
- main        → 배포용 (직접 작업 금지)
- develop     → 개발 통합 브랜치
- feature/*   → 기능 개발
- fix/*       → 버그 수정

### 📍 브랜치 예시
- feature/player-movement
- feature/inventory-system
- fix/enemy-ai-error

### 📍 규칙
- main 브랜치 직접 작업 ❌
- 모든 작업은 develop 기준으로 진행
- 작업 완료 후 Merge 진행
- 사용한 브랜치는 삭제 후 필요 시 재생성

---

## 🧠 3. 네이밍 규칙 (필수)

일관성과 가독성을 최우선으로 한다.

### 📍 네이밍 규칙 표

| 대상 | 규칙 | 예시 |
|------|------|------|
| 클래스 | PascalCase | `CoinSpawner`, `AssetManager` |
| 메서드 | PascalCase | `SpawnCoin()`, `CalculateDecay()` |
| 공개 프로퍼티 | PascalCase | `CurrentCash`, `FamePoint` |
| 비공개 필드 | _camelCase | `_spawnInterval`, `_comboCount` |
| 지역 변수 | camelCase | `decayValue`, `bounceCount` |
| 상수 | UPPER_SNAKE_CASE | `MAX_COMBO_COUNT`, `BASE_DECAY_RATE` |
| 인터페이스 | I + PascalCase | `IPoolable`, `IInvestable` |
| ScriptableObject | PascalCase + SO | `CoinDataSO`, `TierDataSO` |
| 이벤트 | On + PascalCase | `OnCoinCollected`, `OnTierChanged` |
### 접근 제한자
- 모든 필드/메서드에 접근 제한자를 **명시**한다. (생략 금지)
- Unity Inspector에 노출할 필드는 `private` + `[SerializeField]` 사용. `public` 필드 직접 노출 금지.

```
// 좋은 예
[SerializeField] private float _spawnInterval;

// 나쁜 예
public float spawnInterval;
```
---

## 🔀 4. Merge & Pull Request 규칙 (필수)

PR은 작업 내용을 명확하게 전달하는 문서다.

### 📍 제목 형식
- [기능] 캐릭터 선택 시스템 추가
- [버그] 적 공격 안하는 문제 수정

### 📍 내용 작성 예시
- Photon 동기화 구현
- 캐릭터 선택 UI 추가
- 100초 타이머 기능 구현

### 📍 규칙
- 무엇을 했는지 명확하게 작성
- 간결하게 작성
- 코드 확인 없이도 이해 가능하게 작성

---

## 📝 5. 기능 구현 문서 작성 (선택)

기능 개발 전 간단한 설계를 작성한다.

### 📍 목적
- 작업 방향 명확화
- 문제 발생 시 빠른 공유
- 협업 효율 증가

---

# 📂 기능 구현 문서 작성 가이드

## 📁 1. 폴더명 규칙
- 날짜 기반 폴더 생성 (yymmdd 형식)

```md
예시)
26_03_19
```

## 📄2. 파일명 규칙

기능이 명확히 드러나도록 작성
```
예시)
퀘스트 시스템.md
캐릭터 이동 로직.md
```
❌ 잘못된 예시
```
캐릭터 제작.md
몬스터 제작.md

👉 기능 단위로 구체적으로 작성할 것
```
### ✍️ 3. 문서 작성 가이드

기능 구현 전, 아래 형식으로 간단한 설계 문서를 작성한다.
```
📍 작성 구조

기능 이름 작성

구현해야 할 기능 목록 작성

각 기능의 검증(확인) 항목 작성
```
### 작성 템플릿
```
[기능 이름]

- 기능 설명 1
  - 확인 사항 1
  - 확인 사항 2

- 기능 설명 2
  - 확인 사항 1

- 기능 설명 3
  - 확인 사항 1
  ```



  ---

  #  ★★★★★ 유니티 프로젝트 폴더 구조 가이드 ★★★★★



##  1. 핵심 원칙

1. **기능(Feature) 기준으로 폴더를 나눈다**
2. 각 기능 내부에서 객체(Player, Monster 등)를 관리한다
3. 공통 로직은 반드시 따로 분리한다 (Core)
 
---

##  2. 추천 폴더 구조

```
Assets/
├── 01_Scenes/
├── 02_Scripts/
│   ├── Core/                # 공통 시스템
│   │   ├── Managers/
│   │   ├── Utilities/
│   │   ├── Extensions/
│   │
│   ├── Feature/
│   │   ├── Player/
│   │   │   ├── Controller/
│   │   │   ├── Movement/
│   │   │   ├── Combat/
│   │   │   ├── State/
│   │   │   ├── Animation/
│   │   │
│   │   ├── Monster/
│   │   │   ├── AI/
│   │   │   ├── Combat/
│   │   │   ├── State/
│   │   │
│   │   ├── Skill/
│   │   │   ├── PlayerSkill/
│   │   │   ├── MonsterSkill/
│   │   │   ├── System/      # 쿨타임, 데미지 계산 등
│   │   │
│   │   ├── UI/
│   │   │   ├── HUD/
│   │   │   ├── Popup/
│   │   │
│   │   ├── Map/
│   │   │   ├── Procedural/
│   │   │   ├── Tile/
│   │
│   ├── Data/
│   │   ├── ScriptableObjects/
│   │   ├── DTO/
│
├── 03_Prefabs/
├── 04_Art/
├── 05_Audio/
├── 06_Resources/
```
