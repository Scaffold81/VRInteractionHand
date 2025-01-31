using Autohand;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Vr.Test.Weapon
{
    public class Pistol : MonoBehaviour
    {
        [Serializable] public class UnityAmmoEvent : UnityEvent<Pistol, AmmoMagazineBase> { }
        [Serializable] public class UnityPistolBoltEvent : UnityEvent<Pistol> { }

        [SerializeField]
        private GameObject _barrelPoint;
        private AmmoMagazineBase _ammoMagazine;

        [SerializeField]
        public PlacePoint _magazinePoint;

        public UnityAmmoEvent OnAmmoPlaceEvent;
        public UnityAmmoEvent OnAmmoRemoveEvent;
        public UnityPistolBoltEvent OnBoltEvent;

        private bool _magazineIsEmpty;
        private bool _cartridgeInChamber;

        private void OnEnable()
        {
            if (_magazinePoint != null)
            {
                _magazinePoint.OnPlaceEvent += OnAmmoMagazinePlace;
                _magazinePoint.OnRemoveEvent += OnAmmoMagazineRemove;
            }
        }
        private void OnDisable()
        {
            if (_magazinePoint != null)
            {
                _magazinePoint.OnPlaceEvent -= OnAmmoMagazinePlace;
                _magazinePoint.OnRemoveEvent -= OnAmmoMagazineRemove;
            }
        }
        public void Reload()
        {
            if (_ammoMagazine == null) return;

            _magazineIsEmpty = _ammoMagazine.RemoveRound();
            print("_magazineIsEmpty " + _magazineIsEmpty);
            if (!_magazineIsEmpty)
                _cartridgeInChamber = true;
        }

        public virtual void Shoot()
        {
            if (!_cartridgeInChamber) return;

            RaycastHit hit;
            if (Physics.Raycast(_barrelPoint.transform.position, _barrelPoint.transform.forward, out hit))
            {
                print($"{hit.collider.name} {hit.point}");
            }
            _cartridgeInChamber = false;
            Reload();
        }


        private void OnAmmoMagazinePlace(PlacePoint point, Grabbable mag)
        {
            if (mag.TryGetComponent<AmmoMagazineBase>(out var ammo))
            {
                this._ammoMagazine = ammo;
                OnAmmoPlaceEvent?.Invoke(this, _ammoMagazine);
            }
        }

        private void OnAmmoMagazineRemove(PlacePoint point, Grabbable grabbable)
        {
            OnAmmoRemoveEvent?.Invoke(this, _ammoMagazine);
            _ammoMagazine = null;
        }
    }
}
