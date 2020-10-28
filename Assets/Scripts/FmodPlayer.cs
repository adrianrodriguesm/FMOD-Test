using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FmodPlayer : MonoBehaviour
{
    public LayerMask layer;
    float distance = 0.1f;
    float Material;
    void PlayOneShotEvent(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
    }

    /*
    private void FixedUpdate()
    {
        MaterialCheck();
        Debug.DrawRay(transform.position, Vector2.down * distance, Color.blue);
    }*/

    void MaterialCheck()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position, Vector2.down, distance, layer);

        if (hit.collider)
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

    void PlayFootstepsSoundEvent(string path)
    {
        MaterialCheck();
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
        Footsteps.setParameterByName("Material", Material);
        Footsteps.start();
        Footsteps.release();//Destroy the event created after he finish
    }
}
