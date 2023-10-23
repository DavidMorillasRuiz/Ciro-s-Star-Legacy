using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guia : MonoBehaviour
{
    [SerializeField] GameObject guia_Power;


    // Start is called before the first frame update
    void Start()
    {
        guia_Power.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TimePower());
        }
    }

    IEnumerator TimePower()
    {
        guia_Power.SetActive(true);
        yield return new WaitForSeconds(3);
        guia_Power.SetActive(false);
        gameObject.SetActive(false);
    }
}
