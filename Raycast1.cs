using UnityEngine;

public class Raycast1 : MonoBehaviour
{
    public Color color = Color.blue;
    private GameObject selected = null;
    
    private void Update()
    {   if(Input.GetMouseButtonDown(0))
        {
            Deselect();
        }
        
        if(Input.GetMouseButtonUp(0))
        {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hit;

           if (Physics.Raycast(ray, out hit, Mathf.Infinity))
           {
                // AQUI VEREMOS CUANDO HEMOS APUNTADO
                Debug.Log(hit.collider.name);
                Select(hit.collider.gameObject);
           }
        }
    }


    void Select(GameObject gameObject)
    {
        selected = gameObject;
        MeshRenderer renderer = selected.GetComponent<MeshRenderer>();
        renderer.material.color = color;
    }

    void Deselect()
    {
        if(selected)
        {
            MeshRenderer renderer = selected.GetComponent<MeshRenderer>();
            renderer.material.color = Color.white;
            selected = null;
        }
        
    }
}