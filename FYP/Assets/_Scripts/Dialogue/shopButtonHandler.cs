using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class shopButtonHandler : MonoBehaviour
{
    [SerializeField] private Weapon item;
    [SerializeField] private Image[] itemSprites;
    [SerializeField] private Transform ResourcesBar;
    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private float quantityYOffset;
    [SerializeField] private Color textColor;
    private bool canAfford = true;

    void OnEnable()
    {
        foreach (Image x in itemSprites) x.sprite = item.image;
        int counter = 0;
        foreach (Resource x in item.recipe)
        {
            GameObject icon = Instantiate(new GameObject(), ResourcesBar);
            Image sr = icon.AddComponent<Image>();
            sr.sprite = x.resourceSprite;

            GameObject value = Instantiate(new GameObject(), icon.transform);
            TMPro.TextMeshProUGUI tm = value.AddComponent<TMPro.TextMeshProUGUI>();
            tm.text = item.quantities[counter].ToString();
            tm.color = textColor;
            tm.alignment = TMPro.TextAlignmentOptions.Center;
            tm.font = font;

            value.GetComponent<RectTransform>().localPosition = new Vector3(0, quantityYOffset, 0);
            value.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);

            //if(canAfford)
            //    canAfford = ResourceManager.Instance.checkQuantity(x, item.quantities[counter]);

            counter++;
        }

        //GameObject blocker = transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject;
        //blocker.SetActive(!canAfford);

        evaluateAffordability();

        ResourcesBar.GetComponent<HorizontalLayoutGroup>().spacing = item.recipe.Length > 2 ? 2 : 8;
    }

    public void evaluateAffordability()
    {
        canAfford = true;
        int counter = 0;
        foreach (Resource y in item.recipe)
        {
            if (canAfford)
                canAfford = ResourceManager.Instance.checkQuantity(y, item.quantities[counter]);
            counter++;
        }

        GameObject blocker = transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject;
        blocker.SetActive(!canAfford);
    }

    public void buyWeapon()
    {
        int counter = 0;
        foreach (Resource x in item.recipe)
        {
            ResourceManager.Instance.reduceResource(x, item.quantities[counter]);
            counter++;
        }
        
        PlayerManager.Instance.GetComponentInChildren<AttackHandler>().setWeapon(item);
        

        foreach (GameObject button in GameObject.FindGameObjectsWithTag("ShopButton"))
            button.GetComponent<shopButtonHandler>().evaluateAffordability();
    }

    private void OnDisable()
    {
        foreach (Transform x in ResourcesBar.transform) Destroy(x.gameObject);
    }
}