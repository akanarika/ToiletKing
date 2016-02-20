using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class Config : Singleton<Config> {
    public string filename = "config.ini";

    public string[][] playerControls;

    void Awake()
    {
        playerControls = new string[4][];
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

        Debug.Log("FILE LOADED");
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
                                case "player 1 controls":
                                    playerControls[0] = data[1].Trim().Split(',');
                                    Debug.Log("Player 1 Controls: ");
                                    foreach (string val in playerControls[0])
                                    {
                                        Debug.Log(val);
                                    }
                                    break;
                                case "player 2 controls":
                                    playerControls[1] = data[1].Trim().Split(',');
                                    Debug.Log("Player 2 Controls: ");
                                    foreach (string val in playerControls[1])
                                    {
                                        Debug.Log(val);
                                    }
                                    
                                    break;
                                case "player 3 controls":
                                    playerControls[2] = data[1].Trim().Split(',');
                                    Debug.Log("Player 3 Controls: ");
                                    foreach (string val in playerControls[2])
                                    {
                                        Debug.Log(val);
                                    }
                                    
                                    break;
                                case "player 4 controls":
                                    playerControls[3] = data[1].Trim().Split(',');
                                    Debug.Log("Player 4 Controls: ");
                                    foreach (string val in playerControls[3])
                                    {
                                        Debug.Log(val);
                                    }
                                    
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
            Debug.Log(e);
        }
    }
}
