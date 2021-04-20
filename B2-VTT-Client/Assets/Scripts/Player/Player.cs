using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
	[Header("Speeds")]
	public	float	moveSpeed	= 25.0f;
	public	float	rotateSpeedHorizontal	= 20.0f;
	public	float	rotateSpeedVertical		= 500.0f;
	public	float	scrollSpeed	= 20.0f;

	[Header("Max Rotation")]
	public	float	minimumBound	= -80;
	public	float	maximumBound	= 80;

	private Camera	mainCamera;

    public override void OnStartLocalPlayer()
	{
		mainCamera = Camera.main;
		mainCamera.transform.SetParent(transform);
		mainCamera.transform.position		= transform.position;
		mainCamera.transform.eulerAngles	= new Vector3(60, transform.eulerAngles.y, transform.eulerAngles.z);
	}

	private void Update()
	{
		if (!isLocalPlayer)
			return;

		float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		float moveY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

		transform.Translate(transform.right * moveX, Space.World);
		transform.Translate(transform.forward * moveY, Space.World);

		if (Input.GetMouseButtonDown(2))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else if (Input.GetMouseButtonUp(2))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		if (Input.GetMouseButton(2))
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeedHorizontal, 0);

			Vector3 newCameraEulerAngles = mainCamera.transform.localEulerAngles;
			newCameraEulerAngles.x -= Input.GetAxis("Mouse Y") * rotateSpeedVertical * Time.deltaTime;
			newCameraEulerAngles.x = Mathf.Clamp(newCameraEulerAngles.x, minimumBound, maximumBound);
			mainCamera.transform.localEulerAngles = newCameraEulerAngles;
		}

		if (Input.mouseScrollDelta.y != 0)
		{
			transform.Translate(mainCamera.transform.forward * Input.mouseScrollDelta.y * scrollSpeed * Time.deltaTime, Space.World);
		}
	}
}
