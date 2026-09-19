using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SaveKey = "DodosOdyssey_Save";
    public PlayerController player;

    // Saving the states
    public void SaveGame()
    {
        GameSaveData saveData = new GameSaveData();

        // ---------- Player-------------------------------
        saveData.player = new PlayerSaveData();

        // health
        saveData.player.health = player.health;

        // position
        saveData.player.posX = player.transform.position.x;
        saveData.player.posY = player.transform.position.y;
        saveData.player.posZ = player.transform.position.z;

        // rotation
        saveData.player.rotX = player.transform.eulerAngles.x;
        saveData.player.rotY = player.transform.eulerAngles.y;
        saveData.player.rotZ = player.transform.eulerAngles.z;

        string json = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    // Loading the states
    public GameSaveData LoadGame()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            return null;
        }

        string json = PlayerPrefs.GetString(SaveKey);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

        player.health = saveData.player.health;

        player.transform.position = new Vector3(
            saveData.player.posX,
            saveData.player.posY,
            saveData.player.posZ
        );

        player.transform.eulerAngles = new Vector3(
            saveData.player.rotX,
            saveData.player.rotY,
            saveData.player.rotZ
        );

        return saveData;
    }
}
