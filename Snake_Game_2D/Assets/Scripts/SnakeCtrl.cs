using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeCtrl : MonoBehaviour
{
    Vector2 direction = Vector2.zero;
    List<Transform> segments = new List<Transform>();

    public GameObject segmentPrefab;
    public int score = 0;
    public GameObject top,bottom,left,right;
    public Camera cam;
    public GameObject food;
    public Text scoreText;

    public bool isShieldOn = false;
    public bool isScoreBoostOn = false;
    public bool isSpeedBoostOn = false;

    public int massGainVal, massBurnVal;

    public GameObject shield;

     public UIManager uiman;

    public GameObject snake2;
    // Start is called before the first frame update
    void Start()
    {
        
        Time.timeScale = 0.5f;
        
        segments.Add(this.transform);
        orthographicBound(cam);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && direction != Vector2.down && direction != Vector2.up)
        {
            direction = Vector2.up;
        }
        else if(Input.GetKeyDown(KeyCode.S) && direction != Vector2.down && direction != Vector2.up)
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right && direction != Vector2.left)
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.right && direction != Vector2.left)
        {
            direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {

        for(int i = segments.Count -1; i>0; i--)
        {
            if(segments[i] != null)
            {
                segments[i].position = segments[i - 1].position;
            }
        }

        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + direction.x,
            Mathf.Round(this.transform.position.y) + direction.y, 0.0f);
    }

    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab).transform;
        segment.position = segments[segments.Count - 1].position;
        StartCoroutine(chnageTag(segment.gameObject));
        segments.Add(segment);
    }

    public void Restart()
    {
        for(int i =1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }
        segments.Clear();
        segments.Add(this.transform);
        transform.position = Vector3.zero;
        uiman.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "food")
        {
            if (isScoreBoostOn == true)
            {
                score += 20;
            }
            else
            {
                score += 10;
            }     
            scoreText.text = score.ToString();
            Grow();
            ChangeFoodPos(other.gameObject);
        }
        else if (other.tag == "Obs")
        {
            if(isShieldOn == false)
            {
                Restart();
            }
            
        }
        else if(other.tag == "Walls")
        {
            ChangeDirection();
        }
        else if(other.tag == "Shield")
        {
            StartCoroutine(ShieldOn());
            
            Debug.Log("Shield");
        }
        else if (other.tag == "ScoreBoost")
        {
            StartCoroutine(ScoreBoostdOn());
            Debug.Log("Score");
        }
        else if (other.tag == "SpeedBoost")
        {
            StartCoroutine(SpeedBoostdOn());
            if (isSpeedBoostOn == true)
            {  
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0.5f;
            }
            Debug.Log("Speed");
        }
        else if(other.tag == "MassGain")
        {
            MassGainFood();
            score += 50;
            scoreText.text = score.ToString();
            Debug.Log("MassGain Food");
        }
        else if (other.tag == "MassBurn")
        {
            MassBurnFood();
            score -= 50;
            scoreText.text = score.ToString();
            Debug.Log("MassBurnn Food");
        }
       else if (other.tag == "Snake2")
       {
            snake2.SetActive(false);
            uiman.GameOver();
       }
    }


   /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Snake2")
        {
            uiman.GameOver1();
        }
    }
   */




    void MassGainFood()
    {
        for(int i = 0; i<massGainVal;i++)
        {
            Grow();
            Debug.Log("Extra segment added");
        }
        
    }


    void MassBurnFood()
    {  
        for (int i = 0; i<massBurnVal; i++)
        {
            if (segments.Count>massBurnVal)
            { 
                Destroy(segments[segments.Count-1].gameObject);
                segments.RemoveAt(segments.Count - 1);
                Debug.Log("Extra segment burned");
            }
            
        }
    }



    public void ChangeDirection()
    {
        if(direction == Vector2.left)
        {
            this.transform.position = new Vector3(Mathf.Round(right.transform.position.x) + direction.x,
           Mathf.Round(this.transform.position.y) + direction.y, 0.0f);
        }
        if (direction == Vector2.right)
        {
            this.transform.position = new Vector3(Mathf.Round(left.transform.position.x) + direction.x,
           Mathf.Round(this.transform.position.y) + direction.y, 0.0f);
        }
        if (direction == Vector2.up)
        {
            this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + direction.x,
           Mathf.Round(bottom.transform.position.y) + direction.y, 0.0f);
        }
        if (direction == Vector2.down)
        {
            this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + direction.x,
           Mathf.Round(top.transform.position.y) + direction.y, 0.0f);
        }
    }

    public IEnumerator chnageTag(GameObject segment)
    {
        yield return new WaitForSeconds(5);

        segment.tag = "Obs";

        yield return null;
    }

    public void ChangeFoodPos(GameObject food)
    {
        food.transform.position = new Vector3(Random.Range(left.transform.position.x, right.transform.position.x),
        Random.Range(top.transform.position.y, bottom.transform.position.y), 0);
    }

    /// <summary>
    /// colliders will adjust according to screen size
    /// </summary>
    /// <param name="camera"></param>
    public void orthographicBound(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;

        Bounds bounds = new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        top.transform.position = new Vector3(0, bounds.max.y, 0);
        bottom.transform.position = new Vector3(0, bounds.min.y, 0);
        left.transform.position = new Vector3(bounds.min.x,0, 0);
        right.transform.position = new Vector3(bounds.max.x,0, 0);
    }



    IEnumerator ShieldOn()
    {
        yield return new WaitForSeconds(0.1f);
        isShieldOn = true;

        yield return new WaitForSeconds(20);
        isShieldOn = false;
    }

    IEnumerator ScoreBoostdOn()
    {
        yield return new WaitForSeconds(0.1f);
        isScoreBoostOn = true;


        yield return new WaitForSeconds(20);
        isScoreBoostOn = false;
    }

    IEnumerator SpeedBoostdOn()
    {
        yield return new WaitForSeconds(0.1f);
        isSpeedBoostOn = true;


        yield return new WaitForSeconds(10);
        isSpeedBoostOn = false;
    }
}
