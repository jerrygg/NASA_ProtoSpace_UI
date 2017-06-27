 /*
This script is attached to the buttons to Rotate the Carousel
*/

using UnityEngine;
using System.Collections;

public class RotateCarouselPicker : MonoBehaviour {

	public CarouselPicker carouselPicker;
	public int rotateValue = 1;

	public void Rotate()
	{
		carouselPicker.ChangeRotationIndex(rotateValue);
	}

}
