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
        // Si aucune cam�ra n'est assign�e dans l'inspecteur, prends la cam�ra principale
        if (camera == null)
        {
            camera = Camera.main;
        }

        // R�cup�re les composants n�cessaires
        sprite = GetComponent<SpriteRenderer>();
        noExplosionMat = sprite.material;

        cameraTransform = camera.transform;
        initialPosition = cameraTransform.localPosition;
    }


    private void Update()
    {
        /*
        // Si le bouton gauche de la souris est cliqu� et que le mat�riau n'a pas d�j� �t� chang�
        if (Input.GetMouseButtonDown(0)  && IsReady)
        {
            // Change le mat�riel en ExplosionMat
            sprite.material = ExplosionMat;

            // Lance la coroutine pour restaurer le mat�riau apr�s un d�lai
            StartCoroutine(RestoreMaterialAfterDelay(changeDuration));

            // D�clenche le screen shake au m�me moment que le changement de mat�riel
            TriggerShake(shakeDuration, shakeMagnitude);
            SpawnParticlesAtMousePosition(particuleImpact);
            SpawnParticlesAtMousePosition(particleFumes);


        }
        */
        // Gestion du Screen Shake
        if (shakeTimeRemaining > 0)
        {
            // D�place la cam�ra de mani�re al�atoire autour de sa position initiale
            Vector3 shakingPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakingPosition.z = cameraTransform.localPosition.z; //Reset nearClipPlane 
            cameraTransform.localPosition = shakingPosition;

            // R�duit le temps restant du shake
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            // Remet la cam�ra � sa position initiale une fois le shake termin�
            //cameraTransform.localPosition = initialPosition;
        }
    }

    public void ActivateShoot()
    {
        if (!isMaterialChanged)
        {
            // Change le mat�riel en ExplosionMat
            sprite.material = ExplosionMat;

            // Lance la coroutine pour restaurer le mat�riau apr�s un d�lai
            StartCoroutine(RestoreMaterialAfterDelay(changeDuration));

            // D�clenche le screen shake au m�me moment que le changement de mat�riel
            TriggerShake(shakeDuration, shakeMagnitude);
            SpawnParticlesAtMousePosition(particuleImpact);
            SpawnParticlesAtMousePosition(particleFumes);
        }
    }

    // Coroutine pour restaurer le mat�riel d'origine apr�s un d�lai
    private IEnumerator RestoreMaterialAfterDelay(float delay)
    {
        isMaterialChanged = true;
        yield return new WaitForSeconds(delay);
        sprite.material = noExplosionMat;
        isMaterialChanged = false;
    }

    // M�thode pour d�clencher le Screen Shake avec une dur�e et une magnitude personnalis�es
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimeRemaining = shakeDuration;
        initialPosition = cameraTransform.localPosition;
    }

    private void SpawnParticlesAtAimPointPosition(GameObject particuleType, GameObject pointPosition)
    {
        // Obtient la position de la souris en coordonn�es �cran
        Vector3 mousePosition = Input.mousePosition;

        // Convertit la position de la souris en coordonn�es monde (en prenant en compte la distance de la cam�ra)
        Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, camera.nearClipPlane));

        // Instancie le syst�me de particules � la position calcul�e, avec la m�me rotation que le prefab
        GameObject smokeObject = Instantiate(particuleType, worldPosition, Quaternion.identity);
        smokeObject.transform.SetParent(pointPosition.transform);
    }

    private void SpawnParticlesAtMousePosition(GameObject particuleType)
    {
        // Obtient la position de la souris en coordonn�es �cran
        Vector3 mousePosition = Input.mousePosition;

        // Convertit la position de la souris en coordonn�es monde (en prenant en compte la distance de la cam�ra)
        Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, camera.nearClipPlane));

        // Instancie le syst�me de particules � la position calcul�e, avec la m�me rotation que le prefab
        Instantiate(particuleType, worldPosition, Quaternion.identity);
    }
}