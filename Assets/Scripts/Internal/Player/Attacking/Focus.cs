using EzySlice;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Focus : MonoBehaviour
{
    public bool bladeMode;

    private Vector3 normalOffset;
    public Vector3 zoomOffset;
    private float normalFOV;
    public float zoomFOV = 15;

    public Transform cutPlane;

    public Camera TPCamera;

    public Material crossMaterial;

    public LayerMask layerMask;

    public PostProcessVolume post;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cutPlane.gameObject.SetActive(false);

        normalFOV = TPCamera.fieldOfView;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Time.timeScale != 0)
            {
                Zoom(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            Zoom(false);
        }

        if (bladeMode)
        {
            RotatePlane();

            if (Input.GetMouseButtonDown(0))
            {
                Slice();
            }
        }
    }

    public void Slice()
    {
        Collider[] hits = Physics.OverlapBox(cutPlane.position, new Vector3(5, 0.1f, 5), cutPlane.rotation, layerMask);

        if (hits.Length <= 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            SlicedHull hull = SliceObject(hits[i].gameObject, crossMaterial);
            if (hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, crossMaterial);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, crossMaterial);
                AddHullComponents(bottom);
                AddHullComponents(top);
                Destroy(hits[i].gameObject);
            }
        }
    }

    public void AddHullComponents(GameObject go)
    {
        go.layer = 9;
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(100, go.transform.position, 20);
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(cutPlane.position, cutPlane.up, crossSectionMaterial);
    }

    public void Zoom(bool state)
    {
        bladeMode = state;

        cutPlane.localEulerAngles = Vector3.zero;
        cutPlane.gameObject.SetActive(state);

        TPCamera.fieldOfView = state ? zoomFOV : normalFOV;
        Time.timeScale = state ? .2f : 1;
    }

    public void RotatePlane()
    {
        cutPlane.eulerAngles += new Vector3(0, 0, -Input.GetAxis("Mouse X") * 5);
    }

    void FieldOfView(float fov)
    {
        TPCamera.fieldOfView = fov;
    }

    void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }
}
