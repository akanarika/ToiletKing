using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : Singleton<LevelController> {

    protected LevelController() {}
    private float gameTime;
    public GameObject prefab;
    private List<Lilypad> lilypads;
    public Vector3 center { get; private set; }
    public Water water;

    public GameObject[] gui;

    public Player[] players;

    private float height;
    private float width;

    private float activeTime;
    private float tapTime;
    private bool isStart;
    private bool _start;
    private bool _ready;
    private bool _end;
    private bool hasAccelerate;

    public TextMesh timer;
    private float startTime;

    public EndScreen gameOverScreen;

    public bool hasPower;
    private float powerCooldown;

    public GameObject tutorial;

    public GameObject titleScreen;

    private bool unlimited;

    public bool frogCrownsUpdated;

    void Awake()
    {
        frogCrownsUpdated = false;
        tutorial.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        _end = false;
        isStart = true;
        _ready = false;
        _start = false;
        _initializePlayers();
        lilypads = new List<Lilypad>();
        timer.gameObject.SetActive(false);
        titleScreen.SetActive(true);
    }

    void startGame()
    {
        gameOverScreen.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            players[i].startGame();
        }
        prefab = Resources.Load("Lilypad") as GameObject;
        center = new Vector3(0, Constants.Layers.FrogHeight, 0);
        _initializeWater();
        _initializeLilypads();
        _initializeFrogs();
        isStart = false;
        _start = true;
        startTime = Time.time;
        timer.gameObject.SetActive(true);
        hasPower = false;
        powerCooldown = Random.Range(Config.PowerMinCooldown, Config.PowerMaxCooldown);
    }

    private void _initializePlayers()
    {
        players = new Player[4];
    }

    private void _initializeWater() {
        
    }

    private void _initializeLilypads()
    {
        //_createPlayerPads();
        _generateLevelPads();
    }

    private void _initializeFrogs()
    {
        
    }

    // Creates the player lilypads
    private void _createPlayerPads() 
    {
        /*for(int i=0; i<2; i++){
            for(int j=0; j<2; j++){
                Lilypad playerPad = new Lilypad();
                playerPad.PlayerPad = true;
                playerPad.place(26.4f * (i - 0.5f) * 2, 26.4f * (j - 0.5f) * 2);
                lilypads.Add(playerPad);

            }
        }*/
    }

    // Creates the list of lilypads with their positions
    private void _generateLevelPads()
    {
		width = (float)Screen.width;
		height = (float)Screen.height;
		//up Area
		generateAreaPads(2, new Vector2((width-height)/2f,height*3f/4f), new Vector2((width+height)/2f,height-10f));
		//down Area
		generateAreaPads(2, new Vector2((width-height)/2f,0+10f), new Vector2((width+height)/2f,height/4f));
		//left Area
		generateAreaPads(3, new Vector2((width-height)/2f+10f,height/4f), new Vector2(width/2f,height*3f/4f));
		//right Area
		generateAreaPads(3, new Vector2(width/2f,height/4f), new Vector2((width+height)/2f-10f,height*3f/4f));
	}

	private void generateAreaPads(int num, Vector2 p1, Vector2 p2){
		RaycastHit hit;
		GameObject cam = GameObject.Find("Main Camera");
		int generateNum = 0;
		for(int i=0; i<num;){
			generateNum++;
			if(generateNum>200) break;
			Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3( p1.x+(p2.x-p1.x)*Random.value, 
			                                                                  p1.y+(p2.y-p1.y)*Random.value, 0));
			if(Physics.Raycast(ray, out hit)){
				GameObject hitObject = hit.collider.gameObject;
				bool overlay = false;
				Vector2 newPadCenter = new Vector2(hit.point.x, hit.point.z);
				if(hitObject.name == "Table" && Vector3.Distance(hit.point, Vector3.zero)<35f/*table radius*/){
					for(int j=0; j<lilypads.Count; j++){
						if(Vector2.Distance(lilypads[j].getCenter(), newPadCenter)<2f*lilypads[0].radius){
							overlay = true;
						}
					}
					for(int m=0; m<2; m++){
						for(int n=0; n<2; n++){
							Vector2 playerPadCenter = new Vector2(26.4f * (m-0.5f) * 2, 26.4f * (n-0.5f) * 2);
							if(Vector2.Distance(newPadCenter, playerPadCenter)<20f){
								overlay = true;
							}
						}
					}
					if(!overlay){
						i++;
                        Lilypad lilypad = Instantiate(prefab).GetComponent<Lilypad>();
                        lilypad.transform.localScale = new Vector3(Constants.LilypadSize, 1, Constants.LilypadSize);
                        lilypad.PlayerPad = false;
                        lilypad.enabled = true;
                        lilypad.setPosition(newPadCenter);
                        lilypads.Add(lilypad);
                    }
					
				}
			}
		}
		//Destroy(GameObject.Find("pad"));
	}

    public void ready()
    {
        titleScreen.SetActive(false);
        _ready = true;
        if (!_ready)
        {
            tapTime = Time.time;
        }
    }

    public void broadcast(string str)
    {
        for (int i = 0; i < 4; i++)
        {
            players[i].setText(str);
        }
    }

    public void score(int winner, int loser) 
    {
        players[winner - 1].addScore();
        players[loser - 1].subtractScore();
    }

    public void endGame()
    {
        isStart = false;
        _ready = false;
        _start = false;
        _end = true;

        for (int i = 0; i < 4; i++)
        {
            players[i].endGame();
        }
        Utility.Pair<int, int>[] scores = getScores();
        gameOverScreen.setRanks(scores);

        _deactivateLilypads();

    }

    private void updateFrogCrowns()
    {
        if (!frogCrownsUpdated) {
            frogCrownsUpdated = true;
            Utility.Pair<int, int>[] scores = getScores();
            int topPlayer = scores[0].first;
            int nextPlayer = scores[1].first;
            for (int i = 0; i < 4; i++)
            {
                if (i+1 == topPlayer)
                {
                    if (scores[1].second == scores[0].second || scores[0].second < 100)
                    {
                        players[i].crownOff();
                    }
                    else
                    {
                        players[i].crownOn();
                    }
                }
                else
                {
                    players[i].crownOff();
                }
            }
        }
    }

    private Utility.Pair<int, int>[] getScores()
    {
        int[] scores = new int[4];
        for (int i = 0; i < 4; i++)
        {
            scores[i] = players[i].totalScore;
        }

        Utility.Pair<bool, Utility.Pair<int, int>>[] frogScores = new Utility.Pair<bool, Utility.Pair<int, int>>[4];
        for (int i = 0; i < 4; i++)
        {
            frogScores[i] = new Utility.Pair<bool, Utility.Pair<int, int>>(players[i].join, new Utility.Pair<int, int>(i + 1, scores[i]));
        }

        bool sorted = false;
        while (!sorted)
        {
            sorted = true;
            for (int i = 0; i < 3; i++)
            {
                if (frogScores[i].first)
                {
                    if (frogScores[i + 1].first)
                    {
                        if (frogScores[i].second.second < frogScores[i + 1].second.second)
                        {
                            Utility.Pair<bool, Utility.Pair<int, int>> temp = frogScores[i];
                            frogScores[i] = frogScores[i + 1];
                            frogScores[i + 1] = temp;
                            sorted = false;
                        }

                    }
                }
                else
                {
                    if (frogScores[i + 1].first)
                    {
                        Utility.Pair<bool, Utility.Pair<int, int>> temp = frogScores[i];
                        frogScores[i] = frogScores[i + 1];
                        frogScores[i + 1] = temp;
                        sorted = false;
                    }
                }
            }

        }

        Utility.Pair<int, int>[] ranks = new Utility.Pair<int, int>[4];
        for (int i = 0; i < 4; i++)
        {
            ranks[i] = new Utility.Pair<int, int>();
            if (frogScores[i].first)
            {

                ranks[i].first = frogScores[i].second.first;
                ranks[i].second = frogScores[i].second.second;
            }
            else
            {
                ranks[i].first = -1;
                ranks[i].second = -1;
            }
        }
        return ranks;
    }

    private void _deactivateLilypads()
    {
        foreach (Lilypad pad in lilypads)
        {
            pad.kill();
        }

        lilypads = new List<Lilypad>();
    }

    public void inputHit(int player)
    {
        gameOverScreen.hit();
    }

    public void restart()
    {
        SoundManager.Instance.BGMMainThemeStart();
        hasAccelerate = false;
        titleScreen.SetActive(true);
        gameOverScreen.gameObject.SetActive(false);
        _end = false;
        isStart = true;
        _ready = false;
        _start = false;
        lilypads = new List<Lilypad>();
        timer.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            players[i].resetAll();
        }
    }

    public void showEndScreen()
    {
        gameOverScreen.gameObject.SetActive(true);
    }

    // Use this for initialization
	void Start () {
        gameTime = Config.GameDuration;
        unlimited = (gameTime < 0.1f);
        SoundManager.Instance.BGMMainThemeStart();
	}
	// Update is called once per frame
	void Update () {

        
        float count;
        for (int i = 0; i < lilypads.Count; i++) {
            if (lilypads[i] == null) lilypads.Remove(lilypads[i]);
        }

        
        count = Random.Range(0.5f, 9.5f);
        //Debug.Log(count);
        if (_start && lilypads.Count < count) {
       
            generateAreaPads(1, new Vector2((height - width) / 2, 0), new Vector2((height + width) / 2, height));
        }


        float timeLeft = (startTime + Config.GameDuration) - Time.time;
        if (_start)
        {

            updateFrogCrowns();

            hasPower = false;
            for (int i = 0; i < lilypads.Count; i++)
            {
                if (lilypads[i] == null) lilypads.Remove(lilypads[i]);
                if (lilypads[i].isPower) hasPower = true;
            }
            if (!hasPower && powerCooldown < 0)
            {

                int powerIdx = (int)Random.Range(0, lilypads.Count - 1);
                if (!lilypads[powerIdx].currentFrog)
                {
                    lilypads[powerIdx].isPower = true;
                    powerCooldown = Random.Range(Config.PowerMinCooldown + timeLeft / 18f, Config.PowerMaxCooldown + timeLeft / 18f);
                }

            }
            else
            {
                if (powerCooldown >= 0) powerCooldown -= Time.deltaTime;
            }

            if (timeLeft >= Config.GameDuration * 1f / 2f) count = Random.Range(3.5f, 9.5f);
            else if (timeLeft >= Config.GameDuration / 4f && timeLeft < Config.GameDuration * 1f / 2f) count = Random.Range(3.5f, 7.5f);
            else count = Random.Range(3.5f, 6.2f);
            if (lilypads.Count < count)
            {
                generateAreaPads(1, new Vector2((height - width) / 2, 0), new Vector2((height + width) / 2, height));
                //Debug.Log("new pad to be generated");
            }
            if (unlimited)
            {
                timer.text = "";
            }
            else
            {
                if (timeLeft <= 0)
                {
                    showEndScreen();
                    endGame();
                }
                else
                {
                    timer.text = Mathf.CeilToInt(timeLeft).ToString();
                }
            }
        }
        else if (_end)
        {

        }
        else if (isStart)
        {


            if (_ready)
            {

                float timeToStart = (tapTime + Config.ActiveTime) - Time.time;
                if (timeToStart > 0f)
                {
                    int time = Mathf.CeilToInt(timeToStart);
                    broadcast(time.ToString());
                }
                else
                {
                    broadcast("Go!");
                    startGame();
                }
                //Debug.Log("timeTostart:"+timeToStart);
                if (timeToStart < 10f && timeToStart > 0f)
                {
                    tutorial.SetActive(true);
                    tutorial.transform.Rotate(new Vector3(0, 0, 0.45f));
                }
                else {
                    tutorial.SetActive(false);
                }
            }
        } 
        else
        {
            

        }

        if (_start && timeLeft<= Config.GameDuration /4f && !hasAccelerate && timeLeft>0)
        {
            SoundManager.Instance.BGMMainAccelerateStart();
            hasAccelerate = true;
        }
	}

    public static Vector3 transform(Vector3 transform,int layerFrom, int layerTo)
    {
        return transform + Vector3.up * (Constants.Layers.getHeight(layerTo) - Constants.Layers.getHeight(layerFrom));
    }

    public static Vector3 transform(Vector3 transform, int layerTo)
    {
        return new Vector3(transform.x, Constants.Layers.getHeight(layerTo), transform.z);
    }
}
