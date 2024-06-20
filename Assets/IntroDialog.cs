using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroDialog : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dialogs;

    [SerializeField] private GameObject EntireIntroDialog;
    [SerializeField]
    private GameObject healthBar;
    private int currentDialog = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure only the first dialog is active at the start
        if (dialogs.Length > 0)
        {
            for (int i = 0; i < dialogs.Length; i++)
            {
                dialogs[i].SetActive(i == currentDialog);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for spacebar or enter key press
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            NextDialog();
        }
    }

    public void NextDialog()
    {  
        if (currentDialog < dialogs.Length-1)
        {
            dialogs[currentDialog].SetActive(false);
            currentDialog++;
            if (currentDialog < dialogs.Length)
            {
                dialogs[currentDialog].SetActive(true);
            }
        } else {
            healthBar.SetActive(true);            
            EntireIntroDialog.SetActive(false);
            SceneManager.LoadScene("TutorialLevel");
        }
    }
}