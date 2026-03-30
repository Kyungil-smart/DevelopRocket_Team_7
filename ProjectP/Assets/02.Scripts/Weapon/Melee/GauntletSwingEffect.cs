using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GauntletSwingEffect : MonoBehaviour
{
    // 구현 원리 요약:
    // 부채꼴 Mesh 생성 후 짧게 표시

    private float radius;
    private float angle;

    [SerializeField] private float duration = 0.1f;

    public void Init(float r, float a)
    {
        radius = r;
        angle = a;

        GenerateMesh();
        Destroy(gameObject, duration);
    }

    private void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 20;

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        float step = angle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -angle / 2 + step * i;
            float rad = currentAngle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;

            vertices[i + 1] = new Vector3(x, y, 0);
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = new Color(1, 1, 0, 0.4f);

        GetComponent<MeshRenderer>().material = mat;
    }
}