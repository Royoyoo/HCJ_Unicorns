public interface ISaver
{
    void SavePlayer(Player savedSO);
    void LoadPlayerTo(Player targetSO);
    bool IsAvailable();
}