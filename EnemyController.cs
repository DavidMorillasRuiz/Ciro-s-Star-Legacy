using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] float maxHealth;
    [SerializeField] float forcePunch = 10;
    [SerializeField] GameObject starPoint;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        starPoint.SetActive(false);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Damaged();
    }

    void Damaged()
    {
        if(health <= 0)
        {
            TimeTodie();
            starPoint.SetActive(true);
            gameObject.SetActive(false);
            DropItem();
            
        }
    }

    IEnumerator TimeTodie()
    {
        AudioManager.Instance.PlaySFX(6);
        yield return new WaitForSeconds(3);
        anim.SetTrigger("Die");
    }

    public void TakeDamge(int damageToTake)
    {
        health -= damageToTake;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Sword"))
        {
            int damageTaken = other.gameObject.GetComponent<WeaponScript>().weaponDamage;
            health -= damageTaken;
            anim.SetBool("Damages",true);
            Debug.Log(health);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>())
        {
            other.gameObject.GetComponent<PlayerController>().damagePlayer(forcePunch);
        }
        if(other.gameObject.CompareTag("Sword"))
        {
            health--;
        }
    }


    void DropItem()
    {
        Vector3 position = transform.position;
        GameObject star = Instantiate(starPoint, position, Quaternion.identity);
    }


}
