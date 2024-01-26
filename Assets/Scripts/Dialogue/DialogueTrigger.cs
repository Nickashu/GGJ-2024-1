using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public TextAsset[] dialogueJSON;

    public void TriggerDialogue(int idDialogue=0) {
        if (!DialogueController.GetInstance().dialogueActive) {
            bool pauseGame = false;
            if (!gameObject.name.Contains("Joke"))   //Se for um diálogo principal do jogo ou um diálogo pós morte
                pauseGame = true;
            DialogueController.GetInstance().StartDialogue(dialogueJSON[idDialogue], pauseGame);
        }
    }
}
