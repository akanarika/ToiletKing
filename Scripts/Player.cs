using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public int index;
    private KeyCode[] _KeyCode;

    private InputController _inputController;
    private RhythmController _rhythmController;
    private WorldController _worldController;
    private SoundManager _soundManager;
    private Config _config;
    private ScoreController _scoreController;
    private MenuController _menuController;
    private TitleController _titleController;

    private float _triggerCooldown = 0.1f;
    private float _previousTrigger;

    private float _averageOffset;
    private int _beatCount;

    public int streak { get; private set; }
    public List<int> combo { get; private set; }
    private int _lastHitIndex;

    private Actor _actor;
	private Animator feedbackAnim;
	private bool animPlayed;

    public enum PlayerState
    {
        Inactive,
        Ready,
        Active
    }

	public PlayerState _playerState{ get; private set; }

    void Awake()
    {
        streak = 0;
        _previousTrigger = 0;
        _playerState = PlayerState.Inactive;
        _inputController = InputController.Instance;
        _rhythmController = RhythmController.Instance;
        _worldController = WorldController.Instance;
        _soundManager = SoundManager.Instance;
        _config = Config.Instance;
        _scoreController = ScoreController.Instance;
        _menuController = MenuController.Instance;
        _titleController = TitleController.Instance;
        
		feedbackAnim = GameObject.Find ("Feedbacks/Feedback" + index).GetComponent<Animator>();
    }

    public void setState(PlayerState state)
    {
        _playerState = state;
    }

    public void missBeat()
    {
		feedbackAnim.Play("Miss");
        streak = 0;
    }

	private void perfectBeat(){
		feedbackAnim.Play ("Perfect");
        _scoreController.AddPerfect(this);
        streak++;
	}

	private void greatBeat(){
		feedbackAnim.Play("Great");
        _scoreController.AddGreat(this);
	}

    public void trigger(int buttonIndex)
    {
        if (_previousTrigger + _triggerCooldown > Time.time)
        {
            return;
        }
        _actor.Trigger(buttonIndex);
        switch(_playerState) 
        {
            case PlayerState.Inactive:
                switch (_worldController.currentGameState)
                {
                    case WorldController.GameState.Title:
                        _titleController.gameObject.GetComponent<Animator>().Play("titleBackground");
                        _menuController.gameObject.SetActive(true);
                        _menuController.gameObject.GetComponent<Animator>().Play("menuBackground");
                        _worldController.enterMenu();
                        break;
                    case WorldController.GameState.Menu:
                        if (buttonIndex == 0)
                        {
                            _menuController.select();
                        }
                        else
                        {
                            _menuController.scroll();
                        }
                        break;
                    case WorldController.GameState.Start:
                        GameObject.Find("Scoreboard").transform.FindChild("player" + index).gameObject.SetActive(true);
                        Debug.Log("Inactive");
                        Debug.Log("Playing: " + index + ", " + buttonIndex);
                        _soundManager.playSound(_soundManager.playerTriggers[index, buttonIndex, 0]);
                        _worldController.addPlayer(this);
                        _playerState = PlayerState.Active;
                        break;
                    case WorldController.GameState.Idle:
                    default:
                        GameObject.Find("Scoreboard").transform.FindChild("player" + index).gameObject.SetActive(true);
                        Debug.Log("Inactive");
                        Debug.Log("Playing: " + index + ", " + buttonIndex);
                        _soundManager.playSound(_soundManager.playerTriggers[index, buttonIndex, 0]);
                        _worldController.addPlayer(this);
                        _playerState = PlayerState.Ready;
                        
                        break;
                
                }
                break;
                
            case PlayerState.Active:
                // Trigger a beat
                Utility.Pair<float, RhythmController.HitStatus> offset = _rhythmController.beat(this, buttonIndex);
                if (!offset.second.Equals(RhythmController.HitStatus.Invalid) && !offset.second.Equals(RhythmController.HitStatus.Miss))
                {
                    _averageOffset = (_averageOffset * _beatCount + offset.first) / (_beatCount + 1);
                    _beatCount++;
				
                }
                switch(offset.second){
					case RhythmController.HitStatus.Perfect:
				 		perfectBeat();
						break;
					case RhythmController.HitStatus.Great:
						greatBeat();
						break;
					case RhythmController.HitStatus.Miss:
						missBeat();
						break;
					case RhythmController.HitStatus.Invalid:
						break;
				}
                circleFlash();
                Debug.Log("Player " + index + ", key " + buttonIndex + ": " + offset + ", average: " + _averageOffset);
                break;
            case PlayerState.Ready:
                _soundManager.playSound(_soundManager.playerTriggers[index, buttonIndex, 0]);
                Debug.Log("Player ready");
                _worldController.removePlayer(this);
                _playerState = PlayerState.Inactive;
                GameObject.Find("Scoreboard").transform.FindChild("player" + index).gameObject.SetActive(false);
                break;
        }

    }

    private void circleFlash() {
        GameObject circleParent = GameObject.Find("Gap");
        circleParent.transform.FindChild("circle_" + index).GetComponent<Animator>().Play("circle_" + index);
    }

    public void registerActor(Actor actor)
    {
        _actor = actor;
    }

    public void deactivateActor()
    {
        _actor.deactivate();
        GameObject.Find("Scoreboard").transform.FindChild("player" + index).gameObject.SetActive(false);
        streak = 0;
    }

    public void activateActor()
    {
        _actor.activate();

    }

    public void joinGame()
    {
        _playerState = PlayerState.Active;
    }

    public void initialize(int index)
    {
        
    }

    public void reset()
    {
        _averageOffset = 0;
        _beatCount = 0;
    }

    public void registerKeyCode(KeyCode[] KeyCode)
    {
        _inputController.registerPlayerControls(index, KeyCode);
    }

    private void executeMove()
    {
        //_increaseStreak();
        Debug.Log("Streak! : " + streak);
        Debug.Log("executeMove");
    }

    

    // Use this for initialization
	void Start () {
        _inputController.registerPlayer(this);
        _loadKeyCode();
        registerKeyCode(_KeyCode);
        _actor.deactivate();
        reset();
	}

    private void _loadKeyCode()
    {
        Debug.Log(_config.playerControls[index]);
        _KeyCode = new KeyCode[_config.playerControls[index].Length];
        for (int i = 0; i < _config.playerControls[index].Length; i++)
        {
            _KeyCode[i] = _inputController.parseString(_config.playerControls[index][i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
	}

}
