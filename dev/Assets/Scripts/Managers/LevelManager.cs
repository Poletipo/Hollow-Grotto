using UnityEngine.SceneManagement;

public static class LevelManager {
    public enum Level {
        Invalid = -1,

        // Define value by hand
        MainMenu = 0,
        MainScene = 1
    }

    public static void LoadLevel(Level level)
    {
        SceneManager.LoadScene(level.ToString());
    }

}
