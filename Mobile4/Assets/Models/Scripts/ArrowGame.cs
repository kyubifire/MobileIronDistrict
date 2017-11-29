using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class ArrowGame : MonoBehaviour {
	
	//time things, like end time and next create time, and how long a round is
	private float timeEnd;
	private float nextSpawnTime;
	public int time;


	//player/enemy health and their max health
	public int health;
	private int maxHealth;


	private int totalCards;					//This is the stuff that track the cards 
	private int correctCards;				//spawnded per round and how many you got right


	public int enemyHealth;
	private int enemyMaxHealth;


	public ArrowCard aCard;
	//queue for arrow cards
	public List<ArrowCard> arrows;

	//Set enemy damage,health
	public int enemyDamage;
	//your "attack" damage
	public int attackDamage;

	//create the health bar
	public GameObject healthBar;		//stores the health bar prefab
	public GameObject attackBar;		//store the attack bar prefab
	public GameObject enemyBar;
	public GameObject HUD;
	public GameObject backgrounds;
	public GameObject instructionAccess;
	public GameObject Victory;
	public GameObject Defeat;


	public Player player;
	public Enemy enemy;

	private GameObject healthGauge;		//health bar obj
	private GameObject attackGauge;		// attack bar obj
	private GameObject enemyGauge;
	private GameObject instructions;

	//checks if you are in game
	private bool inGame;
	private bool gameEnd;

	//tracks how many hits in a row and how much your gauge has charged
	public int consecutiveHits;
	public int attackPoints;


	// sound effects
	public AudioSource source;
	public AudioClip arrowSound;
	public AudioClip badArrow;

	public int sceneIdx;
	public Button moveOnButton;
	public Button reloadButton1;
	public Button reloadButton2;
	public Button closeBtn;

    // Swipe Stuff
    Vector2 startPos;
    Vector2 endPos;
    Vector2 curSwipe;

    private bool up, down, left, right, swiping;

    // Use this for initialization
    void Start () {

		sceneIdx = SceneManager.GetActiveScene ().buildIndex;
        up = false;
        down = false;
        left = false;
        right = false;
        attackPoints = 0;
		totalCards = 0;
		correctCards = 0;
		//this sets how long befor the game stops instantiating arrows
		timeEnd = Time.time -1;

		maxHealth = health;				//set player and enemy max health to initial health
		enemyMaxHealth = enemyHealth;
		nextSpawnTime = Time.time;


		Instantiate (backgrounds);	
		Instantiate (HUD);	

		//Create the enemy and player
		enemy = (Enemy)Instantiate (enemy);
		player = (Player)Instantiate (player);
		//create the bars for your and enemy health
		healthGauge = (GameObject)Instantiate (healthBar);
		attackGauge = (GameObject)Instantiate (attackBar);
		enemyGauge = (GameObject)Instantiate (enemyBar);
		instructions = (GameObject)Instantiate(instructionAccess);
		player.canMove = false;

		source = GetComponent<AudioSource> ();

		instructions.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);

        inGame = true;
        timeEnd = Time.time + time;
    }

	// Update is called once per frame
	void Update () {
		closeBtn.onClick.AddListener (Click);
		closeBtn.onClick.AddListener (CloseInstructions);

		//when the attack bar is full you attack the enemy and it resets the bar
		if (attackPoints >= 50) {
			attackPoints = 0;
			enemyHealth -= attackDamage;
			consecutiveHits = 1;

			//set breya animation to attack
			player.changeState (1);
		} 

		attackGauge.transform.localScale = new Vector3 ((float)attackPoints / 6f, .5f, 1f);


		//if you are still alive
		if (health > 0) {
			healthGauge.transform.localScale = new Vector3 ((float)7.5 * health / maxHealth, .5f, 1f);
		} else {
			if (!gameEnd) {
				//GAME OVER DEATH
				//stops arrow spawns
				timeEnd = Time.time - 2f;
				player.changeState (66);

				//create death display

				//Instantiate (Defeat);
				Defeat.SetActive(true);
				gameEnd = true;

				if (Input.GetMouseButtonDown (0)) {
					SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
				}
				//readjust the health bar
				healthGauge.transform.localScale = new Vector3 ((float)7.5 * health / maxHealth, .5f, 1f);


				//removes all the extra arrow cards
				while (arrows.Count > 0) {
					Destroy (arrows [0].gameObject);
					arrows.RemoveAt (0);
				}
			}
		}

		if (enemyHealth > 0) {
			enemyGauge.transform.localScale = new Vector3 (((float)12.4 * enemyHealth / enemyMaxHealth), .5f, 1f);

		} else {
			if (!gameEnd) {
				gameEnd = true;
				//Victory
				//Instantiate (Victory);
				Victory.SetActive(true);

				if (Input.GetMouseButtonDown (0)) {
					SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
				}

				enemy.changeState (66);
				enemyGauge.transform.localScale = new Vector3 (((float)12.4 * enemyHealth / enemyMaxHealth), .5f, 1f);


				timeEnd = Time.time - 1;
				int y = 0;
				while (y < arrows.Count) {
					Destroy (arrows [0].gameObject);
					arrows.RemoveAt (0);
					y++;
				}
			}
		}
			
		//THIIS IS EWHERE WE CREATE CARDS

		if (Time.time <= timeEnd && Time.time >= nextSpawnTime) {		//every round within the time limit it will spawn a arrow card
			inGame = true;

			arrows.Add ((ArrowCard)Instantiate (aCard));			//instantiates the arrow card prefan
			nextSpawnTime += (.4f);									// sets the next interval that it spawns
			totalCards += 1;

		} else {
			if (Time.time > timeEnd + 3) {
				inGame = false;
			}
		}

		if (arrows.Count >= 1) {
			if (arrows [0].transform.position.x <= -3.5) {
				badInput ();
				int y = 0;
				while (y < arrows.Count) {
					Destroy (arrows [0].gameObject);
					arrows.RemoveAt (0);
					y++;
				}
				consecutiveHits = 0;
				attackPoints = 0;
			}
		}

        //====================================

        //if this doesnt work try to remove eeverything after the and in the if statment for all the inouts
        //so the if statemnt is just something like
        //if ((Input.GetTouch (0).deltaPosition.y > 0


        //=====================================

        if (Input.touchCount > 0 && !swiping)
        {
            Debug.Log("Entering touch swipe");
            swiping = true;
            // Get the first input
            Touch t = Input.GetTouch(0);

            // Get the different states of each touch
            if (t.phase == TouchPhase.Began)
            {
                // Get where it started from
                Debug.Log("BEGINNING");
                startPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended)
            {
                Debug.Log("ENDING");
                // Get where it stopped
                endPos = new Vector2(t.position.x, t.position.y);

                // Create Vector
                curSwipe = new Vector3(endPos.x - startPos.x, endPos.y - startPos.y);
                curSwipe.Normalize();

                if (curSwipe.x < 0 && curSwipe.y > -0.5f && curSwipe.y < 0.5f)
                {
                    left = true;
                }
                else if (curSwipe.x > 0 && curSwipe.y > -0.5f && curSwipe.y < 0.5f)
                {
                    right = true;
                }
                else if (curSwipe.y > 0 && curSwipe.x > -0.5f && curSwipe.x < 0.5f)
                {
                    up = true;
                }
                else if (curSwipe.y < 0 && curSwipe.x > -0.5f && curSwipe.x < 0.5f)
                {
                    down = true;
                }

                Debug.Log("UP " + up + " DOWN " + down + " LEFT " + left + " RIGHT " + right);
            }
        }
        if (up)
        {                       //checks the card and input
            up = false;
            if (arrows.Count >= 1)
            {                               //if the top of the queue matches input
                if (arrows[0].type == 0)
                {                               //success, else damages you
                    correctInput();
                }
                else
                {
                    badInput();
                }
            }
        }
        if (down)
        {
            down = false;
            if (arrows.Count >= 1)
            {
                if (arrows[0].type == 1)
                {
                    correctInput();
                }
                else
                {
                    badInput();
                }
            }
        }
        if (left)
        {
            left = false;
            if (arrows.Count >= 1)
            {
                if (arrows[0].type == 2)
                {
                    correctInput();
                }
                else
                {
                    badInput();
                }
            }
        }
        if (right)
        {
            right = false;
            if (arrows.Count >= 1)
            {
                if (arrows[0].type == 3)
                {
                    correctInput();
                }
                else
                {
                    badInput();
                }
            }
        }
        swiping = false;
    }

	//what happens if you get it right
	void correctInput(){
		source.PlayOneShot (arrowSound);
		Destroy (arrows [0].gameObject);
		arrows.RemoveAt (0);
		consecutiveHits += 1;
		attackPoints += consecutiveHits;
		correctCards += 1;
	}
	//what happens when you get it wrong
	void badInput(){
		source.PlayOneShot (badArrow);
		Destroy (arrows [0].gameObject);
		arrows.RemoveAt (0);
		health -= enemyDamage;
		consecutiveHits = 0;
		enemy.changeState (1);
	}

	void CloseInstructions() {
		Debug.Log ("geting rid of instructions");
		//Destroy (instructions);
		instructionAccess.SetActive(false);
		instructions.SetActive (false);
	}

	void Click() {
		Debug.Log("You have clicked the button!");
	}
}