using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Random = System.Random;

namespace Assets
{
    public partial class Instantiation : MonoBehaviour
    {
        // Use this for initialization
        // C#

        public Text DisplayName;
        public GameObject Tile0;
        public GameObject Tile1;
        public GameObject Tile2;
        public GameObject Tile3;
        public GameObject Tile4;
        public GameObject Tile5;
        public GameObject Tile6;
        public GameObject CTile1;
        public GameObject CTile2;
        public GameObject CTile3;
        public GameObject CTile4;
        public GameObject CTile5;
        public GameObject CTile6;
        public GameObject TTile1;
        public GameObject TTile2;
        public GameObject TTile3;
        public GameObject TTile4;
        public GameObject TTile5;
        public GameObject TTile6;

        private static Tile _emptyTile = new Tile(
            0,
            0,
            0,
            "empty",
            "empty"
        );
        private static Tile _tile1Descriptor = new Tile(
            1,
            10,
            10,
            "forest",
            "small_stone_with_shrubs"
        );
        private static Tile _tile2Descriptor = new Tile(
            2,
            10,
            15,
            "forest",
            "two_trees"
        );
        private static Tile _tile3Descriptor = new Tile(
            3,
            15,
            15,
            "forest",
            "tree_with_shrooms"
        );
        private static Tile _tile4Descriptor = new Tile(
            4,
            20,
            25,
            "forest",
            "three_trees"
        );
        private static Tile _tile5Descriptor = new Tile(
            5,
            25,
            40,
            "forest",
            "large_rock_with_trees"
        );
        private static Tile _tile6Descriptor = new Tile(
            6,
            20,
            30,
            "forest",
            "trees_and_stump"
        );
        private static Tile _ctile1Descriptor = new Tile(
            7,
            50,
            75,
            "camp",
            "bench_and_fences"
        );
        private static Tile _ctile2Descriptor = new Tile(
            8,
            60,
            80,
            "camp",
            "simple_fire"
        );
        private static Tile _ctile3Descriptor = new Tile(
            9,
            80,
            100,
            "camp",
            "complex_fire"
        );
        private static Tile _ctile4Descriptor = new Tile(
            10,
            100,
            120,
            "camp",
            "simple_tent"
        );
        private static Tile _ctile5Descriptor = new Tile(
            11,
            120,
            140,
            "camp",
            "complex_tent"
        );
        private static Tile _ctile6Descriptor = new Tile(
            12,
            150,
            200,
            "camp",
            "big_tent"
        );
        private static Tile _ttile1Descriptor = new Tile(
            13,
            80,
            100,
            "town",
            "red_simple_house"
        );
        private static Tile _ttile2Descriptor = new Tile(
            14,
            90,
            110,
            "town",
            "red_house_with_fence"
        );
        private static Tile _ttile3Descriptor = new Tile(
            15,
            90,
            110,
            "town",
            "green_house_with_fence"
        );
        private static Tile _ttile4Descriptor = new Tile(
            16,
            80,
            100,
            "town",
            "green_simple_house"
        );
        private static Tile _ttile5Descriptor = new Tile(
            17,
            150,
            200,
            "town",
            "large_green_house"
        );
        private static Tile _ttile6Descriptor = new Tile(
            18,
            150,
            200,
            "town",
            "large_red_house"
        );


        private static List<Tile> _tiles = new List<Tile>(new Tile[] { _tile1Descriptor, _tile2Descriptor, _tile3Descriptor, _tile4Descriptor, _tile5Descriptor, _tile6Descriptor, _ctile1Descriptor, _ctile2Descriptor, _ctile3Descriptor, _ctile4Descriptor, _ctile5Descriptor, _ctile6Descriptor, _ttile1Descriptor, _ttile2Descriptor, _ttile3Descriptor, _ttile4Descriptor, _ttile5Descriptor, _ttile6Descriptor });
        private static List<Tile> _ftiles = new List<Tile>(new Tile[] { _tile1Descriptor, _tile2Descriptor, _tile3Descriptor, _tile4Descriptor, _tile5Descriptor, _tile6Descriptor });
        private static List<Tile> _ctiles = new List<Tile>(new Tile[] { _ctile1Descriptor, _ctile2Descriptor, _ctile3Descriptor, _ctile4Descriptor, _ctile5Descriptor, _ctile6Descriptor });
        private static List<Tile> _ttiles = new List<Tile>(new Tile[] { _ttile1Descriptor, _ttile2Descriptor, _ttile3Descriptor, _ttile4Descriptor, _ttile5Descriptor, _ttile6Descriptor });

        readonly Random _rand = new Random();
        private string _userID;
        private string _authToken;
        private UserData _userData;
        private UserData _viewUserData;
        private const int GRID_SIZE_X = 7;
        private const int GRID_SIZE_Z = 7;
        private const int TILE_SIZE = 6;
        private const int STARTER_TILE_COSTS = 10;

        private const int GOLD_INCREMENT = 100;

        private static GameObject[,] _tileObjects = new GameObject[GRID_SIZE_X, GRID_SIZE_Z];

        void Start()
        {
            //this._userID = "48ec8399-fef1-41ab-a8db-ff7d51136d3e";
            //this._authToken =
            //    "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ0ZXN0NUB0ZXN0LmNvbSIsImVtYWlsIjoidGVzdDVAdGVzdC5jb20iLCJpc3MiOiJjb2RlZ2xleWQiLCJhdWQiOiJDb2RlZ2xleWRBUEkiLCJuYmYiOjE1MDM5OTAxMzguMCwiaWF0IjoxNTAzOTkwMTM4LjAsImV4cCI6MTUwNDU5NDkzOC4wfQ.6rYLGVC4jwbB2J4wNHVzzp8PvKiqyueDKxHZ1a-z6vo";
            //StoreUserID(_userID + "|" + _authToken);

            Application.ExternalCall("my.dashboard.UnityInitDone");
        }

        public void AddGold(int gold)
        {
            _userData.gold += GOLD_INCREMENT;
            StartCoroutine("PutGold");
        }

        private GameObject GetTileByCode(int code)
        {
            switch (code)
            {
                case 0:
                    return Tile0;
                case 1:
                    return Tile1;
                case 2:
                    return Tile2;
                case 3:
                    return Tile3;
                case 4:
                    return Tile4;
                case 5:
                    return Tile5;
                case 6:
                    return Tile6;
                case 7:
                    return CTile1;
                case 8:
                    return CTile2;
                case 9:
                    return CTile3;
                case 10:
                    return CTile4;
                case 11:
                    return CTile5;
                case 12:
                    return CTile6;
                case 13:
                    return TTile1;
                case 14:
                    return TTile2;
                case 15:
                    return TTile3;
                case 16:
                    return TTile4;
                case 17:
                    return TTile5;
                case 18:
                    return TTile6;
                default:
                    return Tile0;
            }
        }

        SimulationValue _simValue;
        Tile _tile;

        public void UpdateSim(int num)
        {
            int spendable = _userData.gold - _userData.goldSpent;
            if (spendable > 10) {
                _simValue = ChooseSimValue();
                _userData.goldSpent -= _simValue.tile.cost; // 'Sell' previous tile
                _tile = _simValue.tile;
                _simValue.tile = UpgradeTile(_simValue.tile, spendable);
                _userData.goldSpent += _simValue.tile.cost; // 'Buy' new tile
                
                if(_tile.code == _simValue.tile.code) {
                    if (num >= 4) {
                        return;
                    }

                    UpdateSim(num+1);
                    return;
                }

                StartCoroutine("PutUserData");
                StartCoroutine("PutSimValue");

                Destroy(_tileObjects[_simValue.xPos / TILE_SIZE, _simValue.yPos / TILE_SIZE]);
                InstatiateSimValue(_simValue);
            }
        }

        private SimulationValue ChooseSimValue()
        {
            int randX = _rand.Next(0, GRID_SIZE_X);
            int randZ = _rand.Next(0, GRID_SIZE_Z);
            int posX = randX * TILE_SIZE;
            int posZ = randZ * TILE_SIZE;
            SimulationValue simValue = _userData.simValues.Find(s => s.xPos == posX && s.yPos == posZ);
            if (simValue == null)
            {
                return ChooseSimValue();
            }
            return simValue;
        }

        private Tile UpgradeTile(Tile tile, int gold)
        {
            List<Tile> upgrades;
            List<Tile> possibleUpgrades = new List<Tile>(_ftiles);

            string tilesets = _userData.serializeStorage.Split(new string[] { "q:" }, StringSplitOptions.None)[1];
            if(tilesets.Contains("camp"))
            {
                possibleUpgrades.AddRange(_ctiles);
            }
            if (tilesets.Contains("town"))
            {
                possibleUpgrades.AddRange(_ttiles);
            }

            if (_rand.Next(1, 5) < 4 && tile.type != "empty")
            {
                upgrades = possibleUpgrades.FindAll(t => t.value > tile.value && t.cost <= gold && t.type.Equals(tile.type));
            }
            else
            {
                upgrades = possibleUpgrades.FindAll(t => t.value > tile.value && t.cost <= gold);
            }

            return upgrades.Count > 0 ? upgrades[_rand.Next(0, upgrades.Count-1)] : tile;
        }

        private float GetRandomAngle()
        {
            int angle = _rand.Next(0, 4);
            return angle * 90f;
        }

        public void StoreUserID(string userIDAuthToken)
        {
            string[] split = userIDAuthToken.Split('|');
       
            this._userID = split[0];
            this._authToken = split[1];

            StartCoroutine("GetUserData");
        }

        public void OnNext()
        {
            _viewUserData.id++;
            StartCoroutine("GetUserDataById");
        }

        public void OnPrev()
        {
            _viewUserData.id--;
            StartCoroutine("GetUserDataById");
        }

        public void OnHome()
        {
            _viewUserData.id = _userData.id;
            DisplayName.text = _userData.displayName;
            DisplaySimModel(_userData);
        }

        IEnumerator GetUserData()
        {
            UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/api/userdata/sim/" + _userID);
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();

            Debug.Log(www.downloadHandler.text);
            _userData = JsonUtility.FromJson<UserData>(www.downloadHandler.text);

            CreateSimModel();
            _viewUserData = _userData;
            DisplayName.text = _viewUserData.displayName;
        }

        IEnumerator GetUserDataById()
        {
            UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/api/userdata/sim/id/" + _viewUserData.id);
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();

            Debug.Log(www.downloadHandler.text);
            _viewUserData = JsonUtility.FromJson<UserData>(www.downloadHandler.text);
            DisplayName.text = _viewUserData.displayName;
            DisplaySimModel(_viewUserData);
        }

        IEnumerator PutUserData()
        {
            UnityWebRequest www = UnityWebRequest.Put("http://localhost:5000/api/userdata/" + _userID, JsonUtility.ToJson(_userData));
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();
        }

        IEnumerator InitSimData()
        {
            UnityWebRequest www = UnityWebRequest.Put("http://localhost:5000/api/userdata/initsim/" + _userID, JsonUtility.ToJson(_userData));
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();
        }

        IEnumerator PutSimValue()
        {
            UnityWebRequest www = UnityWebRequest.Put("http://localhost:5000/api/userdata/sim", JsonUtility.ToJson(_simValue));
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();
        }

        IEnumerator PutGold()
        {
            UnityWebRequest www = UnityWebRequest.Put("http://localhost:5000/api/userdata/gold/" + _userID + "?gold=" + GOLD_INCREMENT, JsonUtility.ToJson(_userData));
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();
        }

        private void CreateSimModel()
        {
            if (_userData.simValues == null || _userData.simValues.Count == 0)
            {
                if (_userData.simValues == null)
                {
                    _userData.simValues = new List<SimulationValue>();
                }

                for (int x = 0; x < GRID_SIZE_X * TILE_SIZE; x = x + TILE_SIZE)
                {
                    for (int z = 0; z < GRID_SIZE_Z * TILE_SIZE; z = z + TILE_SIZE)
                    {
                        Tile tile = _emptyTile;
                        float angleToRotate = GetRandomAngle();
                        
                        _simValue = new SimulationValue(x, z, angleToRotate.ToString(), tile);
                        _userData.simValues.Add(_simValue);

                        InstatiateSimValue(_simValue);
                    }
                }
                Debug.Log(JsonUtility.ToJson(_userData));
                StartCoroutine("InitSimData");
            }
            else
            {
                foreach(SimulationValue simValue in _userData.simValues)
                {
                    InstatiateSimValue(simValue);
                }
            }
        }

        private void DisplaySimModel(UserData data)
        {
            foreach(GameObject obj in _tileObjects)
            {
                Destroy(obj);
            }
            foreach(SimulationValue simValue in data.simValues)
            {
                InstatiateSimValue(simValue);
            }
        }

        void InstatiateSimValue(SimulationValue simValue)
        {
            GameObject obj = GetTileByCode(simValue.tile.code);
            float angleToRotate = float.Parse(simValue.rotation);
            Quaternion q = Quaternion.AngleAxis(angleToRotate, Vector3.up);

            // Transform to account for movement
            int newX = angleToRotate == 270 || angleToRotate == 180 ? simValue.xPos - TILE_SIZE : simValue.xPos;
            int newZ = angleToRotate == 90f || angleToRotate == 180f ? simValue.yPos - TILE_SIZE : simValue.yPos;

            // Create obj
            GameObject tileObj = Instantiate(obj, new Vector3(newX, 0, newZ), q);
            _tileObjects[simValue.xPos / TILE_SIZE, simValue.yPos / TILE_SIZE] = tileObj;
        }

        // Update is called once per frame
        void Update()
        {
        }
    }


    // Models
    [Serializable]
    public class UserData
    {
        public int id;
        public string userId;
        public string displayName;
        public int gold;
        public int goldSpent;
        public string serializeStorage;
        public List<SimulationValue> simValues;

        public UserData() { }

        public UserData(string userId)
        {
            this.gold = 1;
            this.goldSpent = 0;
            this.serializeStorage = "";
            this.simValues = new List<SimulationValue>();
            this.userId = userId;
        }
    }

    [Serializable]
    public class SimulationValue
    {
        public int id;
        public int xPos;
        public int yPos;
        public string rotation;
        public Tile tile;

        public int userDataID;

        public SimulationValue(int x, int y, string r, Tile tile)
        {
            xPos = x;
            yPos = y;
            rotation = r;
            this.tile = tile;
        }
    }

    [Serializable]
    public class Tile
    {
        public int id;
        public int code;
        public int cost;
        public int value;
        public string type;
        public string label;

        public Tile(int code, int cost, int value, string type, string label)
        {
            this.code = code;
            this.cost = cost;
            this.value = value;
            this.type = type;
            this.label = label;
        }
    }
}
