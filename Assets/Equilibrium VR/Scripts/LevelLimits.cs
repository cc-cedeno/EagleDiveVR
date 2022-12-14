using UnityEngine;
using System.Collections;

public class LevelLimits : MonoBehaviour
{ 
    //Use for checking if player out of map. 

    public float Cooldown = 3;
    private bool invertColor = false;
    public float Tint = 0;
    public AudioClip Fall;
    private bool fall = false;
    public GameObject AreaLimit;
    bool initialized=false;

    void Start()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Limits"))
        {
        //manager is defined on the fly as opposed to initialized which gives a few NullRefs.
			GameLogic manager = GameObject.Find("GameManager1").GetComponent<GameLogic>();
            manager.LimitsAlert.gameObject.SetActive(true);
            manager.LimitsAlert.GetComponent<TextMesh>().text = "STAY BACK\n" + Mathf.Round(Cooldown); //LimitsAlert TextMesh on MainCamera in GameLogic script
            Cooldown -= Time.deltaTime;
            Tint += Time.deltaTime/3; //counter
            AreaLimit.GetComponent<Renderer>().material.SetColor("_TintColor", new Color (1,1,1, Tint)); //fadeIn graphical grid
            if (Cooldown <= 0 && !fall)
            {
                manager.impacted = true;
                //GameObject.Find("AudioManager").GetComponent<AudioManager>().HitBoundary();
                StartCoroutine(manager.ResetByFall(0.2F, 0)); //reloading level with current gamemode
                fall = true;
                Cooldown = 0;
            }
        }
        else
            return;

    }

    void OnTriggerExit(Collider other)
    {
         Cooldown = 3;
        invertColor = true;
		//manager is defined on the fly as opposed to initialized which gives a few NullRefs.
		GameLogic manager = GameObject.Find("GameManager1").GetComponent<GameLogic>();
        manager.LimitsAlert.gameObject.SetActive(false);
    }

    void Update()
    {
    	if (!initialized)
    	{
			//manager = GameObject.Find("GameManager").GetComponent<GameLogic>();
			initialized=true;
    	}

        if (invertColor)
        {
            Tint -= Time.deltaTime / 3;
            AreaLimit.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1, 1, 1, Tint)); //FadeOut graphical grid
            if (Tint <= 0)
            {
                Tint = 0;
                invertColor = false;
            }
        }
    }


}
