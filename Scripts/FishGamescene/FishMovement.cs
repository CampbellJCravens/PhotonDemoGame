using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FishMovement : MonoBehaviour
{

    // Photon view component
    PhotonView photonView;

    // This script handles the fish movement

    public string input = "";
    public int fishNumber = 0;
    public GameObject fish;
    public bool debugMode = false;
    public float speed;
    public float minSpeed;
    public float maxSpeed;
    public float targetSpeed;
    [HideInInspector] public float baseSpeed;


    public float surfaceSpeed;
    public float sinkSpeed;

    const float sceneMinX = -7;
    const float sceneMaxX = 7;
    const float sceneMinZ = -3;
    const float sceneMaxZ = 3;

    public int targetCounter = 0;
    Vector3 targetPosition;
    List<Vector3> targetPositions = new List<Vector3>();
    Vector3 lastTargetPosition;
    GameObject targetObject;
    GameObject lastTargetObject;
    bool targetsReached = false;

    Animator anim;

    ParticleSystem fishTrail;

    Rigidbody rigidbody;
    
    public float maxParticles = 40;
    public float minParticles = 15;

    [HideInInspector] public enum FishState { swimming, caught, surfacing, hidden, idle };
    [HideInInspector] public FishState fishState = FishState.swimming;


    
    int fishCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        photonView = transform.parent.gameObject.GetComponent<PhotonView>();
        print ("Fish: on start");
        initializeFish();
        targetPosition = transform.position;
        lastTargetPosition = transform.position;
        if (PhotonNetwork.IsMasterClient) {
            fishState = FishState.idle;
            Destroy(transform.parent.Find("FishBody").gameObject);
        } else {
            startFish();
        }
    }

    void initializeFish() {
        if (!photonView.IsMine && !PhotonNetwork.IsMasterClient && DelayStartLobbyController.photonActive) {
            transform.parent.gameObject.SetActive(false);
        }
        baseSpeed = speed;
        targetSpeed = speed;
        createTarget();
        anim = fish.GetComponent<Animator>();
        fishTrail = transform.Find("FishTrail").GetComponent<ParticleSystem>();
        rigidbody = GetComponent<Rigidbody>();
        determineFishNumber();
        List<string> inputs = new List<string>() {
            "Punch", "Jab", "Block", "Jump", "DownSmash"
        };
        transform.parent.name = "Fish" + fishNumber;
        foreach (string i in inputs) {
            if (photonView.IsMine) {
                if (!Gamescript.activeFishTypes.Contains(i)) {
                    Gamescript.activeFishTypes.Add(i);
                    input = i;
                    setColor();
                    return;
                }
            }
        }
    }

    void determineFishNumber() {
        int randInt = Random.Range(1000, 10000);
        foreach (GameObject f in GameObject.FindGameObjectsWithTag("Fish")) {
            FishMovement script = f.GetComponent<FishMovement>();
            if (randInt == script.fishNumber) {
                determineFishNumber();
                return;
            }
        }
        fishNumber = randInt;
    }

    void startFish() {
        findNextTargetPosition();
        anim.Play("Swim");
    }

    // Update is called once per frame
    void Update()
    { 
        if (fishState == FishState.swimming) {
            matchTargetSpeed();
            swim();
            randomSlowTurn();
        } else if (fishState == FishState.caught) {
            sink();
        } else if (fishState == FishState.surfacing) {
            surface();
        } else if (fishState == FishState.hidden) {
            hideFish();
        }
        
        if (debugMode) { 
            targetObject.transform.position = targetPosition; 
            lastTargetObject.transform.position = lastTargetPosition;
        }
    }

    void randomSlowTurn() {
        float bounds = 0.1f;
        if (transform.position.x > -bounds && transform.position.x < bounds) {
            
        }
    }
    

    Vector3 findRandomPosition(float minX, float maxX) {
        Vector3 randPosition = new Vector3(Random.Range(minX, maxX), 0, Random.Range(sceneMinZ, sceneMaxZ));

        return randPosition;
    }

    void matchTargetSpeed() {
        float speedInc = 1;
        if (speed != targetSpeed) {
            if (speed < targetSpeed) {
                speed += speedInc;
            } else {
                speed -= speedInc;
            }
        }
        float playbackSpeed = speed / (maxSpeed * 0.75f);
        anim.speed = playbackSpeed;

        var main = fishTrail.main;
        var emission = fishTrail.emission;
        float maxParticleRatio = maxSpeed / maxParticles;
        float particles = speed / maxParticleRatio;

        if (speed < maxSpeed * 0.7f) {
            emission.enabled = false;
        } else if (targetCounter > 0) {
            emission.rateOverTime = particles / targetCounter;
        } else {
            emission.enabled = true;
            main.startSpeed = speed * 0.75f;
            emission.rateOverTime = particles;
        }


    }
    void randomizeSpeed() {
        targetSpeed = Random.Range(minSpeed, maxSpeed + 1);
    }

    public void setColor() {
        List<float> rgb = new List<float>();
        if (input == "Punch") { return; }
        if (input == "Jab") {
            rgb = new List<float>() {
                255, 217, 124
            };
        } else if (input == "Block") {
            rgb = new List<float>() {
                156, 255, 124
            };
        } else if (input == "Jump") {
            rgb = new List<float>() {
                132, 124, 255
            };
        } else if (input == "DownSmash") {
            rgb = new List<float>() {
                124, 196, 255
            };
        }
        Color fishColor = new Color(rgb[0]/255f, rgb[1]/255f, rgb[2]/255f);
        fish.transform.Find("Fish").GetComponent<Renderer>().material.color = fishColor;
    }
 
    public void randomizeColor() {
        float randNumber = Random.Range(1,6);
        float randValue = Random.Range(124, 255);
        Color randColor = new Color();
        if (randNumber == 1) {
            randColor = new Color(randValue/255f, 255f/255f, 124f/255f);
        } else if (randNumber == 2) {
            randColor = new Color(randValue/255f, 124f/255f, 255f/255f);
        } else if (randNumber == 3) {
            randColor = new Color(124f/255f, randValue/255f, 255f/255f);
        } else if (randNumber == 4) {
            randColor = new Color(255f/255f, randValue/255f, 124/255f);
        } else if (randNumber == 5) {
            randColor = new Color(255f/255f, 124f/255f, randValue/255f);
        } else if (randNumber == 6) {
            randColor = new Color(124f/255f, 255f/255f, randValue/255f);
        }
        // print ("randNumber = " + randNumber);
        // print ("randValue = " + randValue);
        // print ("rgbValue = (" + randColor.r * 255 + ", " + randColor.g * 255 + ", " + randColor.b * 255 + ")");
        fish.transform.Find("Fish").GetComponent<Renderer>().material.color = randColor;
    }

    public void findNextTargetPosition() {
        if (fishState == FishState.caught) { return; }
        removeAllMoveCircles();
        targetPositions = new List<Vector3>();
        lastTargetPosition = targetPosition;
        randomizeSpeed();
        findNewTarget();
        createMoveCircle();
    }

    void findNewTarget() {
        if (transform.position.x > 0) {
            targetPosition = findRandomPosition(sceneMinX, -3);
        } else {
            targetPosition = findRandomPosition(3, sceneMaxX);
        }
        //targetPosition = findRandomPosition(-sceneMaxX, sceneMaxX);
    }

    void surface() {
        Vector3 target = new Vector3(transform.position.x, 0f, transform.position.z);
        if (transform.position != target) {
            transform.position = Vector3.MoveTowards(transform.position, target, surfaceSpeed * Time.deltaTime);
        } else {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; 
            targetSpeed = maxSpeed * 0.9f;
            fishState = FishState.swimming;
        }
    }

    void hideFish() {
        Vector3 target = new Vector3(transform.position.x, -4.5f, transform.position.z);
        if (transform.position != target) {
            transform.position = Vector3.MoveTowards(transform.position, target, surfaceSpeed * 0.5f * Time.deltaTime);
        } else {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; 
            targetSpeed = maxSpeed * 0.9f;
        }
    }

    void sink() {
        Vector3 target = new Vector3(transform.position.x, -100f, transform.position.x);
        transform.position = Vector3.MoveTowards(transform.position, target, sinkSpeed * Time.deltaTime);
    }

    void swim() {
        if (targetPositions.Count > 0) {
            float step =  speed * Time.deltaTime;
            if (!targetsReached) {
                transform.position = Vector3.MoveTowards(transform.position, targetPositions[targetCounter], step);
            } else {
                findNextTargetPosition();
                targetsReached = false;
            }
        }
    }

    void createTarget() {
        if (debugMode) {
            targetObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            targetObject.name = "target";
            float targetSize = 0f;
            if (debugMode) { targetSize = 0.5f; }
            targetObject.transform.localScale = new Vector3(targetSize, targetSize, targetSize);
            Destroy(targetObject.AddComponent<BoxCollider>());
            // targetObject.GetComponent<Renderer>().material.color = Color.green;
            if (name == "FishControl") {
                targetObject.GetComponent<Renderer>().material.color = Color.yellow;
            } else {
                targetObject.GetComponent<Renderer>().material.color = Color.green;
            }

            lastTargetObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lastTargetObject.name = "lastTargetObject";
            lastTargetObject.transform.localScale = new Vector3(targetSize, targetSize, targetSize);
            Destroy(lastTargetObject.GetComponent<BoxCollider>());
            //lastTargetObject.GetComponent<Renderer>().material.color = Color.red;
            if (name == "FishControl") {
                lastTargetObject.GetComponent<Renderer>().material.color = Color.red;
            } else {
                lastTargetObject.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }

    void createMoveCircle() {
        float quadrant = 0;
        float direction = 1;
        /* if this turn circle is being created by a normal turn, determine the quadrant
        and direction based off lastTargetPosition and targetPosition  */
        // print ("targetPosition = (" + targetPosition.x + ", " + targetPosition.z + ")");
        // print ("lastTargetPos = (" + lastTargetPosition.x + ", " + lastTargetPosition.z + ")");
        if (lastTargetPosition.x < targetPosition.x) {
            if (lastTargetPosition.z > targetPosition.z) {
                quadrant = 90;
                direction = -1;
                //print ("lastTarget (q2): quadrant = " + quadrant + " direction = " + direction);
            } else {
                quadrant = 270;
                direction = 1;
                //print ("lastTarget (q3): quadrant = " + quadrant + " direction = " + direction);
            }
        } else {
            if (lastTargetPosition.x > targetPosition.x) {
                if (lastTargetPosition.z > targetPosition.z) {
                    quadrant = 90;
                    direction = 1;
                    //print ("lastTarget (q1): quadrant = " + quadrant + " direction = " + direction);
                } else {
                    quadrant = 270;
                    direction = -1;
                    //print ("lastTarget (q4): quadrant = " + quadrant + " direction = " + direction);
                }
            }
        }
        
        int circumference = 200;
        int inc = circumference / 10;
        float randRadius = Random.Range(2, findMaxRadius());
        int cubeCount = -1;
        for (int d = 0; d < circumference; d+=inc) {
            cubeCount += 1;
            float radius = randRadius;
            float deg = ((float)d * direction) + quadrant;
            float radian = deg * Mathf.Deg2Rad;
            float dx = Mathf.Cos(radian) * radius;
            float dz = Mathf.Sin(radian) * radius;

            float x = targetPosition.x + dx;
            float z = targetPosition.z + dz;

            createCirclePoint(x, z, cubeCount);
        }
    }
    


    void createCirclePoint(float xPos, float zPos, int cubeCount) {
        targetPositions.Add(new Vector3(xPos, 0, zPos));

        
        float scale = 0.2f;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<BoxCollider>().isTrigger = true;
        cube.name = "moveCircle" + fishNumber + cubeCount;
        cube.tag = "moveCircle";
        cube.transform.position = new Vector3(xPos, 0, zPos);
        cube.transform.localScale = new Vector3(scale, scale, scale);
        cube.transform.SetParent(GameObject.Find("moveCircles").transform);
        if (cubeCount == 0) {
            cube.GetComponent<Renderer>().material.color = Color.black;
        }
        if (!debugMode) {
            cube.GetComponent<MeshRenderer>().enabled = false;
        }
        
    }

    float findMaxRadius() {
        float maxX = 8.63f;
        float maxZ = 4.72f;
        float distanceX = 0;
        float distanceZ = 0;

        distanceX = maxX - Mathf.Abs(targetPosition.x);
        distanceZ = maxZ - Mathf.Abs(targetPosition.z);

        if (distanceX < distanceZ) {
            return distanceX;
        } else {
            return distanceZ;
        }
    }

    void removeAllMoveCircles() {
        foreach (GameObject moveCircle in GameObject.FindGameObjectsWithTag("moveCircle")) {
            if (moveCircle.name.Contains("moveCircle" + fishNumber)) {
                Destroy(moveCircle);
            }
        }
    }
    
    
    List<Collider> triggerList = new List<Collider>();
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Fish") {
            triggerList.Add(other);
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.name == "moveCircle" + fishNumber + targetCounter) {
            if (targetCounter >= targetPositions.Count - 1) {
                targetsReached = true;
                targetCounter = 0;
            } else {
                targetCounter += 1;
            }
            Destroy(other.gameObject);
        }
        if (other.tag == "Fish") {
            if (transform.position.y < other.transform.position.y) {
                submergeToAvoidFish(triggerList.Count);
            } else if (transform.position.y == other.transform.position.y) {
                FishMovement otherFishScript = other.GetComponent<FishMovement>();
                if (speed < otherFishScript.speed) {
                    submergeToAvoidFish(triggerList.Count);
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Fish") {
            surfaceAfterSubmerge(other.gameObject);
            triggerList.Remove(other);
        }
    }

    void submergeToAvoidFish(int triggerCount) {
        if (submerging) { return; }
        submerging = true;
        float rand = Random.Range(0.2f, 0.5f);
        float submergeY = -0.3f - (triggerCount * rand);
        for (int i = 0; i < targetPositions.Count; i++) {
            targetPositions[i] = new Vector3(targetPositions[i].x, submergeY, targetPositions[i].z);
        }
        repositionMoveCircles();
    }

    bool submerging = false;

    void surfaceAfterSubmerge(GameObject fishToSubmerge) {
        float surfaceY = 0f;
        for (int i = 0; i < targetPositions.Count; i++) {
            targetPositions[i] = new Vector3(targetPositions[i].x, surfaceY, targetPositions[i].z);
        }
        repositionMoveCircles();
        submerging = false;
    } 

    void repositionMoveCircles() {
        foreach (GameObject moveCircle in GameObject.FindGameObjectsWithTag("moveCircle")) {
            if (moveCircle.name.Contains("moveCircle" + fishNumber)) {
                for (int i = 0; i < targetPositions.Count; i++) {
                    if (moveCircle.name == "moveCircle" + fishNumber + i) {
                        moveCircle.transform.position = targetPositions[i];
                    }
                }
            }
        }
    }

}
