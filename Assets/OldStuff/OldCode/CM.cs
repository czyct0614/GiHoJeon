using UnityEngine;

public class CameraManager : MonoBehaviour
{
    void Awake()
    {
        // 현재 씬에 있는 모든 카메라를 찾습니다.
        Camera[] cameras = FindObjectsOfType<Camera>();

        // 메인 카메라가 이미 존재하는지 여부를 나타내는 변수입니다.
        bool mainCameraExists = false;

        // 모든 카메라를 반복하여 메인 카메라가 하나만 존재하도록 합니다.
        foreach (Camera camera in cameras)
        {
            // 현재 카메라가 메인 카메라인지 확인합니다.
            if (camera.CompareTag("MainCamera"))
            {
                // 이미 메인 카메라가 존재하는 경우 현재 카메라를 파괴합니다.
                if (mainCameraExists)
                {
                    Destroy(camera.gameObject);
                }
                else
                {
                    // 현재 카메라가 메인 카메라인 경우, mainCameraExists를 true로 설정합니다.
                    mainCameraExists = true;
                }
            }
        }
    }
}
