using UnityEngine;
using System.Collections;

public class Show_secondname : MonoBehaviour {

    public GameObject Second;

	// Use this for initialization
	void Start () {
        Second.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void GazeEntered()
    {
        Second.SetActive(true);
    }

    void GazeExited()
    {
        Second.SetActive(false);
    }
}
