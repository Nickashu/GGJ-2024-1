using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {    //GameController será uma classe Singleton
    private static GameController instance;
    public Transform[] spawnPoints;
    public GameObject player, objDialogueDeath, objDialogueLore, objDialogueJoke;

    [HideInInspector]
    public int currentSpawnPoint=0, typeOfDialogue=-1;
    public bool gameIsPaused=false;
    public float limitMinYMap, limitMaxYMap;

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

    void Update() {
        if (currentSpawnPoint != spawnPoints.Length - 1) {   //Se o jogador tiver passado do último checkpoint
            if (player.transform.position.x > spawnPoints[currentSpawnPoint+1].position.x && player.transform.position.y > spawnPoints[currentSpawnPoint+1].position.y) {  //Se chegamos no próximo checkpoint, atualizamos o spawnPoint
                currentSpawnPoint++;
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
}
