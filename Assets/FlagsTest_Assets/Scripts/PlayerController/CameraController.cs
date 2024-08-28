using UnityEngine;

namespace FlagsTest
{
    public class CameraController :PlayerInitilize
    {
        [SerializeField] float _CameraLerpSpeed = 10;

        Transform targetTransform;

        void LateUpdate ()
        {
            if (Player)
            {
                transform.position = Vector3.Lerp (transform.position, Player.ViewTransform.position, _CameraLerpSpeed * Time.deltaTime);
            }
        }
    }
}
