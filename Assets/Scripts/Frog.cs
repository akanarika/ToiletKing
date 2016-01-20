using UnityEngine;
using System.Collections;

public class Frog : MonoBehaviour {

    private float _jumpDistance;
    private Lilypad target;
    private Rigidbody rb;
    private float launchTime;
    public Lilypad lilypad;
    private static int scaleDemultiplier = 15;
    public bool inAir { get; private set; }
    private Player _player;
    public bool trapped { get; private set; }
    private bool escaped;
    private int trapStep;
    private float recoverTime;
    public int lastFrog;

    private Collider frogCollider;

    public Animator anim;
    public ParticleSystem particles;

    private float poweredTime;

    public Animator rippler;

    public GameObject carat1;
    public GameObject carat2;
    public GameObject carat3;
    public GameObject caratStar;

    private bool isPowered;

    public GameObject crown;

    void Awake()
    {
        showCarat(0);
        //rippler.StopPlayback();
        frogCollider = GetComponent<SphereCollider>();
        trapped = false;
        anim.SetBool("Stuck", false);
        anim.SetBool("Jumping", false);
        escaped = true;
        inAir = false;
        _jumpDistance = Constants.JumpDistance;
        gameObject.layer = Constants.Layers.Frog;
        transform.position = LevelController.transform(transform.position, Constants.Layers.Frog);
        rb = GetComponent<Rigidbody>();

        lastFrog = -1;
        poweredTime = 0;
        isPowered = false;
    }

    public void crownOn()
    {
        crown.SetActive(true);
    }

    public void crownOff()
    {
        crown.SetActive(false);
    }

	// Use this for initialization
	void Start() {

        trapStep = Config.TrapSteps;
	}

    public void showCarat(int level)
    {
        carat1.SetActive(false);
        carat2.SetActive(false);
        carat3.SetActive(false);
        caratStar.SetActive(false);
        if (level == 1)
        {
            carat1.SetActive(true);
        }
        else if (level == 2)
        {
            carat2.SetActive(true);
        }
        else if (level == 3)
        {
            carat3.SetActive(true);
        }
        else if (level > 3)
        {
            caratStar.SetActive(true);
        }
    }

    public void startGame()
    {

        transform.LookAt(LevelController.Instance.center);
        frogCollider.enabled = false;
    }

    public void trap()
    {
		SoundManager.Instance.SFXSYSBubbleTrap ();
        trapStep = Config.TrapSteps;
        trapped = true;

        anim.SetBool("Jumping", false);
        anim.SetBool("Stuck", true);
        escaped = false;
    }

	// Update is called once per frame
	void Update() {
        if (inAir && Time.time > (launchTime + 0.2f) && lilypad != null && (lilypad.transform.position - transform.position).magnitude < Constants.PadDistance)
        {

            anim.SetBool("Jumping", false);
            inAir = false;
        }
        if (lilypad != null && !inAir)
        {
            followLilypad();
        }
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        if ((transform.position - LevelController.Instance.center).magnitude > Constants.GameBoundary ||
            (inAir && Time.time > (launchTime + Config.AirTime)))
        {
            reset();
        }

        if (poweredTime > 0 && isPowered)
        {
            poweredTime -= Time.deltaTime;
        }
        else if (poweredTime <= 0 && isPowered) {
            poweredTime = 0;
            isPowered = false;
            rb.mass = 3f;
            transform.localScale /= 1.5f;
        }
        
	}

    void followLilypad()
    {
        transform.position = LevelController.transform(lilypad.transform.position, Constants.Layers.Frog);
        //rb.AddForce(lilypad.transform.position - transform.position);
    }

    public void setPlayer(Player player)
    {
        _player = player;
    }

    public void landOn(Lilypad pad)
    {
        lilypad = pad;

        anim.SetBool("Jumping", false);
        inAir = false;
    }

    // Frog is reset
    public void reset()
    {
        ripple();
        frogCollider.enabled = false;
        SoundManager.Instance.SFXFrogDrop();
        if (lastFrog == -1)
        {
            _player.loseScore();
        } 
        else
        {
            _player.score(lastFrog);
        }
        inAir = false;
        lilypad = null;
        stop();
        anim.SetBool("Jumping", false);
        trapped = false;
        anim.SetBool("Stuck", false);
        escaped = true;
        if (isPowered) {
            transform.localScale /= 1.5f;
            rb.mass = 3f;
            isPowered = false;
        }
        recoverTime = Time.time;
        _player.reset();
        transform.LookAt(LevelController.Instance.center);
    }

    public void ripple()
    {
        //rippler.Play("waveAnim");
    }

    public void stop()
    {
        
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
         
        //rb.AddForce(-rb.velocity); 
    }
    
    // Frog jumps towards direction vector
    public void jump(int player)
    {
       
        frogCollider.enabled = true;
        if (trapped)
        {

            lastFrog = -1;
			SoundManager.Instance.SFXSYSBubbleEscape();
            trapStep--;

            if (trapStep <= 0)
            {
                recoverTime = Time.time;
                trapped = false;
                SoundManager.Instance.SFXSYSBubblePop();
                anim.SetBool("Stuck", false);
                lilypad.release();
            }
        }
        else if (!escaped)
        {
            SoundManager.Instance.SFXFrogJump(player - 1);
            lastFrog = -1;
            if ((recoverTime + Config.DownTime) < Time.time)
            {
                escaped = true;
            }
        }
        else if (!inAir && (recoverTime + Config.DownTime < Time.time))
        {
            SoundManager.Instance.SFXFrogJump(player - 1);
            lastFrog = -1;
            launchTime = Time.time;
            lilypad = null;
            Vector3 direction = transform.forward;
            RaycastHit hit;
			int frogID;

            bool hasCollided = Physics.Raycast(transform.position, direction, out hit, _jumpDistance);

            if (hasCollided && hit.collider.gameObject.layer == Constants.Layers.Lilypad)
            {
                // Hits lilypad
                target = hit.collider.gameObject.GetComponent<Lilypad>();
                if (target != null)
                {
                    direction = (hit.collider.gameObject.transform.position.toVector2() - transform.position.toVector2()).toVector3() / scaleDemultiplier;

                }

                Debug.Log("Jump onto lilypad");
            }
            else
            {

                Debug.Log("Falls into water");
                // Falls into water
            }

            rb.velocity = (direction * Config.JumpSpeed);
			

            inAir = true;

            anim.SetBool("Jumping", true);
        }
    }
    
    void OnCollisionEnter(Collision other)
    {
        Frog frog = other.collider.gameObject.GetComponent<Frog>();
        if (frog != null)
        {
            if (particles != null)
            {
                particles.Emit(10);
            }
            frog.lastFrog = _player.player;
			SoundManager.Instance.SFXFrogCrash();
            //lilypad = null;
            launchTime = Time.time;
            inAir = true;

            anim.SetBool("Jumping", true);
            trapped = false;

            anim.SetBool("Stuck", false);
            escaped = true;
            if (lilypad != null)
            {
                lilypad.release();
            }
        }

    }
    
    void OnTriggerEnter(Collider other)
    {
        anim.SetBool("Jumping", false);
        inAir = false;
        if (other.name == "Pill") {
            Debug.Log("frogID Pill" + (_player.player - 1));
            SoundManager.Instance.SFXSYSJoin(_player.player - 1);
            rb.mass = 100f;
            if(!isPowered) transform.localScale *= 1.5f;
            other.gameObject.SetActive(false);
            other.transform.parent.GetComponent<Lilypad>().isPower = false;
            //Destroy(other.gameObject);
            poweredTime = 10f;
            isPowered = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        //launchTime = Time.time;

        anim.SetBool("Jumping", true);
        inAir = true;
    }

}
