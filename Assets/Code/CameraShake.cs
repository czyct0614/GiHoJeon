using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public float shakeDuration = 0.5f;
    public float shakeAmount = 2f;
    public float decreaseFactor = 1.0f;

    private Vector3 originalPosition;
    private float shakeTimer = 0f;

    void Start()
    {
        originalPosition = cameraFollow.OriginalPosition-new Vector3(0,0,10);
    }

    void FixedUpdate()
    {
        originalPosition = new Vector3(cameraFollow.OriginalPosition.x,cameraFollow.OriginalPosition.y,-10);
        Shake();
        if (shakeTimer > 0)
        {
            
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;

            shakeTimer -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeTimer = 0f;
            transform.position = originalPosition;
        }
    }

    public void Shake()
    {
        shakeTimer = shakeDuration;
    }
}