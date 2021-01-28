using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// The meat and potatoes of my game
public class LevelGenerator : MonoBehaviour {

    public static LevelGenerator instance;
    // Minimap room outlines and colors
    public GameObject layoutRoom;
    public Color startColor, endColor, shopColor;
    // Where to put end room
    public int distanceToEnd;
    public int coinFlipValue;
    // Shop variables
    public bool includeShop;
    public int minToShop, maxToShop;

    // GameObject that decides where the rooms will go
    public Transform generatorPoint;
    // For direction selection
    public enum direction { up, right, down, left};
    public direction selectedDirection;
    public float xOffset = 18f, yOffset = 10;
    public LayerMask whatRoom;
    private GameObject endRoom;
    private GameObject shopRoom;
    // tracking which room number the shop is
    public int shopRoomNumber;
    // Lists
    [HideInInspector]
    public List<GameObject> layoutRoomObjects = new List<GameObject>();
    //[HideInInspector]
    public List<RoomPrefabs> roomPrefabs = new List<RoomPrefabs>();
    [HideInInspector]
    public List<GameObject> generatedOutlines = new List<GameObject>();
    // Available Centers
    public RoomCenter startCenter, endCenter, shopCenter;
    public List<RoomCenter> potentialCenters, potentialHallwaysLR, potentialHallwaysUD;
    private bool revealed = false;

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start() {
        revealed = false;
        int pos = 0;
        Vector3 temp = new Vector3(0, 0, 0);
        Vector3 origin = generatorPoint.position;
        // Create start room
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;
        // Pick random direction
        selectedDirection = (direction)Random.Range(0, 4);
        // move the genPoint
        moveGenerationPoint();
        for(int i = 0; i < distanceToEnd; i++) { // For as many rooms as you specify....
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation); // New Room
            layoutRoomObjects.Add(newRoom); // Add to list
            // This if statement tracks the furthest room from the start. At the end, that room becomes the end room
            if(Vector3.Distance(origin, newRoom.transform.position) >= Vector3.Distance(origin, temp)) {
                temp = newRoom.transform.position;
                pos = i;
            }
            if(i + 1 == distanceToEnd) { // Setting end room
                endRoom = layoutRoomObjects[pos];
                endRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(pos);
                layoutRoomObjects.Add(endRoom);
            }
            // Move in a random direction and repeat
            selectedDirection = (direction)Random.Range(0, 4);
            moveGenerationPoint();
            // If a room collision is detected
            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatRoom)) {  
                moveGenerationPoint();
            }
        }
        // Should the generator include the shop?
        if(includeShop) {
            int shopSelector = Random.Range(minToShop, maxToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
            layoutRoomObjects.RemoveAt(shopSelector);
            layoutRoomObjects.Insert(shopSelector, shopRoom);
            shopRoomNumber = shopSelector;
        }
        // Make the room outline
        createRoomOutline(Vector3.zero);
        foreach(GameObject room in layoutRoomObjects) {
            createRoomOutline(room.transform.position);
        }

        int f = 0; // variable to track room index for shop
        foreach (GameObject outline in generatedOutlines) {
            bool generateCenter = true;
            bool coinFlip = false; // Bool to decide if special specific room templetes are applied
            int random = Random.Range(0, 10);
            if(random < coinFlipValue) {
                coinFlip = true;
            }        
            if(outline.transform.position == Vector3.zero) { // Make start
                Instantiate(startCenter, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (outline.transform.position == endRoom.transform.position) { // Make end
                Instantiate(endCenter, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (outline.transform.position == shopRoom.transform.position && includeShop) { // Make Shop
                Instantiate(shopCenter, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (generateCenter && outline.name.Contains("Room LR(Clone)") && coinFlip == true && potentialHallwaysLR.Count != 0) { // Special room for LR
                int centerSelect = Random.Range(0, potentialHallwaysLR.Count);
                Instantiate(potentialHallwaysLR[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
                potentialHallwaysLR.RemoveAt(centerSelect); // Remove room from generation
            }
            if (generateCenter && outline.name.Contains("Room UD(Clone)") && coinFlip == true && potentialHallwaysUD.Count != 0) { // Special Room for UD
                int centerSelect = Random.Range(0, potentialHallwaysUD.Count);
                Instantiate(potentialHallwaysUD[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
                potentialHallwaysUD.RemoveAt(centerSelect); // Remove room from generation
            }
            if (generateCenter) { // Standard room
                int centerSelect = Random.Range(0, potentialCenters.Count);
                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                potentialCenters.RemoveAt(centerSelect);  // Remove room from generation
            }
            outline.GetComponent<Room>().index = f; // Set index
            f++;
            
        }
        
    }

    // Update is called once per frame
    void Update() {
        // FOR DEBUGGING
#if UNITY_EDITOR       
        if(Input.GetKey(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reset level
        }
        if (Input.GetKey(KeyCode.K)) { // Show end room
            generatedOutlines[generatedOutlines.Count -1].GetComponentInChildren<Room>().outline.SetActive(true);
            generatedOutlines[generatedOutlines.Count - 1].GetComponentInChildren<Room>().mapHider.SetActive(false);
        }
        if (Input.GetKey(KeyCode.L)) { // Hide end room
            generatedOutlines[generatedOutlines.Count - 1].GetComponentInChildren<Room>().outline.SetActive(false);
            generatedOutlines[generatedOutlines.Count - 1].GetComponentInChildren<Room>().mapHider.SetActive(true);
        }

#endif
        if (PlayerController.instance.edgegame == true && revealed == false ) { // Show end room for EdgeGame powerup
            generatedOutlines[instance.generatedOutlines.Count - 1].GetComponentInChildren<Room>().outline.SetActive(true);
            instance.generatedOutlines[instance.generatedOutlines.Count - 1].GetComponentInChildren<Room>().mapHider.SetActive(false);
            revealed = true;
        }
    }

    public void moveGenerationPoint() { // Switch case to select direction
        switch(selectedDirection) {
            case direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
            case direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
        }
    }

    private void createRoomOutline(Vector3 position) { // Choose outline for minimap
        bool exitUp = Physics2D.OverlapCircle(position + new Vector3(0f, yOffset), .2f, whatRoom);
        bool exitDown = Physics2D.OverlapCircle(position + new Vector3(0f, -yOffset), .2f, whatRoom);
        bool exitRight = Physics2D.OverlapCircle(position + new Vector3(xOffset, 0f), .2f, whatRoom);
        bool exitLeft = Physics2D.OverlapCircle(position + new Vector3(-xOffset, 0f), .2f, whatRoom);

        var roomPrefab = PickRoom(exitUp, exitRight, exitDown, exitLeft);
        if (roomPrefab != null) {
            // add outline to list
            generatedOutlines.Add(Instantiate(roomPrefab.prefab, position, Quaternion.identity, transform));
        }
    }

    // Selects a RoomPrefab in the Room list
    private RoomPrefabs PickRoom(bool exitUp, bool exitRight, bool exitDown, bool exitLeft) {
        foreach (RoomPrefabs rp in roomPrefabs) {
            if (rp.exitUp == exitUp && rp.exitRight == exitRight && rp.exitDown == exitDown && rp.exitLeft == exitLeft) {
                return rp;
            }
        }
        return null;
    }

    
}
// Class for easy room selection
[System.Serializable]
public class RoomPrefabs {
    public bool exitUp;
    public bool exitDown;
    public bool exitLeft;
    public bool exitRight;
    public GameObject prefab;
}
