using UnityEngine;

namespace Player_Script
{
    public class FPSounds : FPControllerModule
    {
        [SerializeField] AudioSource audioSource;

        [SerializeField] AudioClip[] jumpSounds;
        [SerializeField] AudioClip[] landSounds;
        [SerializeField] AudioClip[] footstepSounds;

        [SerializeField] float footstepTimer = 0f;

        NoiseInfo newNoise = new NoiseInfo
        {
            radius = 0f,
            type = NoiseType.Medium
        };

        private void Start()
        {
            controller.Jumped.AddListener(PlayJumpSound);
            controller.Landed.AddListener(OnLanded);
        }

        private void Update()
        {
            UpdateFootstepSounds();
        }

        private void UpdateFootstepSounds()
        {
            if (controller.Grounded && controller.currentSpeed >= 0.2f)
            {
                float rate = preset.footstepWalkRate;
                float volume = preset.footstepWalkVolume;

                if (controller.crouching)
                {
                    rate = preset.footstepCrouchRate;
                    volume = preset.footstepCrouchVolume;
                }
                else
                {
                    float t = Mathf.InverseLerp(preset.walkSpeed, preset.sprintSpeed, controller.currentSpeed);

                    rate = Mathf.Lerp(preset.footstepWalkRate, preset.footstepSprintRate, t);
                    volume = Mathf.Lerp(preset.footstepWalkVolume, preset.footstepSprintVolume, t);
                }

                if (Time.time >= footstepTimer)
                {
                    if (controller.crouching)
                    {
                        newNoise.position = transform.position;
                        newNoise.radius = 5f;
                        newNoise.type = NoiseType.Silent;
                    }
                    else if (controller.sprinting)
                    {
                        newNoise.position = transform.position;
                        newNoise.radius = 25f;
                        newNoise.type = NoiseType.Loud;
                    }
                    else
                    {
                        newNoise.position = transform.position;
                        newNoise.radius = 10f;
                        newNoise.type = NoiseType.Medium;
                    }

                    PlaySound(footstepSounds, newNoise, volume);
                    footstepTimer = Time.time + rate;
                }
            }
        }

        void OnLanded()
        {
            float scale = Mathf.InverseLerp(0f, -20f, controller.verticalVelocity);

            scale *= preset.maxLandSoundVolume;

            newNoise.position = transform.position;
            newNoise.radius = 20f;
            newNoise.type = NoiseType.Loud;

            PlaySound(landSounds, newNoise, scale);
        }

        private void PlaySound(AudioClip[] clips, NoiseInfo noiseInfo, float volumeScale = 1f)
        {
            try
            {
                int index = Random.Range(0, clips.Length);
                AudioClip clip = clips[index];

                audioSource.PlayOneShot(clip, volumeScale);


                NoiseSystem.Instance.MakeNoise(newNoise);

            }
            catch { }
        }

        private void PlayJumpSound()
        {
            newNoise.position = transform.position;
            newNoise.radius = 12f;
            newNoise.type = NoiseType.Medium;

            PlaySound(jumpSounds, newNoise, preset.jumpSoundVolume);
        }
    }
}