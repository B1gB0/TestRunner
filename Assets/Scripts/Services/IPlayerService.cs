using Cinemachine;
using DataBase.Data;
using Player;
using Player.Characteristics;
using Reflex.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Services
{
    public interface IPlayerService : IService
    {
        public CinemachineVirtualCamera FreeLookCamera { get; }
        public Player.Core.Player Player { get; }
        public PlayerData GetPlayerDataByType(PlayerType type);
        public Player.Core.Player CreatePlayerByPrefab(Player.Core.Player playerPrefab, Vector3 spawnPoint);
        public PlayerCharacteristics InitPlayerCharacteristics(PlayerData data);
        public void SpawnPlayer();
        public void GetSceneObjects(Container container, CinemachineVirtualCamera freeLookCamera);
        public void GetJoystickWithAttackButton(Joystick joystick);
    }
}