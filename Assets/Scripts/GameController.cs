using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour {    //GameController será uma classe Singleton
    private static GameController instance;
    public Transform[] spawnPoints;
    public GameObject player;

    [HideInInspector]
    public int currentSpawnPoint=0;  //Esta variável vai armazenar o último spawn do jogador
    public bool playerIsRespawning = false, gameIsPaused=false;
    public float limitMinYMap, limitMaxYMap;

    public static GameController GetInstance() {
        return instance;
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update() {
        if (playerIsRespawning) {
            gameIsPaused = true;
            StartCoroutine(RespawnPlayer());
            playerIsRespawning = false;
        }

        if (currentSpawnPoint != spawnPoints.Length - 1) {   //Se o jogador tiver passado do último checkpoint
            if (player.transform.position.x > spawnPoints[currentSpawnPoint+1].position.x && player.transform.position.y > spawnPoints[currentSpawnPoint+1].position.y) {  //Se chegamos no próximo checkpoint, atualizamos o spawnPoint
                currentSpawnPoint++;
            }
        }
    }


    private IEnumerator RespawnPlayer() {
        yield return new WaitForSeconds(5);
        player.transform.position = spawnPoints[currentSpawnPoint].position;
        player.SetActive(true);
        player.GetComponent<Animator>().SetBool("respawn", true);
        Debug.Log("Tempo de respawn terminou!");
    }
}
