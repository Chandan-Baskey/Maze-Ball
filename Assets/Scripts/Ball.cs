using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Ball : MonoBehaviour
{
    public GameManager gm;

    [Header("Bounce Control")]
    [Range(0f, 1f)]
    public float materialBounciness = 0f;
    public PhysicMaterialCombine bounceCombine = PhysicMaterialCombine.Minimum;
    [Range(0f, 1f)]
    public float bounceDamping = 0f;
    public float maxBounceSpeed = 8f;

    [Tooltip("When true, completely remove post-collision normal component to prevent any bounce.")]
    public bool removeBounciness = true;

    [Header("Auto-fix Ground Materials")]
    [Tooltip("If true, will find objects with the given tag and set their collider PhysicMaterial bounciness to 0 at Awake.")]
    public bool autoZeroGroundMaterials = false;
    public string groundTag = "Ground";

    [Header("Rigidbody Tuning")]
    [Tooltip("Minimum linear drag to apply to the ball to reduce bounciness/energy")]
    public float minLinearDrag = 0.2f;
    [Tooltip("Minimum angular drag to apply to the ball")]
    public float minAngularDrag = 0.05f;

    private Rigidbody rb;
    private Collider col;
    private PhysicMaterial runtimeMaterial;
    private static PhysicMaterial s_sharedRuntimeMaterial;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // Create and assign a runtime PhysicMaterial to control bounciness reliably
        if (col != null)
        {
            float targetBounciness = removeBounciness ? 0f : materialBounciness;
            PhysicMaterialCombine combine = removeBounciness ? PhysicMaterialCombine.Minimum : bounceCombine;
            if (s_sharedRuntimeMaterial == null)
            {
                s_sharedRuntimeMaterial = new PhysicMaterial("BallRuntimeMat_Shared");
                s_sharedRuntimeMaterial.bounciness = targetBounciness;
                s_sharedRuntimeMaterial.bounceCombine = combine;
                var orig = col.material;
                if (orig != null)
                {
                    s_sharedRuntimeMaterial.dynamicFriction = orig.dynamicFriction;
                    s_sharedRuntimeMaterial.staticFriction = orig.staticFriction;
                    s_sharedRuntimeMaterial.frictionCombine = orig.frictionCombine;
                }
            }
            else
            {
                // update shared material values in case settings changed in inspector
                s_sharedRuntimeMaterial.bounciness = targetBounciness;
                s_sharedRuntimeMaterial.bounceCombine = combine;
            }

            runtimeMaterial = s_sharedRuntimeMaterial;
            col.material = runtimeMaterial;
        }

        if (rb != null)
        {
            // reduce tunneling and smooth visuals
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.drag = Mathf.Max(rb.drag, minLinearDrag);
            rb.angularDrag = Mathf.Max(rb.angularDrag, minAngularDrag);
        }

        // Optionally zero bounciness on ground colliders identified by tag
        if (autoZeroGroundMaterials)
        {
            try
            {
                var grounds = GameObject.FindGameObjectsWithTag(groundTag);
                foreach (var g in grounds)
                {
                    var colliders = g.GetComponents<Collider>();
                    foreach (var gc in colliders)
                    {
                        if (gc == null || gc.isTrigger) continue;
                        var baseMat = gc.material;
                        var gmMat = new PhysicMaterial((baseMat != null ? baseMat.name : "Ground") + "_RuntimeZero");
                        gmMat.bounciness = 0f;
                        gmMat.bounceCombine = PhysicMaterialCombine.Minimum;
                        if (baseMat != null)
                        {
                            gmMat.dynamicFriction = baseMat.dynamicFriction;
                            gmMat.staticFriction = baseMat.staticFriction;
                            gmMat.frictionCombine = baseMat.frictionCombine;
                        }
                        gc.material = gmMat;
                    }
                }
            }
            catch (UnityException)
            {
                // Tag not defined or other issue — ignore to avoid runtime error
            }
        }
    }

    void Update()
    {
        if (transform.position.y < -5f)
        {
            if (gm != null)
            {
                gm.GameOver();
                gm.TimeText();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WinGround"))
        {
            if (gm != null) gm.WinGame();
            return;
        }

        if (rb == null) return;

        Vector3 vel = rb.velocity;
        float limit = maxBounceSpeed * 3f;

        if (removeBounciness)
        {
            // remove any component away from contact normals -> no bounce
            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 n = contact.normal;
                float vn = Vector3.Dot(vel, n);
                if (vn > 0f) vel -= n * vn;
            }
        }
        else
        {
            // damp normal components based on bounceDamping
            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 n = contact.normal;
                float vn = Vector3.Dot(vel, n);
                if (vn > 0f)
                {
                    float newVn = Mathf.Min(vn * bounceDamping, maxBounceSpeed);
                    vel -= n * (vn - newVn);
                }
            }
        }

        // safety clamp overall speed
        if (vel.sqrMagnitude > limit * limit)
        {
            vel = vel.normalized * limit;
        }

        rb.velocity = vel;
    }

    private void OnCollisionStay(Collision collision)
    {
        // ensure we don't accumulate extreme speeds while in contact
        if (rb == null) return;
        Vector3 v = rb.velocity;
        float limit = maxBounceSpeed * 3f;
        if (v.sqrMagnitude > limit * limit)
        {
            rb.velocity = v.normalized * limit;
        }
    }
}