using sadsmile;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BSPDungeonGenerator : MonoBehaviour
{
  [Header("타일 설정")]
  [SerializeField]private Tilemap tilemap;
  [SerializeField]private TileBase floorTile;
  [SerializeField]private TileBase wallTile;
    [Header("맵 크기 및 방설정")]
    public int mapWidth;
    public int mapHeight;
    public int minRoomSize;

    [Header("최대 생성할 방의 개수")]
    public int maxRooms;
    //분할 추적하기 위한 카운터
    private int currentRoomCount = 1;
    [Header("오브젝트 프리팹")]
    public GameObject monsterPrefab;
    public GameObject restCampfirePrefab;

    // 생성된 최종 방(Leaf Node)들을 모아둘 리스트
    private List<BSPNode> leafRooms = new List<BSPNode>();
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
            int roomWidth  = Random.Range(minRoomSize - 2, node.rect.width - 2);
            int roomHeight = Random.Range(minRoomSize - 2, node.rect.height - 2);

            int x = node.rect.x + (node.rect.width - roomWidth)   / 2;
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
                    if (tilemap.GetTile(new Vector3Int(x, y, 0)) != floorTile)
                        tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
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
                tilemap.SetTile(new Vector3Int(x + ox, y + oy, 0), floorTile);
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
                var obj = new GameObject();
                var data = obj.AddComponent<BoxCollider2D>();
                data.isTrigger = true;
                data.transform.position = room.roomRect.center;
                data.size = room.roomRect.size;

                var manager= obj.AddComponent<RoomManager>();
                manager.tilemap=tilemap;
                manager.wallTile = wallTile;
                manager.floorTile = floorTile;
                manager.roomRect = room.roomRect;
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

            }
            else if(room.roomType == RoomType.BossNode)
            {

            }
            else if(room.roomType == RoomType.NULL)
            {

            }
        }
    }
}
