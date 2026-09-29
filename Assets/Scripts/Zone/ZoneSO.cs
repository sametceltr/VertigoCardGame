using UnityEngine;


[CreateAssetMenu(fileName = "zoneSo", menuName = "Custom/zoneSo")]
public class ZoneSO : ScriptableObject
{
    [SerializeField] private Zone zone;
    public Zone Zone => zone;
}