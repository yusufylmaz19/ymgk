using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject ballonPrefab;
    [SerializeField] GameObject finishPanel;

    [SerializeField] int numberOfObjects = 10;

    [SerializeField] float spawnRadius = 5f;
    [SerializeField] float minDistanceToCamera = 1f;
    [SerializeField] float maxDistanceToCamera = 10f;

    [SerializeField] Camera arCamera;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] numbers;

    int numberIndex = 0;
    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float randomAngle = Random.Range(0f, 360f);

            float randomDistance = Random.Range(minDistanceToCamera, maxDistanceToCamera);

            float spawnX = transform.position.x + Mathf.Cos(randomAngle) * randomDistance;
            float spawnZ = transform.position.z + Mathf.Sin(randomAngle) * randomDistance;

            float spawnY = transform.position.y;

            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
            Instantiate(ballonPrefab, spawnPosition, Quaternion.identity);
        }
    }
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Shoot(touch.position);
        }
    }
    public void Shoot(Vector2 touchPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                Balloon balloon = hit.collider.GetComponent<Balloon>();
                if (balloon != null)
                {
                    Destroy(balloon.gameObject);

                    audioSource.clip = numbers[numberIndex];
                    audioSource.Play();

                    numberIndex++;

                    if (numberIndex >= numbers.Length)
                    {
                        // Done
                        finishPanel.SetActive(true);

                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
