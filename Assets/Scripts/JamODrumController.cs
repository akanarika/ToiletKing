using UnityEngine;
using System.Collections;

public class JamODrumController : Singleton<JamODrumController> {
	
	public JamoDrum jod;
	
	public GameObject[] spinners = new GameObject[4];
	public Material[] starMaterials = new Material[4];
	public GameObject star;
	public float[] degPerTick = new float[4];
	public float[] spinnerAngle = new float[4];
    public float[] spinDelta = new float[4];
	private float[] initAngle = new float[4];
    private Player[] players = new Player[4];

	private bool once;

    void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            initAngle[i] = spinners[i].transform.rotation.eulerAngles.y;
            players[i] = spinners[i].GetComponent<Player>(); 

        }
       
            
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!once) {
			once = true;
			jod.AddHitEvent(HitHandler);
			jod.AddSpinEvent(SpinHandler);
		}
		for(int i=0; i<4; i++) {
            spinDelta[i] = jod.spinDelta[i];
			//spin
			if(Mathf.Abs(jod.spinDelta[i]) > 0) {
				spinnerAngle[i] += spinDelta[i] * degPerTick[i];
				//spinnerAngle[i] += jod.spinDelta[i];
				spinnerAngle[i] = Mathf.Repeat(spinnerAngle[i], 360);
				Vector3 rot = spinners[i].transform.rotation.eulerAngles;
				rot.y = initAngle[i] + spinnerAngle[i];
				spinners[i].transform.rotation = Quaternion.Euler(rot);
			}
			//hit
			/*if(jod.hit[i]) {
				GameObject starInst = (GameObject)Instantiate(star);
				starInst.GetComponent<Renderer>().material = starMaterials[i];
				switch (i){
				case 0:
					starInst.transform.position = new Vector3(-5, 35, 5);
					break;
				case 1:
					starInst.transform.position = new Vector3(5, 35, 5);
					break;
				case 2:
					starInst.transform.position = new Vector3(5, 35, -5);
					break;
				case 3:
					starInst.transform.position = new Vector3(-5, 35, -5);
					break;
				}
			}*/
		}
		
		if(Input.GetKeyUp(KeyCode.Escape)){
			Application.Quit();
		}
	}
	
	public void SpinHandler(int controllerID, int delta) {
        if (players[controllerID - 1] != null)
        {
            players[controllerID - 1].spin(delta);
        }
		Debug.Log("SPIN EVENT "+(controllerID-1));
	}
		
	public void HitHandler(int controllerID) {
        if (players[controllerID - 1] != null)
        {
            players[controllerID - 1].hit();
        }
		Debug.Log("HIT EVENT "+(controllerID-1));
	}
}
