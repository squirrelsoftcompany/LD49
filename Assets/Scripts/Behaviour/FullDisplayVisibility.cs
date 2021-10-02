using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullDisplayVisibility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool _visibility = true;
    bool Visibility
    {
        get => _visibility;
        set => SetVisibility(value);
    }

    public void SetVisibility(bool value)
    {
        if (value == _visibility) return;

        _visibility = value;
        ApplyVisibility(_visibility);
    }

    void ApplyVisibility(bool value)
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var r in renderers)
        {
            r.shadowCastingMode = value ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Toggle Visiblity")]
    public void ToggleVisiblity()
    {
        if (Application.isPlaying)
        {
            Visibility = !Visibility;
        }
        else
        {
            Debug.LogWarning("Can't call this function while editing.");
        }
    }

    [ContextMenu("Toggle Visiblity", true)]
    public bool CheckToggleVisiblity()
    {
        return Application.isPlaying;
    }
#endif // UNITY_EDITOR
}
