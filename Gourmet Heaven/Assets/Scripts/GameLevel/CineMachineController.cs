using Unity.Cinemachine;
using UnityEngine;

public class CineMachineController : CinemachineExtension
{
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase VirtualCamera, CinemachineCore.Stage stage, ref CameraState state, float deltaTime) // Uses polymorphism to adjust cinemachine behaviour
    {
        if (stage == CinemachineCore.Stage.Finalize) // If in the final stages of processing
        {
            Vector3 Position = state.RawPosition;

            Position.y = Mathf.Clamp(Position.y, -10f, 0f);
            Position.x = 0; // Lock the x-position

            state.RawPosition = Position; // Apply the constraints.
        }
    }
}