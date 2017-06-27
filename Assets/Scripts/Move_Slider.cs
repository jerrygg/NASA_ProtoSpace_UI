using UnityEngine;
using System.Collections;

public class Move_Slider : MonoBehaviour {

    public Vector3 startPos;
    public float increaseAmount;

    public float MoveSensitivity = 10f;
    // Use this for initialization
    void Start () {
        startPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
        MoveS();

        Vector3 tempPos2 = transform.position;
        if (increaseAmount > 0.5f )
        {
            transform.position = startPos;
            increaseAmount = 0f;
        }
        

    }

    private void MoveS()
    {
        if (GestureManager.Instance.IsNavigating )
        {
            /* TODO: DEVELOPER CODING EXERCISE 2.c */

            // 2.c: Calculate rotationFactor based on GestureManager's NavigationPosition.X and multiply by RotationSensitivity.
            // This will help control the amount of rotation.
            //rotationFactor = GestureManager.Instance.NavigationPosition.x * RotationSensitivity;

            Vector3 tempPos = transform.position;
            tempPos.x += GestureManager.Instance.NavigationPosition.x / MoveSensitivity;
            increaseAmount += GestureManager.Instance.NavigationPosition.x / MoveSensitivity;
            transform.position = tempPos;
        }
    }
}
