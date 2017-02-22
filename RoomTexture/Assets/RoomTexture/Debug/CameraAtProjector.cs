using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAtProjector : MonoBehaviour {

    public string roomLayerName = "RoomMeshLayer";
    public Vector4 FrustumTopRightCorner = new Vector4();

	// Use this for initialization
	void Start () {
        UpdateFrustumTopRight();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateFrustumTopRight();
	}

    void UpdateFrustumTopRight()
    {
        Projector projector = GetComponent<Projector>();
        if (projector == null)
            return;

        float FOV = projector.fieldOfView * 0.90f;

        Vector3 u = Vector3.Cross(projector.transform.up, projector.transform.forward);
        Vector3 upV = (Mathf.Tan(Mathf.Deg2Rad * FOV / 2.0f) * projector.transform.forward.normalized).magnitude * projector.transform.up.normalized;
        Vector3 rightU = (Mathf.Tan(Mathf.Deg2Rad * FOV / 2.0f * projector.aspectRatio) * projector.transform.forward.normalized).magnitude * u.normalized;//u.normalized;
        Ray frustumRay = new Ray(projector.transform.position, rightU + upV + projector.transform.forward);
        RaycastHit rayInfo;
        if (Physics.Raycast(frustumRay, out rayInfo, LayerMask.NameToLayer(roomLayerName)))
            FrustumTopRightCorner = rayInfo.point; //FrustumTopRightCorner = (rayInfo.point - projector.transform.position).normalized + projector.transform.position;
        else
            FrustumTopRightCorner = new Vector4(0, 0, 0, 0); //frustumRay.origin + frustumRay.direction * 10.0f;

        // ERROR TESTING COMMENT OUT
        Debug.DrawRay(projector.transform.position, rightU, Color.red);
        Debug.DrawRay(projector.transform.position, upV, Color.yellow);
        //Debug.DrawRay(projector.transform.position, rightU + upV + projector.transform.forward, Color.green);

        Debug.DrawRay(projector.transform.position, new Vector3(FrustumTopRightCorner.x, FrustumTopRightCorner.y, FrustumTopRightCorner.z) - projector.transform.position, Color.blue);

        RaycastHit rayHit;
        Physics.Raycast(new Ray(projector.transform.position, projector.transform.forward), out rayHit);
        FrustumTopRightCorner = rayHit.point;
    }

    void CameraUpdateFrustumTopRight() {
        Camera cam = GetComponent<Camera>();
        Ray frustumRay = cam.ViewportPointToRay(new Vector3(1, 1));
        RaycastHit rayInfo;
        if (Physics.Raycast(frustumRay, out rayInfo, LayerMask.NameToLayer(roomLayerName)))
            FrustumTopRightCorner = rayInfo.point;
        else
            FrustumTopRightCorner = frustumRay.origin + frustumRay.direction * 10.0f;
    }
}
