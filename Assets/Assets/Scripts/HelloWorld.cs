using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    public int speed = 8;
    public GameObject helloText;
    // string myText = "Hello World 2";
    // Start is called before the first frame update
    void Start()
    {
        // print(myText);
        Application.targetFrameRate = 100;
        helloText.GetComponent<TextMeshPro>().text = "GO !";
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1,0,0)* Time.deltaTime * speed);
    }
}
