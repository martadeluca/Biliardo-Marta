using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;

public class BiliardoManager : MonoBehaviour
{
    // Members
    public List<GameObject> ball;

    public Parameters parameters;
    public ChangeParameters changeparameters;
    public TextManager textmanager;
    public RecordScene recordscene;

    public GameObject ballToClone;

    public Light lt1;
    public Light lt2;

    public Slider sliderMaster;
    public Slider sliderProf;
    public Slider sliderProsp;
    public Slider sliderShadow;
    public Slider sliderCS;
    public Slider sliderDC;
    public Slider sliderColor;
    public Slider sliderGravity;

    public GameObject pavimento;
    public GameObject soffitto;
    public GameObject parete1;
    public GameObject parete2;
    public GameObject parete3;
    public GameObject parete4;

    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;

    public Button time;

    public Text contoallarovescia;

    public int k = 0;
    public int t = 0;
    public int counter = 0;

    public float timeLeft = 10.0f;

    string fileName = "Registrazione.txt";
    public StreamWriter sr;

    float altezza;
    bool onetime = false;

    void Start()
    {
        ball = new List<GameObject>();

        parameters.randomSeed = Random.Range(1, 200);

        Random.InitState(parameters.randomSeed);

        textmanager.Inizializzazione();
        textmanager.UpdateAllSliders();

        parete1.transform.localScale = new Vector3(550.0f, parameters.ballSize, 1.0f);
        parete2.transform.localScale = new Vector3(550.0f, parameters.ballSize, 1.0f);
        parete3.transform.localScale = new Vector3(550.0f, parameters.ballSize, 1.0f);
        parete4.transform.localScale = new Vector3(550.0f, parameters.ballSize, 1.0f);
        pavimento.transform.localPosition = new Vector3(0.0f, -parete1.transform.localScale[1] / 2, 0.0f);
        soffitto.transform.localPosition = new Vector3(0.0f, parete1.transform.localScale[1] / 2, 0.0f);

        sr = File.CreateText(fileName);
    }

    void Update()
    {
        ApplyParameters();
        CheckPosition();
        SliderCS(sliderCS.value);
        SliderProsp(sliderProsp.value);
        SliderShadow(sliderShadow.value);
        parameters.gravityValue = sliderGravity.value;

        pavimento.transform.localPosition = new Vector3(0.0f, -(parete1.transform.localScale[1] / 2) + parete1.transform.localPosition[1], 0.0f);
        soffitto.transform.localPosition = new Vector3(0.0f, (parete1.transform.localScale[1] / 2) + parete1.transform.localPosition[1], 0.0f);

        if (sliderProsp.value > 0 && sliderProsp.value == sliderProf.value && sliderProsp.value == sliderCS.value && sliderProsp.value == sliderShadow.value && sliderProsp.value == sliderDC.value)
            sliderMaster.value = sliderProsp.value;

        if (Input.GetKeyDown("space"))
            k++;

        if (k % 4 == 1)
        {
            canvas1.SetActive(false);
            canvas2.SetActive(true);
        }
        else if (k % 4 == 2)
        {
            canvas2.SetActive(false);

			if (!onetime)
			{
                recordscene.StartRecord();
                onetime = true;
            }
        }
        else if (k % 4 == 3)
        {
            canvas3.SetActive(true);

            if (onetime)
            {
                recordscene.StopRecord();
                onetime = false;
            }
        }
        else if (k % 4 == 0)
        {
            canvas3.SetActive(false);
            canvas1.SetActive(true);
        }

        time.GetComponent<Button>().onClick.AddListener(() =>
        {
            t = 1;
        });

        if (t == 1)
        {
            timeLeft -= Time.deltaTime;

            contoallarovescia.GetComponent<Text>().text = "Time Left: " + Mathf.RoundToInt(timeLeft);

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                contoallarovescia.GetComponent<Text>().text = "END OF THE TEST";
                Application.Quit();
            }
        }

        foreach(GameObject b in ball)
        {
            string s = "(" + Time.realtimeSinceStartup + "," + b.transform.position[0] + "," + b.transform.position[1] + "," + b.transform.position[2] + ")";
            sr.WriteLine(s);
        }
    }

    private void FixedUpdate()
    {
        foreach (GameObject b in ball)
        {
            UpdatePalla(b);
        }
    }

    public void UpdatePalla(GameObject b)
    {
        MyBall myball = b.GetComponent<MyBall>();
        myball.velparametro = parameters.ballSpeed;

        if (parete1.transform.position.z - b.transform.position.z == parameters.ballSize / 2 & myball.speed.z > 0)
            myball.direzione.Scale(new Vector3(1.0f, 1.0f, -1.0f));
        if (b.transform.position.z - parete3.transform.position.z == parameters.ballSize / 2 & myball.speed.z < 0)
            myball.direzione.Scale(new Vector3(1.0f, 1.0f, -1.0f));
        if (parete4.transform.position.x - b.transform.position.x == parameters.ballSize / 2 & myball.speed.x > 0)
            myball.direzione.Scale(new Vector3(-1.0f, 1.0f, 1.0f));
        if (b.transform.position.x - parete2.transform.position.x == parameters.ballSize / 2 & myball.speed.x < 0)
            myball.direzione.Scale(new Vector3(-1.0f, 1.0f, 1.0f));
        if (soffitto.transform.position.y - b.transform.position.y == parameters.ballSize / 2 & myball.speed.y > 0)
            myball.direzione.Scale(new Vector3(1.0f, -1.0f, 1.0f));
        if (b.transform.position.y - pavimento.transform.position.y == parameters.ballSize / 2 & myball.speed.y < 0)
            myball.direzione.Scale(new Vector3(1.0f, -1.0f, 1.0f));

        foreach (GameObject p in ball)
        {
            myball.p_test = b.transform.position - p.transform.position;

            if (Mathf.RoundToInt(myball.p_test.magnitude) == parameters.ballSize & AvvicinamentoPalla(b, p))
            {
                myball.direzione *= -1;
                p.GetComponent<MyBall>().direzione *= -1;

                myball.v_temp = myball.speed;
                myball.speed = p.GetComponent<MyBall>().speed;
                p.GetComponent<MyBall>().speed = myball.v_temp;

                if (parameters.gravityValue != 0)
                {
                    myball.c = 0;
                    p.GetComponent<MyBall>().c = 0;
                }
            }
            if (Mathf.RoundToInt(myball.p_test.magnitude) > 0 & Mathf.RoundToInt(myball.p_test.magnitude) < parameters.ballSize & AvvicinamentoPalla(b, p))
            {
                myball.direzione *= -1;
                p.GetComponent<MyBall>().direzione = myball.direzione * -1;

                myball.v_temp = myball.speed;
                myball.speed = p.GetComponent<MyBall>().speed;
                p.GetComponent<MyBall>().speed = myball.v_temp;

                if (parameters.gravityValue != 0)
                {
                    myball.c = 0;
                    p.GetComponent<MyBall>().c = 0;
                }
            }
        }

        if (parameters.gravityValue == 0.0f)
        {
            myball.velocity = new Vector3(parameters.ballSpeed, parameters.ballSpeed, parameters.ballSpeed);
        }
        else
        {
            myball.gravval = parameters.gravityValue;

            if(myball.c == 0)
            {
                myball.velocity = new Vector3(parameters.ballSpeed, parameters.ballSpeed, parameters.ballSpeed);
            }

            if (soffitto.transform.position.y - b.transform.position.y == parameters.ballSize / 2 & myball.speed.y > 0)
            {
                Debug.Log("colpo 1: " + myball.speed);
                myball.ultimavel = - myball.velocity;
                myball.nuovavel = - myball.velocity;
                myball.tstart = 0;

                myball.c = 1;
                myball.z = 1;               
            }

            if (b.transform.position.y - pavimento.transform.position.y == parameters.ballSize / 2 & myball.speed.y < 0)
            {
                Debug.Log("colpo 2: " + myball.speed);
                myball.ultimavel = myball.velocity;
                myball.nuovavel = myball.velocity;
                myball.tstart = 0;

                if (myball.ultimavel.y > 15)
                {
                    myball.c = 1;
                    myball.z = 0;
                }
                else
                {
                    myball.c = 2;
                    myball.ultimavel = myball.velocity;
                }
            }
        }

        myball.speed = new Vector3 (myball.direzione.x * Mathf.Abs(myball.velocity.x), myball.direzione.y * Mathf.Abs(myball.velocity.y), myball.direzione.z * Mathf.Abs(myball.velocity.z));              
        
        myball.p_temp = b.transform.position + (myball.speed * Time.deltaTime);

        if (parete1.transform.position.z - myball.p_temp.z < parameters.ballSize / 2)
            myball.p_temp.z = parete1.transform.position[2] - parameters.ballSize / 2;
        if (myball.p_temp.z - parete3.transform.position.z < parameters.ballSize / 2)
            myball.p_temp.z = parete3.transform.position[2] + parameters.ballSize / 2;
        if (parete4.transform.position.x - myball.p_temp.x < parameters.ballSize / 2)
            myball.p_temp.x = parete4.transform.position[0] - parameters.ballSize / 2;
        if (myball.p_temp.x - parete2.transform.position.x < parameters.ballSize / 2)
            myball.p_temp.x = parete2.transform.position[0] + parameters.ballSize / 2;     
        if (soffitto.transform.position.y - myball.p_temp.y < parameters.ballSize / 2)
            myball.p_temp.y = soffitto.transform.position[1] - parameters.ballSize / 2;
        if (myball.p_temp.y - pavimento.transform.position.y < parameters.ballSize / 2)
            myball.p_temp.y = pavimento.transform.position[1] + parameters.ballSize / 2;

        b.transform.position = myball.p_temp;
    }

    public bool AvvicinamentoPalla(GameObject a, GameObject b)
    {
        bool avv = true;

        Vector3 pa_now = a.transform.position;
        Vector3 pb_now = b.transform.position;

        Vector3 pa_next = a.transform.position + (a.GetComponent<MyBall>().speed * Time.deltaTime);
        Vector3 pb_next = b.transform.position + (b.GetComponent<MyBall>().speed * Time.deltaTime);

        if ((pa_next - pb_next).magnitude < (pa_now - pb_now).magnitude)
            avv = true;
        if ((pa_next - pb_next).magnitude > (pa_now - pb_now).magnitude)
            avv = false;
        
        return avv;
    }

    public void InizializzaPalla(GameObject b)
    {
        MyBall myball = b.GetComponent<MyBall>();

        myball.vel_comp = Mathf.Round(Random.Range(-100.0f, 100.0f) * 100f) / 100f;
        myball.speed = new Vector3(myball.vel_comp, myball.vel_comp, myball.vel_comp);
    }

    public void ApplyParameters()
    {
        ApplyBallNumber();
        ApplyBallSize();
        ApplyBallYSpeed();
        ApplyRandomSeed();
    }

    void ApplyBallNumber()
    {
        int missing = parameters.ballNumber - ball.Count;

        if (missing > 0)
        {
            for (int i = 0; i < missing; i++)
            {              
                GameObject newBall = SpawnNewBall();
                newBall.AddComponent<MyBall>();
                InizializzaPalla(newBall);
                ball.Add(newBall);
            }
        }

        if (missing < 0)
        {
            for (int i = 0; i < -missing; i++)
            {
                Destroy(ball[ball.Count - 1]);
                ball.RemoveAt(ball.Count - 1);
            }
        }

        foreach (GameObject b in ball)
        {
            b.GetComponent<MyBall>().colorHue = Random.Range(0.0f, 1.0f);
            b.GetComponent<MyBall>().colorValue = Random.Range(0.5f, 1.0f);
        }


        SliderColor(sliderColor.value);
    }

    void ApplyBallSize()
    {
        foreach (GameObject b in ball)
        {
            b.transform.localScale = new Vector3(parameters.ballSize, parameters.ballSize, parameters.ballSize);
        }
    }

    void ApplyBallYSpeed()
    {           
        foreach (GameObject b in ball)
        {
            MyBall myball = b.GetComponent<MyBall>();
            myball.speed.y = parameters.depthValue * 1.05f * myball.speed.y;
        }       
    }  

    void ApplyRandomSeed()
    {
       Random.InitState(parameters.randomSeed);
    }
    
    public void SliderMaster(float val)
    {
        SliderProsp(val);
        SliderProf(val);
        SliderCS(val);
        SliderShadow(val);
        SliderDC(val);

        sliderProsp.value = val;
        sliderProf.value = val;
        sliderCS.value = val;
        sliderShadow.value = val;
        sliderDC.value = val;
    }

    public void SliderProf(float val)
    {
        altezza = parameters.ballSize; //era +1

        Vector3 p_p1From = new Vector3(0.0f, 0.0f, 274.5f); //2D
        Vector3 p_p1To = new Vector3(0.0f, -266.0f, 274.5f); //3D

        Vector3 s_p1From = new Vector3(550.0f, altezza, 1.0f); //2D
        Vector3 s_p1To = new Vector3(550.0f, 550.0f, 1.0f); //3D

        Vector3 p_p2From = new Vector3(-274.5f, 0.0f, 0.0f); //2D
        Vector3 p_p2To = new Vector3(-274.5f, -266.0f, 0.0f); //3D

        Vector3 s_p2From = new Vector3(550.0f, altezza, 1.0f); //2D
        Vector3 s_p2To = new Vector3(550.0f, 550.0f, 1.0f); //3D

        Vector3 p_p3From = new Vector3(0.0f, 0.0f, -274.5f); //2D
        Vector3 p_p3To = new Vector3(0.0f, -266.0f, -274.5f); //3D

        Vector3 s_p3From = new Vector3(550.0f, altezza, 1.0f); //2D
        Vector3 s_p3To = new Vector3(550.0f, 550.0f, 1.0f); //3D

        Vector3 p_p4From = new Vector3(274.5f, 0.0f, 0.0f); //2D
        Vector3 p_p4To = new Vector3(274.5f, -266.0f, 0.0f); //3D

        Vector3 s_p4From = new Vector3(550.0f, altezza, 1.0f); //2D
        Vector3 s_p4To = new Vector3(550.0f, 550.0f, 1.0f); //3D

        parete1.transform.localPosition = Vector3.Lerp(p_p1From, p_p1To, val);
        parete1.transform.localScale = Vector3.Lerp(s_p1From, s_p1To, val);

        parete2.transform.localPosition = Vector3.Lerp(p_p2From, p_p2To, val);
        parete2.transform.localScale = Vector3.Lerp(s_p2From, s_p2To, val);

        parete3.transform.localPosition = Vector3.Lerp(p_p3From, p_p3To, val);
        parete3.transform.localScale = Vector3.Lerp(s_p3From, s_p3To, val);

        parete4.transform.localPosition = Vector3.Lerp(p_p4From, p_p4To, val);
        parete4.transform.localScale = Vector3.Lerp(s_p4From, s_p4To, val);

        parameters.depthValue = sliderProf.value;
    }

    public void SliderProsp(float val)
    {
        float dist = (soffitto.transform.localScale.x/2) * (1/Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad / 2));

        Camera.main.transform.position = new Vector3 (0.0f, dist, 0.0f);
        Camera.main.farClipPlane = dist + pavimento.transform.localScale[2] + parete1.transform.localScale[0];
       
        if (val <= 0.3f)
        {
            Camera.main.orthographic = true;
        }

        else
        {
            Camera.main.orthographic = false;
            Camera.main.fieldOfView = val * 120;
        }

        parameters.perspectiveValue = sliderProsp.value;
    }

    public void SliderCS(float val)
    {
        Color emission;
        Color diffuse;
        Color specular;

        foreach (GameObject b in ball)
        {

            if (sliderCS.value < 0.5f)
            {
                emission = Color.Lerp(b.GetComponent<Renderer>().material.color, Color.black, val * 2);
                diffuse = Color.Lerp(Color.black, b.GetComponent<Renderer>().material.color, val * 2);
                specular = Color.Lerp(Color.black, Color.gray, val * 2);
                b.GetComponent<Renderer>().material.SetFloat("_Glossiness", 0.0f);
                b.GetComponent<Renderer>().material.SetColor("_SpecColor", specular);
            }

            else
            {
                emission = Color.black;
                diffuse = b.GetComponent<Renderer>().material.color;
                b.GetComponent<Renderer>().material.SetFloat("_Glossiness", Mathf.Lerp(0.0f, 0.55f, val * 2 - 1));
            }

            b.GetComponent<Renderer>().material.SetColor("_EmissionColor", emission);
            b.GetComponent<Renderer>().material.SetColor("_Color", diffuse);
        }

        parameters.chiaroscuroValue = sliderCS.value;
    }

    public void SliderShadow(float val)
    {
        lt2.shadowStrength = val / 2;

        parameters.shadowValue = sliderShadow.value;
    }

    public void SliderDC(float val)
    {
        if (sliderProf.value >= 0.3f)
        {
            RenderSettings.fog = true;
            RenderSettings.fogDensity = Mathf.Lerp(0.0000f, 0.0023f, val);
        }
        else if (sliderProf.value < 0.3f)
        {
            RenderSettings.fog = false;
        }

        parameters.depthcueingValue = sliderDC.value;
    }

    public void SliderColor(float val)
    {
        foreach (GameObject b in ball)
        {
            float H = b.GetComponent<MyBall>().colorHue;
            float V = b.GetComponent<MyBall>().colorValue;
            float S = val;

            if (val < 0.5f)
            {
                S = 0;
                V = Mathf.Lerp(1, V, val * 2);
            }
            else
            {
                S = val * 2 - 1;
            }

            Color col = Color.HSVToRGB(H, S, V);
            b.GetComponent<Renderer>().material.SetColor("_Color", col);
        }
        parameters.colorValue = sliderColor.value;
    }

    public void CheckPosition()
    {
        if (sliderProf.value == 0){ 
            foreach (GameObject b in ball)
            {
                if (b.transform.position[1] > parameters.ballSize / 2)
                    b.transform.position = new Vector3(b.transform.position[0], parameters.ballSize/2, b.transform.position[2]);
            }
        }
    }

    GameObject SpawnNewBall()
    {
        // TODO: velocity (and better position)
        return Instantiate(ballToClone, RandomEmptyPosition(), Quaternion.identity);
    }

    Vector3 RandomPosition()
    {
        float x = Random.Range(parete4.transform.position.x - parameters.ballSize / 2, parete2.transform.position.x + parameters.ballSize / 2);
        float y = Random.Range(soffitto.transform.position.y - parameters.ballSize / 2,pavimento.transform.position.y + parameters.ballSize / 2);
        float z = Random.Range(parete1.transform.position.z - parameters.ballSize / 2, parete3.transform.position.z + parameters.ballSize / 2);
        return new Vector3(x, y, z);
    }

    public Vector3 RandomEmptyPosition()
    {
        Vector3 newPos = new Vector3(0, Random.Range(soffitto.transform.position.y - parameters.ballSize / 2, pavimento.transform.position.y + parameters.ballSize / 2), 0);
        float maxDist = 0;
        float dist;

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = RandomPosition();

                foreach (GameObject b in ball)
                {
                   dist = Vector3.Distance(b.transform.position, randomPos);

                    if (dist > maxDist)
                    {
                        maxDist = dist;
                        newPos = randomPos;
                    }                                         
                }                         
        }

        foreach (GameObject b in ball)
        {
            if ((newPos - b.transform.position).magnitude < parameters.ballSize)
                newPos = RandomEmptyPosition();
        }

        return newPos;
    }
}
