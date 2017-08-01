using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;
using SimpleJson;

namespace Assets
{
    public partial class Instantiation : MonoBehaviour
    {
        // Use this for initialization
        // C#

        public GameObject Tile1;
        public GameObject Tile2;
        public GameObject Tile3;
        public GameObject Tile4;
        public GameObject Tile5;
        public GameObject Tile6;

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

        private static List<Tile> _tiles = new List<Tile>(new Tile[] { _tile1Descriptor, _tile2Descriptor, _tile3Descriptor, _tile4Descriptor, _tile5Descriptor, _tile6Descriptor });

        readonly Random _rand = new Random();

        private string _userID;
        private string _authToken;
        private UserData _userData;
        private const int GRID_SIZE_X = 20;
        private const int GRID_SIZE_Z = 20;

        void Start()
        {
//            this._userID = "8483cccc-4bc7-425c-8e80-302aa59a34ba";
//            this._authToken =
//                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwiZW1haWwiOiJ0ZXN0QHRlc3QuY29tIiwiaXNzIjoiY29kZWdsZXlkIiwiYXVkIjoiQ29kZWdsZXlkQVBJIiwibmJmIjoxNTAxMDM5NDY3LjAsImlhdCI6MTUwMTAzOTQ2Ny4wLCJleHAiOjE1MDE2NDQyNjcuMH0.ylv4AVNzjepj1cSVkAJQce5RoMwCxLGAU63C_UFEbkI";
//            Debug.Log("store user id");
//            StoreUserID(_userID + "|" + _authToken);

        }

        private GameObject GetTileByCode(int code)
        {
            switch (code)
            {
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
                default:
                    return Tile1;
            }
        }

        public void UpdateSim()
        {
            int spendable = _userData.gold - _userData.goldSpent;
            while (spendable > 15)
            {
                SimulationValue simValue = ChooseSimValue();
                _userData.goldSpent -= simValue.tile.cost; // 'Sell' previous tile
                simValue.tile = UpgradeTile(simValue.tile, spendable);
                _userData.goldSpent += simValue.tile.cost; // 'Buy' new tile
                
                // TODO replace previous tile in webapp

                spendable = _userData.gold - _userData.goldSpent;
            }
        }

        private SimulationValue ChooseSimValue()
        {
            int randX = GenerateNormalRand(GRID_SIZE_X, 0, GRID_SIZE_X/5);
            int randY = GenerateNormalRand(GRID_SIZE_Z, 0, GRID_SIZE_Z / 5);
            return _userData.simValues.Find(s => s.xPos == randX && s.yPos == randY); // Needs to update to handle multi-size tiles
        }

        /// <summary>
        /// Rolls a number from 0 to max following a normal distribution with specified sigma.  Median is max / 2 + diff.
        /// </summary>
        /// <remarks>
        /// Appropriate sigma:
        /// max / 20 for steep median, few outlying rolls.
        /// max / 10 for frequent central rolls, some outlying rolls.
        /// max / 5 for good number of all rolls.  Central rolls still more common.
        /// max / 4 maximum recommended, max / 20 minimum.
        /// </remarks>
        /// <returns> A number following a normal distribution. </returns>
        private int GenerateNormalRand(int max, double medianDiff, double sigma)
        {
            double median = (max / 2) + medianDiff;

            double cauchyRand = median + sigma * Math.Tan(Math.PI * (_rand.NextDouble() - 0.5));
            cauchyRand = cauchyRand > max+1 ? max + 1 : cauchyRand < -1 ? -1 : cauchyRand;
            int rand = (int)Math.Round(cauchyRand);

            return (rand <= max && rand >= 0) ? rand : GenerateNormalRand(max, median, sigma);
        }

        private Tile UpgradeTile(Tile tile, int gold)
        {
            List<Tile> _upgrades;
            if (_rand.Next(1, 5) < 4 && tile.type != "empty")
            {
                _upgrades = _tiles.FindAll(t => t.value > tile.value && t.cost <= gold && t.type.Equals(tile.type));
            }
            else
            {
                _upgrades = _tiles.FindAll(t => t.value > tile.value && t.cost <= gold);
            }

            return _upgrades.Count > 0 ? _upgrades[_rand.Next(0, _upgrades.Count-1)] : tile;
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

        IEnumerator GetUserData()
        {
            UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/api/userdata/sim/" + _userID);
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();

            _userData = JsonUtility.FromJson<UserData>(www.downloadHandler.text);
            CreateSimModel();
        }

        IEnumerator PutUserData()
        {
            UnityWebRequest www = UnityWebRequest.Put("http://localhost:5000/api/userdata/" + _userID, JsonUtility.ToJson(_userData));
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

                for (int x = 0; x < GRID_SIZE_X; x = x + 3)
                {
                    for (int z = 0; z < GRID_SIZE_Z; z = z + 3)
                    {
                        Tile tile = UpgradeTile(_emptyTile, 50);
                        GameObject obj = GetTileByCode(tile.code);
                        float angleToRotate = GetRandomAngle();
                        Quaternion q = Quaternion.AngleAxis(angleToRotate, Vector3.up);

                        // Transform to account for movement
                        int newX = angleToRotate == 270 || angleToRotate == 180 ? x - 3 : x;
                        int newZ = angleToRotate == 90f || angleToRotate == 180f ? z - 3 : z;

                        // Create obj
                        Instantiate(obj, new Vector3(newX, 0, newZ), q);

                        SimulationValue simValue = new SimulationValue(newX, newZ, angleToRotate.ToString(), tile);
                        _userData.simValues.Add(simValue);
                    }
                }
                StartCoroutine("PutUserData");
            }
            else
            {
                foreach(SimulationValue simValue in _userData.simValues)
                {
                    GameObject obj = GetTileByCode(simValue.tile.code);
                    Quaternion q = Quaternion.AngleAxis(float.Parse(simValue.rotation), Vector3.up);
                    
                    // Create obj
                    Instantiate(obj, new Vector3(simValue.xPos, 0, simValue.yPos), q);
                }
            }
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
