using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    private RagdollManager _ragdollManager;
    [SerializeField] private TextMeshPro WASD;
    [SerializeField] private TextMeshPro Jump;
    [SerializeField] private TextMeshPro Crate;
    [SerializeField] private TextMeshPro KeyUp;
    [SerializeField] private TextMeshPro KeyDown;
    [SerializeField] private GameObject crate;
    private Vector3 _crateStartPos;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        _ragdollManager = FindObjectOfType<RagdollManager>();
        WASD.color = Color.black;
        Jump.color = Color.black;
        Crate.color = Color.black;
        KeyUp.color = Color.black;
        KeyDown.color = Color.black;
        _crateStartPos = crate.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // GREEN AND DESTROY
        
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            WASD.color = Color.green;
            StartCoroutine(DestroyInstruction(WASD));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump.color = Color.green;
            StartCoroutine(DestroyInstruction(Jump));
        }

        

        if (_ragdollManager.hasKey)
        {
            KeyUp.color = Color.green;
            StartCoroutine(DestroyInstruction(KeyUp));
        }
        

        if (!_ragdollManager.hasKey && Input.GetKey(KeyCode.G))
        {
            KeyDown.color = Color.green;
            StartCoroutine(DestroyInstruction(KeyDown));
        }
        
        float distance = Vector3.Distance(_crateStartPos, crate.transform.position);
        if (distance > 3f)
        {
            Crate.color = Color.green;
            StartCoroutine(DestroyInstruction(Crate));
        }
        
    }
    
    IEnumerator DestroyInstruction(TextMeshPro text)
    {
        yield return new WaitForSeconds(3);
        text.gameObject.SetActive(false);
    }
    
   
}
