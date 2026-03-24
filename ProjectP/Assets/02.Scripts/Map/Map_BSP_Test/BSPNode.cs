using UnityEngine;
namespace sadsmile
{
    public class BSPNode//1
    {
        public RectInt rect;    // 쪼개진 전체 구역
        public BSPNode left;       // 자식 1
        public BSPNode right;      // 자식 2
        public BSPNode parent;     // 부모 (통로 연결 시 역추적용)
        public RectInt roomRect; // 구역 내에 실제로 그려진 방의 범위

        public BSPNode(RectInt rect, BSPNode parent = null)
        {
            this.rect = rect;
            this.parent = parent;
        }

        public bool IsLeaf() => left == null && right == null;
    }
}
