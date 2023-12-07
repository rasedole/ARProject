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
        maxDis.Add(0.5f / 2);
        maxDis.Add(0.51f / 2);
        maxDis.Add(0.515f / 2);
        maxDis.Add(0.47f / 2);
        for (int i = 0; i < gameStartObject.Length; i++)
        {
            gameStartObject[i].SetActive(true);
        }
        Invoke("FruitSpawn", 1f);
    }

    private void FixedUpdate()
    {
        Vector3 xMove = joystick.Vertical * Time.deltaTime * (Camera.main.transform.localRotation * Vector3.forward);
        Vector3 zMove = joystick.Horizontal * Time.deltaTime * (Camera.main.transform.localRotation * Vector3.right);
        //transform.position += new Vector3(joystick.Horizontal * Time.deltaTime, 0, joystick.Vertical * Time.deltaTime);
        transform.position += xMove += zMove;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, initPos.x - distance, initPos.x + distance), initPos.y, Mathf.Clamp(transform.position.z, initPos.z - distance, initPos.z + distance));
        if (currentFruit != null)
        {
            currentFruit.transform.position = transform.position;
        }
    }

    public void FruitSpawn()
    {
        if (GameManager.Instance.isGameEnd == false)
        {
            int randomNum = Random.Range(0, 4);
            distance = maxDis[randomNum];
            currentFruit = Instantiate(GameManager.Instance.fruitList[randomNum], transform.position, Quaternion.identity);
            body = currentFruit.GetComponent<Rigidbody>();
        }
    }

    public void FruitDown()
    {
        if (GameManager.Instance.isGameEnd == false)
        {
            if (dirtyCheck == false)
            {
                dirtyCheck = true;
                body.useGravity = true;
                currentFruit = null;
                body = null;
                transform.position = initPos;
                Invoke("FruitSpawn", 1f);
                Invoke("DirtyClear", 1f);
            }
        }
    }

    private void DirtyClear()
    {
        dirtyCheck = false;
    }
}
