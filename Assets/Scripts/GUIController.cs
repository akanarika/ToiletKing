using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {

    public TextMesh textSource;


    private float enlargeSize = Constants.enlargeTextSize;
    private float fontSize;

    public GameObject hitpad;

    public ScoreController scorePad;

    public void showScore(bool add, int combo)
    {
        if (scorePad.isActiveAndEnabled)
        {
            scorePad.showScore(add, combo);
        }
    }

    public void setText(string text, bool joined)
    {
        if (joined)
        {
            hitpad.SetActive(false);
            fontSize = enlargeSize;
            textSource.text = text;
            StartCoroutine("changeText");
        }
        else
        {
            hitpad.SetActive(true);
            textSource.text = "";

        }
    }

    IEnumerator changeText()
    {
        while (fontSize > 1.0f)
        {
            textSource.transform.localScale = new Vector3(fontSize,fontSize,fontSize);
            fontSize -= (fontSize - 0.5f) / 10;
            yield return null;
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
