using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour {    //Esta classe ser� �nica para todo o projeto (singleton class)
    private static DialogueController instance;

    public TextAsset variablesJSON;    //Este � o arquivo JSON do ink que cont�m todas as vari�veis de di�logo
    //public GameObject ImgCharacterDialogue;
    public GameObject DialogueBoxContainer;
    //public TextMeshProUGUI txtNameCharacter;
    public TextMeshProUGUI txtDialogue;
    public GameObject[] choices;
    public DialogueVariablesController dialogueVariablesController { get; private set; }

    private TextMeshProUGUI[] choicesTxt;
    private Story dialogue;

    private bool endLine = false;   //Esta vari�vel � respons�vel por guardar se cada linha do di�logo j� terminou ou ainda n�o
    [HideInInspector]
    public bool isInGame = true;
    private float textSpeed = 0.05f;
    private int indexLine;

    public bool dialogueActive { get; private set; }   //Quero que esta vari�vel possa ser lida por outros scripts, mas n�o modificada

    public static DialogueController GetInstance() {
        return instance;
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //Ao carregar pela primeira vez, precisamos carregar as vari�veis criadas no ink para o c�digo. Fa�o isso chamando o pr�prio construtor da classe DialogueVariablesController:
        dialogueVariablesController = new DialogueVariablesController(variablesJSON);
    }

    void Start() {
        if (!SceneManager.GetActiveScene().name.Contains("Main"))
            isInGame = false;
        if (isInGame) {
            DialogueBoxContainer.SetActive(false);
            dialogueActive = false;

            choicesTxt = new TextMeshProUGUI[choices.Length];   //O array deve ter o mesmo tamanho do n�mero de escolhas
            int index = 0;
            foreach (GameObject choice in choices) {
                choicesTxt[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                index++;
            }
        }
    }

    private void Update() {
        if (isInGame) {
            if (dialogueActive) {
                if (GameController.GetInstance().gameIsPaused) {     //Se for um di�logo que pausa o jogo
                    if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
                        PassDialogue();
                }
                else
                    StartCoroutine(passAutomaticDialogue());
            }
            if (endLine) {
                endLine = false;
                if (dialogue.currentChoices.Count > 0)
                    ShowChoices();
            }
        }
    }

    private IEnumerator passAutomaticDialogue() {    //Os di�logos que n�o pausam o jogo ser�o passados automaticamente
        yield return new WaitForSeconds(5);
        StopAllCoroutines();
        PassDialogue();
    }

    public void StartDialogue(TextAsset dialogueJSON) {
        if (dialogueActive) {   //Se j� estiver tendo algum di�logo que n�o pausou o jogo
            PassDialogue(true);
            txtDialogue.text = "";
            indexLine = 0;
        }
        dialogue = new Story(dialogueJSON.text);        //Carregando o di�logo a partir do arquivo JSON passado de par�metro
        dialogueActive = true;
        DialogueBoxContainer.SetActive(true);
        dialogueVariablesController.StartListening(dialogue);  //Para detectar as mudan�as de vari�veis no di�logo

        if (dialogue.canContinue) {
            dialogue.Continue();
            StartCoroutine(PrintDialogue());
        }
    }

    private void PassDialogue(bool endDialogue=false) {
        string fala = dialogue.currentText;

        if (indexLine < fala.Length - 1) {         //Se n�o estiver no final da fala
            StopAllCoroutines();
            indexLine = fala.Length - 1;
            endLine = true;
            txtDialogue.text = fala;
        }
        else {
            if (!endDialogue) {
                if (dialogue.currentChoices.Count == 0) {
                    //SoundController.GetInstance().PlaySound("skip_dialogo", null);
                    if (!dialogue.canContinue)     //Se estiver no final do di�logo
                        EndDialogue();
                    else {
                        dialogue.Continue();
                        StartCoroutine(PrintDialogue());
                    }
                }
            }
        }
    }

    //Fun��o que printa cada linha do di�logo na caixa de di�logo
    private IEnumerator PrintDialogue() {
        //ChangeCharacterDialogue();
        string fala = dialogue.currentText;    //Pegando a fala atual do di�logo

        txtDialogue.text = "";
        for (int i = 0; i < fala.Length; i++) {    //Fazendo as letras aparecerem uma de cada vez
            txtDialogue.text += fala[i];
            indexLine = i;
            yield return new WaitForSeconds(textSpeed);
        }
        endLine = true;
    }

    private void EndDialogue() {   //M�todo chamado ao fim do di�logo
        DialogueBoxContainer.GetComponent<Animator>().SetBool("Off", true);   //Fazendo a caixa de di�logo desaparecer
        dialogueVariablesController.StopListening(dialogue);  //Para parar de detectar as mudan�as de vari�veis no di�logo
        GameController.GetInstance().gameEndDialogue();
        //GameController.checkVariablesDialogue(dialogueVariablesController.variablesValues);    //Fazendo as checagens de vari�veis importantes que podem ter mudado ap�s um di�logo
    }
    public void endAnimationDialogueBoxOff() {   //Quando a caixa de di�logo desaparecer
        txtDialogue.text = "";
        dialogueActive = false;
    }


    private void ShowChoices() {    //Fun��o para mostrar as escolhas do di�logo
        List<Choice> choicesList = dialogue.currentChoices;   //Recuperando as escolhas do di�logo

        int index = 0;
        foreach (Choice choice in choicesList) {
            choicesTxt[index].text = choice.text;
            choices[index].SetActive(true);
            index++;
        }
        for (int i = index; i < choices.Length; i++) {   //Escondendo as escolhas que n�o fazem parte do di�logo
            choices[i].SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex) {    //Fun��o para fazer uma escolha no di�logo
        dialogue.ChooseChoiceIndex(choiceIndex);
        foreach (GameObject choice in choices) {
            choice.SetActive(false);
        }

        if (!dialogue.canContinue)     //Se estiver no final do di�logo
            EndDialogue();
        else {
            dialogue.Continue();
            StartCoroutine(PrintDialogue());
        }
    }

    /*
    private void ChangeCharacterDialogue() {   //Fun��o para mudar o sprite do personagem do di�logo
        List<string> tagsDialogueLine = dialogue.currentTags;   //As tags s�o: nome do personagem e sprite do personagem
        string characterName = "", spriteCharacter = "";
        foreach (string tag in tagsDialogueLine) {
            if (tag.Split(":")[0].Trim() == "character")
                characterName = tag.Split(":")[1].Trim().ToUpper();
            else if (tag.Split(":")[0].Trim() == "state")
                spriteCharacter = tag.Split(":")[1].Trim();
        }
        //if (spriteCharacter != "")
        //    ImgCharacterDialogue.GetComponent<Animator>().Play(spriteCharacter);
        //if (characterName != "")
        //    txtNameCharacter.text = characterName;
    }
    */

    public Ink.Runtime.Object GetVariableState(string variableName) {    //Esta fun��o servir� para recuperar o estado de determinada vari�vel de di�logo
        Ink.Runtime.Object variableValue = null;
        dialogueVariablesController.variablesValues.TryGetValue(variableName, out variableValue);
        if (variableValue == null) {
            Debug.Log("N�o foi poss�vel recuperar o valor da vari�vel de di�logo informada.");
            return null;
        }
        return variableValue;
    }
}
