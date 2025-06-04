using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoverFondoMenu : MonoBehaviour
{
  [SerializeField]  public RawImage _img;
  [SerializeField]  public float _x;
  [SerializeField]  public float _y;
    void Update()
    {
           _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
    }
   
}
