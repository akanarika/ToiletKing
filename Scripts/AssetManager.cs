using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class AssetManager : Singleton<AssetManager> {

    private WorldController _worldController;
    private RhythmController _rhythmController;
    private SoundManager _soundManager;
    private Dictionary<int, Asset> _assetStore;

    public const string filename = "songlist.ini";
    private List<string> songlist;

    void Awake()
    {
        songlist = new List<string>();
        _worldController = WorldController.Instance;
        _soundManager = SoundManager.Instance;
        _rhythmController = RhythmController.Instance;
        _assetStore = new Dictionary<int, Asset>();

    }

    public void saveAsset(Asset asset)
    {
        _assetStore.Add(asset.index, asset);
    }

    public void loadAssets(int index)
    {
        Asset asset = _assetStore[index];
        //_rhythmController.setSong(asset.song, asset.songBPM, asset.songOffset);
        _rhythmController.loadSong(asset);
    }

    // Use this for initialization
	void Start () {

        loadSongList(filename);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public  class Asset
    {

        public AudioClip song;
        public float songOffset;
        public float songBPM;
        public int index;
        public List<RhythmController.Beat> beatList;

        public Asset()
        {
            beatList = new List<RhythmController.Beat>();
        }

        public void addBeat(int player, float delay, int index, int type)
        {
            RhythmController.Beat beat = new RhythmController.Beat(player, delay, index, type);
            addBeat(beat);
        }

        public void addBeat(RhythmController.Beat beat)
        {
            beatList.Add(beat);
        }
    }

    void loadSongList(string filename)
    {
        if (!File.Exists(filename))
        {
            File.CreateText(filename);
            return;
        }

        Debug.Log("Song list loaded");
        try
        {
            string line;
            StreamReader sReader = new StreamReader(filename, Encoding.Default);
            do
            {
                line = sReader.ReadLine();
                if (line != null)
                {
                    // Lines with # are for comments
                    if (!line.Contains("#") && line.Length > 0)
                    {
                        songlist.Add(line);
                        Debug.Log("Song file: " + line);
                    }
                }
            }
            while (line != null);
            sReader.Close();
            foreach(string songfilename in songlist) 
            {
                loadSong(songfilename);
            }
            return;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void loadSong(string songname)
    {
        Asset asset = new Asset();
        if (!File.Exists(songname + ".bm"))
        {
            File.CreateText(songname + ".bm");
            return;
        }
        Debug.Log("Loading " + songname + "...");

        try
        {
            string line;
            StreamReader sReader = new StreamReader(songname + ".bm", Encoding.Default);
            do
            {
                line = sReader.ReadLine();
                if (line != null)
                {
                    // Lines with # are for comments
                    if (!line.Contains("#"))
                    {
                        string[] data = line.Split(':');
                        if (data.Length == 2)
                        {
                            switch (data[0].ToLower())
                            {
                                case "":
                                    //Beat
                                    string[] beatInfo = data[1].Split(',');
                                    int player = int.Parse(beatInfo[0]);
                                    float delay = RhythmController.GetDelay(asset.songBPM, asset.songOffset, float.Parse(beatInfo[1]));
                                    int beatIndex = int.Parse(beatInfo[2]);
                                    int type = 0;
                                    if (beatInfo.Length > 3)
                                    {
                                        type = int.Parse(beatInfo[3]);
                                    }
                                    asset.addBeat(player, delay, beatIndex, type);
                                    break;
                                case "song name":
                                    asset.song = _soundManager.getSound(int.Parse(data[1]));
                                    Debug.Log("Song name: " + data[1]);
                                    if (asset.song == null)
                                    {
                                        Debug.Log("Song cannot be found! " + data[1]);
                                    }
                                    break;
                                case "bpm":
                                    float bpm = float.Parse(data[1]);
                                    asset.songBPM = bpm;
                                    Debug.Log("BPM: " + bpm.ToString());
                                    break;
                                case "offset":
                                    float offset = float.Parse(data[1]);
                                    asset.songOffset = offset;
                                    Debug.Log("Offset: " + offset.ToString());
                                    break;
                                case "index":
                                    asset.index = int.Parse(data[1]);
                                    Debug.Log("Index: " + data[1]);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            while (line != null);
            sReader.Close();
            Debug.Log("Saving asset: " + asset.song.name + ", " + asset.index);
            saveAsset(asset);
            return;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
