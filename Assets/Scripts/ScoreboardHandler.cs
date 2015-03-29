using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public struct ScoreEntry
{
    public string name;
    public int score;
    public int time;
}

public class ScoreboardHandler : MonoBehaviour
{

    private string scoreFile = "scores.sb";
    public ScoreEntry bestScore;

    // Use this for initialization
    void Start()
    {
        bestScore = new ScoreEntry();
        bestScore.name = "";
        bestScore.score = 0;
        bestScore.time = 0;
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
                    bestScore.name = reader.ReadString();
                    bestScore.score = reader.ReadInt32();
                    bestScore.time = reader.ReadInt32();
					Debug.Log(bestScore.score.ToString());
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
                writer.Write("");
                writer.Write(0);
                writer.Write(0);
            }
        }
        catch (IOException e)
        {
            Debug.Log("ERROR: Failed to write new highscore file...");
            Debug.Log(e.Message);
        }
    }

    public void AddNewBestScore(string name, int score, int time)
    {
        bestScore.name = name;
        bestScore.score = score;
        bestScore.time = time;

		List<ScoreEntry> scores = GetAllEntries();
        using (BinaryWriter writer = new BinaryWriter(new FileStream(scoreFile, FileMode.Create)))
        {
			writer.Write(bestScore.name);
			writer.Write(bestScore.score);
			writer.Write(bestScore.time);

            foreach (ScoreEntry entry in scores)
            {
				writer.Write(entry.name);
				writer.Write(entry.score);
				writer.Write(entry.time);
            }
        }

    }

    public void AddScore(string name, int score, int time)
    {
        try
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(scoreFile, FileMode.Append)))
            {
                writer.Write(name);
                writer.Write(score);
                writer.Write(time);
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
                    bestScore.name = reader.ReadString();
                    bestScore.score = reader.ReadInt32();
                    bestScore.time = reader.ReadInt32();

                    try
                    {
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            ScoreEntry entry = new ScoreEntry();
                            entry.name = reader.ReadString();
                            entry.score = reader.ReadInt32();
                            entry.time = reader.ReadInt32();

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
