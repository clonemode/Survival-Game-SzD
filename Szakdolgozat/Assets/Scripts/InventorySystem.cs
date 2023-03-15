using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject ItemInfoUI;
    public static InventorySystem Instance { get; set; }
    public GameObject inventoryScreenUI;
    public GameObject slotsmanager;
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;
    public bool isOpen;


    //Pickup PopUp
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;
    public Sprite backpack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        isOpen = false;
        PopulateSlotList();
        Cursor.visible = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isOpen)
        {
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isOpen = true;
            SelectionManager.Instance.Crosshair.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.V) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SelectionManager.Instance.Crosshair.gameObject.SetActive(false);
            }
            isOpen = false;
            SelectionManager.Instance.Crosshair.gameObject.SetActive(false);
        }
        SelectionManager.Instance.Crosshair.gameObject.SetActive(true);
    }
    private void PopulateSlotList()
    {
        foreach (Transform child in slotsmanager.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    public void AddToInventory(string itemName)
    {
        whatSlotToEquip = FindNextEmptySlot();

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);
        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItem();
    }
    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;
        for (var i = slotList.Count-1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter !=0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);

                    counter -= 1;
                }
            }
        }
        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItem();
    }

    //other version removeItem
    /*
    public void RemoveItem(string nameToRemove, int amountToRemove) 
    {
        var itemsToRemove = slotList
        .Where(s => s.transform.childCount > 0 && s.transform.GetChild(0).name == nameToRemove + "(Clone)")
        .Take(amountToRemove);

        foreach (var item in itemsToRemove) 
        {
            Destroy(item.transform.GetChild(0).gameObject);
        }
    }
    */

    internal void ReCalculateList()
    {
        itemList.Clear();

        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name; //Stone (Clone)
                string str2 = "(Clone)";
                string result = name.Replace(str2,"");

                itemList.Add(result);
            }
        }
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckSlotAvailable(int emptyNeeded)
    {
        int emptySlot = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 0)
            {
                emptySlot += 1;
            } 
        }

        if (emptySlot >= emptyNeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
     
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        StartCoroutine(DisablePickupopUp());
    }
    public IEnumerator DisablePickupopUp()
    {
        yield return new WaitForSeconds(5);
        pickupAlert.SetActive(false);
    }
}