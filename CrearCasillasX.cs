using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrearCasillasX : MonoBehaviour
{

	public GameObject CartaPrefab;
	public int ancho;
	public Transform CartasParent;
	private List<GameObject> cartas = new List<GameObject>();

	public Texture2D[] texturas;

	public int contadorClicks;
	public Text textoContadorIntentos;

	public Casillas CartaMostrada;
	public bool sePuedeMostrar = true;
	public int numParejasEncontradas;
	WinMiniGame winMiniGame;

    private void Awake()
    {
		winMiniGame = GetComponent<WinMiniGame>();
    }

    private void Start()
    {
		Crear();
		
	}
	public void Reiniciar()
	{
		ancho = 0;
		GameObject[] cartasEli = GameObject.FindGameObjectsWithTag("Carta");
		for (int i = 0; i < cartasEli.Length; i++)
		{
			DestroyImmediate(cartasEli[i]);
		}

		contadorClicks = 0;
		textoContadorIntentos.text = "Intentos";
		CartaMostrada = null;
		sePuedeMostrar = true;
		numParejasEncontradas = 0;
		Crear();
	}

	public void Crear()
	{

		int cont = 0;
		for (int i = 0; i < ancho; i++)
		{
			for (int x = 0; x < ancho; x++)
			{
				float factor = 9.0f / ancho;
				Vector3 posicionTemp = new Vector3(x * factor, 0, i * factor);

				GameObject cartaTemp = Instantiate(CartaPrefab, posicionTemp,
					Quaternion.Euler(new Vector3(0, 180, 0)));

				cartaTemp.transform.localScale *= factor;

				cartas.Add(cartaTemp);

				cartaTemp.GetComponent<Casillas>().positionOriginal = posicionTemp;
				//cartaTemp.GetComponent<Carta> ().idCarta = cont;

				cartaTemp.transform.parent = CartasParent;

				cont++;
			}
		}
		AsignarTexturas();
		Barajar();
	}

	void AsignarTexturas()
	{
		for (int i = 0; i < cartas.Count; i++)
		{
			cartas[i].GetComponent<Casillas>().AsignarTextura(texturas[i / 2]);
			cartas[i].GetComponent<Casillas>().numCartas = i / 2;
		}
	}

	void Barajar()
	{
		int aleatorio;

		for (int i = 0; i < cartas.Count; i++)
		{
			aleatorio = Random.Range(i, cartas.Count);

			cartas[i].transform.position = cartas[aleatorio].transform.position;
			cartas[aleatorio].transform.position = cartas[i].GetComponent<Casillas>().positionOriginal;

			cartas[i].GetComponent<Casillas>().positionOriginal = cartas[i].transform.position;
			cartas[aleatorio].GetComponent<Casillas>().positionOriginal = cartas[aleatorio].transform.position;
		}
	}

	public void HacerClick(Casillas _carta)
	{
		if (CartaMostrada == null)
		{
			CartaMostrada = _carta;
		}
		else
		{
			contadorClicks++;
			ActualizarUI();
			if (CompararCartas(_carta.gameObject, CartaMostrada.gameObject))
			{
				print("Enhorabuena! Has encontrado una pareja!");
				numParejasEncontradas++;
				if (numParejasEncontradas == cartas.Count / 2)
				{
					print("Enhorabuena! Has encontrado todas las parejas!");
					winMiniGame.winMenuShow();

				}

			}
			else
			{
				_carta.EsconderCartas();
				CartaMostrada.EsconderCartas();
			}
			CartaMostrada = null;

		}

	}

	

	public bool CompararCartas(GameObject carta1, GameObject carta2)
	{
		bool resultado;
		if (carta1.GetComponent<Casillas>().numCartas == carta2.GetComponent<Casillas>().numCartas)
		{
			resultado = true;
		}
		else
		{
			resultado = false;
		}
		return resultado;
	}

	public void ActualizarUI()
	{
		textoContadorIntentos.text = "Attempts: " + contadorClicks;
	}
}
