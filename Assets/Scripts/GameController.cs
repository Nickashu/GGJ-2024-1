using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {    //GameController será uma classe Singleton
    private static GameController instance;
    public Transform[] spawnPoints;
    public GameObject player, objDialogueDeath, objDialogueLore, objDialogueJoke, canvasPause;

    [HideInInspector]
    public int currentSpawnPoint=0, typeOfDialogue=-1;
    [HideInInspector]
    public bool gameIsPaused=false, gameStopped=false;
    public float limitMinYMap, limitMaxYMap;
    private bool isInGame;

    private Dictionary<string, int> jokeDialoguesDictionary = new Dictionary<string, int> {
        {"jokeTest", 1 },
        {"jokeSpikes", 2 }
    };
    private Dictionary<string, int> loreDialoguesDictionary = new Dictionary<string, int> {
        {"lore0", 1 },
        {"lore1", 2 }
    };
    private Dictionary<string, int> deathDialoguesDictionary = new Dictionary<string, int> {
        {"deathOffLimits", 1 },
        {"deathSpikes", 2 }
    };

    public enum DialogueTypes {   //Estes serão os tipos de diálogos possíveis no jogo
        Death,
        Lore,
        Joke
    }


    public static GameController GetInstance() {
        return instance;
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        if (!SceneManager.GetActiveScene().name.Contains("Main"))    //Se não estivermos na cena do jogo
            isInGame = false;
        else
            isInGame = true;
    }

    void Update() {
        if (isInGame) {
            if (currentSpawnPoint != spawnPoints.Length - 1) {   //Se o jogador tiver passado do último checkpoint
                if (player.transform.position.x > spawnPoints[currentSpawnPoint + 1].position.x && player.transform.position.y > spawnPoints[currentSpawnPoint + 1].position.y) {  //Se chegamos no próximo checkpoint, atualizamos o spawnPoint
                    currentSpawnPoint++;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (gameStopped) {
                    gameStopped = false;
                    canvasPause.SetActive(false);
                }
                else {
                    gameStopped = true;
                    canvasPause.SetActive(true);
                }
            }
        }
    }

    public void gameStartDialogue(int idTypeOfDialogue, string details) {    //Este método será chamado sempre que um diálogo se iniciar
        typeOfDialogue = idTypeOfDialogue;
        int idDialogue=0;    //0 será o índice padrão
        switch (typeOfDialogue) {
            case (int)DialogueTypes.Death:
                gameIsPaused = true;
                if (deathDialoguesDictionary.ContainsKey(details))
                    idDialogue = deathDialoguesDictionary[details];
                objDialogueDeath.GetComponent<DialogueTrigger>().TriggerDialogue(idDialogue);
                break;
            case (int)DialogueTypes.Joke:
                if (jokeDialoguesDictionary.ContainsKey(details))
                    idDialogue = jokeDialoguesDictionary[details];
                objDialogueJoke.GetComponent<DialogueTrigger>().TriggerDialogue(idDialogue);
                break;
            case (int)DialogueTypes.Lore:
                gameIsPaused = true;
                if (loreDialoguesDictionary.ContainsKey(details))
                    idDialogue = loreDialoguesDictionary[details];
                objDialogueLore.GetComponent<DialogueTrigger>().TriggerDialogue(idDialogue);
                break;
        }
    }

    public void gameEndDialogue() {    //Este método é chamado sempre que um diálogo termina
        switch (typeOfDialogue) {
            case (int)DialogueTypes.Death:
                StartCoroutine(RespawnPlayer());
                break;
            //case (int)DialogueTypes.Joke:
            //    break;
            case (int)DialogueTypes.Lore:
                gameIsPaused = false;
                break;
        }
        typeOfDialogue = -1;
    }


    private IEnumerator RespawnPlayer() {
        yield return new WaitForSeconds(2);
        player.transform.position = spawnPoints[currentSpawnPoint].position;
        player.SetActive(true);
        player.GetComponent<Animator>().SetBool("respawn", true);
    }

    public void ResetGame() {
        gameStopped = false;
        Debug.Log("Jogo resetado!");
    }

    public bool gamePaused() {
        if (DialogueController.GetInstance().dialogueActive || gameIsPaused || gameStopped)
            return true;
        return false;
    }





    public void returnToMenu() {
        TransitionsController.GetInstance().LoadMenu();
    }
    public void resumeGame() {
        gameStopped = false;
        canvasPause.SetActive(false);
    }
}
