using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {


	public Collider minimapBoundingBox;
	// Use this for initialization
	void Start () {
		MinimapManager.Instance.UpdateMinimap(minimapBoundingBox);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
