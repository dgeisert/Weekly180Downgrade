using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class IslandLevel
{
    [SerializeField] public Color color;
    [SerializeField] public GameObject[] objects;
    [SerializeField] public float objectRate;
}

public class IslandBuilder : MonoBehaviour
{
    public float grade = 20;
    public GameObject road;
    public Material islandMat;
    public int size = 40;
    public float scale = 1;
    public float height = 5;
    public float perlinScale = 0.1f;

    [SerializeField] public IslandLevel[] levels;

    public IslandBuilder Init(float grade)
    {
        this.grade = grade;
        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> verts2 = new List<Vector3>();
        List<Color> vertColors = new List<Color>();
        List<int> tris = new List<int>();
        GameObject.Instantiate(
            road,
            transform.position + new Vector3((size / 2 - 2), -grade / 100 * (size / 2) + 0.5f, size / 2) * scale,
            Quaternion.Euler(Mathf.Atan(grade / 100) * 180 / Mathf.PI, 0, 0)
        ).transform.SetParent(transform);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float y = (i * j % 17) / 29f - j * grade / 100;

                verts.Add(scale * new Vector3(i + Random.value, y, j + Random.value));
                if (i > 0 && j > 0)
                {

                    Color tri1Color = levels[new int[]
                    {
                        Area(verts[(i - 1) * size + j - 1].y),
                            Area(verts[(i - 1) * size + j].y),
                            Area(verts[i * size + j - 1].y)
                    }.Max()].color;
                    vertColors.Add(tri1Color);
                    vertColors.Add(tri1Color);
                    vertColors.Add(tri1Color);

                    verts2.Add(verts[(i - 1) * size + j - 1] + Vector3.up * Area(verts[(i - 1) * size + j - 1].y * scale));
                    verts2.Add(verts[(i - 1) * size + j] + Vector3.up * Area(verts[(i - 1) * size + j].y * scale));
                    verts2.Add(verts[i * size + j - 1] + Vector3.up * Area(verts[i * size + j - 1].y * scale));

                    tris.Add(verts2.Count - 3);
                    tris.Add(verts2.Count - 2);
                    tris.Add(verts2.Count - 1);

                    Color tri2Color = levels[new int[]
                    {
                        Area(verts[i * size + j].y),
                            Area(verts[(i - 1) * size + j].y),
                            Area(verts[i * size + j - 1].y)
                    }.Max()].color;

                    vertColors.Add(tri2Color);
                    vertColors.Add(tri2Color);
                    vertColors.Add(tri2Color);

                    verts2.Add(verts[i * size + j] + Vector3.up * Area(verts[i * size + j].y * scale));
                    verts2.Add(verts[(i - 1) * size + j] + Vector3.up * Area(verts[(i - 1) * size + j].y * scale));
                    verts2.Add(verts[i * size + j - 1] + Vector3.up * Area(verts[i * size + j - 1].y * scale));

                    tris.Add(verts2.Count - 1);
                    tris.Add(verts2.Count - 2);
                    tris.Add(verts2.Count - 3);

                    if ((i < 7 || i > 10) &&
                        levels != null && levels.Length >= Area(verts[i * size + j].y) && levels[Area(verts[i * size + j].y)].objects.Length > 0 &&
                        levels[Area(verts[i * size + j].y)].objectRate > Random.value * Mathf.PerlinNoise(-transform.position.x + i * perlinScale * 2, -transform.position.z + j * perlinScale * 2))
                    {
                        GameObject.Instantiate(RandomObject(levels[Area(verts[i * size + j].y)]),
                            transform.position + verts2[verts2.Count - 3],
                            Quaternion.Euler(0, Random.value * 360, 0),
                            transform);
                    }
                }
            }
        }

        mesh.vertices = verts2.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.colors = vertColors.ToArray();
        mesh.RecalculateNormals();

        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = islandMat;
        gameObject.AddComponent<MeshCollider>();
        return this;
    }

    int Area(float height)
    {
        return Mathf.Clamp((int) ((height / scale + 1f) * 2f), 0, levels.Length - 1);
    }

    GameObject RandomObject(IslandLevel objectList)
    {
        return objectList.objects[Mathf.FloorToInt(Random.value * objectList.objects.Length)];
    }

    void GenerateMesh() { }
}