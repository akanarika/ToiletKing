using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {

    public EndFrog firstFrog;
    public EndFrog secondFrog;
    public EndFrog thirdFrog;
    public EndFrog fourthFrog;

    private float timer;
    private float timeout;

    public GameObject restartButton;

    public bool gameEnded;

    public void Awake()
    {

        gameEnded = false;
    }

    public void setRanks(Utility.Pair<int,int>[] ranks)
    {
        gameEnded = true;
        timer = Time.time;
        firstFrog.showFrog(ranks[0].first, ranks[0].second);
        secondFrog.showFrog(ranks[1].first, ranks[1].second);
        thirdFrog.showFrog(ranks[2].first, ranks[2].second);
        fourthFrog.showFrog(ranks[3].first, ranks[3].second);
    }

    public void hit()
    {
        if (restartButton.activeSelf)
        {
            gameEnded = false;
            restartButton.SetActive(false);
            LevelController.Instance.restart();
        }
    }

	// Use this for initialization
	void Start () {
        timeout = Config.EndScreenTimeout;
        restartButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!restartButton.activeSelf && gameEnded && (timer + timeout) < Time.time)
        {
            restartButton.SetActive(true);
        }
	}
}
