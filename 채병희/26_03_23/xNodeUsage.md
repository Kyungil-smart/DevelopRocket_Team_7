# 사용 이유
- Monster 의 행동을 State 로 관리하기 위함.
- State 사용시 Graph 적으로 보이기도 좋고 개발 편의성도 높아짐.

# 간단 사용 방법

## Script 생성

### xNode Script

- 각 State 에 대한 정의.
- 아래 내용에 대하여 필수 구현 필요
  - Input / Output Port
  - Behavior ; 해당 State 에서 객체가 해야 할 행위에 대한 정의
  - Transaction ; 특정 Transaction 에 따른 State Change 가 발생. Node 간 간선에 해당.

### xNode Graph Script

- Node 의 Graph 를 관리하는 스크립트.
- 해당 Script 를 만들면 Unity 생성 메뉴에 같은 이름으로 Graph 가 생성됨.

### State Machine

- State Machine 은 State Node 를 특정 Trigger 발생시 지정된 Event 로 변경해주는 장치.
- 즉, 어떤 Node 의 행동이 끝나고 난 이후 State 변경이 발생되면 해당 Node 로 옮겨주면 된다.
  
  

