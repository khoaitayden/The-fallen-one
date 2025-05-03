using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTriggerHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _waterMask;
    [SerializeField] private GameObject _splashParticles;
    [SerializeField] private AudioSource waterslashAudioSource;
    private EdgeCollider2D _edgeColl;
    private InteracableWater _water;
    

    private void Awake()
    {
        _edgeColl = GetComponent<EdgeCollider2D>();
        _water = GetComponent<InteracableWater>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger entered by {collision.gameObject.name} on layer {collision.gameObject.layer}");
        
        if ((_waterMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            Debug.Log("Water mask matched!");
            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log($"Object velocity: {rb.linearVelocity}");
                
                if (_splashParticles != null)
                {
                    Vector2 localPos = transform.localPosition;
                    Vector2 hitObjectPos = collision.transform.position;
                    Bounds hitObjectBounds = collision.bounds;

                    Vector3 spawnPos = Vector3.zero;

                    if (collision.transform.position.y >= _edgeColl.points[1].y + _edgeColl.offset.y + transform.position.y)
                    {
                        spawnPos = hitObjectPos + new Vector2(0f, -hitObjectBounds.extents.y);
                    }
                    else
                    {
                        spawnPos = hitObjectPos + new Vector2(0f, hitObjectBounds.extents.y);
                    }

                    Instantiate(_splashParticles, spawnPos, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("No splash particles assigned!");
                }
                int multiplier = rb.linearVelocity.y < 0 ? -1 : 1;
                float vel = rb.linearVelocity.y * _water.ForceMultiplier;
                vel = Mathf.Clamp(Mathf.Abs(vel), 0f, _water.MaxForce);
                vel *= multiplier;

                _water.Splash(collision, vel);
                waterslashAudioSource.Play();
                Debug.Log($"Applied splash with velocity: {vel}");
            }
            else
            {
                Debug.LogWarning("No Rigidbody2D found on colliding object!");
            }
        }
    }
}