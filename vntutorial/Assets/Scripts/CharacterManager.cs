using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Responsible for adding and mantaining characters in the scene
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    //all characters must be attached to the character panel
    public RectTransform characterPanel;

    //a list of all characters currently in the scene
    public List<Character> characters = new List<Character>();


    //easy lookup for our characters
    public Dictionary<string, int> characterDictionary = new Dictionary<string, int>();


    void Awake()
    {
        instance = this;
    }



    // try to get a character by the name provided from the character list
    public Character GetCharacter (string characterName, bool createCharacterIfDoesNotExist = true, bool enableCreatedCharacterOnStart = true)
    {   

        //search our dictionary to find characters if on list
        int index = -1;
        if (characterDictionary.TryGetValue(characterName, out index))
        {
            return characters [index];

        }

        else if (createCharacterIfDoesNotExist)
        
        {
            return CreateCharacter(characterName, enableCreatedCharacterOnStart);
        
        }

        return null;
    }



    //creates the character
    public Character CreateCharacter(string characterName, bool enableOnstart = true) 
    {
        Character newCharacter = new Character (characterName, enableOnstart);
        characterDictionary.Add(characterName, characters.Count);
        characters.Add(newCharacter);

        return newCharacter;

    
    }

    public class CHARACTERPOSITIONS
    {
        public Vector2 bottomLeft = new Vector2(0, 0);
        public Vector2 topRight = new Vector2(1f, 1f);
        public Vector2 center = new Vector2(0.5f, 0.5f);
        public Vector2 bottomRight = new Vector2(1f, 0);


    
    }
    public static CHARACTERPOSITIONS characterpositions = new CHARACTERPOSITIONS();

}
