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
    private List<GameObject> _listDestructible;
    public List<GameObject> ListDestructible {
        get {
            _listDestructible.Clear();
            Destructible[] dest = FindObjectsOfType<Destructible>();
            _listDestructible.Capacity = dest.Length;

            for (int i = 0; i < dest.Length; i++) {
                _listDestructible.Insert(i, dest[i].gameObject);
            }
            return _listDestructible;
        }
        private set { _listDestructible = value; }
    }

    private void Initialize() {

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
        ListDestructible = new List<GameObject>();
    }

}
