using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour
{
    [SerializeField] Player playerSO;
    [SerializeField] Config configSO;

    [SerializeField] bool resetPlayerToDefault = default;
    [SerializeField] Player defaultPlayerSO = default;
    
    static Data _instance;
    static ISaver _saver; 
    public static Player Player
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Data>();
            }
            return _instance.playerSO;
        }
        private set => _instance.playerSO = value;
    }
    public static Config Config
    {
        get
        {
            if (_instance == null)
            { 
                _instance = FindObjectOfType<Data>();
            }
            return _instance.configSO;
        }
        private set => _instance.configSO = value;
    }

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Init()
    {
        _saver = new PlayerPrefsSaver();
        
        if (_saver.IsAvailable() && !resetPlayerToDefault)
        {
            LoadPlayer();
        }
        else
        {
            ResetPlayer();
        }
    }

    public static void SavePlayer()
    {
        _saver.SavePlayer(Player);
    }

    static void LoadPlayer()
    {
        _saver.LoadPlayerTo(Player);
    }
    
    [ContextMenu("Reset Player To default")]
    void ResetPlayer()
    {
        JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(defaultPlayerSO), playerSO);
    }

    public static void DoHardReset()
    {
        _instance.ResetPlayer();
        SavePlayer();
        SceneManager.LoadScene(0);
    }

    //For bots
    public static Player CreateCopyOfDefaultPlayer()
    {
        var copy = ScriptableObject.CreateInstance<Player>();
        JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(_instance.defaultPlayerSO), copy);
        return copy;
    }
}
