using UnityEngine;
using TMPro;

public enum GunType
{
    Player,
    Enemy
}

public class ProjectileGun : MonoBehaviour
{
    [Header("Bullet Prefab")]
    public GameObject bulletPrefab;

    [Header("Bullet Force Settings")]
    public float shootForce, upwardForce;

    [Header("Gun Stats")]
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShoots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    // bools
    bool shooting, readyToShoot, reloading;

    [Header("References")]
    public Camera fpsCam;
    public Transform firePoint;

    [Header("Graphics and UI")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    [Header("Debug")]
    public bool allowInvoke = true;

    private void Awake()
    {
        // make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        // set ammo display, if it exists
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }

    private void MyInput()
    {
        // check if allowed to hold down button and take corresponding input
        //shooting = allowButtonHold ? PlayerInputController.instance.Current.ShootHoldInput : PlayerInputController.instance.Current.ShootInput;

        // reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        // reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        // shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            // set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // just a ray through the middle of your screen

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPoint = hit.point;
            Debug.DrawLine(firePoint.position, hit.point, Color.magenta);
        }
        else
        {
            Debug.DrawLine(firePoint.position, hit.point, Color.white);
            targetPoint = ray.GetPoint(75); // just a point far away from the player
        }

        // calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - firePoint.position;

        // calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); // add spread to the last direction

        // camera shake
        //StartCoroutine(CameraShake.instance.Shake(0.05f, 0.05f));

        // instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        // rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;

        // add froces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        // instantiate muzzle flash if you have one
        if (muzzleFlash != null)
        {
            GameObject muz = Instantiate(muzzleFlash, firePoint.position, Quaternion.identity);
            Destroy(muz, .5f);

        }
        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked)
        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;
        }

        // if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShoots);
    }

    private void ResetShot()
    {
        // allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
