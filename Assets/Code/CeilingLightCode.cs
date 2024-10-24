using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLightCode : MonoBehaviour
{

    public bool hacked;

    //해킹 지속시간
    public int ceilingHackingDuration;

    //ResetAfterDelay() 코루틴 한번만 실행되게 하는 변수
    private bool isHackingActivate;

    private LightRangeCode lightRangeCode;

    public GameObject hackedPrefab;

    void Start()
    {
        hacked = false;

        Transform childTransform = transform.Find("LightRange");

        if (childTransform != null)
        {

            lightRangeCode = childTransform.GetComponent<LightRangeCode>();

        }

    }

    void Update()
    {
        if (hacked && !isHackingActivate)
        {

            StartCoroutine(ResetAfterDelay());

            lightRangeCode.turnOff = true;

        }

    }

    private IEnumerator ResetAfterDelay()
    {

        isHackingActivate = true;

        GameObject hackedObject = Instantiate(hackedPrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(ceilingHackingDuration);

        Destroy(hackedObject);

        hacked = false;

        lightRangeCode.turnOff = false;

        isHackingActivate = false;

        lightRangeCode.reset = true;
    }

}
