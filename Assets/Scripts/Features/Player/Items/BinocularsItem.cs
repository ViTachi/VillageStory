using DG.Tweening;
using UnityEngine;

public class BinocularsItem : HoldableItem, IUsableItem
{
    private bool _look = false;
    private float _startFOV = 0;
    private float _targetFOV = 5;
    public void Use(IUseItemContext context)
    {
        var cinemachineCamera = context.CinemachineCamera;
        if (_startFOV == 0f) _startFOV = cinemachineCamera.Lens.FieldOfView;

        float fov = cinemachineCamera.Lens.FieldOfView;
        if (_look)
        {   
            DOTween.To(() => fov, x => cinemachineCamera.Lens.FieldOfView = x, _startFOV, 0.5f);
        }
        else
        {
            DOTween.To(() => fov, x => cinemachineCamera.Lens.FieldOfView = x, _targetFOV, 0.5f);
        }
        _look = !_look;
    }
}
