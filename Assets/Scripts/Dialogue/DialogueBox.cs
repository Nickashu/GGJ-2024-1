using UnityEngine;

public class DialogueBox : MonoBehaviour {
    private void endAnimationOff() {
        gameObject.SetActive(false);
        DialogueController.GetInstance().endAnimationDialogueBoxOff();
    }
}
