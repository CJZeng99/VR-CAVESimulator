using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Attach this script to an empty gameObject. Create a secondary camera 
/// gameObject for offscreen rendering (not your main camera) and connect it 
/// with this script. Offscreen camera should have a texture object attached to it.  
/// OffscreenCamera texture object is used for rendering (please see camera properties). 
/// </summary> 
public class OffscreenRendering : MonoBehaviour
{
	#region public members 	
	/// <summary> 	
	/// The desired number of screenshots per second. 	
	/// </summary> 	
	[Tooltip("Number of screenshots per second.")]
	public int ScreenshotsPerSecond;
	/// <summary> 	
	/// Camera used to render screen to texture. Offscreen camera 	
	/// with desired target texture size should be attached here, 	
	/// not the main camera. 	
	/// </summary> 	
	[Tooltip("The camera that is used for off-screen rendering.")]

	public Camera UserCameraLeft;
	public Camera UserCameraRight;
	public Camera HandCameraLeft;
	public Camera HandCameraRight;

	public GameObject LeftPlane;
	public GameObject RightPlane;
	public GameObject BottomPlane;
	public RenderTexture LeftPlaneLeftTexture;
	public RenderTexture RightPlaneLeftTexture;
	public RenderTexture BottomPlaneLeftTexture;
	public RenderTexture LeftPlaneRightTexture;
	public RenderTexture RightPlaneRightTexture;
	public RenderTexture BottomPlaneRightTexture;

	public LineRenderer LeftEyeLeftPlaneLT;
	public LineRenderer LeftEyeLeftPlaneLB;
	public LineRenderer LeftEyeLeftPlaneRT;
	public LineRenderer LeftEyeLeftPlaneRB;
	public LineRenderer LeftEyeRightPlaneLT;
	public LineRenderer	LeftEyeRightPlaneLB;
	public LineRenderer	LeftEyeRightPlaneRT;
	public LineRenderer	LeftEyeRightPlaneRB;
	public LineRenderer LeftEyeBottomPlaneLT;
	public LineRenderer	LeftEyeBottomPlaneLB;
	public LineRenderer	LeftEyeBottomPlaneRT;
	public LineRenderer	LeftEyeBottomPlaneRB;
	public LineRenderer RightEyeLeftPlaneLT;
	public LineRenderer RightEyeLeftPlaneLB;
	public LineRenderer RightEyeLeftPlaneRT;
	public LineRenderer RightEyeLeftPlaneRB;
	public LineRenderer RightEyeRightPlaneLT;
	public LineRenderer RightEyeRightPlaneLB;
	public LineRenderer RightEyeRightPlaneRT;
	public LineRenderer RightEyeRightPlaneRB;
	public LineRenderer RightEyeBottomPlaneLT;
	public LineRenderer RightEyeBottomPlaneLB;
	public LineRenderer RightEyeBottomPlaneRT;
	public LineRenderer RightEyeBottomPlaneRB;

	#endregion
	/// <summary> 	
	/// Keep track of saved frames. 	
	/// counter is added as postifx to file names. 	
	/// </summary>

	private Camera OffscreenCameraLeft;
	private Camera OffscreenCameraRight;

	private GameObject[] plane;
	private float[] w = new float[3];
	private float[] h = new float[3];
	private Vector3[] pa = new Vector3[3];
	private	Vector3[] pb = new Vector3[3];
	private	Vector3[] pc = new Vector3[3];
	private Vector3[] pd = new Vector3[3];

	private bool debug;

	// Use this for initialization 	
	void Start()
	{
		StartCoroutine("RenderFrames");

		plane = new GameObject[] {LeftPlane, RightPlane, BottomPlane};

		for (int i = 0; i < 3; i++)
		{
			w[i] = plane[i].GetComponent<MeshFilter>().mesh.bounds.extents.x * 2;
			h[i] = plane[i].GetComponent<MeshFilter>().mesh.bounds.extents.z * 2;

			pa[i] = plane[i].transform.TransformPoint(plane[i].GetComponent<MeshFilter>().mesh.bounds.max);
			pb[i] = plane[i].transform.TransformPoint(plane[i].GetComponent<MeshFilter>().mesh.bounds.max - new Vector3(w[i], 0f, 0f));
			pc[i] = plane[i].transform.TransformPoint(plane[i].GetComponent<MeshFilter>().mesh.bounds.max - new Vector3(0f, 0f, h[i]));
			pd[i] = plane[i].transform.TransformPoint(plane[i].GetComponent<MeshFilter>().mesh.bounds.min);
		}

		LeftEyeLeftPlaneLT.SetPosition(1, pc[0]);
		LeftEyeLeftPlaneLB.SetPosition(1, pa[0]);
		LeftEyeLeftPlaneRT.SetPosition(1, pd[0]);
		LeftEyeLeftPlaneRB.SetPosition(1, pb[0]);
		RightEyeLeftPlaneLT.SetPosition(1, pc[0]);
		RightEyeLeftPlaneLB.SetPosition(1, pa[0]);
		RightEyeLeftPlaneRT.SetPosition(1, pd[0]);
		RightEyeLeftPlaneRB.SetPosition(1, pb[0]);
		LeftEyeRightPlaneLT.SetPosition(1, pc[1]);
		LeftEyeRightPlaneLB.SetPosition(1, pa[1]);
		LeftEyeRightPlaneRT.SetPosition(1, pd[1]);
		LeftEyeRightPlaneRB.SetPosition(1, pb[1]);
		RightEyeRightPlaneLT.SetPosition(1, pc[1]);
		RightEyeRightPlaneLB.SetPosition(1, pa[1]);
		RightEyeRightPlaneRT.SetPosition(1, pd[1]);
		RightEyeRightPlaneRB.SetPosition(1, pb[1]);
		LeftEyeBottomPlaneLT.SetPosition(1, pc[2]);
		LeftEyeBottomPlaneLB.SetPosition(1, pa[2]);
		LeftEyeBottomPlaneRT.SetPosition(1, pd[2]);
		LeftEyeBottomPlaneRB.SetPosition(1, pb[2]);
		RightEyeBottomPlaneLT.SetPosition(1, pc[2]);
		RightEyeBottomPlaneLB.SetPosition(1, pa[2]);
		RightEyeBottomPlaneRT.SetPosition(1, pd[2]);
		RightEyeBottomPlaneRB.SetPosition(1, pb[2]);
	}

	void Update()
    {
		if(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.5f)
        {
			OffscreenCameraLeft = HandCameraLeft;
			OffscreenCameraRight = HandCameraRight;

			debug = OVRInput.Get(OVRInput.Button.One);
		}
		else
        {
			OffscreenCameraLeft = UserCameraLeft;
			OffscreenCameraRight = UserCameraRight;

			debug = false;
        }

		LeftEyeLeftPlaneLT.enabled = debug;
		LeftEyeLeftPlaneLB.enabled = debug;
		LeftEyeLeftPlaneRT.enabled = debug;
		LeftEyeLeftPlaneRB.enabled = debug;
		LeftEyeRightPlaneLT.enabled = debug;
		LeftEyeRightPlaneLB.enabled = debug;
		LeftEyeRightPlaneRT.enabled = debug;
		LeftEyeRightPlaneRB.enabled = debug;
		LeftEyeBottomPlaneLT.enabled = debug;
		LeftEyeBottomPlaneLB.enabled = debug;
		LeftEyeBottomPlaneRT.enabled = debug;
		LeftEyeBottomPlaneRB.enabled = debug;
		RightEyeLeftPlaneLT.enabled = debug;
		RightEyeLeftPlaneLB.enabled = debug;
		RightEyeLeftPlaneRT.enabled = debug;
		RightEyeLeftPlaneRB.enabled = debug;
		RightEyeRightPlaneLT.enabled = debug;
		RightEyeRightPlaneLB.enabled = debug;
		RightEyeRightPlaneRT.enabled = debug;
		RightEyeRightPlaneRB.enabled = debug;
		RightEyeBottomPlaneLT.enabled = debug;
		RightEyeBottomPlaneLB.enabled = debug;
		RightEyeBottomPlaneRT.enabled = debug;
		RightEyeBottomPlaneRB.enabled = debug;

		LeftEyeLeftPlaneLT.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeLeftPlaneLB.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeLeftPlaneRT.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeLeftPlaneRB.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeRightPlaneLT.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeRightPlaneLB.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeRightPlaneRT.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeRightPlaneRB.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeBottomPlaneLT.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeBottomPlaneLB.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeBottomPlaneRT.SetPosition(0, HandCameraLeft.transform.position);
		LeftEyeBottomPlaneRB.SetPosition(0, HandCameraLeft.transform.position);
		RightEyeLeftPlaneLT.SetPosition(0, HandCameraRight.transform.position);
		RightEyeLeftPlaneLB.SetPosition(0, HandCameraRight.transform.position);
		RightEyeLeftPlaneRT.SetPosition(0, HandCameraRight.transform.position);
		RightEyeLeftPlaneRB.SetPosition(0, HandCameraRight.transform.position);
		RightEyeRightPlaneLT.SetPosition(0, HandCameraRight.transform.position);
		RightEyeRightPlaneLB.SetPosition(0, HandCameraRight.transform.position);
		RightEyeRightPlaneRT.SetPosition(0, HandCameraRight.transform.position);
		RightEyeRightPlaneRB.SetPosition(0, HandCameraRight.transform.position);
		RightEyeBottomPlaneLT.SetPosition(0, HandCameraRight.transform.position);
		RightEyeBottomPlaneLB.SetPosition(0, HandCameraRight.transform.position);
		RightEyeBottomPlaneRT.SetPosition(0, HandCameraRight.transform.position);
		RightEyeBottomPlaneRB.SetPosition(0, HandCameraRight.transform.position);
	}

	/// <summary>     
	/// Captures x frames per second.      
	/// </summary>     
	/// <returns>Enumerator object</returns>     
	IEnumerator RenderFrames()
	{
		while (true)
		{
			yield return new WaitForEndOfFrame();
			// Remember currently active render texture. 			
			RenderTexture currentRT = RenderTexture.active;

			render(LeftPlaneLeftTexture, OffscreenCameraLeft, 0);
			render(RightPlaneLeftTexture, OffscreenCameraLeft, 1);
			render(BottomPlaneLeftTexture, OffscreenCameraLeft, 2);
			render(LeftPlaneRightTexture, OffscreenCameraRight, 0);
			render(RightPlaneRightTexture, OffscreenCameraRight, 1);
			render(BottomPlaneRightTexture, OffscreenCameraRight, 2);

			// Reset previous render texture. 			
			RenderTexture.active = currentRT;

			yield return new WaitForSeconds(1.0f / ScreenshotsPerSecond);
		}
	}

	/// <summary>     
	/// Stop image capture.     
	/// </summary>     
	public void StopRendering()
	{
		StopCoroutine("RenderFrames");
	}

	/// <summary> 	
	/// Resume image capture. 	
	/// </summary> 	
	public void ResumeRendering()
	{
		StartCoroutine("RenderFrames");
	}

	private void render(RenderTexture texture, Camera camera, int index)
    {
		// Set target texture as active render texture. 			
		RenderTexture.active = texture;
		camera.targetTexture = texture;
		// Compute projection matrix
		camera.projectionMatrix = computeProjection(camera, index);
		// Render to texture		
		camera.Render();
		// Read offscreen texture 			
		Texture2D offscreenTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
		offscreenTexture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0, false);
		offscreenTexture.Apply();

		UnityEngine.Object.Destroy(offscreenTexture);
	}

	private Matrix4x4 computeProjection(Camera eye, int index)
    {
		Vector3 pa_this = pa[index];
		Vector3 pb_this = pb[index];
		Vector3 pc_this = pc[index];
		Vector3 pe = eye.transform.position;
		Vector3 va = pa_this - pe;
		Vector3 vb = pb_this - pe;
		Vector3 vc = pc_this - pe;
		Vector3 vr = Vector3.Normalize(pb_this - pa_this);
		Vector3 vu = Vector3.Normalize(pc_this - pa_this);
		Vector3 vn = Vector3.Normalize(Vector3.Cross(vr, vu));
		float n = eye.nearClipPlane;
		float f = eye.farClipPlane;
		float d = -Vector3.Dot(vn, va);
		float l = Vector3.Dot(vr, va) * n / d;
		float r = Vector3.Dot(vr, vb) * n / d;
		float b = Vector3.Dot(vu, va) * n / d;
		float t = Vector3.Dot(vu, vc) * n / d;
		Matrix4x4 P = Matrix4x4.Frustum(l, r, b, t, n, f);

		Matrix4x4 M_T = Matrix4x4.identity;
		M_T.SetColumn(0, new Vector4(-vr.x, -vu.x, -vn.x, 0f));
		M_T.SetColumn(1, new Vector4(-vr.y, -vu.y, -vn.y, 0f));
		M_T.SetColumn(2, new Vector4(vr.z, vu.z, vn.z, 0f));

		Matrix4x4 T = Matrix4x4.identity;
		T.SetColumn(3, new Vector4(-pe.x, -pe.y, -pe.z, 1f));

		return P * M_T * T;
	}
}