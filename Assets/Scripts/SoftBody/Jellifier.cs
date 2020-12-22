using UnityEngine;
using UnityEngine.EventSystems;

public class Jellifier : MonoBehaviour
{
    public float bounceSpeed;
    public float fallForce;
    public float stiffness;

    private MeshFilter meshFilter;
    private Mesh mesh;

    JellyVertex[] jellyVertices;
    Vector3[] currentMeshVertices;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        GetVertices();
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 inputPoint = hit.point + (hit.point * 0.1f);
                ApplyPressureToPoint(inputPoint, fallForce);
            }
        }

        UpdateVertices();
    }

    private void GetVertices()
    {
        int meshLen = mesh.vertices.Length;

        jellyVertices = new JellyVertex[meshLen];
        currentMeshVertices = new Vector3[meshLen];

        for (int i = 0; i < meshLen; i++)
        {
            jellyVertices[i] = new JellyVertex(i, mesh.vertices[i], mesh.vertices[i], Vector3.zero);
            currentMeshVertices[i] = mesh.vertices[i];
        }
    }


    private void UpdateVertices()
    {
        if (jellyVertices != null)
        {
            for (int i = 0; i < jellyVertices.Length; i++)
            {
                jellyVertices[i].UpdateVelocity(bounceSpeed);
                jellyVertices[i].Settle(stiffness);

                jellyVertices[i].currentVertexPosition += jellyVertices[i].currentVelocity * Time.deltaTime;
                currentMeshVertices[i] = jellyVertices[i].currentVertexPosition;
            }

            mesh.vertices = currentMeshVertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        ContactPoint[] collisionPoints = other.contacts;

        for (int i = 0; i < collisionPoints.Length; i++)
        {
            Vector3 inputPoint = collisionPoints[i].point + (collisionPoints[i].point * 0.1f);
            ApplyPressureToPoint(inputPoint, fallForce);
        }
    }

    public void ApplyPressureToPoint(Vector3 _point, float _pressure)
    {
        for (int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].ApplyPressureToVertex(transform, _point, _pressure);
        }
    }
}
