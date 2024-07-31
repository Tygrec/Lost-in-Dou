using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class LightingManager : MonoBehaviour {
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _preset;

    private void Update() {
        if (_preset == null)
            return;
        
        UpdateLighting(Game.G.Time.GlobalTimer / 1440f);
    }
    private void UpdateLighting(float timePercent) {
        RenderSettings.ambientLight = _preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = _preset.FogColor.Evaluate(timePercent);

        if (_directionalLight != null) {
            _directionalLight.color = _preset.DirectionalColor.Evaluate(timePercent);
            _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }
    private void OnValidate() {

        if (_directionalLight != null)
            return;

        if (RenderSettings.sun != null) {
            _directionalLight = RenderSettings.sun;
        }
        else {
            Light[] lights = GameObject.FindObjectsOfType<Light>();

            foreach (var light in lights) {

                if (light.type == LightType.Directional) {
                    _directionalLight = light;
                    return;
                }
            }
        }
    }
}
