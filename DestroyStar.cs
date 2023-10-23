using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyStar : MonoBehaviour
{
    [SerializeField] float rotasteSpeed;
    public int points = 1;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotasteSpeed = 0.5f;
        transform.Rotate(0, rotasteSpeed, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.SumarPuntos(points);
            gameObject.SetActive(false);
        }
    }
}
