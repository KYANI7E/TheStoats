using UnityEngine;

public class PlayAudio : MonoBehaviour {
    private static PlayAudio instance;

    [SerializeField]
    private AudioSource[] players;

    // Ensure only one instance of PlayAudio exists in the scene
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        players = GameObject.FindGameObjectWithTag("audioplayer").GetComponentsInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

    }

    public static PlayAudio Instance {
        get {
            if (instance == null) {
                Debug.LogError("PlayAudio instance is null.");
            }
            return instance;
        }
    }

    public void PlayClip(AudioClip audioPath) {
        foreach (AudioSource audioSource in players) {
            if (!audioSource.isPlaying) {
                audioSource.clip = audioPath;
                audioSource.Play();
                return;
            }
        }
        Debug.LogWarning("No available audio sources to play: " + audioPath.name);
    }
}
