using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class UniversalSOManagerWindow : EditorWindow
{
    private Dictionary<string, List<ScriptableObject>> soDatabase = new Dictionary<string, List<ScriptableObject>>();
    private List<string> typeNames = new List<string>();
    private int selectedTypeIndex = 0;

    private ScriptableObject selectedSO;
    private Vector2 leftScrollPos;
    private Vector2 rightScrollPos;

    // ==========================================
    // 🚫 [블랙리스트] 드롭다운에서 숨기고 싶은 타입들을 여기에 추가하세요!
    // ==========================================
    private readonly HashSet<string> ignoredTypes = new HashSet<string>()
    {
        "InputActionAsset",
        "Readme",
        "UniversalRendererData",
        "UniversalRenderPipelineAsset",
        "UniversalRenderPipelineGlobalSettings",
        "VolumeProfile",
        "RenderTexture",
        "SpriteAtlasAsset",
        "TMP_FontAsset",
        "TMP_SpriteAsset",
        "TMP_Settings",
        "TMP_StyleSheet",
        // 필요에 따라 앞으로 거슬리는 타입이 생기면 여기에 이름만 추가하면 됩니다.
    };

    [MenuItem("Tools/통합 SO 데이터 매니저")]
    public static void ShowWindow()
    {
        GetWindow<UniversalSOManagerWindow>("SO Manager");
    }

    private void OnEnable()
    {
        LoadAllSOs();
    }

    private void LoadAllSOs()
    {
        soDatabase.Clear();
        typeNames.Clear();
        selectedSO = null;

        // Assets 폴더 전체에서 모든 ScriptableObject 검색
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            // 🚫 [경로 예외 처리] Settings 폴더나 Plugins 폴더 하위의 SO는 아예 무시
            if (assetPath.Contains("/Settings/") || assetPath.Contains("/Plugins/"))
                continue;

            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            if (so != null)
            {
                string typeName = so.GetType().Name;

                // 🚫 [타입 예외 처리] 블랙리스트에 있는 타입이면 건너뜀
                if (ignoredTypes.Contains(typeName))
                    continue;

                // 딕셔너리에 해당 타입이 없으면 새 리스트 생성
                if (!soDatabase.ContainsKey(typeName))
                {
                    soDatabase[typeName] = new List<ScriptableObject>();
                }

                soDatabase[typeName].Add(so);
            }
        }

        // 드롭다운 메뉴 구성을 위해 타입 이름들만 뽑아서 알파벳 순 정렬
        typeNames = soDatabase.Keys.ToList();
        typeNames.Sort();

        // 새로고침 시 인덱스 초과 방지
        if (selectedTypeIndex >= typeNames.Count)
        {
            selectedTypeIndex = 0;
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        if (GUILayout.Button("새로고침", EditorStyles.toolbarButton, GUILayout.Width(80)))
        {
            LoadAllSOs();
        }

        GUILayout.Space(10);
        GUILayout.Label("SO 타입 선택:", GUILayout.Width(80));

        if (typeNames.Count > 0)
        {
            int previousIndex = selectedTypeIndex;
            selectedTypeIndex = EditorGUILayout.Popup(selectedTypeIndex, typeNames.ToArray(), EditorStyles.toolbarPopup);

            if (previousIndex != selectedTypeIndex)
            {
                selectedSO = null;
                GUI.FocusControl(null);
            }
        }
        else
        {
            GUILayout.Label("검색된 SO가 없습니다.");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (typeNames.Count == 0) return;

        GUILayout.BeginHorizontal();
        DrawLeftPanel();
        GUILayout.Space(10);
        DrawRightPanel();
        GUILayout.EndHorizontal();
    }

    private void DrawLeftPanel()
    {
        leftScrollPos = GUILayout.BeginScrollView(leftScrollPos, GUILayout.Width(220), GUILayout.ExpandHeight(true));

        string currentSelectedType = typeNames[selectedTypeIndex];
        List<ScriptableObject> currentList = soDatabase[currentSelectedType];

        GUILayout.Label($"{currentSelectedType} 목록 ({currentList.Count}개)", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        foreach (ScriptableObject so in currentList)
        {
            GUI.backgroundColor = (selectedSO == so) ? Color.cyan : Color.white;

            if (GUILayout.Button(so.name, EditorStyles.miniButton))
            {
                selectedSO = so;
                GUI.FocusControl(null);
            }
            GUI.backgroundColor = Color.white;
        }

        GUILayout.EndScrollView();
    }

    private void DrawRightPanel()
    {
        rightScrollPos = GUILayout.BeginScrollView(rightScrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        if (selectedSO == null)
        {
            GUILayout.Label("데이터 수정기", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("좌측 목록에서 수정할 데이터를 선택해주세요.", MessageType.Info);
        }
        else
        {
            string typeName = selectedSO.GetType().Name;

            // 타이틀과 위치 찾기 버튼을 가로로 배치
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{selectedSO.name} 데이터", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace(); // 버튼을 우측으로 밀어주기 위해 추가

            // 💡 추가된 부분: 프로젝트 폴더에서 위치 찾기 버튼
            if (GUILayout.Button("📁 위치 찾기", GUILayout.Width(80), GUILayout.Height(20)))
            {
                // 1. 프로젝트 창에서 해당 에셋을 노란색으로 핑(하이라이트) 찍어줍니다.
                EditorGUIUtility.PingObject(selectedSO);
                // 2. 해당 에셋을 실제로 선택된 상태로 만들어줍니다. (Project 창 스크롤 이동을 위해 필요)
                Selection.activeObject = selectedSO;
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            SerializedObject serializedSO = new SerializedObject(selectedSO);
            serializedSO.Update();

            SerializedProperty property = serializedSO.GetIterator();
            bool enterChildren = true;

            while (property.NextVisible(enterChildren))
            {
                if (property.name == "m_Script")
                {
                    enterChildren = false;
                    continue;
                }

                EditorGUILayout.PropertyField(property, true);
                enterChildren = false;
            }

            serializedSO.ApplyModifiedProperties();
        }

        GUILayout.EndScrollView();
    }
}