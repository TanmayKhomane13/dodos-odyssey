using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    // Player
    public PlayerSaveData player;

    // Enemies
    public List<EnemySaveData> enemies;

    // Boss
    public BossSaveData boss;

    // Game progress
    public GameProgressData progress;
}

[Serializable]
public class PlayerSaveData
{
    public float health;

    public float posX;
    public float posY;
    public float posZ;

    public float rotX;
    public float rotY;
    public float rotZ;
}

[Serializable]
public class EnemySaveData
{
    public string id;

    public bool isDead;
    public float health;

    public float posX;
    public float posY;
    public float posZ;

    public float rotX;
    public float rotY;
    public float rotZ;
}

[Serializable]
public class BossSaveData
{
    public bool isDead;
    public float health;

    public float posX;
    public float posY;
    public float posZ;

    public float rotX;
    public float rotY;
    public float rotZ;
}

[Serializable]
public class GameProgressData
{
    public string currentLevel;
    public bool levelCompleted;
}