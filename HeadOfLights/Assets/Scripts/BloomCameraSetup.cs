using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomCameraManager : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public LayerMask bloomLayer; // Layer uniquement pour les objets à effet bloom
    [Tooltip("Index du renderer dans l'URP Asset (0 = défaut, 1 = Bloom, etc.)")]
    public int bloomRendererIndex = 1;

    [Header("Bloom Settings")]
    public float bloomIntensity = 1.2f;
    public float bloomThreshold = 1f;

    void Start()
    {
        CreateBloomCamera();
        CreateBloomVolume();
    }

    void CreateBloomCamera()
    {
        // Crée un nouvel objet caméra
        GameObject bloomCamObj = new GameObject("BloomCamera");
        bloomCamObj.transform.SetParent(mainCamera.transform);
        bloomCamObj.transform.localPosition = Vector3.zero;
        bloomCamObj.transform.localRotation = Quaternion.identity;

        // Ajoute et configure la caméra
        Camera bloomCam = bloomCamObj.AddComponent<Camera>();
        bloomCam.clearFlags = CameraClearFlags.Depth;
        bloomCam.cullingMask = bloomLayer;
        bloomCam.depth = mainCamera.depth + 1;
        bloomCam.allowMSAA = false;
        bloomCam.allowHDR = true;

        // Copie FOV et paramètres de projection
        bloomCam.fieldOfView = mainCamera.fieldOfView;
        bloomCam.nearClipPlane = mainCamera.nearClipPlane;
        bloomCam.farClipPlane = mainCamera.farClipPlane;

        // Ajoute Data pour URP
        var data = bloomCamObj.AddComponent<UniversalAdditionalCameraData>();
        data.renderPostProcessing = true;
        data.SetRenderer(bloomRendererIndex); // Utilise l'index manuel
        data.requiresColorOption = CameraOverrideOption.On;
        data.requiresDepthOption = CameraOverrideOption.On;
    }

    void CreateBloomVolume()
    {
        GameObject volumeObj = new GameObject("BloomVolume");
        volumeObj.layer = LayerMaskToLayerIndex(bloomLayer);

        var volume = volumeObj.AddComponent<Volume>();
        volume.isGlobal = true;

        VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();
        Bloom bloom;
        if (!profile.TryGet(out bloom))
        {
            bloom = profile.Add<Bloom>(true);
            bloom.intensity.value = bloomIntensity;
            bloom.threshold.value = bloomThreshold;
            bloom.active = true;
        }

        volume.sharedProfile = profile;
    }

    int LayerMaskToLayerIndex(LayerMask mask)
    {
        int layer = 0;
        int bitmask = mask.value;
        while ((bitmask >>= 1) != 0) layer++;
        return layer;
    }
}
