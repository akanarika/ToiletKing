using UnityEngine;
using System.Collections;

public class EndFrog : MonoBehaviour {

    public GameObject frog1;
    public GameObject frog2;
    public GameObject frog3;
    public GameObject frog4;

    public TextMesh text;

    public void showFrog(int frog, int amount) 
    {
        frog1.SetActive(false);
        frog2.SetActive(false);
        frog3.SetActive(false);
        frog4.SetActive(false);
        switch (frog)
        {
            case 1:
                frog1.SetActive(true);
                text.text = amount.ToString();
                break;
            case 2:
                frog2.SetActive(true);
                text.text = amount.ToString();
                break;
            case 3:
                frog3.SetActive(true);
                text.text = amount.ToString();
                break;
            case 4:
                frog4.SetActive(true);
                text.text = amount.ToString();
                break;
            default:
                text.text = "";
                break;
        }
    }


}
