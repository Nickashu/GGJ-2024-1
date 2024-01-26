using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {    //GameController ser� uma classe Singleton
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

    public enum DialogueTypes {   //Estes ser�o os tipos de di�logos poss�veis no jogo
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
        if (currentSpawnPoint != spawnPoints.Length - 1) {   //Se o jogador tiver passado do �ltimo checkpoint
            if (player.transform.position.x > spawnPoints[currentSpawnPoint+1].position.x && player.transform.position.y > spawnPoints[currentSpawnPoint+1].position.y) {  //Se chegamos no pr�ximo checkpoint, atualizamos o spawnPoint
                currentSpawnPoint++;
            }
        }
    }

    public void gameStartDialogue(int idTypeOfDialogue, string details) {    //Este m�todo ser� chamado sempre que um di�logo se iniciar
        typeOfDialogue = idTypeOfDialogue;
        int idDialogue=0;    //0 ser� o �ndice padr�o
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

    public void gameEndDialogue() {    //Este m�todo � chamado sempre que um di�logo termina
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
