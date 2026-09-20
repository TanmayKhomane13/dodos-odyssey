using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SaveKey = "DodosOdyssey_Save";

    public PlayerController player;
    public EnemyController[] enemies;

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
        // -------------------------------------------------

        // ------------ Enemies ----------------------------
        saveData.enemies = new System.Collections.Generic.List<EnemySaveData>();

        foreach (EnemyController enemy in enemies)
        {
            EnemySaveData enemyData = new EnemySaveData();

            // ID
            enemyData.id = enemy.enemyID;

            // health
            enemyData.health = enemy.health;

            // position
            enemyData.posX = enemy.transform.position.x;
            enemyData.posY = enemy.transform.position.y;
            enemyData.posZ = enemy.transform.position.z;

            // rotation
            enemyData.rotX = enemy.transform.eulerAngles.x;
            enemyData.rotY = enemy.transform.eulerAngles.y;
            enemyData.rotZ = enemy.transform.eulerAngles.z;

            // dead state
            enemyData.isDead = enemy.IsDead();

            saveData.enemies.Add(enemyData);

        }
        // -------------------------------------------------

        string json = JsonUtility.ToJson(saveData);
        Debug.Log(json);

        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    // function to check if save exists
    public bool HasSave()
    {
        return PlayerPrefs.HasKey(SaveKey);
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

        // --------- Player --------------------------
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
        // -------------------------------------------

        // ------------ Enemies ----------------------
        foreach (EnemyController enemy in enemies)
        {
            foreach (EnemySaveData enemyData in saveData.enemies)
            {
                if (enemy.enemyID == enemyData.id)
                {
                    // health
                    enemy.health = enemyData.health;

                    // position
                    enemy.transform.position = new Vector3(
                        enemyData.posX,
                        enemyData.posY,
                        enemyData.posZ
                    );

                    // rotation
                    enemy.transform.eulerAngles = new Vector3(
                        enemyData.rotX,
                        enemyData.rotY,
                        enemyData.rotZ
                    );

                    // dead state
                    enemy.LoadState(enemyData.health, enemyData.isDead);

                    break;
                }
            }
        }

        return saveData;
    }
}
