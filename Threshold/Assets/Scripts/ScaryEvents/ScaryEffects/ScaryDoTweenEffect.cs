using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace ScaryEvents.ScaryEffects
{
    public enum DoTweenType
    {
        None,
        Move,
        Rotate,
        Scale,
        Shake,
        Fade,
        WavyTexture,
        MoveMeshVertices,
        MoveAllRoomObjectsUp
    }

    public class ScaryDoTweenEffect : ScaryEffect
    {
        [Header("DoTween Settings")]
        public DoTweenType doTweenType;
        public bool isRelative;
        public LoopType doTweenLoopType = LoopType.Restart;
        public int doTweenLoops = 1;
    
        // DoTween variables
        // targetPosition and targetRotation is based on World Space. local Space �� Relative �� case ���� ����ϸ� ���� ��.
        public Vector3 targetPosition = Vector3.zero;
        public Vector3 targetRotation = Vector3.zero;
        public Vector3 targetScale = Vector3.one;
        public float shakePosition = 1;
        public bool isFadeOut = false;
        public float amplitude = 0.1f;
        public float frequency = 1.0f;
        public float speed = 1.0f;
        public float vertexOffset = 0.1f;
        public float centerRadius = 1.0f;
        
        public override void StartEffectInternal()
        {
            switch (doTweenType)
            {
                case DoTweenType.Move:
                    Position();
                    break;
                case DoTweenType.Rotate:
                    Rotation();
                    break;
                case DoTweenType.Scale:
                    Scale();
                    break;
                case DoTweenType.Shake:
                    Shaking();
                    break;
                case DoTweenType.Fade:
                    Fade();
                    break;
                case DoTweenType.WavyTexture:
                    WavyTexture();
                    break;
                case DoTweenType.MoveMeshVertices:
                    MoveMeshVertices();
                    break;
                case DoTweenType.MoveAllRoomObjectsUp:
                    MoveAllRoomObjectsUp();
                    break;
            }
            
            DelayAndStopEffect();
        }

        #region Dotween Functions
    
        public void Position()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            if (a == null) return;
            a.DOMove(new Vector3(targetPosition.x,targetPosition.y,targetPosition.z), duration)
                .SetEase(ease)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType);
        }

        public void Rotation()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            if (a == null) return;
            a.DORotate(new Vector3(targetRotation.x,targetRotation.y,targetRotation.z), duration, RotateMode.FastBeyond360)
                .SetEase(ease)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType);
        }

        public void Scale()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            if (a == null) return;
            a.DOScale(new Vector3(targetScale.x, targetScale.y, targetScale.z), duration)
                .SetEase(ease)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType);
        }

        public void Shaking()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            if (a == null) return;
            a.DOShakePosition(duration, shakePosition, doTweenLoops, 90, false, isFadeOut)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType);;
        }

        public void Fade()
        {
            var a = targetSource.GetCurrentTarget<Renderer>("renderer");
            if (a == null) return;
            var material = a.material;

            Color color = material.color;
            color.a = 0;
            material.color = color;

            material.DOFade(1, duration)
                .SetEase(ease)
                .SetLoops(doTweenLoops, doTweenLoopType);
        }

        public void WavyTexture()
        {
            Material a;
            var ff = targetSource.GetCurrentTarget<Renderer>("renderer");

            if(ff == null) 
                return;
            else 
                a = ff.material;

            Vector2 originalOffset = a.mainTextureOffset;

            a.DOOffset(new Vector2(originalOffset.x, originalOffset.y + amplitude), frequency)
                .SetEase(ease)
                .SetLoops(doTweenLoops, doTweenLoopType)
                .OnStepComplete(() => a.mainTextureOffset = originalOffset);
        }

        public void MoveMeshVertices()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            if (a == null) return;
            
            var meshFilter = a.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                ApplyMeshEffect(meshFilter);
                return;
            }

            var meshFilters = a.GetComponentsInChildren<MeshFilter>();
            foreach (var mf in meshFilters)
            {
                ApplyMeshEffect(mf);
            }
        }

        private void ApplyMeshEffect(MeshFilter meshFilter)
        {
            Mesh mesh = meshFilter.mesh;
            if (mesh != null)
            {
                if (!mesh.isReadable)
                {
                    return;
                }
            }
            Vector3[] originalVertices = mesh.vertices;

            Vector3[] movedVertices = new Vector3[originalVertices.Length];
            float[] randomOffsets = new float[originalVertices.Length];

            // 각 버텍스의 임의의 오프셋 생성
            for (int i = 0; i < originalVertices.Length; i++)
            {
                randomOffsets[i] = Random.Range(0.0f, vertexOffset);
            }

            // 버텍스를 자연스럽게 흘러내리게 하는 애니메이션
            DOVirtual.Float(0, 1, duration, value =>
            {
                for (int i = 0; i < originalVertices.Length; i++)
                {
                    float offset = Mathf.Lerp(0, randomOffsets[i], value);
                    movedVertices[i] = originalVertices[i] + Vector3.down * offset;
                }
                mesh.vertices = movedVertices;
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
            })
            .SetEase(ease)
            .SetLoops(doTweenLoops, doTweenLoopType);
        }

        public void MoveAllRoomObjectsUp()
        {
            GameObject[] roomObjects = GameObject.FindGameObjectsWithTag("RoomObject");
            Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();

            foreach (GameObject parentObject in roomObjects)
            {
                foreach (Transform child in parentObject.transform)
                {
                    if (child.tag != "FixedObject")
                    {
                        if (!originalPositions.ContainsKey(child))
                            originalPositions[child] = child.position;

                        child.DOMove(child.position + targetPosition, duration)
                            .SetEase(ease)
                            .SetRelative(isRelative)
                            .SetLoops(doTweenLoops, doTweenLoopType);

                        child.DOShakeRotation(duration, shakePosition)
                            .SetRelative(isRelative)
                            .SetLoops(doTweenLoops, doTweenLoopType)
                            .OnComplete(() =>
                            {
                                child.DOMove(originalPositions[child], 0)
                                    .SetEase(Ease.Linear);
                            });
                    }
                }
            }
        }
        
        #endregion
    }
}