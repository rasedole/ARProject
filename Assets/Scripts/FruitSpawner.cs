using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    private bool dirtyCheck;
    public GameObject currentFruit;
    public Rigidbody body;
    public Vector3 initPos;
    public FixedJoystick joystick;
    private List<float> maxDis = new List<float>();
    private float distance;
    public GameObject[] gameStartObject;

    private void OnEnable()
    {
        initPos = transform.position;
        maxDis.Add(0.58f);
        maxDis.Add(0.6f);
        maxDis.Add(0.575f);
        maxDis.Add(0.53f);
        for(int i = 0; i < gameStartObject.Length; i++)
        {
            gameStartObject[i].SetActive(true);
        }
        Invoke("FruitSpawn", 1f);
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(joystick.Horizontal * Time.deltaTime, 0, joystick.Vertical * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -distance, distance), transform.position.y, Mathf.Clamp(transform.position.z, -distance, distance));
        if (currentFruit != null)
        {
            currentFruit.transform.position = transform.position;
        }
    }

    public void FruitSpawn()
    {
        int randomNum = Random.Range(0, 4);
        distance = maxDis[randomNum];
        currentFruit = Instantiate(GameManager.Instance.fruitList[randomNum]);
        body = currentFruit.GetComponent<Rigidbody>();
    }

    public void FruitDown()
    {
        if (dirtyCheck == false)
        {
            dirtyCheck = true;
            body.useGravity = true;
            currentFruit = null;
            body = null;
            transform.position = initPos;
            Invoke("FruitSpawn", 1f);
            Invoke("DirtyClear", 3f);
        }
    }

    private void DirtyClear()
    {
        dirtyCheck = false;
    }
}
