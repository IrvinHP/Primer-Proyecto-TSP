using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class UInst : MonoBehaviour
{
    public int currentIndex = 0;

    //Variables de objetos
    [SerializeField]
    public List<GameObject> instObj;

    //Variables de Textos
    public List<string> mensajes;
    public TextMeshProUGUI textos;

    void Start()
    {
        /*currentIndex = 0;
        instObj[0].SetActive(true);
        UpdateVisibility();

        for (int i = 1; i <= instObj.Count; i++)
        {
            instObj[i].SetActive(false);
        }*/
    }

    public void CycleObjects()
    {
        currentIndex = (currentIndex + 1) % instObj.Count;

        UpdateVisibility();
    }

    public void UpdateVisibility()
    {
        for (int i = 0; i < instObj.Count; i++)
        {
            instObj[i].SetActive(i == currentIndex);
        }
    }

    public void CycleText()
    {
        currentIndex = (currentIndex + 1) % mensajes.Count;

        UpdateText();
    }

    public void UpdateText ()
    {
        if (mensajes.Count > 0 && textos != null)
        {
            textos.text = mensajes[currentIndex];
        }
    }

    void Update()
    {
        
    }
}
