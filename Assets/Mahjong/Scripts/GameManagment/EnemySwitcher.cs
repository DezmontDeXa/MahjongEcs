using DDX;
using Scellecs.Morpeh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwitcher : MonoBehaviour
{
    [SerializeField] private Installer _enemyInstaller;
    [SerializeField] private Enemies _currentEnemy;

    private void Awake()
    {

    }

    private void Update()
    {
        for (int i = 0; i < _enemyInstaller.updateSystems.Length; i++)
        {
            ((AttackSystem)_enemyInstaller.updateSystems[i].System).Enabled = i == _currentEnemy.GetHashCode();
            _enemyInstaller.updateSystems[i].Enabled = i == _currentEnemy.GetHashCode();
        }
    }


    public enum Enemies
    {
        Angel, Druid, Orc, Succub, Priest, Warrior, Queen
    }
}
