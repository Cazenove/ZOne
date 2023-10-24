using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZOne;

public class ItemOwner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_Slots = new List<GameObject>();

    private void Awake()
    {
        EventUtil.Instance.Register(this);
    }

    [EventBus]
    public void OnGotWeapon(GotWeaponEvent evt)
    {
        evt.Weapon.transform.SetParent(m_Slots[0].transform);
        evt.Weapon.transform.localPosition = Vector3.zero;
    }
}
