using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private Slider slider;
   
    void Start()
    {
        slider=GetComponent<Slider>();
    }

    // Update is called once per frame
    public void Cambiarvidamax(float max)
    {
        slider.maxValue = max;
    }

    public void Cambiarvidactual(float cant)
    {
        slider.value = cant;
    }

    public void Iniciarbarravida(float cant)
    {
        Cambiarvidamax(cant);
        Cambiarvidactual(cant);
    }
}
