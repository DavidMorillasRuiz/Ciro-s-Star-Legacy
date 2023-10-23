using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] List<GameObject> checkPoints;
    [SerializeField] Vector3 vectorPoint;
    private PlayerController playerController;
    private Animator anim;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.life <= 0)
        {
            AudioManager.Instance.PlaySFX(5);
            anim.SetBool("Dies",true);
            StartCoroutine(Lose());
        }
    }

    IEnumerator Lose()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = vectorPoint;
        playerController.life = playerController.lifeMax;
        anim.SetBool("Dies", false);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("checkPoint"))
        {
            vectorPoint = player.transform.position;
            Caracteristicas.LvStatic.panelLevel.SetActive(true);
            //Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Caracteristicas.LvStatic.panelLevel.SetActive(false);
    }

}
