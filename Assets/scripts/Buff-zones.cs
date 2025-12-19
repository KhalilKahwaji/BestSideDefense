using UnityEngine;

public class BuffZone : MonoBehaviour
{
    private Tower sourceTower;

    private void Awake()
    {
        sourceTower = transform.parent.GetComponentInChildren<Tower>();

        if (sourceTower == null)
            Debug.LogError("BuffZone: Tower not found!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Tower target = other.GetComponent<Tower>();
        if (target == null || target == sourceTower) return;

        Debug.Log($"BuffZone of {sourceTower.name} detected tower {target.name} entering.");

        ApplyBuff(target);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Tower target = other.GetComponent<Tower>();
        if (target == null || target == sourceTower) return;

        RemoveBuff(target);
    }

    void ApplyBuff(Tower target)
    {
        switch (sourceTower.buffType)
        {
            case BuffType.Speed:
                target.attackSpeed += sourceTower.buffSpeedAmount;
                break;

            case BuffType.Damage:
                target.damage += sourceTower.buffDamageAmount;
                break;

            case BuffType.Range:
                target.range += sourceTower.buffRangeAmount;
                break;

            case BuffType.Pierce:
                target.pierce += sourceTower.buffPierceAmount;
                break;
        }
    }

    void RemoveBuff(Tower target)
    {
        switch (sourceTower.buffType)
        {
            case BuffType.Speed:
                target.attackSpeed -= sourceTower.buffSpeedAmount;
                break;

            case BuffType.Damage:
                target.damage -= sourceTower.buffDamageAmount;
                break;

            case BuffType.Range:
                target.range -= sourceTower.buffRangeAmount;
                break;

            case BuffType.Pierce:
                target.pierce -= sourceTower.buffPierceAmount;
                break;
        }
    }
}
