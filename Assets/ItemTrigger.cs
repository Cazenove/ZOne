using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZOne;

[RequireComponent(typeof(Collider))]
public class ItemTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject m_BindObject;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<PlayerController>() != null)
        {
            EventUtil.Send(new GotWeaponEvent
            {
                Weapon = m_BindObject,
            });
            gameObject.SetActive(false);
        }
    }
}
