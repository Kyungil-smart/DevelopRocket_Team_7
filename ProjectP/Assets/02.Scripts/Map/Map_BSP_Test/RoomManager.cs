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
        private bool RoomClear=false;
        private bool isCleared = false;
        public EnemySpawnMsg m_spawnMSG;

        public int MonsterSpawnCount = 0;
        /*
          EnemySpawnMsg
         public Dictionary<string, int> spawnNums;
         몬스터 프리팹 관리,int는 마릿수

         public List<Vector2> positions;
        몬스터 들이 소환되는 위치

        소환되어야할 총 수
         */

        /*
        몬스터가 죽을때마다 post보내주시는데 
        카운트 1보내주시면

        룸매니저에서 현재 스폰된 마릿수에서 -1씩하고
        그러다가 0이되면 문 열기 호출
         */
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 플레이어가 처음 방에 들어왔을 때
            if (!isCleared && !isPlayerInside && collision.CompareTag("Player"))
            {
                isPlayerInside = true;
                RoomClear=false;
                CloseDoors();
                // 몬스터 소환 요청
                PostManager.Instance.Post(PostMessageKey.EnemySpawned, m_spawnMSG);
            }
        }
        

        private void CloseDoors()
        {            // 방의 테두리(상하좌우 1칸 밖)를 스캔
            for (int x = roomRect.x -1; x <= roomRect.xMax; x++)
            {
                for (int y = roomRect.y -1; y <= roomRect.yMax; y++)
                {
                    // 정확히 테두리 라인인지 확인
                    if (x == roomRect.x - 1 || x == roomRect.xMax || y == roomRect.y - 1 || y == roomRect.yMax)
                    {
                        Vector3Int pos = new Vector3Int(x, y, 0);

                        // 테두리인데 바닥 타일(floorTile)이 깔려있다면 입구여서 이부분 막아야 함
                        var tilename = tilemap.GetTile(pos);
                        if (tilename == floorTile)
                        {
                            // 1. 나중에 문을 열기 위해 위치를 기억
                            doorPositions.Add(pos);

                            // 2. 입구를 벽 타일로 바꾸서 길 막기
                            tilemap.SetTile(pos, wallTile);
                        }
                    }
                }
            }
        }

        // 몬스터를 다 잡았을 때 호출할 함수
         
        public void OpenDoors( )
        {
            RoomClear = true;
            //벽에서 다시 길로 변경
            foreach (Vector3Int pos in doorPositions)
            {
                tilemap.SetTile(pos, floorTile);
            }
            isCleared = true;
        }
        private void OnEnable()
        {
            PostManager.Instance.Subscribe<int>(PostMessageKey.EnemyDeadAlram, CheckClear);

        }
        private void OnDisable()
        {
            PostManager.Instance.Unsubscribe<int>(PostMessageKey.EnemyDeadAlram, CheckClear);

        }

        public void CheckClear(int count)
        {
            //방에 안들어왔거나 이미 클리어 했으면 패스
            if (isPlayerInside == false || RoomClear == true) return;

            MonsterSpawnCount -= count;
            if (MonsterSpawnCount <= 0)
            {
                OpenDoors();
            }
        }
    }
       
    }