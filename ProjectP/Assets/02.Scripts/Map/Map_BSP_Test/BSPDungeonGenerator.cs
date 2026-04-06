using sadsmile;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
[System.Serializable]
public struct EnemySpawnOpction
{
    public int Min;
    public int Max;
}
public class BSPDungeonGenerator : MonoBehaviour
{
  [Header("타일 설정")]
  [SerializeField]private Tilemap tilemap;
  [SerializeField]private TileBase floorTile;
  [SerializeField]private TileBase wallTile;
    [SerializeField] private List<TileBase> _Tiles;
    [Header("맵 크기 및 방설정")]
    public int mapWidth;
    public int mapHeight;
    public int minRoomSize;
    public int maxRoomSize;
    [Header("최대 생성할 방의 개수")]
    public int maxRooms;
    //분할 추적하기 위한 카운터
    private int currentRoomCount = 1;
    [Header("오브젝트 프리팹")]
    public List<GameObject> monsterPrefab;
    [Header("보스 오브젝트 프리팹")]
    public  GameObject  _bossMonsterPrefab;
    // 생성된 최종 방(Leaf Node)들을 모아둘 리스트
    private List<BSPNode> leafRooms = new List<BSPNode>();
    [Header("휴식방 석상 오브젝트")]
    [SerializeField]private GameObject _restRoomStatuePrefab;
    [SerializeField]private List<GameObject> _restRoomsGimmic = new List<GameObject>();
    [SerializeField]private int CurrentRestRoomGimmicIndex = 0;
    [Header("몬스터 스폰 마릿수")]
    [Tooltip("추적")] public EnemySpawnOpction m_chase;
    [Tooltip("원거리")] public EnemySpawnOpction m_Range;
    [Tooltip("탱커")] public EnemySpawnOpction m_Tank;
    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
         
        currentRoomCount = 1;
        leafRooms.Clear();

        // 1. 트리 생성 (공간 분할)
        var root = new BSPNode(new RectInt(0, 0, mapWidth, mapHeight));
        Divide(root);
        GenerateContents(root);
        AssignRoomTypes();
        SpawnObjectsInRooms();
    }

    private void Divide(BSPNode node)
    {
        if (currentRoomCount >= maxRooms) return;
        // 방의 가로 세로 길이가 minRoomSize*2 보다 작거나 같으면 그만 재귀하고 돌아가라
        // can_not_Divide_Further : 더 이상 나눌 수 없습니다
        bool can_not_Divide_Further = node.rect.width <= minRoomSize * 2 && node.rect.height <= minRoomSize * 2;
        if (can_not_Divide_Further) return;

        //공간을 가로 또는 세로 로 자를건지 랜덤으로 정함
        bool splitHorizontal = Random.value > 0.5f;
        //가로가 세로보다 1.5배 이상 엄청 길다면?
        //무조건 세로로 잘라서(splitHorizontal = false) 뚱뚱해진 가로 길이를 반토막 냅니다.
        if (node.rect.width > node.rect.height * 1.5f)
        {
            splitHorizontal = false;
        }
        else if (node.rect.height > node.rect.width * 1.5f)
        {
            //반대로 세로로 너무 길쭉하다면?
            //무조건 가로로 잘라서(splitHorizontal = true) 위아래 길이를 줄여줍니다.
            splitHorizontal = true;
        }

        ///</summary> 
        ///Random.Range(minRoomSize, node.rect.height - minRoomSize) 있는데
        ///가로 방향으로 자르게 될 때 최소사이즈인 minRoomsize를 최소로 만큼 그리고
        /// 추가 작성 바람
        ///</summary>
        int splitPos = splitHorizontal//가로 방향으로 자를거면 true //세로는 false
            ? Random.Range(minRoomSize, node.rect.height - minRoomSize)//true
            : Random.Range(minRoomSize, node.rect.width - minRoomSize);//false
        ///</summary> 
        /// 밑에 코드들이 이제 실제로 구역을 나누는 로직이라고 보면 된다.
        ///  수평으로 자르게 되면 if문에 걸리고
        ///  수직으로 자르게 된다면 else 문으로 갈것이다.
        ///  
        /// 만약 수평으로 자르게 된다면
        ///              Root
        ///               |
        ///        |            |
        ///       LeftNode  RightNode
        ///          | 
        ///      |      | 
        ///   LeftNode   RightNode
        ///   
        ///  Left Right 에 값을 할당해줌
        ///   이걸 더 나눌수 없을 때 까지 나눔
        ///</summary>
        if (splitHorizontal)//수평 자르기 인가
        {
            node.left  = new BSPNode(new RectInt(node.rect.x, node.rect.y, node.rect.width, splitPos), node);
            node.right = new BSPNode(new RectInt(node.rect.x, node.rect.y + splitPos, node.rect.width, node.rect.height - splitPos), node);
        }
        else//수직 자르기 인가
        {
            node.left  = new BSPNode(new RectInt(node.rect.x, node.rect.y, splitPos, node.rect.height), node);
            node.right = new BSPNode(new RectInt(node.rect.x + splitPos, node.rect.y, node.rect.width - splitPos, node.rect.height), node);
        }

        // 분할에 성공했으므로 방(구역) 개수 1 증가
        currentRoomCount++;

        Divide(node.left);
        Divide(node.right);
    }


    private  void GenerateContents(BSPNode node)
    {
        if(node.IsLeaf())
        { 
             //  방 상하좌우에 둘 여백(Padding)을 설정 .
            // 이 값을 늘릴수록 방 사이의 간격이 넓어짐 
            int padding = 3;

            // 1. 최대 크기 계산 시, 노드 크기에서 양옆/위아래 패딩(padding * 2)만큼 뺍니다.
            int maxW = Mathf.Min(maxRoomSize, node.rect.width - (padding * 2));
            int maxH = Mathf.Min(maxRoomSize, node.rect.height - (padding * 2));

            // 2. 최소 크기(minRoomSize)가 최대 크기(max)를 초과하지 않도록 안전장치를 둡니다.
            int minW = Mathf.Min(minRoomSize, maxW);
            int minH = Mathf.Min(minRoomSize, maxH);

            // 3. 제한된 최소/최대값 안에서 방의 크기를 랜덤으로 결정합니다.
            int roomWidth = Random.Range(minW, maxW);
            int roomHeight = Random.Range(minH, maxH);

            // 방을 노드의 중앙에 배치   
            int x = node.rect.x + (node.rect.width - roomWidth) / 2;
            int y = node.rect.y + (node.rect.height - roomHeight) / 2;

            node.roomRect = new RectInt(x, y, roomWidth, roomHeight);
            DrawRoomWithWalls(node.roomRect);
            leafRooms.Add(node);
        }
        else
        {
            GenerateContents(node.left);
            GenerateContents(node.right);
            if (node.left != null && node.right != null)
            {
                DrawCorridor(node.left, node.right);
            }
        }
    }
    private void DrawRoomWithWalls(RectInt rect)
    {
        for (int x = rect.x - 1; x <= rect.xMax; x++)
        {
            for (int y = rect.y - 1; y <= rect.yMax; y++)
            {
                if (x == rect.x - 1 || x == rect.xMax || y == rect.y - 1 || y == rect.yMax)
                {
                    bool isfloor= _Tiles.Contains(tilemap.GetTile(new Vector3Int(x, y, 0)));
                    //tilemap.GetTile(new Vector3Int(x, y, 0)) != floorTile
                    if (!isfloor)
                        tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), _Tiles[Random.Range(0, _Tiles.Count - 1)]);
                }
            }
        }
    }
    private void DrawCorridor(BSPNode left, BSPNode right)
    {
        Vector2Int start = new Vector2Int((int)left.rect.center.x, (int)left.rect.center.y);
        Vector2Int end = new Vector2Int((int)right.rect.center.x, (int)right.rect.center.y);
        for (int x = Mathf.Min(start.x, end.x); x <= Mathf.Max(start.x, end.x); x++)
        {
            SetFloorTile(x, start.y);
        }
        for (int y = Mathf.Min(start.y, end.y); y <= Mathf.Max(start.y, end.y); y++)
        {
            SetFloorTile(end.x, y);
        }
    }
    //private void SetFloorTile(int x, int y)
    //{ 
    //    tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
    //    for (int i = -1; i <= 1; i++)
    //    {
    //        for (int j = -1; j <= 1; j++)
    //        {
    //            Vector3Int pos = new Vector3Int(x + i, y + j, 0);
    //            if (tilemap.GetTile(pos) == null)
    //            {
    //                tilemap.SetTile(pos, wallTile);

    //            }
    //        }
    //    }
    //}
    private void SetFloorTile(int x, int y)
    {
        int corridorSize = 2; // 🌟 통로 굵기 (이 값을 3으로 바꾸면 3칸 굵기가 됩니다!)

        // 1. 통로 바닥을 지정한 굵기(2x2)만큼 넓게 깝니다.
        for (int ox = 0; ox < corridorSize; ox++)
        {
            for (int oy = 0; oy < corridorSize; oy++)
            {
                tilemap.SetTile(new Vector3Int(x + ox, y + oy, 0), _Tiles[Random.Range(0,_Tiles.Count-1)]);
            }
        }

        // 2. 넓어진 통로 두께에 맞춰서 벽을 치는 범위도 (-1 부터 +corridorSize 까지) 늘려줍니다.
        for (int i = -1; i <= corridorSize; i++)
        {
            for (int j = -1; j <= corridorSize; j++)
            {
                Vector3Int pos = new Vector3Int(x + i, y + j, 0);

                // 주변에 아무것도 없는 허공(null)일 때만 벽을 세웁니다.
                if (tilemap.GetTile(pos) == null)
                {
                    tilemap.SetTile(pos, wallTile);
                }
            }
        }
    }
    private void AssignRoomTypes()
    {
        if (leafRooms.Count == 0) return;

        // 리스트 섞기 (랜덤하게 역할을 부여하기 위함)
        ShuffleList(leafRooms);

        // 예시 로직: 
        // 첫 번째 방 = 시작 방
        // 두 번째 방 = 휴식 방
        // 마지막 방 = 보스 방
        // 나머지 = 60% 확률로 몬스터 방, 40% 확률로 일반 방(또는 보물)

        for (int i = 0; i < leafRooms.Count; i++)
        {
            if (i == 0)
            {
                leafRooms[i].roomType = RoomType.StartNode;
            }
            else if (i == 1 && leafRooms.Count > 2)
            {
                leafRooms[i].roomType = RoomType.RestNode;
            }
            else if (i == leafRooms.Count - 2 && leafRooms.Count > 3)
            {
                leafRooms[i].roomType = RoomType.RestNode;
            }
            else if (i == leafRooms.Count - 1 && leafRooms.Count > 3)
            {
                leafRooms[i].roomType = RoomType.BossNode;
            }
            else
            {
              leafRooms[i].roomType = Random.value > 0.4f ? RoomType.MiddleNode : RoomType.NULL;
            }
        }
    }
    // 리스트를 랜덤하게 섞는 유틸리티 함수  
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    private void SpawnObjectsInRooms()
    {
        foreach (BSPNode room in leafRooms)
        {
            Vector3 centerPos = new Vector3(room.roomRect.center.x, room.roomRect.center.y, 0);
            
            if (room.roomType ==RoomType.MiddleNode)
            {
                EnemySpawnMsg spawnMSG=new EnemySpawnMsg();
                spawnMSG.positions=new List<Vector2>();
                spawnMSG.spawnNums=new Dictionary<string, int>();
                int spawnCount = 0;
                foreach (var prefab in monsterPrefab)
                {
                    //탱커1==최소 1~ 최대 3
                    //추적2==최소 2~ 최대 3
                    //원거리3==최소 6~ 최대 9
                    if (prefab.name== "MonsterTank")
                    {
                        var spawnNums = UnityEngine.Random.Range(m_Tank.Min, m_Tank.Max);
                        spawnMSG.spawnNums.Add(prefab.name, spawnNums);
                        spawnCount += spawnNums;
                    }
                    else if(prefab.name == "MonsterChase")
                    {
                        var spawnNums = UnityEngine.Random.Range(m_chase.Min, m_chase.Max);
                        spawnMSG.spawnNums.Add(prefab.name, spawnNums);
                        spawnCount += spawnNums;
                    }
                    else if(prefab.name == "MonsterRange")
                    {
                        var spawnNums = UnityEngine.Random.Range(m_Tank.Min, m_Tank.Max);
                        spawnMSG.spawnNums.Add(prefab.name, spawnNums);
                        spawnCount += spawnNums;
                    }
                    ///////
                }
                for(int i=0; i< spawnCount; i++)
                {
                    spawnMSG.positions.Add(new Vector2(
                        Random.Range(room.roomRect.xMin + 1.5f, room.roomRect.xMax - 1.5f),
                        UnityEngine.Random.Range(room.roomRect.yMin + 1.5f, room.roomRect.yMax - 1.5f)
                    ));
                }
                var obj = new GameObject();
                var data = obj.AddComponent<BoxCollider2D>();
                data.isTrigger = true;
                data.transform.position = room.roomRect.center;
                data.size = room.roomRect.size - new Vector2(1, 1); ;

                var manager= obj.AddComponent<RoomManager>();
                manager.m_CurrentRoomType = room.roomType;
                manager.tilemap=tilemap;
                manager.wallTile = wallTile;
                manager.floorTile = floorTile;
                manager.roomRect = room.roomRect;
                manager.Tiles=_Tiles;

                manager.m_spawnMSG = spawnMSG;
                manager.MonsterSpawnCount = spawnCount;
            }
            else if (room.roomType == RoomType.StartNode)
            {
                //플레이어 프리팹을 받아서 생성 해야 할거 같다.
                //시작 노드의 중앙에 스폰 예정
                var SponPos = room.roomRect.center;
                // 시작 방에 플레이어에게 위치 정보 전달
                PostManager.Instance.Post<Vector2>(PostMessageKey.InitPlayerPosition, SponPos);
            }
            else if(room.roomType == RoomType.RestNode)
            {
                var obj=Instantiate(_restRoomStatuePrefab, room.roomRect.center, Quaternion.identity);
                var pos = obj.transform.GetChild(0);
                if(CurrentRestRoomGimmicIndex ==0)
                {
                    Tilemap data= pos.GetComponent<Tilemap>();
                    data.color = Color.red;
                }
                else
                {
                    Tilemap data = pos.GetComponent<Tilemap>();
                    data.color = new Color(0,255,255);
                }
                var AddObj=Instantiate( _restRoomsGimmic[CurrentRestRoomGimmicIndex++]);
                AddObj.transform.SetParent(pos.transform);
                 

            }
            else if(room.roomType == RoomType.BossNode)
            {
                
                var obj = new GameObject("BossRoom");
                var data = obj.AddComponent<BoxCollider2D>();
                data.isTrigger = true;
                data.transform.position = room.roomRect.center;
                data.size = room.roomRect.size-new Vector2(1,1);

                var manager = obj.AddComponent<RoomManager>();
                manager.m_CurrentRoomType = room.roomType;
                manager.tilemap = tilemap;
                manager.wallTile = wallTile;
                manager.floorTile = floorTile;
                manager.roomRect = room.roomRect;
                manager.MonsterSpawnCount = 1;
                manager.Tiles = _Tiles;
                var spawnManager=obj.AddComponent<BossMonsterSpawn>();
                spawnManager.bossMonsterPrefab = _bossMonsterPrefab;
                spawnManager.spawnPosition= room.roomRect.center;
            }
            else if(room.roomType == RoomType.NULL)
            {

            }
        }
    }
}
