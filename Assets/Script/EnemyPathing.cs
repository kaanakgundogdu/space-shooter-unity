using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
	[SerializeField] List<Transform> waypoints;//Konum verilerini tutaacak
    WaveConfig waveConfig;


    int waypointIndex = 0;


    void Start()
	{
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position; // Starting position
    }

    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig; // Bu classdaki waveconfig ile şimdiki fonksiyonu çağırıken kullanıdğımız wave configi eşitliyoruz.
    }


    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1) // Array de Lenght List de Count
        {
            // MoveTowards için --> Vectore2 Current , Vector2 target, float max distance
            var targetPosition = waypoints[waypointIndex].transform.position; // Gideceğimiz yer
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition) //Eğer gideceğimiz yere ulaştıysak arttır.
            {
                waypointIndex++;
            }
        }
        else //Tüm yerler bittiyse destroy
        {
            Destroy(gameObject);
        }
    }
}
