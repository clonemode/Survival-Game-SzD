using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public GameObject interaction_Info_UI;
    public GameObject selectedObject;
    public Image Crosshair;
    Text interaction_text;
    public bool onTarget;
    public bool handIsVisible;

    private void Start()
    {
        onTarget = true;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }
    private void Awake()
    {
        if(Instance!=null&& Instance!= this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable && interactable.playerInRange && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
                handIsVisible = true;
            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                handIsVisible = false;
            }
        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
        }
    }
}
