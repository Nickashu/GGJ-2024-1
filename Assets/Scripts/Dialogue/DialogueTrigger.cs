using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public TextAsset[] dialogueJSON;

    public void TriggerDialogue(int idDialogue) {
        if (!DialogueController.GetInstance().dialogueActive) {
            DialogueController.GetInstance().StartDialogue(dialogueJSON[idDialogue]);
        }
    }
}
