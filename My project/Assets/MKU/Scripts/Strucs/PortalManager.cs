using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.Models;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.Strucs
{
    public class PortalManager : MonoBehaviour
    {
        public Portal portal = Portal.None;
        public Transform _position;

        public CharacterPatterns _character;
        public List<CharactersSelect> _characters = new();

        private void Start()
        {
            if (Singleton.Instance.portalManager == null) Singleton.Instance.portalManager = this;
            if (Singleton.Instance.portal != Portal.None) ShowAvatar();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.GetComponent<CharController>() != null)
                Singleton.Instance.OnLoadScene(portal.ToString());
        }

        private async Task ShowAvatar()
        {
        }

        public async Task<UpdateCharacter> ReGenProgression(string Id)
        {
            return null;
        }
    }
}