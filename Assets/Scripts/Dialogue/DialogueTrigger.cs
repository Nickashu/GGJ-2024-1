using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public TextAsset[] dialogueJSON;

    public void TriggerDialogue(int idDialogue) {
        DialogueController.GetInstance().StartDialogue(dialogueJSON[idDialogue]);
    }
}
