using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AspectSelection : MonoBehaviour,IPointerDownHandler
{
    public AspectType aspecttype;

    public void OnPointerDown(PointerEventData eventData)
    {
        MatchExecutor.Instance.ChangeAspect(aspecttype);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
