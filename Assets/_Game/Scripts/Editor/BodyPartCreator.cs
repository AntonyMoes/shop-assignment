using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Game.Scripts.Character;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using BodyPart = _Game.Scripts.Character.BodyPart;
using Object = UnityEngine.Object;

namespace _Game.Scripts.Editor {
    public static class BodyPartCreator {
        private const string ControllerName = "Controller.controller";
        private const string TreeName = "Animation";
        private const string StandTreeName = "Stand";
        private const string WalkTreeName = "Walk";

        private const string StandDown = "StandDown";
        private const string StandUp = "StandUp";
        private const string StandRight = "StandRight";
        private const string StandLeft = "StandLeft";
        private const string WalkDown = "WalkDown";
        private const string WalkUp = "WalkUp";
        private const string WalkRight = "WalkRight";
        private const string WalkLeft = "WalkLeft";

        private const string SpritePropertyName = "m_Sprite";
        private const float WalkVelocityThreshold = 0.01f;
        private const int RequiredSpriteCount = 64;

        private static readonly Dictionary<string, (int[], float)> Animations = new() {
            [StandDown] = (new[] { 0 }, 1f),
            [StandUp] = (new[] { 8 }, 1f),
            [StandRight] = (new[] { 16 }, 1f),
            [StandLeft] = (new[] { 24 }, 1f),
            [WalkDown] = (new[] { 32, 33, 34, 35, 36, 37 }, 6.67f),
            [WalkUp] = (new[] { 40, 41, 42, 43, 44, 45 }, 6.67f),
            [WalkRight] = (new[] { 48, 49, 50, 51, 52, 53 }, 6.67f),
            [WalkLeft] = (new[] { 56, 57, 58, 59, 60, 61 }, 6.67f),
        };

        private static readonly Dictionary<string, Vector2> StandBlendTree = new() {
            [StandDown] = new Vector2(0f, -1f),
            [StandUp] = new Vector2(0f, 1f),
            [StandRight] = new Vector2(1f, 0f),
            [StandLeft] = new Vector2(-1f, 0f),
        };

        private static readonly Dictionary<string, Vector2> WalkBlendTree = new() {
            [WalkDown] = new Vector2(0f, -1f),
            [WalkUp] = new Vector2(0f, 1f),
            [WalkRight] = new Vector2(1f, 0f),
            [WalkLeft] = new Vector2(-1f, 0f),
        };

        private static readonly List<(string, Dictionary<string, Vector2>, float)> SubTrees = new() {
            (StandTreeName, StandBlendTree, 0f),
            (WalkTreeName, WalkBlendTree, WalkVelocityThreshold),
        };

        [MenuItem("GameObject/Shop Assignment/Create body part", true, 10)]
        private static bool Validate() {
            return Selection.activeObject is Texture2D;
        }

        [MenuItem("GameObject/Shop Assignment/Create body part", false,10)]
        private static void CreateBodyPart() {
            var objectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            var sprites = AssetDatabase.LoadAllAssetsAtPath(objectPath)
                .Select(asset => asset as Sprite)
                .Where(sprite => sprite != null)
                .ToArray();

            if (sprites.Length != RequiredSpriteCount) {
                throw new Exception("Need full spriteSheet to create body part");
            }

            var fileName = Path.GetFileNameWithoutExtension(objectPath);
            var basePath = Path.Combine(Path.GetDirectoryName(objectPath)!, fileName);
            Directory.CreateDirectory(basePath);

            var clips = Animations.Select(pair => {
                var (animationName, (frames, framerate)) = pair;
                return CreateClip(sprites, basePath, animationName, frames, framerate);
            }).ToDictionary(clip => clip.name, clip => clip);

            var controller = CreateController(basePath, clips);

            CreateBodyPart(fileName, basePath, controller);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static AnimationClip CreateClip(Sprite[] sprites, string basePath, string animationName, int[] frames, float framerate) {
            var clip = new AnimationClip { frameRate = framerate, wrapMode = WrapMode.Loop};
            var spriteBinding = new EditorCurveBinding {
                type = typeof(SpriteRenderer),
                path = "",
                propertyName = SpritePropertyName
            };

            var spriteKeyFrames = Enumerable.Range(0, frames.Length)
                .Select(i => new ObjectReferenceKeyframe { time = i / framerate, value = sprites[frames[i]] })
                .ToArray();
            
            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);

            AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeyFrames);
            AssetDatabase.CreateAsset(clip, Path.Combine($"{basePath}", $"{animationName}.anim"));

            return clip;
        }

        private static AnimatorController CreateController(string basePath, Dictionary<string, AnimationClip> clips) {
            var controller = AnimatorController.CreateAnimatorControllerAtPath(Path.Combine($"{basePath}", $"{ControllerName}"));

            controller.AddParameter(AnimationParameter.HorizontalDirection, AnimatorControllerParameterType.Float);
            controller.AddParameter(AnimationParameter.VerticalDirection, AnimatorControllerParameterType.Float);
            controller.AddParameter(AnimationParameter.Velocity, AnimatorControllerParameterType.Float);

            controller.CreateBlendTreeInController(TreeName, out var tree);
            tree.blendType = BlendTreeType.Simple1D;
            tree.blendParameter = AnimationParameter.Velocity;
            tree.minThreshold = 0f;
            tree.maxThreshold = WalkVelocityThreshold;

            foreach (var (treeName, subtreeData, threshold) in SubTrees) {
                var subtree = tree.CreateBlendTreeChild(threshold);
                subtree.name = treeName;
                subtree.blendType = BlendTreeType.SimpleDirectional2D;
                subtree.blendParameter = AnimationParameter.HorizontalDirection;
                subtree.blendParameterY = AnimationParameter.VerticalDirection;

                foreach (var (animation, position) in subtreeData) {
                    subtree.AddChild(clips[animation], position);
                }
            }

            return controller;
        }

        private static void CreateBodyPart(string name, string basePath, AnimatorController controller) {
            var bodyPart = new GameObject { name = name };

            var animator = bodyPart.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            var bodyPartComponent = bodyPart.AddComponent<BodyPart>();
            bodyPartComponent.SetAnimator(animator);
            bodyPart.AddComponent<SpriteRenderer>();

            PrefabUtility.SaveAsPrefabAsset(bodyPart, Path.Combine(basePath, $"{name}.prefab"));
            Object.DestroyImmediate(bodyPart);
        }
    }
}