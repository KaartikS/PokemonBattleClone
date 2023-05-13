using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;
    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator SetHpSmooth(float newHp)
    {
        float currentHp = health.transform.localScale.x;
        float changeAmt = currentHp - newHp;

        while(currentHp - newHp > Mathf.Epsilon)
        {
            currentHp -= (changeAmt * Time.deltaTime);
            health.transform.localScale = new Vector3(currentHp, 1f);
            yield return null;
        }

        SetHP(newHp);
    }
}
