using Orkanoid.Core.Levels;
using Orkanoid.Game;
using SunsetSystems.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Orkanoid.Core.Saves
{
    [RequireComponent(typeof(Tagger))]
    public class SaveLoadManager : MonoBehaviour
    {
        private static string SAVE_DATA_PATH;

        private void Awake()
        {
            SAVE_DATA_PATH = Application.persistentDataPath + "/orkanoid.sav";
        }

        public void SaveGame()
        {
            SaveGameData();
        }

        public async void LoadGame()
        {
            SaveData savedData = LoadGameData();
            if (savedData != null)
            {
                await this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER).LoadSavedLevel(savedData);
            }
        }

        private void SaveGameData()
        {
            SaveData data = PrepareSaveData();
            BinaryFormatter bf = new();
            FileStream file = File.Create(SAVE_DATA_PATH);
            bf.Serialize(file, data);
            file.Close();
        }

        private SaveData LoadGameData()
        {
            SaveData data = null;
            if (File.Exists(SAVE_DATA_PATH))
            {
                BinaryFormatter bf = new();
                FileStream file = File.Open(SAVE_DATA_PATH, FileMode.Open);
                data = bf.Deserialize(file) as SaveData;
                file.Close();
            }
            return data;
        }

        public bool SaveFileExists()
        {
            return File.Exists(SAVE_DATA_PATH);
        }

        private SaveData PrepareSaveData()
        {
            SaveData data = new();
            data.levelID = GameManager.CurrentLevel;
            data.currentScore = GameManager.CurrentScore;
            data.currentLives = GameManager.CurrentLives;
            data.ballDamage = Ball.CurrentBallDamage;
            data.ballScale = Ball.CurrentBallScale.x / Ball.OriginalBallScale.x;
            data.paddleScale = Paddle.CurrentPaddleScale.x / Paddle.OriginalPaddleScale.x;
            data.bricks = new();
            foreach (IBrick brick in BrickGrid.Grid)
            {
                if (brick != null)
                    data.bricks.Add(new(brick.GetGridPosition(), brick.GetBrickID(), brick.GetHitsTaken()));
            }
            return data;
        }
    }

    [Serializable]
    internal class SaveData
    {
        public int levelID, currentScore, currentLives, ballDamage;
        public List<BrickData> bricks;
        public float ballScale, paddleScale;
    }

    [Serializable]
    internal class BrickData
    {
        public Vector2S Position { get; }
        public string TemplateID { get; }
        public int HitsTaken { get; }

        public BrickData(Vector2Int Position, string TemplateID, int HitsTaken)
        {
            this.Position = new(Position.x, Position.y);
            this.TemplateID = TemplateID;
            this.HitsTaken = HitsTaken;
        }
    }

    [Serializable]
    internal struct Vector2S
    {
        public float x;
        public float y;

        public Vector2S(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Vector2S)
            {
                return false;
            }

            var s = (Vector2S)obj;
            return x == s.x &&
                   y == s.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }

        public static bool operator ==(Vector2S a, Vector2S b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2S a, Vector2S b)
        {
            return a.x != b.x && a.y != b.y;
        }

        public static implicit operator Vector2(Vector2S x)
        {
            return new Vector2(x.x, x.y);
        }

        public static implicit operator Vector2S(Vector2 x)
        {
            return new Vector2S(x.x, x.y);
        }
    }

    [Serializable]
    internal struct Vector3S
    {
        public float x;
        public float y;
        public float z;

        public Vector3S(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Vector3S)
            {
                return false;
            }

            var s = (Vector3S)obj;
            return x == s.x &&
                   y == s.y &&
                   z == s.z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public static bool operator ==(Vector3S a, Vector3S b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vector3S a, Vector3S b)
        {
            return a.x != b.x && a.y != b.y && a.z != b.z;
        }

        public static implicit operator Vector3(Vector3S x)
        {
            return new Vector3(x.x, x.y, x.z);
        }

        public static implicit operator Vector3S(Vector3 x)
        {
            return new Vector3S(x.x, x.y, x.z);
        }
    }
}
