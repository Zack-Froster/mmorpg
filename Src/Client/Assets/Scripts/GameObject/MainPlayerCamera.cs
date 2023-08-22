using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera> {

	public Camera camera;
	public Transform viewPoint;

	public GameObject player;


	private void LateUpdate()
    {

		if (player == null && User.Instance.CurrentPlayerInputController != null)
        {
			player = User.Instance.CurrentPlayerInputController.gameObject;
        }
		if(player != null)
        {
			this.transform.position = player.transform.position;
			this.transform.rotation = player.transform.rotation;
        }
    }
}
