using UnityEngine;

namespace FlagsTest
{
    public class CameraController :PlayerInitilize
    {
        [SerializeField] float _CameraLerpSpeed = 10;

        void LateUpdate ()
        {
            if (IsInited)
            {
                transform.position = Vector3.Lerp (transform.position, Player.transform.position, _CameraLerpSpeed * Time.deltaTime);
            }
        }
    }
}
