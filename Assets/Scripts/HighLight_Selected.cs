using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighLight_Selected : MonoBehaviour {

    public float currentSize;
    public Text DisplayText;
    //For the big display one
    public GameObject Big_Parent_RelatedObj;
    //For UI parents
    public GameObject Parent_RelatedObj;
    public Material selected;
    private Material[] defaultMaterials;
    //For the big display one materials
    private Material[] BP_defaultMaterials;
    //For UI parents materials
    private Material[] P_defaultMaterials;
    private Color original;

    // Use this for initialization
    void Start () {
        original = GetComponent<Renderer>().material.GetColor("_EmissionColor");

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        transform.localScale = new Vector3(1.2f,1.2f,1.2f);
        DisplayText.text = name;
        defaultMaterials = GetComponent<Renderer>().materials;
        P_defaultMaterials = Parent_RelatedObj.GetComponent<Renderer>().materials;
        BP_defaultMaterials = Big_Parent_RelatedObj.GetComponent<Renderer>().materials;
        for (int i = 0; i < P_defaultMaterials.Length; i++)
        {
            P_defaultMaterials[i].SetColor("_EmissionColor", new Color(0.9f, 0.7f, 0f));
            BP_defaultMaterials[i].SetColor("_EmissionColor", new Color(0.9f, 0.7f, 0f));
        }

        

        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i].SetColor("_EmissionColor", new Color(0.9f,0.7f,0f));
        }


        //Renderer rend = GetComponent<Renderer>();
        //rend.material.SetColor("_EmissionColor",Color.yellow);
        Debug.Log("I'm Hit");
    }

    void OnCollisionStay(Collision collision)
    {
        defaultMaterials = GetComponent<Renderer>().materials;
        P_defaultMaterials = Parent_RelatedObj.GetComponent<Renderer>().materials;
        BP_defaultMaterials = Big_Parent_RelatedObj.GetComponent<Renderer>().materials;
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        DisplayText.text = name;

        for (int i = 0; i < P_defaultMaterials.Length; i++)
        {
            P_defaultMaterials[i].SetColor("_EmissionColor", new Color(0.9f, 0.7f, 0f));
            BP_defaultMaterials[i].SetColor("_EmissionColor", new Color(0.9f, 0.7f, 0f));
        }

        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i].SetColor("_EmissionColor", new Color(0.9f, 0.7f, 0f));
        }

        //Renderer rend = GetComponent<Renderer>();
        //rend.material.SetColor("_EmissionColor", Color.yellow);
    }

    void OnCollisionExit(Collision collision)
    {
        transform.localScale = new Vector3(currentSize, currentSize, currentSize);
        DisplayText.text = "";
        defaultMaterials = GetComponent<Renderer>().materials;
        P_defaultMaterials = Parent_RelatedObj.GetComponent<Renderer>().materials;
        BP_defaultMaterials = Big_Parent_RelatedObj.GetComponent<Renderer>().materials;
        for (int i = 0; i < P_defaultMaterials.Length; i++)
        {
            P_defaultMaterials[i].SetColor("_EmissionColor", original);
            BP_defaultMaterials[i].SetColor("_EmissionColor", original);
        }

        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i].SetColor("_EmissionColor", original);
        }

        //Renderer rend = GetComponent<Renderer>();
        //rend.material.SetColor("_EmissionColor", Color.blue);
    }
}
