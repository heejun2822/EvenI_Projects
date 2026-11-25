using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weaponRenderer;
    [SerializeField] private Transform weaponPivot;

    [SerializeField] private SpriteRenderer characterRenderer;

    private BaseChracterController _controller;

    private void Awake()
    {
        _controller = GetComponent<BaseChracterController>();
    }

    void Start()
    {
        _controller.OnLookEvent += OnAim;        
    }
    
    private void OnAim(Vector2 newAimDirection)
    {
        RotateArm(newAimDirection);
    }

    private void RotateArm(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        weaponRenderer.flipY = Mathf.Abs(rotZ) > 90f;
        characterRenderer.flipX = weaponRenderer.flipY;

        weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    public void SetWeaponSprite(Sprite sprite, Vector3 position)
    {
        weaponRenderer.sprite = sprite;
        weaponRenderer.transform.localPosition = position;
    }

    public void EquipItem(DropItem dropItem)
    {
        RangedWeaponSO rangedWeaponSO = dropItem.WeaponSo as RangedWeaponSO;
        SetWeaponSprite(rangedWeaponSO.weaponSprite, rangedWeaponSO.offset);
    }
}
