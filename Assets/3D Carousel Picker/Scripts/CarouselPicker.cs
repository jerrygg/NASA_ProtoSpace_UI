/*
This script should be added to the Parent objects,
All objects in the CarouselPicker will be children of this object.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarouselPicker : MonoBehaviour {

	//<summary>
	//Radius of the Carousel
	//</summary>
	public float radius = 1f; 

	//<summary>
	//Speed at switch the Carousel will rotate during swiping
	//</summary>
    public float rotateSpeed = 1f;

	//<summary>
	//Speed at switch the Carousel will rotate after swiping
	//</summary>
    public float snapSpeed = 1f;
    
	//<summary>
	//Weather or not objects will lerp into position or not
	//</summary>
	public bool lerpObjects =  true;

	//<summary>
	//the speed in which objects move (if lerpObjects is true)
	//</summary>
    public float objectSpeed = 5f;

	//<summary>
	//this number is used to how much the carousel should rotate 
	//</summary>
    public int rotationIndex = 0;

	//<summary>
	//how much the carousel's rotation show be offset.
	//</summary>
    public float zRotationOffset = 180f;

	//<summary>
	//If you are not going to override the CarouselPicker or use buttons to control it, then you should set this value to true.
	//This will enable full screen swiping.
	//</summary>
    public bool allowSwipe = false;

	public enum swipingTypes {Vertical, Horizontal};

	//<summary>
	//use to determine what direction of swiping will change the carousel.
	//</summary>
    public swipingTypes swipingType = swipingTypes.Horizontal;

	//<summary>
	//this will inverse the swiping direction, if true.
	//</summary>
    public bool invertSwipe = false;

	[HideInInspector]
	//<summary>
	//this is used to determine if the user is swiping, or not.
	//</summary>
	public bool swiping = false;

	//<summary>
	//this is a multiplier to change the distance swiped into an angle of how much the carousel should turn.
	//</summary>
    public float swipe2Angle = 1f;


	//private variables
	private float anglePart = 0f; //360 divided by the number of carousel Objects
	private int objectCount; //number of carousel Objects
	private List<GameObject> objectList = new List<GameObject>(); //List of all carousel Objects
    private bool swipeToProcess = false; //used to determine if a swipe has ended and needs to be processed
	private Vector2 swipeStartPos; //the location of where the swipe begain
	private Vector2 swipeEndPos; //the location of where the swipe ended


	// Use this for initialization
	void Awake () 
	{
		//used to set some variables and get a list of all the carousel Objects.
		RefreshList();
	}

	
	// Update is called once per frame
	void Update () 
	{
		//refresh the list of carousel objects (if needed)
		RefreshList();

		//process swipes if needed
		SwipeManager();

		//rotate the carousel based on the rotationIndex
		Vector3 R = gameObject.transform.localRotation.eulerAngles;
		float swipeOffset = swiping? getSwipeOffset() :0f;
		float speed = swiping? rotateSpeed:snapSpeed;
		gameObject.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(R),Quaternion.Euler(R.x,R.y,zRotationOffset + ( anglePart * rotationIndex) + swipeOffset ), speed * Time.deltaTime); 

		//move objects in the carousel to their proper position
		if (lerpObjects)
		{
			int i = 0;
			foreach(GameObject Obj in objectList)
			{
				Vector3 NewPos = new Vector3(Mathf.Sin(anglePart * Mathf.Deg2Rad * i) * radius,Mathf.Cos(anglePart * Mathf.Deg2Rad  * i) * radius,0f);
				Obj.transform.localPosition = Vector3.Lerp (Obj.transform.localPosition, NewPos, objectSpeed * Time.deltaTime);
				i += 1;
			}
		}
		else
		{
			int i = 0;
			foreach(GameObject Obj in objectList)
			{
				Obj.transform.localPosition = new Vector3(Mathf.Sin(anglePart * Mathf.Deg2Rad * i) * radius,Mathf.Cos(anglePart * Mathf.Deg2Rad  * i) * radius,0f);
				i += 1;
			}
		}
	}

	private void SwipeManager()
	{
		//don't process swipe if allowSwipe is false
		if (!allowSwipe)
		{
			return;
		}

		//process swipe using mouse, if in OSXEditor or WindowsEditor
		//this can be adjusted if need be
		if (
			Application.platform == RuntimePlatform.OSXEditor
			||
			Application.platform == RuntimePlatform.WindowsEditor
			)
		{
			if (Input.GetMouseButtonDown(0))
			{				
				StartSwipe(Input.mousePosition);
			}

			if (Input.GetMouseButton(0))
			{				
				ContinueSwipe(Input.mousePosition);
			}
			
			if (Input.GetMouseButtonUp(0))
			{
				EndSwipe(Input.mousePosition);
			}
		}
		//if not in Editor process swipes using Touches 
		else 
		{
			Touch[] Touches = Input.touches;
			
			if (Touches.Length >= 1)
			{
				if (Touches[0].phase == TouchPhase.Began )
				{
					StartSwipe(Touches[0].position);
				}

				if (Touches[0].phase == TouchPhase.Moved 
				    || Touches[0].phase == TouchPhase.Stationary 
				    )
				{
					ContinueSwipe(Touches[0].position);
				}

				if (Touches[0].phase == TouchPhase.Ended 
				    || Touches[0].phase == TouchPhase.Canceled 
				    )
				{
					EndSwipe(Touches[0].position);
				}
			}

		}

		// Change rotationIndex based on swipe
		if (swipeToProcess)
		{
//			print ("+" + Mathf.RoundToInt( getSwipeOffset() / anglePart).ToString());
			
			rotationIndex += (int)( Mathf.RoundToInt( getSwipeOffset() / anglePart)  );
			swipeToProcess = false;
		}

	}

	//this SwipeManager is used for overriding the carousel, see OverrideCarouselPicker.cs for an example of how to use this
	public enum SwipeStates {Start = 0,Continue = 1,End = 2}
	public void SwipeManager(SwipeStates swipeState, Vector2 inputPos)
	{
		if (swipeState == SwipeStates.Start)
		{
			StartSwipe(inputPos);
		}

		if (swipeState == SwipeStates.Continue)
		{
			ContinueSwipe(inputPos);
		}

		if (swipeState == SwipeStates.End)
		{
			EndSwipe(inputPos);
		}

		if (swipeToProcess)
		{
//			print ("+" + Mathf.RoundToInt( getSwipeOffset() / anglePart).ToString());
			
			rotationIndex += (int)( Mathf.RoundToInt( getSwipeOffset() / anglePart)  );
			swipeToProcess = false;
		}
	}
	
	//used during the start of a swipe
	private void StartSwipe(Vector2 inputPos)
	{
		swipeStartPos =  inputPos; 
		swiping = true;

		swipeEndPos =  inputPos; 
	}

	//used during the swipe
	private void ContinueSwipe(Vector2 inputPos)
	{
		swipeEndPos =  inputPos; 
	}

	//used at the End of a swipe
	private void EndSwipe(Vector2 inputPos)
	{
		swipeEndPos =  inputPos; 
		
		swipeToProcess = true;
		swiping = false;
	}

	//used to calculate how much the swipe should turn the carousel
	private float getSwipeOffset()
	{
		float invert = invertSwipe ? -1f: 1f;

		if (swipingType == swipingTypes.Horizontal)
		{
			return ((swipeEndPos.x - swipeStartPos.x) * swipe2Angle * invert);
		}
		else
		{
			return ((swipeEndPos.y - swipeStartPos.y) * swipe2Angle * invert);
		}
	}

	//used to change the rotationIndex
	public void ChangeRotationIndex(int Delta)
	{
		rotationIndex += Delta;
	}

	//used to change the rotationIndex
	public void AddRotationIndex()
	{
		rotationIndex += 1;
	}

	//used to change the rotationIndex
	public void SubtractRotationIndex()
	{
		rotationIndex -= 1;
	}

	//used to refresh list of carousel Objects and other key variables
	public void RefreshList()
	{
		if (objectCount != gameObject.transform.childCount)
		{
			objectCount = gameObject.transform.childCount;

			anglePart = 360f/objectCount;

			objectList.Clear();

			foreach (Transform child  in transform)
			{
				objectList.Add (child.gameObject);
			}
		}
	}
}
