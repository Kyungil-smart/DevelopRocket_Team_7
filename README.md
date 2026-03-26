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
- Assets  
  - **importAssets 폴더**
    - 에셋 관리는 병희님께서 관리 해주심.
    - 본인이 사용하는 에셋은 갱신(추가/삭제)될 때마다 Export As Asset Package 사용하셔서 따로 백업 바람.
  - **Scenes 폴더**
    -  Test 폴더 <-- 테스트를 하기 위한 용도 본인명 폴더 만든 후 테스트 바람.
    -  Main폴더 <-- 우리가 만들 게임의 씬 저장 폴더
  - **Scripts 폴더**
    - Common  <-- 공통으로 사용할 스크립트 들
         - Interface 폴더
         - core 폴더
         - Manager 폴더
    - 그 외는 큰 기능 단위로 먼저 묶은 다음 그 안에서 세부 기능으로 나눠주세요.  
    예시)  
    Player/  
 ┣ Movement/  
 ┣ Combat/  
 ┣ Jump/  
  - **SO (ScriptableObject) 폴더**
    - Scripts <-- 데이터를 만들기 위한  『스크립터블 오브젝트』 스크립트 보관용
    - Datas <--위에 데이터를 에셋으로 데이화 시켜 저장하는 폴더
  -  **Animaction   폴더**
      - 2D 캐릭터 또는 몬스터 애니메이션 및 기타등을 해당 폴더에서 관리한다. 
      - 관리하는 내용은 :Animator Controller,Animation Clip
      - 해당 폴더에서 데이터 관리시 연관성 있는 데이터 끼리 폴더 만들어서 그 폴더 내부에서 관리 바람  
     예시)  
     - PlayerAnimaction폴더  
       - 플레이어 애니메이션 클립 폴더
       - 애니메이션 컨트롤러
    
     - enemyAnimaction폴더
      - 몬스터 애니메이션 클립 폴더
      - 몬스터 애니메이션 컨트롤러
       
        


  - **Prefabs  폴더**  
 어떤 것들이 프리팹 될지 모르니깐   
알아볼수 있게 폴더 생성해서 잘 분리 하기.

 
  
