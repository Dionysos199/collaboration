using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class raymarchCamera : SceneViewFilter
{
    [SerializeField]
    private Shader _shader;
    public Material _raymarchMaterial
    {
        get
        {
            if (!_raymarchMat && _shader)
            {
                _raymarchMat = new Material(_shader);
                _raymarchMat.hideFlags = HideFlags.HideAndDontSave;
            }
            return _raymarchMat;
        }
    }
    private Material _raymarchMat;
    public Camera _camera
    {
        get
        {
            if (!_cam)
            {
                _cam = GetComponent<Camera>();

            }
            return _cam;
        }

    }
    private Camera _cam;
    public Transform _directionalLight;

    public float _maxDistance;
    public Color _mainColor;


    [SerializeField]
    private Vector4 _sphere1, _box1, _sphere2;
    [Header("Signed Distance Field")]
    public float _box1Round, _boxSphereSmooth, _sphereIntersectSmooth;
    


        private Vector4        _cone1, _hexPrism1, _cylinder1, _torus1;

    //public Vector3 _mandleBrot1;
    //public Vector4 _mandleBrotColor1;
 

    //public Vector2 _torusParam;
    //public Vector3 _modInterval;



    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!_raymarchMaterial)
        {
            Graphics.Blit(source, destination);
            return;
        }
        _raymarchMaterial.SetVector("_lightDir", _directionalLight ? _directionalLight.forward : Vector3.down);
        _raymarchMaterial.SetMatrix("_CamFrustum", CamFrustum(_camera));
        _raymarchMaterial.SetMatrix("_CamToWorld", _camera.cameraToWorldMatrix);
        _raymarchMaterial.SetFloat("_maxDistance", _maxDistance);
        _raymarchMaterial.SetFloat("_box1Round", _box1Round);
        _raymarchMaterial.SetFloat("_boxSphereSmooth", _boxSphereSmooth);
        _raymarchMaterial.SetFloat("_sphereIntersectSmooth", _sphereIntersectSmooth);



        _raymarchMaterial.SetVector("_sphere1", _sphere1);
        _raymarchMaterial.SetVector("_sphere2", _sphere2);


        _raymarchMaterial.SetVector("_box1", _box1);
        _raymarchMaterial.SetVector("_cone1", _cone1);
        _raymarchMaterial.SetVector("_hexPrism1", _hexPrism1);
        _raymarchMaterial.SetVector("_cylinder1", _cylinder1);
        _raymarchMaterial.SetVector("_torus1", _torus1);
        //_raymarchMaterial.SetVector("_torusParam", _torusParam);

        //_raymarchMaterial.SetVector("_mandleBrotColor1", _mandleBrotColor1);
        //_raymarchMaterial.SetVector("_mandleBrot1", _mandleBrot1);


        _raymarchMaterial.SetColor("_mainColor", _mainColor);
        //_raymarchMaterial.SetVector("_modInterval",_modInterval);

        RenderTexture.active = destination;

        _raymarchMaterial.SetTexture("MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho();
        _raymarchMaterial.SetPass(0);
        GL.Begin(GL.QUADS);

        // BL
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f);

        // BR
        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f);
        // TR
        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f);
        // TL
        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();


    }
    private Matrix4x4 CamFrustum(Camera cam)
    {
        Matrix4x4 frustum = Matrix4x4.identity;
        float fov = Mathf.Tan((cam.fieldOfView * .5f) * Mathf.Deg2Rad);

        Vector3 goUp = Vector3.up * fov;

        Vector3 goRight = Vector3.right * fov * cam.aspect;

        Vector3 TL = (-Vector3.forward - goRight + goUp);
        Vector3 TR = (-Vector3.forward + goRight + goUp);
        Vector3 BR = (-Vector3.forward + goRight - goUp);
        Vector3 BL = (-Vector3.forward - goRight - goUp);

        frustum.SetRow(0, TL);
        frustum.SetRow(1, TR);
        frustum.SetRow(2, BR);
        frustum.SetRow(3, BL);

        return frustum;

    }
}
