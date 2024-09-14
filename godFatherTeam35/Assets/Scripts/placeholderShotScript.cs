using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeholderShotScript : MonoBehaviour
{
    [SerializeField] private Material ExplosionMat;
    private Material noExplosionMat;

    [SerializeField] private float changeDuration = 1f;
    private SpriteRenderer sprite;

    private bool isMaterialChanged = false;

    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    [SerializeField] private new Camera camera;
    private Transform cameraTransform;
    private Vector3 initialPosition;
    private float shakeTimeRemaining = 0f;

    [SerializeField] private GameObject particleFumes;
    [SerializeField] private GameObject particuleImpact;

    public bool IsReady { get; set; }

    private void Start()
    {
        // Si aucune caméra n'est assignée dans l'inspecteur, prends la caméra principale
        if (camera == null)
        {
            camera = Camera.main;
        }

        // Récupère les composants nécessaires
        sprite = GetComponent<SpriteRenderer>();
        noExplosionMat = sprite.material;

        cameraTransform = camera.transform;
        initialPosition = cameraTransform.localPosition;
    }


    private void Update()
    {
        /*
        // Si le bouton gauche de la souris est cliqué et que le matériau n'a pas déjà été changé
        if (Input.GetMouseButtonDown(0)  && IsReady)
        {
            // Change le matériel en ExplosionMat
            sprite.material = ExplosionMat;

            // Lance la coroutine pour restaurer le matériau après un délai
            StartCoroutine(RestoreMaterialAfterDelay(changeDuration));

            // Déclenche le screen shake au même moment que le changement de matériel
            TriggerShake(shakeDuration, shakeMagnitude);
            SpawnParticlesAtMousePosition(particuleImpact);
            SpawnParticlesAtMousePosition(particleFumes);


        }
        */
        // Gestion du Screen Shake
        if (shakeTimeRemaining > 0)
        {
            // Déplace la caméra de manière aléatoire autour de sa position initiale
            Vector3 shakingPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakingPosition.z = cameraTransform.localPosition.z; //Reset nearClipPlane 
            cameraTransform.localPosition = shakingPosition;

            // Réduit le temps restant du shake
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            // Remet la caméra à sa position initiale une fois le shake terminé
            //cameraTransform.localPosition = initialPosition;
        }
    }

    public void ActivateShoot()
    {
        if (!isMaterialChanged)
        {
            // Change le matériel en ExplosionMat
            sprite.material = ExplosionMat;

            // Lance la coroutine pour restaurer le matériau après un délai
            StartCoroutine(RestoreMaterialAfterDelay(changeDuration));

            // Déclenche le screen shake au même moment que le changement de matériel
            TriggerShake(shakeDuration, shakeMagnitude);
            SpawnParticlesAtMousePosition(particuleImpact);
            SpawnParticlesAtMousePosition(particleFumes);
        }
    }

    // Coroutine pour restaurer le matériel d'origine après un délai
    private IEnumerator RestoreMaterialAfterDelay(float delay)
    {
        isMaterialChanged = true;
        yield return new WaitForSeconds(delay);
        sprite.material = noExplosionMat;
        isMaterialChanged = false;
    }

    // Méthode pour déclencher le Screen Shake avec une durée et une magnitude personnalisées
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimeRemaining = shakeDuration;
        initialPosition = cameraTransform.localPosition;
    }

    private void SpawnParticlesAtAimPointPosition(GameObject particuleType, GameObject pointPosition)
    {
        // Obtient la position de la souris en coordonnées écran
        Vector3 mousePosition = Input.mousePosition;

        // Convertit la position de la souris en coordonnées monde (en prenant en compte la distance de la caméra)
        Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, camera.nearClipPlane));

        // Instancie le système de particules à la position calculée, avec la même rotation que le prefab
        GameObject smokeObject = Instantiate(particuleType, worldPosition, Quaternion.identity);
        smokeObject.transform.SetParent(pointPosition.transform);
    }

    private void SpawnParticlesAtMousePosition(GameObject particuleType)
    {
        // Obtient la position de la souris en coordonnées écran
        Vector3 mousePosition = Input.mousePosition;

        // Convertit la position de la souris en coordonnées monde (en prenant en compte la distance de la caméra)
        Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, camera.nearClipPlane));

        // Instancie le système de particules à la position calculée, avec la même rotation que le prefab
        Instantiate(particuleType, worldPosition, Quaternion.identity);
    }
}