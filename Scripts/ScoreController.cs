using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreController : Singleton<ScoreController>
{

    public int[] scores { get; private set; }
    private GameObject scoreboard;
	// Use this for initialization
	void Start () {
        scores = new int[4];
        for (int i = 0; i < 4; i++) {
            scores[i] = 0;
        }
        scoreboard = GameObject.Find("Scoreboard");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddPerfect(Player player) {
        scores[player.index] += 10;
        GameObject score = scoreboard.transform.FindChild("player" + player.index).gameObject;
        score.GetComponent<Text>().text = "score: " + scores[player.index];
        score.GetComponent<Animator>().Play("Pop");
    }

    public void AddGreat(Player player) {
        scores[player.index] += 5;
        GameObject score = scoreboard.transform.FindChild("player" + player.index).gameObject;
        score.GetComponent<Text>().text = "score: " + scores[player.index];
        score.GetComponent<Animator>().Play("Pop");
    }

    public void reset() {
        for (int i = 0; i < 4; i++)
        {
            scores[i] = 0;
            GameObject score = scoreboard.transform.FindChild("player" + i).gameObject;
            score.GetComponent<Text>().text = "score: 0";
        }
        
    }
}
