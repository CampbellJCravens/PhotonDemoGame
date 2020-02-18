using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTouch : MonoBehaviour
{

    public GameObject touchEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        print ("WATER TOUCH");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 effectPoint = new Vector3(mousePosition.x, 0, mousePosition.z);

        GameObject duplicate = Instantiate(touchEffect, effectPoint, touchEffect.gameObject.transform.rotation);
        StartCoroutine(DestroyEffect(duplicate));
    }

    IEnumerator DestroyEffect(GameObject effect) {
        yield return new WaitForSeconds(2);
        Destroy(effect);
    }
}
