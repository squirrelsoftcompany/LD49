using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PovManager : MonoBehaviour
{
    private static PovManager _inst = null;
    public static PovManager Inst
    {
        get => _inst;
    }

    private Cinemachine.CinemachineVirtualCamera _CVC;
    private Transform _pivotCamera;

    public enum RocketPOV
    {
        eFullDisplay = 0,
        eBooster,
        eStage1,
        eStage2,
        eStage3,
        eStage4,
        eStage5,
        eStage6, // cap when 5 modules
        eDesk, // Desk
        eNBPov
    }

    private RocketPOV _currentRocketPov = RocketPOV.eFullDisplay;
    public RocketPOV CurrentRocketPOV { get => _currentRocketPov; set => SetCurrentRocketPOV(value); }

    public List<Cinemachine.CinemachineVirtualCamera> m_CVCs;

    // Start is called before the first frame update
    void Start()
    {
        if (_inst == null) _inst = this;

        if (m_CVCs.Count != (int)RocketPOV.eNBPov || m_CVCs.Any(x => x == null)) Debug.LogError("Mandatory: " + (int)RocketPOV.eNBPov + " not null CVC.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentRocketPOV(RocketPOV rocketPOV)
    {
        if (rocketPOV == _currentRocketPov) return;

        var previousCVC = m_CVCs[((int)_currentRocketPov)];
        var neoCVC = m_CVCs[((int)rocketPOV)];

        var neoTransposer = neoCVC.GetCinemachineComponent<Cinemachine.CinemachineOrbitalTransposer>();
        var previousTransposer = previousCVC.GetCinemachineComponent<Cinemachine.CinemachineOrbitalTransposer>();
        if (neoTransposer) neoTransposer.m_XAxis.Value = previousTransposer ? previousTransposer.m_XAxis.Value : 0;
        
        previousCVC.Priority = 0;
        neoCVC.Priority = 10;

        _currentRocketPov = rocketPOV;
    }

#if UNITY_EDITOR
    [ContextMenu("Switch View")]
    public void SwitchView()
    {
        if (Application.isPlaying)
        {
            CurrentRocketPOV = (RocketPOV)( (int)(CurrentRocketPOV + 1) % (int)RocketPOV.eNBPov );
        }
        else
        {
            Debug.LogWarning("Can't call this function while editing.");
        }
    }
#endif // UNITY_EDITOR

#if UNITY_EDITOR
    [ContextMenu("Switch To FD")]
#endif // UNITY_EDITOR
    public void SwitchToFD()
    {
        if (Application.isPlaying)
        {
            CurrentRocketPOV = RocketPOV.eFullDisplay;
        }
        else
        {
            Debug.LogWarning("Can't call this function while editing.");
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Switch To Menu")]
#endif // UNITY_EDITOR
    public void SwitchToMenu()
    {
        if (Application.isPlaying)
        {
            CurrentRocketPOV = RocketPOV.eDesk;
        }
        else
        {
            Debug.LogWarning("Can't call this function while editing.");
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Switch View", true)]
    public bool CheckSwitchView()
    {
        return Application.isPlaying;
    }

    [ContextMenu("Switch To FD", true)]
    public bool CheckSwitchToFD()
    {
        return Application.isPlaying;
    }

    [ContextMenu("Switch To Menu", true)]
    public bool CheckSwitchToMenu()
    {
        return Application.isPlaying;
    }
#endif // UNITY_EDITOR
}
