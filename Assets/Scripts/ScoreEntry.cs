[System.Serializable]
public class ScoreEntry
{
    public string username;
    public int score;
    public string date;
    public bool isLatest; 

    public ScoreEntry(string user, int s)
    {
        username = user;
        score = s;
        date = System.DateTime.Now.ToString("MMM dd, hh:mm tt");
        isLatest = true;
    }
}