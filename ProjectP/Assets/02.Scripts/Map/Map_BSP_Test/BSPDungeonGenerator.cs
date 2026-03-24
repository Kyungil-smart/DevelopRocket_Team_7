using UnityEngine;
using UnityEngine.Tilemaps;
namespace sadsmile
{
    public class BSPDungeonGenerator : MonoBehaviour
    {
        [Header("타일 설정")]
        public Tilemap tilemap;
        public TileBase floorTile;
        public TileBase wallTile;

        [Header("맵 크기 설정")]
        public int mapWidth = 60;
        public int mapHeight = 60;
        public int minRoomSize = 7;

        private void Start()
        {
            GenerateDungeon();
        }

        // R&D용: 스페이스바를 누르면 새로 생성 (New Input System 대응)
        private void Update()
        {
#if ENABLE_INPUT_SYSTEM
            if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
#else
        if (Input.GetKeyDown(KeyCode.Space))
#endif
            {
                tilemap.ClearAllTiles();
                GenerateDungeon();
            }
        }

        public void GenerateDungeon()//2
        {
            // 1. 트리 생성 (공간 분할)
            BSPNode root = new BSPNode(new RectInt(0, 0, mapWidth, mapHeight));
            Divide(root);

            // 2. 방 생성 및 통로 연결
            GenerateContents(root);
        }

        void Divide(BSPNode node)//3
        {
            //이 코드는 "이 구역을 더 쪼갤 수 있는지,
            //아니면 여기서 멈추고 방을 만들 준비를 할지"를 결정하는 **재귀 탈출 조건(Exit Condition)*
            //"더 이상 쪼개면 우리가 쓸 수 있는 제대로 된 방이 안 나오니까 그만 쪼개고,
            //지금 이 구역을 최종 방 후보(Leaf Node)로 확정해
            if (node.rect.width <= minRoomSize * 2 && node.rect.height <= minRoomSize * 2) return;

            bool splitHorizontal = Random.value > 0.5f;
            if (node.rect.width > node.rect.height * 1.5f) splitHorizontal = false;
            else if (node.rect.height > node.rect.width * 1.5f) splitHorizontal = true;
            /*
             * 리뷰: 단순히 랜덤하게 자르는 게 아니라 **가로세로 비율(Aspect Ratio)**을 체크하는 아주 좋은 로직입니다.

    이유: 이 코드가 없으면 국수처럼 아주 길쭉한 방이 생길 수 있습니다. 가로가 세로보다 1.5배 길면 무조건 세로로 잘라서(splitHorizontal = false) 정사각형에 가깝게 유지하도록 강제하는 장치입니다.
             */
            int splitPos = splitHorizontal
                ? Random.Range(minRoomSize, node.rect.height - minRoomSize)
                : Random.Range(minRoomSize, node.rect.width - minRoomSize);

            if (splitHorizontal)
            {
                node.left = new BSPNode(new RectInt(node.rect.x, node.rect.y, node.rect.width, splitPos), node);
                node.right = new BSPNode(new RectInt(node.rect.x, node.rect.y + splitPos, node.rect.width, node.rect.height - splitPos), node);
            }
            else
            {
                node.left = new BSPNode(new RectInt(node.rect.x, node.rect.y, splitPos, node.rect.height), node);
                node.right = new BSPNode(new RectInt(node.rect.x + splitPos, node.rect.y, node.rect.width - splitPos, node.rect.height), node);
            }

            Divide(node.left);
            Divide(node.right);
        }

        void GenerateContents(BSPNode node)//4
        {
            if (node.IsLeaf())
            {
                // 방 크기 결정 및 그리기
                int roomWidth = Random.Range(minRoomSize - 2, node.rect.width - 2);
                int roomHeight = Random.Range(minRoomSize - 2, node.rect.height - 2);
                int x = node.rect.x + (node.rect.width - roomWidth) / 2;
                int y = node.rect.y + (node.rect.height - roomHeight) / 2;

                node.roomRect = new RectInt(x, y, roomWidth, roomHeight);
                DrawRoomWithWalls(node.roomRect);
            }
            else
            {
                GenerateContents(node.left);
                GenerateContents(node.right);

                // 자식 노드 사이를 통로로 연결
                if (node.left != null && node.right != null)
                {
                    DrawCorridor(node.left, node.right);
                }
            }
        }

        void DrawRoomWithWalls(RectInt rect)//4
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

        void DrawCorridor(BSPNode left, BSPNode right)//5
        {
            // 각 구역의 중심점 계산
            Vector2Int start = new Vector2Int((int)left.rect.center.x, (int)left.rect.center.y);
            Vector2Int end = new Vector2Int((int)right.rect.center.x, (int)right.rect.center.y);

            // L자형 통로 생성
            for (int x = Mathf.Min(start.x, end.x); x <= Mathf.Max(start.x, end.x); x++)
                SetFloorTile(x, start.y);

            for (int y = Mathf.Min(start.y, end.y); y <= Mathf.Max(start.y, end.y); y++)
                SetFloorTile(end.x, y);
        }

        void SetFloorTile(int x, int y)//5
        {
            tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);

            // 통로 주변 빈 공간에 벽 세우기
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector3Int pos = new Vector3Int(x + i, y + j, 0);
                    if (tilemap.GetTile(pos) == null)
                        tilemap.SetTile(pos, wallTile);
                }
            }
        }
    }
}
