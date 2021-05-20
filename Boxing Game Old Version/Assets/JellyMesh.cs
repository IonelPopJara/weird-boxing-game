using UnityEngine;

public class JellyMesh : MonoBehaviour
{
    public float jellyIntensity = 1f;
    public float jellyMass = 1f;
    public float jellyStiffness = 1f;
    public float jellyDamping = 0.75f;
    
    private Mesh originalMesh, meshClone;
    private MeshRenderer mRenderer;
    private JellyVertex[] jv;
    private Vector3[] vertexArray;

    private void Start()
    {
        originalMesh = GetComponent<MeshFilter>().sharedMesh;
        meshClone = Instantiate(originalMesh);
        GetComponent<MeshFilter>().sharedMesh = meshClone;
        mRenderer = GetComponent<MeshRenderer>();
        jv = new JellyVertex[meshClone.vertices.Length];
        for (int i = 0; i < meshClone.vertices.Length; i++)
            jv[i] = new JellyVertex(i, transform.TransformPoint(meshClone.vertices[i]));
    }

    private void FixedUpdate()
    {
        vertexArray = originalMesh.vertices;
        for (int i = 0; i < jv.Length; i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jv[i].ID]);
            float intensity = (1 - (mRenderer.bounds.max.y - target.y) / mRenderer.bounds.size.y) * jellyIntensity;
            jv[i].Shake(target, jellyMass, jellyStiffness, jellyDamping);
            target = transform.InverseTransformPoint(jv[i].Position);
            vertexArray[jv[i].ID] = Vector3.Lerp(vertexArray[jv[i].ID], target, intensity);
        }
        meshClone.vertices = vertexArray;
    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 Velocity, Force;

        public JellyVertex(int _id, Vector3 _pos)
        {
            ID = _id;
            Position = _pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)
        {
            Force = (target - Position) * s;
            Velocity = (Velocity + Force / m) * d;
            Position += Velocity;
            if ((Velocity + Force + Force / m).magnitude < 0.001f)
                Position = target;
        }
    }
}
