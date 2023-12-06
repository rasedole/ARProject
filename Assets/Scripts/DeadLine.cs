using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Fruits" && other.gameObject.GetComponent<FruitObject>().deadCheck)
        {
            GameManager.Instance.GameEnd();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Fruits" && other.gameObject.GetComponent<FruitObject>().deadCheck == false)
        {
            other.gameObject.GetComponent<FruitObject>().deadCheck = true;
        }
    }
}
