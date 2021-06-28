using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public GameObject playerGameObject;
    private int maxHealth = 100;
    //[Range(0, 100)]
    [SerializeField] private int health = 100;
    private Material cameraShader;
    private TextMeshProUGUI healthText;
    private PlayerMovement movementScript;
    private RectMask2D healthIconMask;
    private bool dying = false;
    
    void Start()
    {
        if(Instance) Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        playerGameObject = transform.GetChild(1).gameObject;

        cameraShader = GetComponentInChildren<CustomImageEffect>().EffectMaterial;

        healthText = GameObject.Find("Player Life Counter").GetComponent<TextMeshProUGUI>();

        healthIconMask = GameObject.Find("Health Mask").GetComponent<RectMask2D>();
        
        movementScript = GetComponentInChildren<PlayerMovement>();
    }

    public void setPlayerPostion(Vector3 newPostion, bool fadeIn)
    {
        transform.position = newPostion;

        playerGameObject.transform.localPosition = new Vector3(0, 0, 0);
        
        if(fadeIn) GameObject.Find("Transition Sprite").GetComponent<Animator>().Play("Fade_out");
    }

    void Update()
    {
        
        if(health <= maxHealth*0.9) cameraShader.SetFloat("Desaturation", 1f-(health/(maxHealth*0.9f)));
        else cameraShader.SetFloat("Desaturation", 0);
        
        healthText.text = health.ToString();

        healthIconMask.padding = new Vector4(0, 0,0, Mathf.Abs((((float)health / maxHealth) * 34) - 34));

        if (health <= 0 && dying == false) StartCoroutine(Die());
    }

    public void takeDamage(int damage) => health -= damage;

    IEnumerator Die()
    {
        dying = true;
        playerGameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        
        GameObject attackObject = playerGameObject.transform.GetChild(0).gameObject;
        attackObject.SetActive(false);
        movementScript.enabled = false;
        GetComponentInChildren<Animator>().Play("Die");
        
        cameraShader.SetInt("Invert", 1);

        GetComponentInChildren<Camera>().orthographicSize = 2;

        yield return new WaitForSecondsRealtime(2);
        
        attackObject.SetActive(true);
        movementScript.enabled = true;
        GetComponentInChildren<Animator>().Play("Idle");
        
        cameraShader.SetInt("Invert", 0);

        GetComponentInChildren<Camera>().orthographicSize = 5.5f;

        health = 100;

        SceneManager.LoadSceneAsync("Hub");
        ResourceManager.Instance.clearResources();
        Destroy(GameObject.Find("Level Manager"));
        
        dying = false;
    }
}
