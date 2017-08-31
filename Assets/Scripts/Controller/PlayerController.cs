using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public ETCJoystick Joystick;
    public Camera MainCamera;
    public GameObject ViewPoint;

    void Start ()
    {
        Joystick.onMoveStart.AddListener(OnMoveStart);
        Joystick.onMove.AddListener(OnMove);
        Joystick.onMoveEnd.AddListener(OnMoveEnd);
	}

    public void OnMoveStart()
    {
    }

    private float CollationPlayerForward()
    {
        var vec1 = transform.forward.normalized;
        var vec2 = MainCamera.transform.forward.normalized;
        var dot = Vector3.Dot(vec1, vec2);
        var rotate = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (!float.IsNaN(rotate))
        {
            Debug.Log(string.Format("Rotate: {0}", rotate));
            transform.Rotate(Vector3.up, rotate);
        }
        return rotate;
    }

    private void CreateRotateMatrixForViewPoint(float angle)
    {
        _matrix = Matrix4x4.identity;
        _matrix.SetTRS(Vector3.zero, Quaternion.Euler(0, angle, 0), Vector3.one);
    }

    public void OnMove(Vector2 v)
    {
        Vector4 offset = new Vector4(v.x, 0, v.y, 1);
        transform.LookAt(MainCamera.transform.TransformVector(offset) + transform.position);
        transform.Translate(transform.forward * Time.deltaTime * 2, Space.World);
    }

    public void OnMoveEnd()
    {

    }

    private Mesh _mesh;
    public Vector3[] vertices;
    public int[] triangle;

    private Vector4 _startPos;
    private Matrix4x4 _matrix;

    public float _x;
    public float _y;
    public float _z;

    private Vector2 _vec = new Vector2(1, 1);
    private void Awake()
    {
        _mesh = new Mesh();
        _mesh.vertices = vertices;
        _mesh.triangles = triangle;
        GetComponent<MeshFilter>().mesh = _mesh;

        _startPos = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1);
    }

    private void Update()
    {
        //Translate(_vec);
    }

    private void Translate(Vector2 vec)
    {
        _matrix = Matrix4x4.identity;

        _matrix.SetTRS(new Vector3(_x + vec.x, _y, _z + vec.y) * 0.2f, Quaternion.identity, Vector3.one);

        var t = _matrix * _startPos;
        transform.position = new Vector3(t.x, t.y, t.z);
    }
}
