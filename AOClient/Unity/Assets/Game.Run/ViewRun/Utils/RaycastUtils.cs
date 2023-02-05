using UnityEngine;

public static class RaycastUtils
{
    public static bool CastMapPoint(out Vector3 hitPoint)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, 1 << LayerMask.NameToLayer("Map")))
        {
            hitPoint = hit.point;
            return true;
        }
        hitPoint = Vector3.zero;
        return false;
    }
}