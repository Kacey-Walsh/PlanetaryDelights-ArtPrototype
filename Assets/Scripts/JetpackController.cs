using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackController : MonoBehaviour
{
    
    public Transform PlayerTransform;
    
    public bool isRising;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("f"))
        {
            StartCoroutine(JetpackUp());
        }

        if (Input.GetKeyUp("f"))
        {
            StartCoroutine(StopJetpack());
        }
    }

    IEnumerator JetpackUp()
    {
        isRising = true;
        yield return new WaitForSeconds(0f);
    }

    IEnumerator StopJetpack()
    {
        isRising = false;


        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(0f);
    }
}
