using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScaleCamPerspective : MonoBehaviour
{
    private Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        float aspectRatio = (float)16 / 9;
        camera.aspect = aspectRatio;
    }
}