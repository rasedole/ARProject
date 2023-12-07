using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitObject : MonoBehaviour
{
    public int fruitNum;
    public bool deadCheck;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Fruits" && collision.gameObject.GetComponent<FruitObject>().fruitNum == fruitNum)
        {
            GameManager.Instance.CombineFruit(this.gameObject, collision.gameObject);
        }
        deadCheck = true;
    }
}
