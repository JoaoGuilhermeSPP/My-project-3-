using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        [Range(0f, 1f)]
        public float parallaxEffect = 0.5f;
        public bool invertMovement = false;
    }

    public ParallaxLayer[] layers;
    public Transform cameraTransform;

    private Vector3 lastCameraPosition;
    private MovimentoCamera movimentoCamera;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;

        movimentoCamera = cameraTransform.GetComponent<MovimentoCamera>();
    }

    void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        Vector3 cameraDelta = cameraTransform.position - lastCameraPosition;

        float inertiaMultiplier = 1f;

        if (movimentoCamera != null)
        {
            bool inerciaAtiva = !movimentoCamera.movendoCamera &&
                                movimentoCamera.velocidadeInstantanea.magnitude > 0.01f;

            if (inerciaAtiva)
                inertiaMultiplier = 1.5f;
        }

        foreach (var layer in layers)
        {
            Vector3 offset = cameraDelta * layer.parallaxEffect * inertiaMultiplier;

            if (layer.invertMovement)
                offset = -offset;

            if (layer.layerTransform != null)
                layer.layerTransform.position += offset;
        }

        lastCameraPosition = cameraTransform.position;
    }
}
