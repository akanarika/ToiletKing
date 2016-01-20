using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager> {

    protected SoundManager() { }

	public Transform soundManager;

	public AudioSource bgmAS;
	public AudioSource[] sfxFrogAS;

	public AudioClip bgmMainC;
	public AudioClip bgmMainOpeningC;
	public AudioClip bgmMainAccelerateOpeningC;
	public AudioClip bgmMainAccelerateC;
	public AudioClip bgmTitleC;
	public AudioClip bgmTutorialC;
	public AudioClip sfxSYSTitleC;
	public AudioClip sfxSYSCountDown321C;
    public AudioClip sfxSYSCountDownGoC;
	public AudioClip[] sfxSYSKillCountC;
	public AudioClip sfxSYSBubbleTrapC;
	public AudioClip sfxSYSBubbleEscapeC;
    public AudioClip sfxSYSBubblePopC;
	public AudioClip[] sfxFrogJumpC;
	public AudioClip[] sfxFrog1C;
	public AudioClip[] sfxFrog2C;
	public AudioClip[] sfxFrog3C;
	public AudioClip[] sfxFrog4C;
	public AudioClip sfxFrogCrashC;
	public AudioClip sfxFrogDropC;
	public AudioClip sfxFrogSplashC;

	AudioClip[][] sfxFrogC;

	//test
	AudioSource testAS;
	GameObject testGO;

	// Use this for initialization
	void Start () {
		// Debug.Log("Sound Manager start working");
		sfxFrogC = new AudioClip[4][];
		sfxFrogC [0] = sfxFrog1C;
		sfxFrogC [1] = sfxFrog2C;
		sfxFrogC [2] = sfxFrog3C;
		sfxFrogC [3] = sfxFrog4C;

	}
	
	// Update is called once per frame
	void Update () {
		//test
		if (Input.GetKeyDown ("up"))	BGMMainThemeStart (); //call when the race start
		if (Input.GetKeyDown ("right"))	BGMMainAccelerateStart (); // call when the race accelerate
		if (Input.GetKeyDown ("down"))	BGMStop (); //
		if (Input.GetKeyDown ("t"))	SFXSYSTitle (); // call when the title shows up

		if (Input.GetKeyDown ("b"))	SFXSYSBubbleTrap (); // call when the frog jump into a bubble
		if (Input.GetKeyDown ("e"))	SFXSYSBubbleEscape (); // call everytime when the frog is in the bubble trying to jump out

		if (Input.GetKeyDown ("c"))	SFXFrogCrash (); // call when 2 frogs crash
		if (Input.GetKeyDown ("s"))	SFXFrogSplash (); // call when a frog land on a lotus pads
		if (Input.GetKeyDown ("d"))	SFXFrogDrop (); // call when a frog drop into water

        if (Input.GetKeyDown("k")) SFXSYSCountDown(3);
        if (Input.GetKeyDown("l")) SFXSYSCountDown(2);
        if (Input.GetKeyDown(";")) SFXSYSCountDown(1);
        if (Input.GetKeyDown("'")) SFXSYSCountDown(0);

        if (Input.GetKeyDown("n")) SFXSYSKillCount(2);
        if (Input.GetKeyDown("m")) SFXSYSKillCount(3);
        if (Input.GetKeyDown(",")) SFXSYSKillCount(4);
        if (Input.GetKeyDown(".")) SFXSYSKillCount(5);

        if (Input.GetKeyDown("1")) SFXFrogJump(0); //call when frog[X] jump (X=0~3)
        if (Input.GetKeyDown("2")) SFXFrogJump(1);
        if (Input.GetKeyDown("3")) SFXFrogJump(2);
        if (Input.GetKeyDown("4")) SFXFrogJump(3);

        if (Input.GetKeyDown("5")) SFXSYSJoin(0); //call when a player join the game or when he win the game
        if (Input.GetKeyDown("6")) SFXSYSJoin(1);
        if (Input.GetKeyDown("7")) SFXSYSJoin(2);
        if (Input.GetKeyDown("8")) SFXSYSJoin(3);
    }


    public void BGMMainThemeStart(){
		Debug.Log("BGM Main Start");
		bgmAS.clip = bgmMainOpeningC;
		bgmAS.volume = 0.5f;
		bgmAS.loop = false;
		bgmAS.Play();
		StartCoroutine (SequencePlay (bgmAS, bgmMainC, bgmAS.volume,true));
	}

	public void BGMMainAccelerateStart(){
		Debug.Log("BGM Main Accelerate Start");
		bgmAS.clip = bgmMainAccelerateOpeningC;
		bgmAS.volume = 0.5f;
		bgmAS.loop = false;
		bgmAS.Play(); 
		StartCoroutine (SequencePlay (bgmAS, bgmMainAccelerateC, bgmAS.volume,true));
	}

	public void BGMTitleStart(){
		bgmAS.clip = bgmTitleC;
		bgmAS.loop = false;
		bgmAS.Play();
	}

	public void BGMStop(){
		StartCoroutine(MusicFadeOut(bgmAS, 0.7f));
	}

	public void SFXSYSTitle(){
		AudioPlay (sfxSYSTitleC, soundManager, 1f, 1f);
	}

	public void SFXSYSCountDown(int count){
        if (count > 0) {
            AudioPlay(sfxSYSCountDown321C, soundManager, 1f, 1f);
        }
        else if (count == 0) {
            AudioPlay(sfxSYSCountDownGoC, soundManager, 1f, 1f);
        }
        else {
            Debug.Log("Input count illegal");
        }
	}

	public void SFXSYSKillCount(int killcount){
        float volumn;
        if (killcount <= 1 || killcount > 5)
        {
            return;
        }
        else if (killcount == 2) volumn = 0.5f;
        else volumn = 0.3f;
		AudioPlay (sfxSYSKillCountC[killcount-2], soundManager, volumn, 1f);
	}

	public void SFXSYSJoin(int frogID){
		//if (sfxFrogAS [frogID].isPlaying) {
		//	return;
		//}
        //sfxFrogAS [frogID].clip = sfxFrogC [frogID] [0];
        //sfxFrogAS [frogID].Play ();


        AudioPlay(sfxFrogC[frogID][0], soundManager, 1f, 1f);

	}

	public void SFXSYSBubbleTrap(){
		float pitch;
		pitch = Random.Range (0.8f, 1.2f); 
		AudioPlay (sfxSYSBubbleTrapC, soundManager, 0.5f, pitch);
	}

	public void SFXSYSBubbleEscape(){
		float pitch;
		pitch = Random.Range (0.8f, 1.2f); 
		AudioPlay (sfxSYSBubbleEscapeC, soundManager, 1f, pitch);
	}

    public void SFXSYSBubblePop()
    {
        float pitch;
        pitch = Random.Range(0.8f, 1.2f);
        AudioPlay(sfxSYSBubblePopC, soundManager, 1f, pitch);
    }

    public void SFXFrogCroak(int frogID){
		if (sfxFrogAS [frogID].isPlaying) {
			return;
		}
		int random = Random.Range (0, 4);
		//Debug.Log (random);
		if(random==1||random==2){
            sfxFrogAS[frogID].clip = sfxFrogC[frogID][random];
        }
		sfxFrogAS [frogID].Play ();
	}

	public void SFXFrogJump(int frogID){
		float pitch;
		pitch = Random.Range (0.8f, 1.2f); 
		AudioPlay (sfxFrogJumpC[frogID], soundManager, 0.5f, pitch);
		SFXFrogSplash (0.5f);
		SFXFrogCroak (frogID);
	}

	public void SFXFrogCrash(){
		float pitch;
		pitch = Random.Range (0.8f, 1.2f); 
		AudioPlay (sfxFrogCrashC, soundManager, 0.3f, pitch);
	}

	public void SFXFrogDrop(){
		float pitch;
		pitch = Random.Range (0.8f, 1.2f); 
		AudioPlay (sfxFrogDropC, soundManager, 1f, pitch);
	}

	public void SFXFrogSplash(float volumn){
		float pitch;
		pitch = Random.Range (0.8f, 1.2f); 
		AudioPlay (sfxFrogSplashC, soundManager, volumn, pitch);
	}
	public void SFXFrogSplash(){
		SFXFrogSplash (1f);
	}

	IEnumerator SequencePlay(AudioSource audios, AudioClip audioc, float volume,bool loop){
		while (audios.isPlaying) {
			yield return new WaitForSeconds (Time.deltaTime);
		}
		audios.clip = audioc;
		audios.volume = volume;
		audios.loop = loop;
		audios.Play ();
		// Debug.Log (dialogAS.clip);
	}


	IEnumerator MusicFadeIn(AudioSource audios, float fadeInSpeed, float maxVolumn){
//		Debug.Log("Start fade in.");
		audios.Play();
		while (audios.volume < maxVolumn){
			audios.volume += fadeInSpeed * Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
//		Debug.Log("Complete fade in.");
	}

	IEnumerator MusicFadeOut(AudioSource audios, float fadeOutSpeed){
//		Debug.Log("Start fade out.");
		while (audios.volume >= 0.01){
			audios.volume -= fadeOutSpeed * Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		audios.Stop ();
	}
	void AudioPlay(AudioClip clip, Transform emitter, float volume, float pitch)
	{
        if (clip != null)
        {
            //Create an empty game object
            GameObject go = new GameObject("Audio: " + clip.name);
            go.transform.position = emitter.position;
            go.transform.parent = emitter;

            //Create the source
            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
            Destroy(go, clip.length);
        }
	}

	public AudioSource Play(AudioClip clip, Transform emitter)	{
	return Play(clip, emitter, 1f, 1f);
	}
	 
	public AudioSource Play(AudioClip clip, Transform emitter, float volume)	{
	return Play(clip, emitter, volume, 1f);
	}
	 
	/// Plays a sound by creating an empty game object with an AudioSource
	/// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
	public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch)
	{
	//Create an empty game object
	GameObject go = new GameObject ("Audio: " + clip.name);
	go.transform.position = emitter.position;
	go.transform.parent = emitter;
	 
	//Create the source
	AudioSource source = go.AddComponent<AudioSource>();
	source.clip = clip;
	source.volume = volume;
	source.pitch = pitch;
	source.Play ();
	Destroy (go, clip.length);
	return source;
	}
	 
	public AudioSource Play(AudioClip clip, Vector3 point)
	{
	return Play(clip, point, 1f, 1f);
	}
	 
	public AudioSource Play(AudioClip clip, Vector3 point, float volume)
	{
	return Play(clip, point, volume, 1f);
	}
	 
	/// Plays a sound at the given point in space by creating an empty game object with an AudioSource
	/// in that place and destroys it after it finished playing.
	public AudioSource Play(AudioClip clip, Vector3 point, float volume, float pitch)
	{
	//Create an empty game object
	GameObject go = new GameObject("Audio: " + clip.name);
	go.transform.position = point;
	 
	//Create the source
	AudioSource source = go.AddComponent<AudioSource>();
	source.clip = clip;
	source.volume = volume;
	source.pitch = pitch;
	source.Play();
	Destroy(go, clip.length);
	return source;
	}
}
