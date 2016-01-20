using UnityEngine;
using System.Collections;

public class Lilypad : MonoBehaviour {

   	public bool PlayerPad;
	public bool isOnTable;
    public float posY = 0.2f;
    public float radius = 7f;
	private Water water;
    private float waterSpeedMultiplier = 15f;

    private bool _translatable;

    private Vector2 center;
    private Rigidbody rb;
    public Collider landingZone;
    public Collider standingZone;

    public GameObject waterShatter;
    public GameObject droplet;

    public Frog currentFrog { get; private set; }

    public float previousDropletTime;
    public float dropletCooldown;

    private float life;
    private float showupTime;

    public bool isPower;
    public GameObject pill;

    public float waterShatterTime;
    public float waterShatterDuration;

    void Awake()
    {
        waterShatterTime = Time.time;
        waterShatterDuration = Random.Range(Constants.waterDurationMin, Constants.waterDurationMax);
        previousDropletTime = Time.time;
        dropletCooldown = Random.Range(10, 20);
        rb = gameObject.GetComponent<Rigidbody>();
        _translatable = !PlayerPad;
        gameObject.layer = Constants.Layers.Lilypad;
        landingZone.gameObject.layer = Constants.Layers.LandingZone;
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        droplet.SetActive(false);
        waterShatter.SetActive(false);
        pill.SetActive(false);
    }

    public void trap() {
        droplet.SetActive(true);
        if (currentFrog != null)
        {
            currentFrog.trap();
        }
    }


    public void release()
    {
        droplet.SetActive(false);
    }

    public void removeFrog()
    {
        currentFrog = null;
    }

    public void putFrog(Frog frog)
    {
        if (currentFrog != null)
        {
            currentFrog.reset();
            currentFrog = frog;
        }
    }

	// Use this for initialization
	void Start ()
    {
        waterShatterTime = Time.time;
        rb.velocity = new Vector3(0, 0, 0);
        //water = LevelController.Instance.water;
        life = Random.Range(Constants.lilypadMinLife, Constants.lilypadMaxLife);
        showupTime = 0f;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if ((waterShatterTime + waterShatterDuration) < Time.time)
        {

            waterShatterDuration = Random.Range(Constants.waterDurationMin, Constants.waterDurationMax);
            waterShatterTime = Time.time;
            waterShatter.SetActive(!waterShatter.activeSelf);
        }


        if (currentFrog != null && (currentFrog.transform.position - transform.position).magnitude > Constants.PadDistance)
        {
            currentFrog = null;
        }
        if (droplet.activeSelf && currentFrog != null && !currentFrog.trapped)
        {
            currentFrog.trap();
        }

        if ((dropletCooldown + previousDropletTime) < Time.time)
        {
            previousDropletTime = Time.time;
            dropletCooldown = Random.Range(Constants.DropletMinCooldown, Constants.DropletMaxCooldown);
            trap();
        }

        if (droplet.activeSelf)
        {
            previousDropletTime = Time.time;
        }
        
		if(Vector3.Distance(transform.position,Vector3.zero)<40f/*table radius*/){
			isOnTable = true;
		}else{
			isOnTable = false;
            Destroy(this.gameObject);
		}
        showup();
		floatWithWater();
        wither();
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        //transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        if (isPower) {
            pill.SetActive(true);
        }
		
	}
    
	
	public void setPosition(Vector2 pos){
		transform.position = new Vector3(pos.x, posY, pos.y);
        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
        center = pos;
        
	}

    public Vector2 getCenter(){
        return center;
    }

	private void floatWithWater(){
        //Debug.Log(water.GetComponent<Water>().driftDirection);
        //this.gameObject.transform.position
        /*if (water != null)
        {
            rb.AddForce(water.currentDirection.toVector3() * waterSpeedMultiplier);  
			//Debug.Log ("floating");
        }*/
        if(water == null) water = this.gameObject.AddComponent<Water>();
        rb.AddForce(water.currentDirection.toVector3() * waterSpeedMultiplier);
	}

    private void showup() {
        showupTime += Time.deltaTime;
        if(showupTime<1f) this.transform.Find("Green").GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(255, 255, 255, 0), Color.white, showupTime);
    }

    private void wither(){
        life -= Time.deltaTime;
        if (life < 0 && life >= -2f)
        {
            this.transform.Find("Green").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(255, 255, 255, 0), -life / 2f);
            this.transform.Find("Old").GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(255, 255, 255, 0), Color.white, -life / 2f);
        }
        else if (life < -2f && life >= -4f)
        {
            this.transform.Find("Old").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(255, 255, 255, 0), -life / 3f - 1f);
            this.transform.Find("Oldest").GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(255, 255, 255, 0), Color.white, (-life / 2f - 1f));
        }
        else if (life < -4f) {
            this.transform.Find("Old").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(255, 255, 255, 0), -life / 3f - 1f);
            this.transform.Find("Oldest").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(255, 255, 255, 0), (-life / 2f - 2f));
            Invoke("kill", 2);
        }
	}

    public void kill() {
        //LevelController.Instance.hasPower = false;

        if (currentFrog != null)
        {
            currentFrog.reset();
        }
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other) {
        Frog frog = other.GetComponent<Frog>();
        if (frog != null)
        {
            if (currentFrog != null)
            {
                currentFrog.lilypad = null;
            }
            currentFrog = frog;
            frog.landOn(this);
            frog.stop();
			SoundManager.Instance.SFXFrogSplash();
            //frog.transform.position = Vector3.Lerp(frog.transform.position, transform.position, 20);
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        Frog frog = other.GetComponent<Frog>();
        if (frog != null)
        {
            if (currentFrog == frog)
            {
                frog.lilypad = null;
                currentFrog = null;
            }
        }

    }
}
