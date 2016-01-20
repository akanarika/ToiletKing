using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {


    public GameObject M50;
    public GameObject A100;
    public GameObject A110;
    public GameObject A130;
    public GameObject A160;
    public GameObject A200;

    private float M50Scale = 0.7f;
    private float A100Scale = 0.8f;
    private float A110Scale = 0.9f;
    private float A130Scale = 1f;
    private float A160Scale = 1.2f;
    private float A200Scale = 1.5f;

    private float fontSize = Constants.ScoreSize;
    private GameObject activeElement;


    public void showScore(bool add, int combo) 
    {
        if (gameObject.activeSelf)
        {
            if (add)
            {
                if (combo == 0)
                {
                    fontSize = A100Scale * Constants.ScoreSize;
                    activeElement = A100;
                }
                else if (combo == 1)
                {
                    fontSize = A110Scale * Constants.ScoreSize;
                    activeElement = A110;
                }
                else if (combo == 2)
                {
                    fontSize = A130Scale * Constants.ScoreSize;
                    activeElement = A130;
                }
                else if (combo == 3)
                {
                    fontSize = A160Scale * Constants.ScoreSize;
                    activeElement = A160;
                }
                else
                {
                    fontSize = A200Scale * Constants.ScoreSize;
                    activeElement = A200;
                }
            }
            else
            {
                fontSize = M50Scale * Constants.ScoreSize;
                activeElement = M50;
            }
            M50.SetActive(false);
            A100.SetActive(false);
            A110.SetActive(false);
            A130.SetActive(false);
            A160.SetActive(false);
            A200.SetActive(false);
            activeElement.SetActive(true);
            StartCoroutine("changeSize");
        }
    }


    IEnumerator changeSize()
    {
        while (fontSize > 0f)
        {
            activeElement.transform.localScale = new Vector3(fontSize, fontSize, fontSize);
            fontSize -= 0.2f;
            yield return null;
        }
        activeElement.SetActive(false);
    }

    void setAllInactive()
    {

        M50.SetActive(false);
        A100.SetActive(false);
        A110.SetActive(false);
        A130.SetActive(false);
        A160.SetActive(false);
        A200.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        setAllInactive();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
