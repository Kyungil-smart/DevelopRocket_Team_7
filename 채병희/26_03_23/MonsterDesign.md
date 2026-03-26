# Monster Design

> Monster (Enemy) Object 에 사용될 여러 Component 및 Script/Class 구성에 대한 기록 문서.

## Component

- Data
  - ScriptableObject 로 부터 Data를 Instantidate 혹은 Deepcopy 를 한 데이터.
- State Machine
  - Monster 의 State 를 관리하는 Machine- 
- Actions
  - Pures; 직접적인 객체 제어가 없는 Action 위주
    - 데미지 계산, 몬스터 버프/디버프 같은 것들 등
  - Monobehaviors; 직접적인 객체 제어가 있는 Action 위주
    - Movement
      - Monster 의 이동을 담당
    - 그 밖에 애니메이션, 물리제어 등 
  

## Class Design

> 아래는 확정은 아니며, 추후 기획에 따라 변동이 있을 예정

![alt text](../pics/monsterClassStructure.png)

- Monster 에 StateMachine Component 를 중심으로 이하 위 그림과 같은 구조로 설계 중.
- Move 나 Attack 관련 Monobehavior 는 Component 로 들어가 "Action" 에서 이를 불러와 실행하는 방향으로 진행.
  - 중간 Agent 가 있어 해당 Agent 가 Action 관리
  - Agent 에서 Observer Pattern 을 이용하여 Backboard 내 flag 와 연결 -> 느슨한 연결

