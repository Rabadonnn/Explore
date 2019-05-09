using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Explore
{
    public abstract class GameObject
    {
        private static List<GameObject> Objects = new List<GameObject>();
        private static string defaultName = "undefined";
        public string Name {get;}
        protected Vector2 position;
        public Rectangle rectangle;
        protected Texture2D texture;
        public bool isDead = false;

        public GameObject() {
            position = Vector2.Zero;
            Name = defaultName;
            Objects.Add(this);
        }

        public GameObject(Vector2 pos) {
            position = pos;
            Name = defaultName;
            Objects.Add(this);
        }

        public GameObject(string _Name) {
            position = Vector2.Zero;
            Name = _Name;
            Objects.Add(this);
        }

        public GameObject(Vector2 pos, string _Name) {
            position = pos;
            Name = _Name;
            Objects.Add(this);
        }
        
        public virtual void SetTexture(Texture2D _texture) {
            texture = _texture;
        }

        public static GameObject GetObject(string _name) {
            for (int i = 0; i < Objects.Count; i++) {
                if (Objects[i].Name == _name) {
                    return Objects[i];
                }
            }
            return null;
        }

        public static List<GameObject> GetObjects(string _name) {
            List<GameObject> result = new List<GameObject>();
            for (int i = 0; i < Objects.Count; i++) {
                if (Objects[i].Name == _name) {
                    result.Add(Objects[i]);
                }
            }
            return result;
        }

        public static List<GameObject> GetAllObjects() {
            return Objects;
        }

        public static void UpdateList() {
            for (int i = 0; i < Objects.Count; i++) {
                if (Objects[i].isDead) {
                    Objects.RemoveAt(i);
                }
            }
        }
    }
}