using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class shopButtonHandler : MonoBehaviour
{
    [SerializeField] Weapon item;
    [SerializeField] Image[] itemSprites;
    [SerializeField] Transform ResourcesBar;
    [SerializeField] float quantityYOffset;
    [SerializeField] Color textColor;
    void OnEnable()
    {
        foreach(Image x in itemSprites) x.sprite = item.image;
        int counter = 0;
        foreach (Resource x in item.recipe) {
            GameObject icon = Instantiate(new GameObject(), ResourcesBar);
            Image sr = icon.AddComponent<Image>();
            sr.sprite = x.resourceSprite;

            GameObject value = Instantiate(new GameObject(), icon.transform);
            TMPro.TextMeshProUGUI tm = value.AddComponent<TMPro.TextMeshProUGUI>();
            tm.text = item.quantities[counter].ToString();
            tm.color = textColor;
            tm.alignment = TMPro.TextAlignmentOptions.Center;

            value.GetComponent<RectTransform>().localPosition = new Vector3(0, quantityYOffset, 0);
            value.GetComponent<RectTransform>().sizeDelta = new Vector2(50,50);
            print(value.GetComponent<RectTransform>().rect.width);

            counter ++;
        }
        
        ResourcesBar.GetComponent<HorizontalLayoutGroup>().spacing = item.recipe.Length > 2 ? 2 : 8;
    }

    private void OnDisable()
    {
        foreach (Transform x in ResourcesBar.transform) Destroy(x.gameObject);
    }
}
