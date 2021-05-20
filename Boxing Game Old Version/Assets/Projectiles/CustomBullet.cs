using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    //[Header("Components")]
    //public Rigidbody rb;
    //public GameObject explosion;

    //[Header("Layer Mask")]
    //public LayerMask whatIsDamageable;

    //[Header("Bullet Stats")]
    //[Range(0f, 1f)] public float bounciness;
    //public bool useGravity;

    //[Header("Damage")]
    //public int explosionDamage;
    //public float explosionRange;
    //public float explosionForce;

    //[Header("Life Time")]
    //public int maxCollisions;
    //public float maxLifetime;
    //public bool explodeOnTouch = true;

    //int collisions;
    //PhysicMaterial physics_mat;

    //bool hasExploded;

    //private void Start()
    //{
    //    Setup();
    //    //AudioManager.instance.Play("Bullet Shoot");
    //}

    //private void Update()
    //{
    //    // when to explode
    //    if (collisions > maxCollisions) Explode();

    //    // count down lifetime
    //    maxLifetime -= Time.deltaTime;
    //    if (maxLifetime <= 0) Explode();
    //}

    //private void Explode()
    //{
    //    // instantiate explosion
    //    if (explosion != null && !hasExploded)
    //    {
    //        GameObject explosionParticle = Instantiate(explosion, transform.position, Quaternion.identity).gameObject;
    //        Destroy(explosionParticle, .5f);
    //    }
        
    //    hasExploded = true;

    //    //check for enemies
    //    Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsDamageable);
    //    for (int i = 0; i < enemies.Length; i++)
    //    {
    //        // search for IDamageable
    //        var damageable = enemies[i].GetComponentInParent<IDamageable>();
    //        if (damageable != null)
    //            damageable.TakeDamage(explosionDamage);

    //        // get component of rigidbody if there is one and apply forces
    //        if (enemies[i].GetComponent<Rigidbody>())
    //            enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
    //    }

    //    // add a little delay, just to make sure everything works fine
    //    //Invoke(nameof(Delay), 0.05f);
    //    Destroy(gameObject,0.01f);
    //}

    //private void Delay()
    //{
    //    Destroy(gameObject);
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // count up collisions
    //    collisions++;

    //    // explode if bullet hits an enemy || player directly and explodeOnTouch is activated
    //    if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Bullet") && explodeOnTouch) Explode();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Explode();
    //}

    //private void Setup()
    //{
    //    // create a new physics material
    //    physics_mat = new PhysicMaterial
    //    {
    //        bounciness = bounciness,
    //        frictionCombine = PhysicMaterialCombine.Maximum,
    //        bounceCombine = PhysicMaterialCombine.Maximum
    //    };

    //    // assing material to collider
    //    GetComponent<SphereCollider>().material = physics_mat;

    //    hasExploded = false;
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, explosionRange);
    //}
}
