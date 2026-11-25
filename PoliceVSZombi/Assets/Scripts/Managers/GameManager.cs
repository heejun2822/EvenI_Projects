using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool isPlaying = false;
    public bool IsPlaying { get => isPlaying; }

    [SerializeField] private Player player;
    public Player Player { get { return player; } }

    [SerializeField] PrefabDatabase prefabDatabase;
    public PrefabDatabase PrefabDatabase { get { return prefabDatabase; } }

    DataManager dataManager;
    ItemDropManager itemDropManager;
    ProjectileManager projectileManager;


    [SerializeField] int currentWaveIndex;
    private int currentSpawnCount = 0;
    public float spawnInterval = .5f;

    [SerializeField] EnemySpawner enemySpawner;

    public event Action<int> OnWaveStart;

    [SerializeField] CameraShake cameraShake;

    void Start()
    {
        dataManager = DataManager.Instance;
        itemDropManager = ItemDropManager.Instance;
        projectileManager = ProjectileManager.Instance;

        prefabDatabase.Initialize();

        player.Initialize();
        player.HealthSystem.OnDeath += GameOver;
        player.HealthSystem.OnDamage += () => { ShakeCamera(0.2f, 2, 1); };

        StartGame();
    }

    public void StartGame()
    {
        //isPlaying = true;
        Pause(false);
        player.ResetPlayer();

        currentSpawnCount = 0;
        currentWaveIndex = 0;

        StartCoroutine("StartNextWave");
        // StopCoroutine("StartNextWave");

        // Coroutine c = StartCoroutine(StartNextWave());
        // StopCoroutine(c);
    }

    public void ResetGame()
    {
        projectileManager.ResetObjecPool();

        DestroyOfType<Enemy>();
        DestroyOfType<DropItem>();

        cameraShake.StopShake();

        StartGame();
    }

    private void DestroyOfType<T>() where T : Component
    {
        T[] objs = GameObject.FindObjectsOfType<T>();

        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }
    }

    IEnumerator StartNextWave()
    {
        while (true)
        {
            if(currentSpawnCount == 0)
            {
                WaveDatas waveData = dataManager.WaveDatasLoader.GetByKey(currentWaveIndex + 1);
                OnWaveStart?.Invoke(currentWaveIndex + 1);

                yield return new WaitForSeconds(2);

                for (int i = 0; i < waveData.enemies.Count; i++)
                {
                    int enemyId = waveData.enemies[i];
                    int enemyCount = waveData.counts[i];

                    EnemyDatas enemyData = dataManager.EnemyDatasLoader.GetByKey(enemyId);
                    GameObject enemyPrefab = prefabDatabase.GetPrefabByKey(enemyData.prefabId);

                    // 일반 몬스터
                    for (int j = 0; j < enemyCount; j++)
                    {
                        GameObject enemyObject = enemySpawner.SpawnEnemy(enemyPrefab);

                        Enemy enemy = enemyObject.GetComponent<Enemy>();
                        enemy.Initialize(enemyId);
                        enemy.HealthSystem.OnDeath += () => OnEnemyDeath(enemy.GetExperience());
                        enemy.HealthSystem.OnDeath += () => DropItem(enemy);

                        currentSpawnCount++;
                        yield return new WaitForSeconds(spawnInterval);
                    }

                }
                
                if(waveData.bossID != 0)
                {
                    EnemyDatas enemyData = dataManager.EnemyDatasLoader.GetByKey(waveData.bossID);
                    GameObject enemyPrefab = prefabDatabase.GetPrefabByKey(enemyData.prefabId);

                    GameObject enemyObject = enemySpawner.SpawnEnemy(enemyPrefab);

                    Enemy enemy = enemyObject.GetComponent<Enemy>();
                    enemy.Initialize(waveData.bossID);
                    enemy.HealthSystem.OnDeath += () => OnEnemyDeath(enemy.GetExperience());
                    enemy.HealthSystem.OnDeath += () => DropItem(enemy);

                    ShakeCamera(3, 5, 2);

                    currentSpawnCount++;
                }


                currentWaveIndex++;
            }

            yield return null;
        }

    }

    private void OnEnemyDeath(int experience)
    {
        currentSpawnCount--;
        player.GainExperience(experience); 
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        // isPlaying = false;
        StopAllCoroutines();
        UIPanelManager.Instance.OpenPanel(UIType.GameOverPanel);
    }

    void DropItem(Enemy enemy)
    {
        itemDropManager.DropRandomWeapon(enemy.GetDropRate(), enemy.transform.position);
    }

    public void Pause(bool isPause)
    {
        isPlaying = !isPause;

        if (isPause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        if (cameraShake == null)
            return;

        cameraShake.ShakeCamera(duration, amplitude, frequency);
    }


}

