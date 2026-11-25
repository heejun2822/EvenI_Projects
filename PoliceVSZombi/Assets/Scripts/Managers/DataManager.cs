using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private CharacterDatasLoader _characterDatasLoader;
    public CharacterDatasLoader CharacterDatasLoader
    {
        get { return _characterDatasLoader; }
    }

    private PlayerStatDatasLoader _playerStatDatasLoader;
    public PlayerStatDatasLoader PlayerStatDatasLoader
    {
        get { return _playerStatDatasLoader; }
    }

    private ExperienceDatasLoader _experienceDatasLoader;
    public ExperienceDatasLoader ExperienceDatasLoader
    {
        get { return _experienceDatasLoader; }
    }

    private EnemyDatasLoader _enemyDatasLoader;
    public EnemyDatasLoader EnemyDatasLoader
    {
        get { return _enemyDatasLoader; }
    }

    private EnemyStatDatasLoader _enemyStatDatasLoader;
    public EnemyStatDatasLoader EnemyStatDatasLoader
    {
        get { return _enemyStatDatasLoader; }
    }

    private WaveDatasLoader _waveDatasLoader;
    public WaveDatasLoader WaveDatasLoader
    {
        get { return _waveDatasLoader; }
    }

    private void Awake()
    {
        _characterDatasLoader = new CharacterDatasLoader();
        _playerStatDatasLoader = new PlayerStatDatasLoader();
        _experienceDatasLoader = new ExperienceDatasLoader();
        _enemyDatasLoader = new EnemyDatasLoader();
        _enemyStatDatasLoader = new EnemyStatDatasLoader();
        _waveDatasLoader = new WaveDatasLoader();
    }
}

