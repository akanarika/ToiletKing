using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private LevelController _levelController;

    public int player;
    public Frog frog;
    private Lilypad _pad;
    private Vector3 initialPosition;
    private bool isStart;

    public GameObject padInactive;
    public GameObject padActive;
    public bool join { get; private set; }
    public int totalScore { get; private set; }
    private int combo;

    public GUIController gui;

    private bool isEnd;

    void Awake()
    {
        init();   
    }

    void init()
    {
        isEnd = false;
        combo = 0;
        totalScore = 0;
        isStart = true;
        join = false;
        initialPosition = transform.position;
        _levelController = LevelController.Instance;
        padActive.SetActive(false);
        padInactive.SetActive(true);
        setText("Hit pad to join");
     
    }

    public void crownOn()
    {
        frog.crownOn();
    }

    public void crownOff()
    {
        frog.crownOff();
    }

    public void spin(float amount)
    {
        transform.Rotate(transform.up, amount);
        transform.Rotate(transform.up, amount);
        if (!isStart)
        {
            if (frog != null)
            {
                frog.transform.Rotate(Vector3.up, amount);
            }
        }
    }

    public void hit()
    {
        if (isEnd)
        {
            _levelController.inputHit(player);
        }
        else if (isStart)
        {
            if (padActive.activeSelf)
            {
                _levelController.ready();
                padActive.SetActive(false);
                padInactive.SetActive(true);
                join = false;
            }
            else
            {
                _levelController.ready();
                padActive.SetActive(true);
                padInactive.SetActive(false);
                join = true;
            }
        }
        else
        {
            if (!frog.gameObject.activeSelf)
            {
                join = true;
                startGame();
            }
            else
            {
                if (frog != null)
                {
                    frog.jump(player);
                }
            }
        }
    }

    public void resetAll()
    {
        frog.reset();
        init();
    }

    public void endGame()
    {
        isEnd = true;
        frog.gameObject.SetActive(false);
    }

    public void setText(string str)
    {
        
        gui.setText(str, join);
        
    }

    public void addScore()
    {
        if (combo == 0)
        {
            totalScore += 100;
        }
        else if (combo == 1)
        {
            totalScore += 110;
        }
        else if (combo == 2)
        {
            totalScore += 130;
        }
        else if (combo == 3)
        {
            totalScore += 160;
        }
        else
        {
            totalScore += 200;
        }
        _levelController.frogCrownsUpdated = false;
        combo++;
        frog.showCarat(combo);
        gui.setText(totalScore.ToString(), join);
        gui.showScore(true, combo);
    }

    public void subtractScore()
    {
        combo = 0;
        _levelController.frogCrownsUpdated = false;
        frog.showCarat(combo);
        totalScore -= 50;
        gui.setText(totalScore.ToString(), join);
        gui.showScore(false, combo);
    }

    public void score(int winner)
    {
        _levelController.score(winner, player);
    }

    public void loseScore()
    {
        subtractScore();
    }

    public void reset()
    {
        frog.transform.position = new Vector3(initialPosition.x, 0.5f, initialPosition.z);
    }

    public void startGame()
    {
        isStart = false;
        if (join)
        {
            joinGame();
        }
    }

    public void joinGame()
    {
        join = true;
        setText("Go!");
        frog.gameObject.SetActive(true);
        frog.startGame();
        padInactive.SetActive(false);
        padActive.SetActive(true);
    }

	// Use this for initialization
	void Start () {
        if (player != 0)
        {
            _levelController.players[player - 1] = this;
        }
        if (frog != null)
        {
            frog.gameObject.SetActive(false);
            frog.transform.position = transform.position + Vector3.up * 0.3f;
            frog.setPlayer(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (isStart)
        {
            
        }
	}
}
