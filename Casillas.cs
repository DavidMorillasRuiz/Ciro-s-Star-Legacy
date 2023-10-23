using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Casillas : MonoBehaviour
{
    public Material colorCasillas;
    public int numCartas = 0;
    public Vector3 positionOriginal;
    public Texture2D textura;
    [SerializeField] Texture2D texturaReverso;

    [SerializeField] float timeDelay;
    [SerializeField] bool showCard;
    public GameObject crearCasillas;

    private void Awake()
    {
        crearCasillas = GameObject.Find("MiniGame_Manager");
    }

    private void Start()
    {
        EsconderCartas();
        
    }


    private void OnMouseDown()
    {
        print(numCartas.ToString());
        MostrarCarta();
    }

    public void AsignarTextura(Texture2D _textura)
    {
        textura = _textura;
        
    }

   public  void MostrarCarta()
   {
       if(!showCard && crearCasillas.GetComponent<CrearCasillasX>().sePuedeMostrar)
       {
            showCard = true;
            crearCasillas.GetComponent<CrearCasillasX>().HacerClick(this);
            GetComponent<MeshRenderer>().material.mainTexture = textura;
            
       }
   }

    public void EsconderCartas()
    {
        Invoke("Hide", timeDelay);
        crearCasillas.GetComponent<CrearCasillasX>().sePuedeMostrar = false;
    }

    void Hide()
    {
        GetComponent<MeshRenderer>().material.mainTexture = texturaReverso;
        showCard = false;
        crearCasillas.GetComponent<CrearCasillasX>().sePuedeMostrar = true;
    }
}
