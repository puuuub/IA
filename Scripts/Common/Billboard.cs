using UnityEngine;

public class Billboard : MonoBehaviour
{
	private Vector3 _vAxis = Vector3.up;
	private Vector3 vLook;                  // constructed billboard forward vector
	private Vector3 vRight;                 // constructed billboard right vector
	private Vector3 vUp;                    // constructed billboard up vector

	private Transform billboardTransform;   // link to billboard object transform
	private Transform cameraTransform;      // link to camera object transform

	public bool IsReverseObject = true;
	void Awake()
	{
		billboardTransform = this.transform;
		cameraTransform = Camera.main.transform;
	}

	void Update()
	{
		if (!IsReverseObject)
		{
			vLook = billboardTransform.position - cameraTransform.position;
		}
		else
		{
			vLook = cameraTransform.position - billboardTransform.position;
		}
		vLook.Normalize();

		if (Camera.main.GetComponent<Camera>().orthographic == true)
		{
			billboardTransform.forward = cameraTransform.forward;
		}
		else
		{
			float visible = Mathf.Abs(Vector3.Dot(_vAxis, vLook));
			if (visible >= 1)
			{
				vLook = _vAxis;
			}
			else
			{
				vRight = Vector3.Cross(_vAxis, vLook);
				vRight.Normalize();
				vLook = Vector3.Cross(vRight, _vAxis);
				vUp = Vector3.Cross(vLook, vRight);
				billboardTransform.rotation = Quaternion.LookRotation(vLook, vUp);
			}
		}
	}
}

