using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace sadsmile
{
    public class RoomManager : MonoBehaviour
    {
        [Header("타일 정보")]
        public Tilemap tilemap;
        public TileBase wallTile;  // 문을 닫을 때 쓸 타일
        public TileBase floorTile; // 문을 열 때 쓸 타일

        [Header("방 정보")]
        public RectInt roomRect;   // 맵 생성기에서 넘겨받을 이 방의 크기 정보

        // 찾은 문의 위치를 저장해둘 리스트 (나중에 다시 열어야 하니까)
        private List<Vector3Int> doorPositions = new List<Vector3Int>();

        private bool isPlayerInside = false;
        private bool isCleared = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 플레이어가 처음 방에 들어왔을 때
            if (!isCleared && !isPlayerInside && collision.CompareTag("Player"))
            {
                isPlayerInside = true;
                CloseDoors(); 
            }
        }

      
        private void CloseDoors()
        {            // 방의 테두리(상하좌우 1칸 밖)를 스캔합니다.
            for (int x = roomRect.x -1; x <= roomRect.xMax; x++)
            {
                for (int y = roomRect.y -1; y <= roomRect.yMax; y++)
                {
                    // 정확히 테두리 라인인지 확인
                    if (x == roomRect.x - 1 || x == roomRect.xMax || y == roomRect.y - 1 || y == roomRect.yMax)
                    {
                        Vector3Int pos = new Vector3Int(x, y, 0);

                        // 테두리인데 바닥 타일(floorTile)이 깔려있다면, 그곳이 바로 입구!
                        var tilename = tilemap.GetTile(pos);
                        if (tilename == floorTile)
                        {
                            // 1. 나중에 문을 열기 위해 위치를 기억해둡니다.
                            doorPositions.Add(pos);

                            // 2. 입구를 벽 타일로 바꿔서 쾅! 닫아버립니다.
                            tilemap.SetTile(pos, wallTile);
                        }
                    }
                }
            }
        }

        // 몬스터를 다 잡았을 때 호출할 함수
        [ContextMenu("해방")]
        public void OpenDoors()
        {
           //벽에서 다시 길로 변경
            foreach (Vector3Int pos in doorPositions)
            {
                tilemap.SetTile(pos, floorTile);
            }
            isCleared = true;
        }
    }
}