using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MKU.Scripts.Enums;
using MKU.Scripts.HelthSystem;
using MKU.Scripts.Models;
using MKU.Scripts.Singletons;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;

namespace MKU.Scripts.CharacterSystem
{
    public class SpawnPointCharacter : MonoBehaviour
    {
        public CharacterPatterns _character;
        public List<CharactersSelect> _characters = new();
        private void Start()
        {
            if (Singleton.Instance.portalManager == null
                || Singleton.Instance.portal == Portal.None)ShowAvatar();
        }

        private async Task ShowAvatar()
        {
            Debug.Log($"{nameof(ShowAvatar)} >> {Singleton.Instance._character == null}");
            if (_character != null)
            {
                _characters.ForEach(async x =>
                {
                    if (x._character.name == _character.name)
                    {
                        var update = await ReGenProgression(Singleton.Instance._character.id);
                        if (update != null)
                        {
                            Vector3 position = new Vector3(update.positionX, update.positionY, update.positionZ);
                            GameObject character = null; 
                            if(position != Vector3.zero)character = Instantiate(x.characterModel, position, Quaternion.identity);
                            if(position == Vector3.zero)character = Instantiate(x.characterModel, transform.position, Quaternion.identity);
                            CharController controller = character.GetComponentInChildren<CharController>();
                            controller._base.Attributes = new _Attributs(_character.Str, _character.Agi, _character.Vit,
                                _character.Inteligence, _character.Luk, _character.Def);
                            controller.Id = Singleton.Instance._character.id;
                            CharSettings._Instance._charController = controller;
                            CharSettings._Instance._player = character;
                            CharSettings._Instance._inputManager = controller.inputManager;
                            controller.progression.level = update.level;
                            controller.progression.currentExperience = update.xp;
                        }
                    }
                });
            }
            if (_character == null)
            {
                _characters.ForEach(async x =>
                {
                    if (x.characterName == Singleton.Instance._character.classCharacter)
                    {
                        var update = await ReGenProgression(Singleton.Instance._character.id);
                        if (update != null)
                        {
                            Character _char = Singleton.Instance._character;
                            Vector3 position = new Vector3(update.positionX, update.positionY, update.positionZ);
                            GameObject character = null; 
                            if(position != Vector3.zero)character = Instantiate(x.characterModel, position, Quaternion.identity);
                            if(position == Vector3.zero)character = Instantiate(x.characterModel, transform.position, Quaternion.identity);
                            CharController controller = character.GetComponentInChildren<CharController>();
                            controller._base.Attributes = new _Attributs(_char.str, _char.agi, _char.vit,
                                _char.inteligence, _char.luk, _char.def);
                            controller.Id = Singleton.Instance._character.id;
                            CharSettings._Instance._charController = controller;
                            CharSettings._Instance._player = character;
                            CharSettings._Instance._inputManager = controller.inputManager;
                            controller.progression.level = update.level;
                            controller.progression.currentExperience = update.xp;
                        }
                        if (update == null)
                        {
                            Character _char = Singleton.Instance._character;
                            Vector3 position = new Vector3(0,0,0);
                            GameObject character = null;
                            if (position != Vector3.zero) character = Instantiate(x.characterModel, position, Quaternion.identity);
                            if (position == Vector3.zero) character = Instantiate(x.characterModel, transform.position, Quaternion.identity);
                            CharController controller = character.GetComponentInChildren<CharController>();
                            controller._base.Attributes = new _Attributs(_char.str, _char.agi, _char.vit,
                                _char.inteligence, _char.luk, _char.def);
                            controller.Id = Singleton.Instance._character.id;
                            CharSettings._Instance._charController = controller;
                            CharSettings._Instance._player = character;
                            CharSettings._Instance._inputManager = controller.inputManager;
                            controller.progression.level = update.level;
                            controller.progression.currentExperience = update.xp;
                        }
                    }
                });
            }
        }

        public async Task<UpdateCharacter?>  ReGenProgression(string Id)
        {
            string url = "http://cursed.agencia4red.com:8400/Character/"+$"{Id}";
           // Debug.Log($"{nameof(ReGenProgression)} >> {url}");
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Debug.Log($"{nameof(ReGenProgression)} >> {responseBody}");
                        var update = JsonConvert.DeserializeObject<UpdateCharacter>(responseBody);
                        return update;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error No Data: {ex.Message}");
                    }
                }
            }
            return null;
        }
    }
}