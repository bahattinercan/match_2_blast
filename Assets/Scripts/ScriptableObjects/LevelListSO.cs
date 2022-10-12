using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/LevelList")]
public class LevelListSO : ScriptableObject
{
    public List<LevelSO> levels;
}