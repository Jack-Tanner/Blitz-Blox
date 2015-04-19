using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

//0 = Blockade
//1 = Arcade

public struct ScoreEntry
{
    public int type;
    public int score;
}

public class ScoreboardHandler : MonoBehaviour
{

    private string scoreFile = "scores.sb";
    public ScoreEntry bestArcadeScore;
    public ScoreEntry bestBlockadeScore;

    // Use this for initialization
    void Start()
    {
        bestBlockadeScore = new ScoreEntry();
        bestBlockadeScore.type = 0;
        bestBlockadeScore.score = 0;

        bestArcadeScore = new ScoreEntry();
        bestArcadeScore.type = 1;
        bestArcadeScore.score = 0;
        ReadHeader();
    }

    private bool ReadHeader()
    {
        if (File.Exists(scoreFile))
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(scoreFile, FileMode.Open)))
                {
                    bestBlockadeScore.type = reader.ReadInt32();
                    bestBlockadeScore.score = reader.ReadInt32();

                    bestArcadeScore.type = reader.ReadInt32();
                    bestArcadeScore.score = reader.ReadInt32();
                    return true;
                }
            }
            catch (FileLoadException)
            {
                Debug.Log("Failed to load highscores.. creating new file.");
            }
        }
        else
        {
            CreateNewFile();
        }

        return false;
    }

    private void CreateNewFile()
    {
        try
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(scoreFile, FileMode.Create)))
            {
                writer.Write(0);
                writer.Write(0);

                writer.Write(1);
                writer.Write(0);
            }
        }
        catch (IOException e)
        {
            Debug.Log("ERROR: Failed to write new highscore file...");
            Debug.Log(e.Message);
        }
    }

    public void AddNewBestScore(int score, int gamemode)
    {
		List<ScoreEntry> scores = GetAllEntries();
        
        using (BinaryWriter writer = new BinaryWriter(new FileStream(scoreFile, FileMode.Create)))
        {
            if(gamemode == 0)
            {
                writer.Write(0);
                writer.Write(score);

                writer.Write(1);
                writer.Write(bestArcadeScore.score);

                writer.Write(0);
                writer.Write(bestBlockadeScore.score);
            }
            else
            {
                writer.Write(0);
                writer.Write(bestBlockadeScore.score);
                
                writer.Write(1);
                writer.Write(score);

                writer.Write(1);
                writer.Write(bestArcadeScore.score);
            }

            

            foreach (ScoreEntry entry in scores)
            {
				writer.Write(entry.type);
				writer.Write(entry.score);
            }
        }

        if (gamemode == 0)
        {
            bestBlockadeScore.score = score;
        }
        else
        {
            bestArcadeScore.score = score;
        }

    }

    public void AddScore(int score, int gamemode)
    {
        try
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(scoreFile, FileMode.Append)))
            {
                writer.Write(gamemode);
                writer.Write(score);
            }
        }
        catch (IOException e)
        {
            Debug.Log("ERROR: Failed to write new highscore file...");
            Debug.Log(e.Message);
        }
    }

    public List<ScoreEntry> GetAllEntries()
    {
        List<ScoreEntry> scores = new List<ScoreEntry>();

        if (File.Exists(scoreFile))
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(scoreFile, FileMode.Open)))
                {
                    try
                    {
                        bestBlockadeScore.type = reader.ReadInt32();
                        bestBlockadeScore.score = reader.ReadInt32();

                        bestArcadeScore.type = reader.ReadInt32();
                        bestArcadeScore.score = reader.ReadInt32();

                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            ScoreEntry entry = new ScoreEntry();
                            entry.type = reader.ReadInt32();
                            entry.score = reader.ReadInt32();

                            scores.Add(entry);
                        }
                    }
                    catch (IOException)
                    {

                        Debug.Log("ERROR: Failed to read entry.. File might be corrupt.");
                    }

                }
            }
            catch (FileLoadException)
            {
                Debug.Log("Failed to load highscores..");
            }
        }

        return scores;
    }
}
