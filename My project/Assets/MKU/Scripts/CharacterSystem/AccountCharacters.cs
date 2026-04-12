using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MKU.Scripts.Models;
using MKU.Scripts.Singletons;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.CharacterSystem
{
    public class AccountCharacters : MonoBehaviour
    {
        public int i = 0;
        public GameObject loading;
        public Button buttonPay;
        public string UserId;
        public UserCharacters _userCharacters;
        public List<Character> _characters = new ();
        public string nickName;
        public UserCharacter userCharacter = null;
        public CharacterPatterns _character;
        public List<CharacterPatterns> _patterns = new();
        public List<CharactersSelect> charactersSelects;
        public List<PlayerStatus> _playerStatus;
        public TextMeshProUGUI _xp;
        public GameObject position;
        public CharacterSelector _CharacterSelector;
        public SelectCharacter _select;

        public string Clan;
        public string Rarity;
        private void Start()
        {
            if (_patterns.Count <= 0)
            {
                UserId = Singleton.Instance.Id;
                if (!string.IsNullOrWhiteSpace(UserId)) GetAllCharacters(UserId);
                buttonPay.onClick.AddListener(() => PlayGame());
            }
            if (_patterns.Count > 0)
            {
                _patterns.ForEach(c =>
                {
                    charactersSelects.ForEach(s =>
                    {
                        if (c.name == s._character.name && !c.Staked)
                        {
                            SelectCharacter select = Instantiate<SelectCharacter>(s._SelectCharacter, position.transform);
                            select.accountCharacters = this;
                            select.classCharacter = s._character.name;
                            select.rent.SetActive(c.IsRented);
                                    

                        }
                    });
                });
                ShowAvatar();
            }

        }

        async void PlayGame()
        {
            Singleton.Instance.OnLoadScene("Level_01");
        }

        async void GetAllCharacters(string user)
        {
            string url = $"http://cursed.agencia4red.com:5300/Characters/{user}";
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
                        _userCharacters = JsonConvert.DeserializeObject<UserCharacters>(responseBody);
                        _userCharacters._characters.ForEach(c => _characters.Add(c));
                        Debug.Log($"{nameof(GetAllCharacters)} >> {_userCharacters._characters.Count} >> {responseBody}");
                        Debug.Log($"{nameof(GetAllCharacters)} >> {JsonConvert.SerializeObject(_characters)}");
                        _userCharacters._characters.ForEach(c =>
                        {
                            charactersSelects.ForEach(s =>
                            {
                                if (c.classCharacter == s._character.name && !c.staked)
                                {
                                    SelectCharacter select = Instantiate<SelectCharacter>(s._SelectCharacter, position.transform);
                                    _select = select;
                                    select.accountCharacters = this;
                                    select.classCharacter = s._character.name;
                                    select.rent.SetActive(c.isRented);
                                    

                                }
                            });
                        });
                        ShowAvatar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error No Data: {ex.Message}");
                    }
                }
            }
        }
        
            

        private void ShowAvatar()
        {
            for (int j = 0; j < _userCharacters._characters.Count(); j++)
            {
                if (j == i)
                {
                    OnSelectAvatar(_userCharacters._characters[i].classCharacter);
                }
            }
        }

        public void OnSelectAvatar(string classCharacter)
        {
            charactersSelects.ForEach(s =>
            {
                s.characterModel.SetActive(s._character.name == classCharacter);
            });
            Singleton.Instance._character = _characters.Find(c => c.classCharacter == classCharacter);
            GameObject characterModel = charactersSelects.Find(x => x._character.name == classCharacter).characterModel;
            CharacterPatterns _patterns = charactersSelects.Find(x => x._character.name == classCharacter)._character;
            OnSetAtributtes(characterModel, Singleton.Instance._character, _patterns);
        }
        private async void OnSetAtributtes(GameObject _hero, Character character, CharacterPatterns _characterPatterns)
        {
            Singleton.Instance._character = character;
            if (character == null)
            {
                Singleton.Instance._financeController.GetBalance();
                Character _ch = new RestoreCharacter().GetCharacter(_characterPatterns);
                _CharacterSelector.UIUpdate(_ch,_characterPatterns);
            }

            if (character != null)
            {
                Singleton.Instance._financeController.GetBalance();
                var update = await ReGenProgression(character.id);
                if (update != null)
                {
                    _xp.text = $"{update.xp}";
                    _select._level.text = $"{update.level}";
                }
                Singleton.Instance.Id = UserId;
                Debug.Log($"{nameof(OnSetAtributtes)} >> {character.id}");
                _CharacterSelector.UIUpdate(character,_characterPatterns);
            }
        }
        
        public async Task<UpdateCharacter?>  ReGenProgression(string Id)
        {
            string url = "http://MKU.Scripts.ddns.net:8400/Character/"+$"{Id}";
            Debug.Log($"{nameof(ReGenProgression)} >> {url}");
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