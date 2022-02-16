using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class Character
{
    public string characterName;

    //the root is the container for all images related to the character in the scene
    [HideInInspector] public RectTransform root;

    public bool isMultiLayerCharacter { get { return renderers.renderer == null; } }

    public bool enabled { get { return root.gameObject.activeInHierarchy; } set { root.gameObject.SetActive(value); } }
    public Vector2 anchorPadding {get { return root.anchorMax - root.anchorMin; } }
   
    //make this character say something
    public void Say(string speech, bool add = false) 
    {    if (!enabled)
            enabled = true;
        if (!add)
            DialogueSystem.instance.Say(speech, true, characterName);
        else
            DialogueSystem.instance.SayAdd(speech, characterName);
    
    }


    Vector2 targetPosition;
    Coroutine moving;
    bool isMoving { get { return moving != null; } }
    public void MoveTo ( Vector2 Target, float speed, bool smooth = true) 
    {   //if we are moving stop moving
        StopMoving();
        //start moving coroutine
        moving = CharacterManager.instance.StartCoroutine(Moving (Target, speed, smooth));
    
    }

    public void StopMoving(bool arriveAtTargetPositionImmediately = false)
    { if (isMoving)
        {
            CharacterManager.instance.StopCoroutine(moving);
            if (arriveAtTargetPositionImmediately)
                SetPosition(targetPosition);
        }

        moving = null;
    
    
    }

    public void SetPosition(Vector2 target)
    {
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;
        Vector2 minAnchorTarget = new Vector2(maxX - targetPosition.x, maxY - targetPosition.y);
        
        root.anchorMin = minAnchorTarget;
        root.anchorMax = root.anchorMin + padding;
     



    }
    IEnumerator Moving(Vector2 target, float speed, bool smooth)
    {
        targetPosition = target;

        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;

        Vector2 minAnchorTarget = new Vector2(maxX - targetPosition.x, maxY - targetPosition.y);
        speed *= Time.deltaTime; 

        while (root.anchorMin != minAnchorTarget) 
        {
            root.anchorMin = (!smooth) ? Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed) : Vector2.Lerp(root.anchorMin, minAnchorTarget, speed);
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame();
        }

    }


    //create a new character
    public Character(string _name, bool enableOnStart = true)
    {
        CharacterManager cm = CharacterManager.instance;

        //locate the character prefab
        GameObject prefab = Resources.Load("Characters/Character[Raelin]") as GameObject;  

        //spawn an instance of the prefab in the character panel
        GameObject ob = GameObject.Instantiate(prefab, cm.characterPanel);

        root = ob.GetComponent<RectTransform>();
        characterName = _name;

        //get the renderer(s)

        renderers.renderer = ob.GetComponentInChildren<RawImage>();

        if (isMultiLayerCharacter) 
        {
            renderers.bodyRenderer = ob.transform.Find("bodyLayer").GetComponent<Image> ();
            renderers.expressionRenderer = ob.transform.Find("expressionLayer").GetComponent<Image>();

        }

        //dialogue = DialogueSystem.instance;
        enabled = enableOnStart;
    
    }



    
    
    [System.Serializable]


    //store the renderers
    public class Renderers
    {

        public RawImage renderer;

        public Image bodyRenderer;
        public Image expressionRenderer;
    }


    public Renderers renderers = new Renderers();
}
