/*
Used to determine what carousel object is in the SelectSphere Trigger, and Display it in the UI
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Collider))]
public class SelectSphere : MonoBehaviour 
{
	public GameObject SelectedObject;
	public Text text;

	void OnTriggerStay(Collider other) 
	{
		SelectedObject = other.gameObject;
		text.text = SelectedObject.name;
	}

}
