using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FruitBucket
{
    public GameObject fruit1;
    public GameObject fruit2;

    public FruitBucket(GameObject go1, GameObject go2)
    {
        fruit1 = go1;
        fruit2 = go2;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> fruitList;
    public TMP_Text scoreTxt;
    public List<FruitBucket> fruitBucket = new();
    public GameObject gameBox;
    public GameObject spawner;
    public bool isGameStart;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public Dictionary<int, int> scoreTable = new();
    public bool isGameEnd;
    public GameObject gameOverUI;
    public TMP_Text gameOverText;
    public ParticleSystem particle;
    private List<ARRaycastHit> hits = new();

    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            scoreTxt.text = "Score : " + score.ToString();
        }
    }
    private int _score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogError("GameManager is Already Exist!");
        }
    }

    private void Start()
    {
        score = 0;
        scoreTable.Add(0, 1);
        scoreTable.Add(1,3);
        scoreTable.Add(2,6);
        scoreTable.Add(3,10);
        scoreTable.Add(4,15);
        scoreTable.Add(5,25);
        scoreTable.Add(6,33);
        scoreTable.Add(7,40);
        scoreTable.Add(8,55);
        scoreTable.Add(9,66);
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector2 touchPos = Input.GetTouch(0).position;
                if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    if (isGameStart == false)
                    {
                        gameBox.transform.position = hitPose.position;
                        gameBox.transform.rotation = hitPose.rotation;
                        gameBox.SetActive(true);
                        StartGame();
                        planeManager.SetTrackablesActive(false);
                        planeManager.enabled = false;
                    }
                }
            }
        }
    }

    public void StartGame()
    {
        if (isGameStart == false)
        {
            spawner.transform.position = gameBox.transform.position + new Vector3(0, 1.15f, 0);
            spawner.SetActive(true);
            isGameStart = true;
        }
    }

    public void CombineFruit(GameObject fruit1, GameObject fruit2)
    {
        int newIndex;
        if (CheckBucket(fruit1, fruit2, out newIndex))
        {
            int fruitNum = fruit1.GetComponent<FruitObject>().fruitNum;
            if (fruitNum < 9)
            {
                score += scoreTable[fruitNum];
                GameObject newFruit = Instantiate(fruitList[fruitNum + 1], (fruit1.transform.position + fruit2.transform.position) / 2, Quaternion.identity);
                newFruit.GetComponent<Rigidbody>().angularVelocity = fruit1.GetComponent<Rigidbody>().angularVelocity + fruit2.GetComponent<Rigidbody>().angularVelocity;
                newFruit.GetComponent<Rigidbody>().useGravity = true;
                ParticleSystem temp = Instantiate(particle,newFruit.transform.position, Quaternion.identity);
                //temp.transform.position = newFruit.transform.position;
            }
            fruitBucket.RemoveAt(newIndex);
            Destroy(fruit1);
            Destroy(fruit2);
        }
        else
        {
            fruitBucket.Add(new FruitBucket(fruit1, fruit2));
        }
    }

    public bool CheckBucket(GameObject newFruit1, GameObject newFruit2, out int index)
    {
        for (int i = 0; i < fruitBucket.Count; i++)
        {
            if ((fruitBucket[i].fruit1 == newFruit1 || fruitBucket[i].fruit1 == newFruit2) && (fruitBucket[i].fruit2 == newFruit1 || fruitBucket[i].fruit2 == newFruit2))
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public void GameEnd()
    {
        isGameEnd = true;
        gameOverUI.SetActive(true);
        gameOverText.text = "Game Over\n" + "Score : " + score;
        gameOverText.color = Color.red;
        Time.timeScale = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
