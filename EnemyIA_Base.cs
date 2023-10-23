using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Libreria para usar clases de NavMesh

public class EnemyIA_Base : MonoBehaviour
{
    Animator anim;

    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent; //Referencia al componente NavMeshAgent, que la IA
    [SerializeField] Transform target; //Referemcia al transform del objecto que la IA va a perseguir
    [SerializeField] LayerMask targetLayer; //Determina cual es la capa de deteccion del target
    [SerializeField] LayerMask groundLayer; //Determina cual es la capa de deteccion del suelo

    [Header("Patroling Stats")]
    public Vector3 walkPoint; //Direccion a la que la IA se va a mover si no detecta al target
    [SerializeField] float walkPointRange; //Rango de direccion de movimiento si la IA no detecta target
    bool walkPointSet; //Bool que determina si la IA ha llegado al objectivo y entonces cambia de objectivo

    [Header("Attack Configuration")]
    public float timeBetweenAttacks; //Tiempo entre ataque
    bool alreadyAttacked; //Bool para determinar si se ha atacado

    [Header("State & Detection")]
    [SerializeField] float sightRange; //Rango de deteccion de la IA
    [SerializeField] float attackRange; //Rango a partir del cual la IA ataca
    [SerializeField] bool targetInSightRange; //Bool que determine si el target esta a distancia de ataque
    [SerializeField] bool targetInAttackRange; //Bool que determine si el target esta a distancia de ataque

    private void Awake()
    {
        target = GameObject.Find("PlayerLayer").transform; //Al inicio del juego referencia al target a perseguir
        agent = GetComponent<NavMeshAgent>(); //Referencia al inicio del juego al componente de IA solo
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //Chequear si el target esta en los rangos de deteccion y ataque
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);

        //Cambios dinamicos de estado de la IA
        //Si no detecta al target ni esta en rango de ataque: patrulla
        if (!targetInSightRange && !targetInAttackRange) Patroling();
        //Si detecta al target pero no esta en rango de ataque: persigue
        if (targetInSightRange && !targetInAttackRange) ChaseTarget();
        //Si detecta al target y esta en rango de ataque: ataca
        if (targetInSightRange && targetInAttackRange) AttackTarget();
    }

    void Patroling()
    {
        anim.SetBool("Patrolling", true);
        anim.SetBool("Atacar", false);
        anim.SetBool("Perseguir", false);
        anim.SetBool("Damages", false);
        if (!walkPointSet)
        {
            //Si no hay punto al que dirigirse, inicia el metodo para buscarlo
            SearchWalkPoint();
        }
        else
        {
            //Si hay punto al que dirigirse, mueve la IA hacia ese punto
            agent.SetDestination(walkPoint);
        }

        //Lineas para que la IA busque un nuevo destino de patrullaje una vez ha llegado al destino actual
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        //Crear el sistema de puntos a patrullar
        anim.SetBool("Patrolling", true);
        anim.SetBool("Atacar", false);
        anim.SetBool("Perseguir", false);
        anim.SetBool("Damages", false);
        //Sistema de puntos a patrullar random
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //direccion a la que se mueve la IA
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Deteccion si no hay suelo debajo para evitar caidas
        if(Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true; //Comienza el movimiento, porque hay suelo en la direccion destino
        }
    }

    void ChaseTarget()
    {
        anim.SetBool("Patrolling", false);
        anim.SetBool("Atacar", false);
        anim.SetBool("Perseguir", true);
        //Una vez que lo detecta, lo persigue
        agent.SetDestination(target.position);
    }

    void AttackTarget()
    {
        anim.SetBool("Patrolling", false);
        anim.SetBool("Atacar", true);
        anim.SetBool("Perseguir", false);
        //Una vez que comience a atacar no se mueve
        agent.SetDestination(transform.position);
        //La IA mira directamente al target
        transform.LookAt(target);

        if(!alreadyAttacked)
        {
            //Si no hemos atacado ya, atacamos
            //AQUI IRIA EL CODIGO DEL ATAQUE A PERSONALIZAR

            

            alreadyAttacked = true;
            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        anim.SetBool("Patrolling", true);
        anim.SetBool("Atacar", false);
        anim.SetBool("Perseguir", false);
        alreadyAttacked = false;
    }

    //Funcion para que los gizmos se dibujen en escena 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
