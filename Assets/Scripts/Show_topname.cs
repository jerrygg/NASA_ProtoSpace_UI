using UnityEngine;
using System.Collections;

public class Show_topname : MonoBehaviour {

    public GameObject Top;

	// Use this for initialization
	void Start () {
        Top.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GazeEntered()
    {
        Top.SetActive(true);
    }

    void GazeExited()
    {
        Top.SetActive(false);
    }
}
