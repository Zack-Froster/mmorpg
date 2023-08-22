using Entities;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoSingleton<GameObjectManager> {

    Dictionary<int, UnityEngine.GameObject> Characters = new Dictionary<int, UnityEngine.GameObject>();

    protected override void OnStart()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }

    void Update()
    {

    }
    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    private void OnCharacterLeave(Character cha)
    {
        if (!Characters.ContainsKey(cha.entityId))
            return;
        if (Characters[cha.entityId] != null)
        {
            Destroy(Characters[cha.entityId]);
            this.Characters.Remove(cha.entityId);
        }
    }
    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }
    private void CreateCharacterObject(Character character)
    {
        if (!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed", character.Define.TID, character.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj, this.transform);
            go.name = "Character_" + character.Id + "_" + character.Name;
            Characters[character.entityId] = go;

            Transform transform = go.GetComponent<UICharacterHead>().head.transform;
            UIWorldElementManager.Instance.AddCharacterNameBar(transform, character);
        }
        this.InitGameObject(Characters[character.entityId], character);
    }
    public void InitGameObject(GameObject go, Character character)
    {
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);

        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            ec.entity = character;
            ec.isPlayer = character.IsCurrentPlayer;
            ec.Ride(character.Info.Ride);
        }

        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            if (character.IsCurrentPlayer)
            {
                User.Instance.CurrentPlayerInputController = pc;
                MainPlayerCamera.Instance.player = go;
                pc.enabled = true;
                pc.character = character;
                pc.entityController = ec;
            }
            else
            {
                pc.enabled = false;
            }
        }
    }


    public RideController LoadRide(int rideId, Transform parent)
    {
        var rideDefie = DataManager.Instance.Rides[rideId];
        Object obj = Resloader.Load<Object>(rideDefie.Resource);
        if (obj == null)
        {
            Debug.LogErrorFormat("Ride[{0}] Resource[{1}] not existed", rideDefie.ID, rideDefie.Resource);
            return null;
        }
        GameObject go = (GameObject)Instantiate(obj, parent);
        go.name = "Ride_" + rideDefie.ID + "_" + rideDefie.Name;
        return go.GetComponent<RideController>();
    }
}
