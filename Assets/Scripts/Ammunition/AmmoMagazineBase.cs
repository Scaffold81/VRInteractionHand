using UnityEngine;

namespace Vr.Test.Weapon
{
    public class AmmoMagazineBase : MonoBehaviour
    {
        [SerializeField]
        public int _rounds = 12;

        public bool RemoveRound()
        {
            if (_rounds > 0)
            {
                _rounds -= 1;
                return false;
            }
            return true;
        }
    }
}