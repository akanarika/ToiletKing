using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class Config : Singleton<Config> {
    public static string filename = "config.ini";

    public static int GameDuration;
    public static int ActiveTime;
    public static int TrapSteps;
    public static float DownTime;
    public static float AirTime;
    public static int JumpSpeed;
    public static int PowerMinCooldown;
    public static int PowerMaxCooldown;
    public static int EndScreenTimeout;

    void Start()
    {
        Debug.Log("FILE LOADED");
        loadFile(filename);
        // Other stuff 
    }

    void loadFile(string filename)
    {
        if (!File.Exists(filename))
        {
            File.CreateText(filename);
            return;
        }

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
                    if (!line.Contains("#"))
                    {
                        // Value property identified by string before the colon.
                        string[] data = line.Split(':');
                        if (data.Length == 2)
                        {
                            switch (data[0].ToLower())
                            {
                                case "game duration":
                                    GameDuration = int.Parse(data[1].Trim());
                                    Debug.Log("Game Duration: " + GameDuration.ToString());
                                    break;
                                case "active time":
                                    ActiveTime = int.Parse(data[1].Trim());
                                    Debug.Log("Active time: " + ActiveTime.ToString());
                                    break;
                                case "trap steps":
                                    TrapSteps = int.Parse(data[1].Trim());
                                    Debug.Log("Trap Steps: " + TrapSteps.ToString());
                                    break;
                                case "down time":
                                    DownTime = int.Parse(data[1].Trim()) * 1.0f / 1000;
                                    Debug.Log("Down Time: " + DownTime.ToString());
                                    break;
                                case "air time":
                                    AirTime = int.Parse(data[1].Trim()) * 1.0f / 1000;
                                    Debug.Log("Air Time: " + AirTime.ToString());
                                    break;
                                case "jump speed":
                                    JumpSpeed = int.Parse(data[1].Trim());
                                    Debug.Log("Jump Speed: " + JumpSpeed.ToString());
                                    break;
                                case "power minimum cooldown":
                                    PowerMinCooldown = int.Parse(data[1].Trim());
                                    Debug.Log("Power Min Cooldown: " + JumpSpeed.ToString());
                                    break;
                                case "power maximum cooldown":
                                    PowerMinCooldown = int.Parse(data[1].Trim());
                                    Debug.Log("Power Max Cooldown: " + JumpSpeed.ToString());
                                    break;
                                case "end screen timeout":
                                    EndScreenTimeout = int.Parse(data[1].Trim());
                                    Debug.Log("End Screen Timeout: " + JumpSpeed.ToString());
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
            return;
        }
        catch (Exception e)
        {
        }
    }
}
