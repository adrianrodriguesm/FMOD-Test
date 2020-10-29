using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FmodPlayer : MonoBehaviour
{
    public LayerMask layerGround;

    PlayerCharacter player;

    /// Footsteps
    float distance = 0.05f;
    float Material;
    FMOD.Studio.EventInstance Footsteps;

    /// Jump and Fall
    FMOD.Studio.EventInstance Landing;
    bool wasGrounded = true;

    private void Start()
    {
        player = GetComponent<PlayerCharacter>();
        Footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Ellen/Ellen_Footsteps");
        Landing = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Ellen/Ellen_Land");
    }



    private void FixedUpdate()
    {
       
        //Debug.DrawRay(transform.position, Vector2.down * distance, Color.blue);

        ///Fall sound
        PlayerLanded();
        wasGrounded = IsGrounded();
     

    }

    void PlayerLanded()
    {
        if (IsGrounded() && !wasGrounded && player.m_MoveVector.y < 0)
        {
            
            Landing.setParameterByName("Velocity", player.m_MoveVector.y);
            Footsteps.setParameterByName("FootstepsDuck", player.m_MoveVector.y);
            Landing.start();

        }
    }

    RaycastHit2D IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, distance, layerGround);

    }

    void MaterialCheck()
    {
        RaycastHit2D hit;

        hit = IsGrounded();

        if (hit)
        {
            switch (hit.collider.tag)
            {
                case "Material: Earth":
                    Material = 1;
                    break;
                case "Material: Stone":
                    Material = 2;
                    break;
                default:
                    Material = 1;
                    break;

            }
        }
    }

    void PlayFootstepsSoundEvent()
    {
        
        Footsteps.start();
        
    }

    private void OnDestroy()
    {
        Footsteps.release();//Destroy the event created after he finish
        Landing.release();

    }

    void PlayMeleeSoundEvent(string path)
    {
        MaterialCheck();
        Footsteps.setParameterByName("Material", Material);
        FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
    }
}
