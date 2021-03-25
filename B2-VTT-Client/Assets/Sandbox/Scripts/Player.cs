using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
	{
		Camera.main.transform.SetParent(transform);
		Camera.main.transform.localPosition	= transform.position;
		Camera.main.transform.rotation		= transform.rotation;
	}
}
