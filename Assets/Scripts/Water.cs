using UnityEngine;
using System.Collections;

public class Water : Singleton<Water> {

    public float driftForce { get; private set; }
    public Vector2 driftDirection { get; private set; }
    private Vector2 _newDriftDirection;

	public Vector2 currentDirection;

	private float changeRate;
	private float time;
    void Awake()
    {
        changeRate = Mode.Normal.WaterDirectionChangeRate;
        updateWaterDirection();
    }

	// Use this for initialization
	void Start () {

	}

    public void updateWaterDirection()
    {
        float dx = (1.0f * Random.Range(0, 1000) - 500) / 500;
        float dz = (1.0f * Random.Range(0, 1000) - 500) / 500;
        _newDriftDirection = new Vector2(dx, dz).normalized;
    }

	// Update is called once per frame
	void Update () {
		if(time>changeRate*10f){
			driftDirection = _newDriftDirection;
			updateWaterDirection();
			time=0;
		}
		currentDirection = Vector2.Lerp(driftDirection, _newDriftDirection, changeRate * time/*Time.deltaTime*/);
		time+=Time.deltaTime;
	}
}
