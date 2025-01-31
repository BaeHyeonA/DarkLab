using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ScaryEvents.ScaryEffects
{
    public enum StateType
    {
        None,
        Active,
        Deactive,
        Spawn,
        SpawnAndPlayAnimation,
        SpawnShadow,
        MoveShadow
    }

    public class ScaryStateEffect : ScaryEffect
    {
        [Header("State Settings")]
        public StateType stateType;
        public bool frontCreation = true;
        public bool isDisappearance = true;
        public bool createBigObject = false;
        public float deactiveDelay = 0;
        public AnimationClip[] animationClips;
        public GameObject objectToSpawn;
        public Vector3 targetPosition = Vector3.zero;
        public Vector3 targetRotation = Vector3.zero;
        public bool isRelative;
        public LoopType doTweenLoopType = LoopType.Restart;
        public int doTweenLoops = 1;

        private GameObject playerTransform;

        public override void StartEffectInternal()
        {
            switch (stateType)
            {
                case StateType.Active:
                    Active();
                    break;
                case StateType.Deactive:
                    StartCoroutine(Deactive());
                    break;
                case StateType.Spawn:
                    StartCoroutine(Spawn());
                    break;
                case StateType.SpawnAndPlayAnimation:
                    StartCoroutine(SpawnAndPlayAnimation());
                    break;
                case StateType.SpawnShadow:
                    StartCoroutine(SpawnShadow());
                    break;
                case StateType.MoveShadow:
                    ShadowMove();
                    break;
            }
            
            DelayAndStopEffect();
        }

        private void Active()
        {
            GameObject a = targetSource.GetCurrentTarget<Transform>("transform").gameObject;

            a.SetActive(true);
            
            DelayAndStopEffect();
        }

        private IEnumerator Deactive()
        {
            GameObject a = targetSource.GetCurrentTarget<Transform>("transform").gameObject;

            yield return new WaitForSeconds(delay);

            a.SetActive(false);

            StopEffect();
        }

        private IEnumerator Spawn()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            GameObject targetObject = Instantiate(objectToSpawn, a.position, a.rotation);
            
            if(isDisappearance)
            {
                yield return new WaitForSeconds(deactiveDelay);

                targetObject.SetActive(false);
            }
        }

        private IEnumerator SpawnAndPlayAnimation()
        {   
            Transform playerTransform = MainManager.Instance.player.transform;
            Camera playerCamera = playerTransform.gameObject.GetComponentInChildren<Camera>();

            PlayerController playerController = playerTransform.GetComponent<PlayerController>();

            GameObject targetObject;

            playerController.playerEventOff = false;

            if(frontCreation)
            {
                Vector3 spawnPosition = playerTransform.position + new Vector3(playerTransform.forward.x, 0, playerTransform.forward.z);
                targetObject = Instantiate(objectToSpawn, spawnPosition, playerTransform.rotation * Quaternion.Euler(0, 180, 0));

                if(createBigObject)
                    targetObject.transform.position -= new Vector3(0, 1.4f, 0);
            }
            else 
            {
                Quaternion rotation = Quaternion.Euler(targetRotation);
                targetObject = Instantiate(objectToSpawn, targetPosition , rotation);
            }

            // 카메라를 생성된 오브젝트의 얼굴 부분으로 이동 및 고정
            if (targetObject != null && playerCamera != null)
            {
                Vector3 directionToObject = targetObject.transform.position - playerCamera.transform.position;
                directionToObject.y += 3f;
                Quaternion lookRotation = Quaternion.LookRotation(directionToObject);
                playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, lookRotation, Time.deltaTime * 10f);
            }

            Animator animator = targetObject.GetComponent<Animator>();

            if (animator != null && animationClips.Length > 0)
            {
                int randomIndex = Random.Range(0, animationClips.Length);
                AnimationClip selectedClip = animationClips[randomIndex];

                animator.Play(selectedClip.name);
            }

            if(isDisappearance)
            {
                yield return new WaitForSeconds(deactiveDelay);

                targetObject.SetActive(false);
                playerController.playerEventOff = true;
            }
        }

        private IEnumerator SpawnShadow()
        {
            Transform playerTransform = MainManager.Instance.player.transform;
            Camera playerCamera = playerTransform.gameObject.GetComponentInChildren<Camera>();

            GameObject targetObject;
            if(frontCreation)
            {
                Vector3 spawnPosition = playerTransform.position + new Vector3(playerTransform.forward.x, 0, playerTransform.forward.z);
                targetObject = Instantiate(objectToSpawn, spawnPosition, playerTransform.rotation * Quaternion.Euler(0, 180, 0));

                if(createBigObject)
                    targetObject.transform.position -= new Vector3(0, 1.4f, 0);
            }
            else 
            {
                Quaternion rotation = Quaternion.Euler(targetRotation);
                targetObject = Instantiate(objectToSpawn, targetPosition , rotation);
            }

            Animator animator = targetObject.GetComponent<Animator>();

            if (animator != null && animationClips.Length > 0)
            {
                int randomIndex = Random.Range(0, animationClips.Length);
                AnimationClip selectedClip = animationClips[randomIndex];

                animator.Play(selectedClip.name);
            }

            if(isDisappearance)
            {
                yield return new WaitForSeconds(deactiveDelay);

                targetObject.SetActive(false);
            }
        }

        private void ShadowMove()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            Vector3 spawnPosition = new Vector3(a.position.x, a.position.y - 0.5f, a.position.z);
            GameObject targetObject = Instantiate(objectToSpawn, spawnPosition, a.rotation);

            var b = targetObject.transform;
            b.DOMove(new Vector3(targetPosition.x,targetPosition.y,targetPosition.z), duration)
                .SetEase(ease)
                .SetRelative(isRelative)
                .SetLoops(doTweenLoops, doTweenLoopType)
                .OnComplete(() => targetObject.SetActive(false));
        }
    }
}
