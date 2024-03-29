﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerControl : MonoBehaviour 
{
	public static int playerScore = 0;
	private int highScore;

	private int chassisNum;
	private int turretNum;
	private int numHardPoints;
	private List<int> weaponsNum = new List<int>();

	private string chassisName;
	private string turretName;
	private string weaponName;

	private Vector3 vehicleLocation;

	private GameObject tempChassis;
	private GameObject tempTurret;
	private GameObject compareWeapon;
	private List<GameObject> tempWeapons = new List<GameObject>();
	private int tempInt;
	private TurretControl.HardPoint currentHardPoint;

	private UnityEngine.UI.Text scoreText1;
	private UnityEngine.UI.Text scoreText2;
	private UnityEngine.UI.Text highScoreText1;
	private UnityEngine.UI.Text highScoreText2;


	public float shadeDimSpeed;
	
	private SpriteRenderer shadeSprite;
	private UnityEngine.UI.Image gameOverImage;

	public Color beatScoreColor;
    public GameObject retry_Button;
    public GameObject menu_Button;

	private static List<UnityEngine.UI.Text> messageObject = new List<UnityEngine.UI.Text>();
	private static string currentMessage;
	private static float messageStartTime;
	public static float messageCharDelay = 0.1f;
    public float messageTotalDuration;
    public GameObject pause_text;
    bool isPaused = false;

	// Use this for initialization
	void Awake() 
	{
		vehicleLocation = new Vector3(5f, -5f, 1f);

		messageStartTime = Time.time;
		messageObject = new List<UnityEngine.UI.Text>();

		if(PlayerPrefs.HasKey("Chassis"))
			chassisNum = PlayerPrefs.GetInt("Chassis");
		if(PlayerPrefs.HasKey("Turret"))
			turretNum = PlayerPrefs.GetInt("Turret");

		int ii = 1;
		while(true)
		{
			if(PlayerPrefs.HasKey("Weapon_"+ii))
			{
				weaponsNum.Add(PlayerPrefs.GetInt("Weapon_"+ii));
				//Debug.Log("Weapon_"+ii);
				ii++;
			}
			else
			{
				break;
			}
		}

		InitializePlayer();
	}


	void Start()
	{
		scoreText1 = GameObject.FindGameObjectWithTag("Score").GetComponent<UnityEngine.UI.Text>();
		scoreText2 = GameObject.FindGameObjectWithTag("Score2").GetComponent<UnityEngine.UI.Text>();
		highScoreText1 = GameObject.FindGameObjectWithTag("HighScore").GetComponent<UnityEngine.UI.Text>();
		highScoreText2 = GameObject.FindGameObjectWithTag("HighScore2").GetComponent<UnityEngine.UI.Text>();
		shadeSprite = GameObject.FindGameObjectWithTag("Shade").GetComponent<SpriteRenderer>();
		gameOverImage = GameObject.FindGameObjectWithTag("GameOver").GetComponent<UnityEngine.UI.Image>();
		messageObject.Add(GameObject.FindGameObjectsWithTag("Message")[0].GetComponent<UnityEngine.UI.Text>());
		messageObject.Add(GameObject.FindGameObjectsWithTag("Message")[1].GetComponent<UnityEngine.UI.Text>());

		playerScore = 0;
		if(PlayerPrefs.HasKey("HighScore"))
		{
			highScore = PlayerPrefs.GetInt("HighScore");
			highScoreText1.text = highScore.ToString();
			highScoreText2.text = highScore.ToString();
		}
	}


	// Update is called once per frame
	void Update() 
	{
		scoreText1.text = playerScore.ToString();
		scoreText2.text = playerScore.ToString();

		if(playerScore > highScore)
		{
			if(highScoreText2.color != beatScoreColor)
			{
				highScoreText2.color = beatScoreColor;
				GameObject.FindGameObjectWithTag("HighScore").transform.parent.GetComponent<UnityEngine.UI.Text>().color = beatScoreColor;
				//Debug.Log("Color change");
			}
			highScore = playerScore;
			highScoreText1.text = highScore.ToString();
			highScoreText2.text = highScore.ToString();

			PlayerPrefs.SetInt("HighScore", highScore);
		}

//		if(Input.GetButton("Submit"))
//		{
//			Application.LoadLevel(1);
//		}


		if((Time.time-messageStartTime) > messageTotalDuration)
		{
			foreach(UnityEngine.UI.Text item in messageObject)
			{
				item.text = "";
			}
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == false)
            {
                Time.timeScale = 0;
                isPaused = true;
                //pause_text.GetComponentInChildren<UnityEngine.UI.Text>().color = new Color(0, 0, 0, 255);
                pause_text.SetActive(true);
                menu_Button.SetActive(true);
                retry_Button.SetActive(true);

            }
            else
            {
                Time.timeScale = 1;
                isPaused = false;
                //pause_text.GetComponentInChildren<UnityEngine.UI.Text>().color = new Color(0, 0, 0, 0);
                pause_text.SetActive(false);
                menu_Button.SetActive(false);
                retry_Button.SetActive(false);
            }
        }
	}


	void InitializePlayer()
	{
		switch(chassisNum)
		{
		case 1:
			chassisName = "Tank";
				break;
		case 2:
			chassisName = "Jeep";
				break;
		case 3:
			chassisName = "Drone";
				break;
		}
		switch(turretNum)
		{
		case 1:
			turretName = "Turret_2";
			break;
		case 2:
			turretName = "Turret_3";
			break;
		case 3:
			turretName = "Turret_4";
			break;
		}


		// Show Chassis object
		tempChassis = Instantiate(Resources.Load("Chassis/"+chassisName), vehicleLocation, Quaternion.identity) as GameObject;
		
		// Show Turret
		tempTurret = Instantiate(Resources.Load("Turret/"+turretName), vehicleLocation, Quaternion.identity) as GameObject;
		tempTurret.transform.parent = tempChassis.transform;
		numHardPoints = tempTurret.GetComponent<TurretControl>().hardPoints.Count;

		tempInt = 0;
		// Show Weapons
		foreach(int weapon in weaponsNum)
		{
			switch(weapon)
			{
			case 1:
				weaponName = "MachineGun";
				break;
			case 2:
				weaponName = "Cannon";
				break;
			case 3:
				weaponName = "ShotGun";
				break;
			case 4:
				weaponName = "RailGun";
				break;
			case 5:
				weaponName = "Laser";
				break;
			case 6:
				weaponName = "EMP";
				break;
			case 7:
				weaponName = "PotassiumBlaster";
				break;
			}

//			foreach(GameObject weap in tempWeapons)
//			{
//				if(weap!=null)
//				{
//					tempInt++;
//				}
//				else
//				{
//					break;
//				}
//			}
			////Debug.Log("Current Hardpoint "+tempInt);
			if(tempInt<numHardPoints)
			{
				currentHardPoint = tempTurret.GetComponent<TurretControl>().hardPoints[tempInt];
			}
			
			if(weapon>0)
			{
				tempWeapons.Add(Instantiate(Resources.Load("Weapon/"+weaponName), vehicleLocation, Quaternion.identity) as GameObject);
				// Attach weapon to Turret
				tempWeapons[tempWeapons.Count-1].transform.parent = tempTurret.transform;
				// move weapon to hardpoint position
				tempWeapons[tempWeapons.Count-1].transform.localPosition = new Vector3(currentHardPoint.Location.x, currentHardPoint.Location.y, 0f);
				tempWeapons[tempWeapons.Count-1].transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, currentHardPoint.Angle));
			}
			else
			{
				tempWeapons.Add(null);
			}
			tempInt++;
		}
	}


	public void PlayerDeath()
	{
		Time.timeScale = 0f;
		StartCoroutine("DeathMenu");
	}


	public static void ShowMessage(string message)
	{
		currentMessage = message;
		messageStartTime = Time.time;
		foreach(UnityEngine.UI.Text item in messageObject)
		{
			item.text = "";
		}
		GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerControl>().StartCoroutine("PrintMessage");
	}


	IEnumerator PrintMessage()
	{	//Debug.Log("Message '"+currentMessage+"' started");
		int ii = 0;
		while(ii < currentMessage.Length)
		{
			foreach(UnityEngine.UI.Text item in messageObject)
			{
				//Debug.Log(""+currentMessage[ii]);
				item.text = item.text + currentMessage[ii];
			}
			ii++;
			yield return new WaitForSeconds(messageCharDelay);
		}
		GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerControl>().StopCoroutine("PrintMessage");
		yield return null;
	}
	
	
	IEnumerator DeathMenu()
	{
		while(true)
		{
			if(shadeSprite.color.a < 1f)
			{
				shadeSprite.color = new Color(0f, 0f, 0f, shadeSprite.color.a + shadeDimSpeed); 
				gameOverImage.color = new Color(200f, 0f, 0f, gameOverImage.color.a + shadeDimSpeed);
			}

            retry_Button.SetActive(true);
			menu_Button.SetActive(true);

			if(Input.GetButton("Submit"))
			{
                Retry();
			}

			if(Input.GetButton("Cancel"))
			{
                QuitToMenu();
			}
			
			yield return null;
		}
	}

    public void Retry()
    {
        Time.timeScale = 1f;
        Application.LoadLevel(1);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        Application.LoadLevel(0);
    }

}
