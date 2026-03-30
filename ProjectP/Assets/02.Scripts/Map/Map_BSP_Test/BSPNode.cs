using UnityEngine;

public enum RoomType { NULL, StartNode, MiddleNode, RestNode, BossNode }

public class BSPNode
{
    public RectInt rect;       // 쪼개진 전체 구역
    public BSPNode left;       // 자식 1
    public BSPNode right;      // 자식 2
    public BSPNode parent;     // 부모 (통로 연결 시 역추적용)
    public RectInt roomRect;   // 구역 내에 실제로 그려진 방의 범위

    public RoomType roomType = RoomType.NULL; //방의 속성 일단 NULL
    public BSPNode(RectInt rect, BSPNode parent = null)
    {
        this.rect = rect;
        this.parent = parent;
    }
    //방인지 확인 메소드
    public bool IsLeaf() => left == null && right == null;
}