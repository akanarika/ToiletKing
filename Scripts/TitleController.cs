using UnityEngine;
using System.Collections;

public class TitleController : Singleton<TitleController> {

    public Pulser pulser;

    private bool _active;
    public bool active
    {
        get
        {
            return _active;
        }
        set
        {
            gameObject.SetActive(value);
            transform.localScale = Vector3.one;
            pulser.reset();
            _active = value;
        }

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
