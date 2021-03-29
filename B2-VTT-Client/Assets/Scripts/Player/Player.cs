using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
	public float	moveSpeed	= 25.0f;
	public float	rotateSpeed	= 20.0f;
	public float	scrollSpeed	= 20.0f;

	private Camera	mainCamera;

    public override void OnStartLocalPlayer()
	{
		mainCamera = Camera.main;
		mainCamera.transform.SetParent(transform);
		mainCamera.transform.position		= transform.position;
		mainCamera.transform.eulerAngles	= new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z);
	}

	private void Update()
	{
		if (!isLocalPlayer)
			return;

		float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		float moveY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

		transform.Translate(transform.right * moveX, Space.World);
		transform.Translate(transform.forward * moveY, Space.World);

		if (Input.GetMouseButton(2))
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, 0);
			mainCamera.transform.Rotate(Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime * -1, 0, 0);
		}

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

		if (Input.mouseScrollDelta.y != 0)
		{
			transform.Translate(mainCamera.transform.forward * Input.mouseScrollDelta.y * scrollSpeed * Time.deltaTime, Space.World);
		}
	}
}
