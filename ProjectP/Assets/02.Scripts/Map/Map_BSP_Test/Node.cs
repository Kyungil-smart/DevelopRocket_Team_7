using UnityEngine;

public class Node//1
{
    public RectInt rect;    // 쪼개진 전체 구역
    public Node left;       // 자식 1
    public Node right;      // 자식 2
    public Node parent;     // 부모 (통로 연결 시 역추적용)
    public RectInt roomRect; // 구역 내에 실제로 그려진 방의 범위

    public Node(RectInt rect, Node parent = null)
    {
        this.rect = rect;
        this.parent = parent;
    }

    public bool IsLeaf() => left == null && right == null;
}