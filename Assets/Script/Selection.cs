using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    [SerializeField]
    Material inactivo;
    [SerializeField]
    Material activo;

    public bool gazed;
    Renderer renderObj;
    public GameObject objeto;

    // Start is called before the first frame update
    void Start()
    {
        objeto = this.gameObject;
        gazed = false;
        inactivo = gameObject.gameObject.GetComponent<Renderer>().material;
        renderObj = gameObject.GetComponent<Renderer>();
        SetMaterial(gazed);
    }
    public void OnPointerEnter()
    {
        gazed = true;
        SetMaterial(gazed);
    }
    
    public void OnPointerExit()
    {
        gazed = false;
        SetMaterial(gazed);
    }

    void SetMaterial(bool gazedAt)
    {
        if (inactivo != null && activo != null)
        {
            //El material va a tener el valor dea activo o inactivo dependiendo del valor de gazedAt
            renderObj.material = gazedAt ? activo : inactivo;
        }
    }
}
