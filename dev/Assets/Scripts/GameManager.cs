using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public static GameManager Instance {
        get {
            if (_instance == null) {
                GameObject gameManagerGameObject = Resources.Load<GameObject>("GameManager");
                GameObject managerObject = Instantiate(gameManagerGameObject);
                _instance = managerObject.GetComponent<GameManager>();
                _instance.Initialize();

                // Prevents having to recreate the manager on scene change
                // https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    public GameObject Player { get; private set; }
    public ChunkManager ChunkManager { get; private set; }
    public MeshGenerator MeshGenerator { get; private set; }
    public List<GameObject> listDestructible;

    private void Initialize() {

        listDestructible = new List<GameObject>();

        SceneManager.sceneLoaded += OnSceneLoaded;


        OnSceneLoaded();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        OnSceneLoaded();
    }

    private void OnSceneLoaded() {
        Player = GameObject.FindGameObjectWithTag("Player");
        ChunkManager = FindObjectOfType<ChunkManager>().GetComponent<ChunkManager>();
        MeshGenerator = FindObjectOfType<MeshGenerator>().GetComponent<MeshGenerator>();
    }

}
