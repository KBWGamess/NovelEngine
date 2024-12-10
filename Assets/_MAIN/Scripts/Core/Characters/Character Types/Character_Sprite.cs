using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        private const string SPRITE_RENDERERD_PARENT_NAME = "Renderers";
        private const string SPRITESHEET_DEFAULT_SHEETNAME = "Default";
        private const char SPRITESHEET_TEX_SPRITE_DELIMITTER = '-';
        public CanvasGroup rootCG => root.GetComponent<CanvasGroup>();

        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

        public string artAssetsDirectory = "";

        public override bool isVisible
        {
            get { return isRevealing || rootCG.alpha == 1; }
            set { rootCG.alpha = value ? 1 : 0; }
        }

        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            rootCG.alpha = ENABLE_ON_START ? 1 : 0;
            artAssetsDirectory = rootAssetsFolder + "/Images";

            GetLayers();

            Debug.Log($"Created Sprite Character: '{name}'");
        }

        public void GetLayers()
        {
            Transform rendererRoot = animator.transform.Find(SPRITE_RENDERERD_PARENT_NAME);

            if (rendererRoot == null)
                return;

            for (int i = 0; i < rendererRoot.transform.childCount; i++)
            {
                Transform child = rendererRoot.transform.GetChild(i);

                Image rendererImage = child.GetComponentInChildren<Image>();

                if (rendererImage != null)
                {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                    layers.Add(layer);
                    child.name = $"Layer: {i}";
                }
            }
        }

        public void SetSprite(Sprite sprite, int layer = 0)
        {
            layers[layer].SetSprite(sprite);
        }

        public Sprite GetSprite(string spriteName)
        {
            if (config.sprites.Count > 0)
            {
                if (config.sprites.TryGetValue(spriteName, out Sprite sprite))
                    return sprite;
            }

            if (config.characterType == CharacterType.SpriteSheet)
            {
                string[] data = spriteName.Split(SPRITESHEET_TEX_SPRITE_DELIMITTER);
                Sprite[] spriteArray = new Sprite[0];

                if (data.Length == 2)
                {
                    string textureName = data[0];
                    spriteName = data[1];
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{textureName}");
                }
                else
                {
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{SPRITESHEET_DEFAULT_SHEETNAME}");
                }

                if (spriteArray.Length == 0)
                    Debug.LogWarning($"zalupa");

                return Array.Find(spriteArray, sprite => sprite.name == spriteName);
            }
            else
            {
                return Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName}");
            }
        }

        public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1)
        {
            CharacterSpriteLayer spriteLayer = layers[layer];

            return spriteLayer.TransitionSprite(sprite, speed);
        }

        public override IEnumerator ShowingOrHiding(bool show, float speedMultiplier = 1f)
        {
            float targetAlpha = show ? 1f : 0f;
            CanvasGroup self = rootCG;

            while (self.alpha != targetAlpha) 
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime * speedMultiplier);
                yield return null;
            }

            co_revealing = null;
            co_hiding = null;
        }

        public override void SetColor(Color color)
        {
            base.SetColor(color);

            color = displayColor;

            foreach (CharacterSpriteLayer layer in layers)
            {
                layer.StopChangingColor();
                layer.SetColor(color);
            }
        }

        public override IEnumerator ChangingColor(Color color, float speed)
        {
            foreach (CharacterSpriteLayer layer in layers)
                layer.TransitionColor(color, speed);
            
            yield return null;

            while (layers.Any(l => l.isChangingColor))
                yield return null;

            co_changingColor = null;
        }

        public override IEnumerator Highlighting(float speedMultiplier, bool immediate = false)
        {
            Color targetColor = displayColor;

            foreach(CharacterSpriteLayer layer in layers)
            {
                if (immediate)
                    layer.SetColor(displayColor);
                else
                    layer.TransitionColor(targetColor, speedMultiplier);
            }

            yield return null;

            while (layers.Any(l => l.isChangingColor))
                yield return null;

            co_highlighting = null;
        }

        public override IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            foreach (CharacterSpriteLayer layer in layers)
            {
                if (faceLeft)
                    layer.FaceLeft(speedMultiplier, immediate);
                else
                    layer.FaceRight(speedMultiplier, immediate);
            }

            yield return null;

            while(layers.Any(l => l.isFlipping))
                yield return null;

            co_flipping = null;
        }

        public override void OnReceiveCastingExpression(int layer, string expression)
        {
            Sprite sprite = GetSprite(expression);

            if (sprite == null)
            {
                return;
            }

            TransitionSprite(sprite, layer);
        }
    }
}