using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking.Types;

public class Creature : MonoBehaviour
{
    float _alpha = 1;
    public float alpha
    {
        get
        {
            return _alpha;
        }
        set
        {
            _alpha = value;
            battlecrySprite.color = new Color(battlecrySprite.color.r, battlecrySprite.color.g, battlecrySprite.color.b, 0);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, _alpha);
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, _alpha);
            highlight.color = new Color(highlight.color.r, highlight.color.g, highlight.color.b, _alpha);
            tauntSprite.color = new Color(tauntSprite.color.r, tauntSprite.color.g, tauntSprite.color.b, _alpha);
            shieldSprite.color = new Color(shieldSprite.color.r, shieldSprite.color.g, shieldSprite.color.b, _alpha*0.6f);
            silenceSprite.color = new Color(silenceSprite.color.r, silenceSprite.color.g, silenceSprite.color.b, _alpha);
            stealthSprite.color = new Color(stealthSprite.color.r, stealthSprite.color.g, stealthSprite.color.b, _alpha);
            freezeSprite.color = new Color(stealthSprite.color.r, stealthSprite.color.g, stealthSprite.color.b, _alpha);
            triggerSprite.color = new Color(triggerSprite.color.r, triggerSprite.color.g, triggerSprite.color.b, _alpha);
            deathrattleSprite.color = new Color(deathrattleSprite.color.r, deathrattleSprite.color.g, deathrattleSprite.color.b, _alpha);
            skull.color = new Color(skull.color.r, skull.color.g, skull.color.b, _alpha);
            testname.color= new Color(testname.color.r, testname.color.g, testname.color.b, _alpha);
            health.color= new Color(health.color.r, health.color.g, health.color.b, _alpha);
            damage.color= new Color(damage.color.r, damage.color.g, damage.color.b, _alpha);

        }
    }
    
    public void SetSortingOrder(int x)
    {
        x = x * 10;
        battlecrySprite.sortingOrder = x - 1;
        spriteRenderer.sortingOrder = x+1;
        icon.sortingOrder = x+2;
        highlight.sortingOrder = x+1;
        tauntSprite.sortingOrder = x;
        shieldSprite.sortingOrder = x+3;
        silenceSprite.sortingOrder = x+3;
        stealthSprite.sortingOrder = x+4;
        freezeSprite.sortingOrder = x+4;
        triggerSprite.sortingOrder = x+5;
        deathrattleSprite.sortingOrder = x+5;
        skull.sortingOrder = x+5;
        testname.GetComponent<MeshRenderer>().sortingOrder = x + 3;
        health.GetComponent<MeshRenderer>().sortingOrder = x + 3;
        damage.GetComponent<MeshRenderer>().sortingOrder = x + 3;
    }
    public bool isElevated = false;
    public void SetElevated(bool elevated)
    {
        isElevated = elevated;
        string x = elevated ? "creatureElevated" : "creature";
        string s = elevated ? "shadowCreatureElevated" : "shadowCreature";
        battlecrySprite.sortingLayerName = x;
        highlight.sortingLayerName = x;
        spriteRenderer.sortingLayerName = x;
        icon.sortingLayerName = x;
        tauntSprite.sortingLayerName = x;
        shieldSprite.sortingLayerName = x;
        silenceSprite.sortingLayerName = x;
        stealthSprite.sortingLayerName = x;
        freezeSprite.sortingLayerName = x;
        triggerSprite.sortingLayerName = x;
        deathrattleSprite.sortingLayerName = x;
        skull.sortingLayerName = x;
        testname.GetComponent<MeshRenderer>().sortingLayerName = x;
        health.GetComponent<MeshRenderer>().sortingLayerName = x;
        damage.GetComponent<MeshRenderer>().sortingLayerName = x;
    }
    public TMP_Text testname;
    public TMP_Text health, damage;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer icon;
    public DropShadow shadow;
    public SpriteRenderer tauntSprite;
    public SpriteRenderer shieldSprite;
    public SpriteRenderer silenceSprite;
    public SpriteRenderer stealthSprite;
    public SpriteRenderer freezeSprite;
    public SpriteRenderer triggerSprite;
    public SpriteRenderer deathrattleSprite;
    public SpriteRenderer battlecrySprite;

    public SpriteRenderer highlight;
    public Sprite highlightNormal;
    public Sprite highlightTaunt;

    public Sprite highlightTargetNormal;
    public Sprite highlightTargetTaunt;

    public Board board;

    public Minion minion;
    public Sprite[] frameSprites;
    public int index => minion.index;
    public Card.Cardname card => minion.card;
    public int order = 0;
    public bool preview = false;
    public bool init = false;
    public MinionBoard.MinionSource source;
    public void Set(Minion c)
    {
        minion = c;
        name = c.card + " (Creature)";
        SetSortingOrder(minion.index);

        Database.CardInfo info = Database.GetCardData(c.card);
        testname.text = "";// info.name;

        prevDmg = c.baseDamage;
        prevHP = c.baseHealth;
        health.text = c.baseHealth.ToString();
        damage.text = c.baseDamage.ToString();
        basehp = c.baseHealth;
        basedmg = c.baseDamage;

        spriteRenderer.sprite = frameSprites[(int)info.classType];
        icon.sprite = board.cardObject.GetComponent<Card>().cardSprites[(int)c.card];

        CheckAuras();
        CheckTriggers();

    }
    public bool IsFriendly()
    {
        if (board.enemyMinions.Contains(minion))
            return false;
        else
            return true;
    }
    void Start()
    {
        StartCoroutine(floater());
    }
    public Coroutine TriggerBattlecry()
    {
        return StartCoroutine(cryer());
    }
    public Coroutine TriggerTrigger()
    {
        return StartCoroutine(trigAnim());
    }
    IEnumerator trigAnim()
    {
        yield return board.animationManager.LerpZoom(triggerSprite.gameObject, Vector3.one*1.7f, 10);
        yield return board.animationManager.LerpZoom(triggerSprite.gameObject, Vector3.one, 10);
    }
    IEnumerator cryer()
    {
        yield return board.animationManager.LerpZoom(battlecrySprite.gameObject, Vector3.one, 30);
        board.animationManager.LerpZoom(battlecrySprite.gameObject, Vector3.zero, 30);
    }

    public Color baseText;
    public Color greenText;
    public Color redText;
    int prevDmg = 0;
    int prevHP = 0;
    public static IEnumerator txtBounce(TMP_Text text)
    {
        int frames = 5;
        for (float i=0;i<frames;i++)
        {
            text.transform.localScale += Vector3.one * 0.15f;
            yield return AnimationManager.Wait(1);
        }
        for (float i=0;i<frames;i++)
        {
            text.transform.localScale += Vector3.one * -0.15f; 
            yield return AnimationManager.Wait(1);
        }
    }
    void Update()
    {
    }

    public int hp, dmg, maxhp, basehp, basedmg;

    public void UpdateDisplay()
    {
        if (prevDmg != dmg)
        {
            StartCoroutine(txtBounce(damage));
        }
        if (prevHP != hp)
        {
            StartCoroutine(txtBounce(health));
        }
        prevDmg = dmg;
        prevHP = hp;
        damage.text = dmg.ToString();
        health.text = hp.ToString();

        //==================== 
        if (hp < maxhp)
            health.color = redText;
        else if (hp > basehp) //TODO: color should factor in max hp when its reduced then buffed by external
        {
            health.color = greenText;
        }
        else
            health.color = baseText;

        //=====================
        if (dmg > basedmg)
        {
            damage.color = greenText;
        }
        else
            damage.color = baseText;

    }

    public void Highlight(bool target = false)
    {
        if (isElevated) return;
        if (minion.ELUSIVE && (board.targetMode == Board.TargetMode.Spell || board.targetMode == Board.TargetMode.HeroPower)) return;

        if (tauntSprite.enabled)
            highlight.sprite = target? highlightTargetTaunt : highlightTaunt;
        else
            highlight.sprite = target? highlightTargetNormal : highlightNormal;

        highlight.enabled = true;
    }
    public void Unhighlight()
    {
        highlight.enabled = false;
    }
    public void EnableTaunt()
    {
        tauntSprite.enabled = true;
        board.animationManager.LerpZoom(tauntSprite.gameObject, Vector3.one, 10, 0.1f);
    }
    public void DisableTaunt()
    {
        tauntSprite.enabled = false;
        board.animationManager.LerpZoom(tauntSprite.gameObject, Vector3.zero, 10, 0.1f);
    }

    public void EnableShield()
    {
        shieldSprite.enabled = true;
        board.animationManager.LerpZoom(shieldSprite.gameObject, Vector3.one, 10, 0.1f);
    }
    public void DisableShield()
    {
        shieldSprite.enabled = false;
        board.animationManager.LerpZoom(shieldSprite.gameObject, Vector3.zero, 5, 0.1f);
    }

    public void EnableFreeze()
    {
        freezeSprite.enabled = true;
        freezeSprite.transform.localScale = Vector3.one * 1.15f;
        board.animationManager.LerpZoom(freezeSprite.gameObject, Vector3.one, 10, 0.1f);
    }
    public void DisableFreeze()
    {
        freezeSprite.enabled = false;
        board.animationManager.LerpZoom(freezeSprite.gameObject, Vector3.zero, 5, 0.1f);
    }

    public void CheckTriggers()
    {
        if (minion.triggers.Count>0)
        {
            bool d = false;
            foreach (Trigger t in minion.triggers)
            {
                if (t.type == Trigger.Type.Deathrattle)
                {
                    deathrattleSprite.enabled = true;
                    d = true;
                }
            }
            if (d == false)
                triggerSprite.enabled = true;
        }
        else
        {
            triggerSprite.enabled = false;
            deathrattleSprite.enabled = false;
        }
    }

    public void CheckAuras()
    {
        if (minion.HasAura(Aura.Type.Silence))
        {
            if (silenceSprite.enabled == false)
            {
                silenceSprite.enabled = true;
                silenceSprite.transform.localScale = Vector3.one * 1.15f;
                board.animationManager.LerpZoom(silenceSprite.gameObject, Vector3.one, 10, 0.1f);
            }
        }
        else
        {
            silenceSprite.enabled = false;
        }
        
        if (minion.HasAura(Aura.Type.Taunt))
        {
            if (tauntSprite.enabled == false)
                EnableTaunt();
        }
        else
        {
            if (tauntSprite.enabled == true)
                DisableTaunt();
        }

        if (minion.HasAura(Aura.Type.Shield))
        {
            if (shieldSprite.enabled == false)
                EnableShield();
        }
        else
        {
            if (shieldSprite.enabled == true)
                DisableShield();
        }

        if (minion.HasAura(Aura.Type.Freeze))
        {
            if (freezeSprite.enabled == false)
                EnableFreeze();
        }
        else
        {
            if (freezeSprite.enabled == true)
                DisableFreeze();
        }

        if (minion.HasAura(Aura.Type.Spellpower))
        {
            if (spAnim == null)
                spAnim = StartCoroutine(spellpowerEffect());
        }
        else
        {
            if (spAnim != null)
            {
                StopCoroutine(spAnim);
                spAnim = null;
            }
        }

        if (minion.STEALTH)
        {
            stealthSprite.enabled = true;
            shieldSprite.color = new Color(1, 1, 1, 0.25f);
            tauntSprite.color = new Color(1, 1, 1, 0.6f);
            freezeSprite.color = new Color(1, 1, 1, 0.6f);
            silenceSprite.color = new Color(1, 1, 1, 0.6f);
        }
        else
        {
            stealthSprite.enabled = false;
            shieldSprite.color = new Color(1,1,1,0.45f);
            tauntSprite.color = Color.white;
            freezeSprite.color = Color.white;
            silenceSprite.color = Color.white;
        }
    }

    Coroutine spAnim = null;
    IEnumerator spellpowerEffect()
    {
        while (true)
        {
            Vector3 pos = this.transform.localPosition + new Vector3(Random.Range(-1.8F, 1.8f), Random.Range(-1.6F, 1f));
            board.animationManager.CreateSpellpowerParticle(pos, AnimationManager.Effect.boardFire, Board.GetColor("92DCBA"));
            yield return Board.Wait(10);
        }
    }
    public SpriteRenderer skull;
    public void ShowSkull()
    {
        skull.enabled = true;
        skull.transform.localScale = Vector3.one * 1.5f;
        board.animationManager.LerpZoom(skull.gameObject, Vector3.one * 2, 5, 0.3f);
    }
    public void HideSkull()
    {
        skull.enabled = false;
    }
    int hoverTimer = 0;
    private void OnMouseOver()
    {
        if (preview) return;

        if (board.targeting && board.dragTargeting)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (board.targetingMinion == minion)
                {
                    //cancel by releasing on self
                    board.EndTargeting(true);
                    return;
                }
                board.TargetMinion(minion);
            }
        }
        if (hoverTimer < 30)
        {
            hoverTimer++; 
            if (hoverTimer == 30)
                board.ShowHoverTip(this);
        }
        
    }
    private void OnMouseEnter()
    {
        board.hoveredMinion = this;
        if (board.targeting && board.targetingMinion!=minion && highlight.enabled)
        {
            board.ShowSkulls(this);
        }
    }
    private void OnMouseExit()
    {
        board.hoveredMinion = null;
        hoverTimer = 0;
        board.HideHoverTip();

        if (highlight.enabled)
            board.HideSkulls();
    }

    int dragCounter = 0;
    const int dragTime = 8;
    private void OnMouseDrag()
    {
        if (preview) return;
        if (board.currTurn == false) return;
        if (board.disableInput) return;
        if (board.targetingMinion==minion)
        {
            if (dragCounter < dragTime) dragCounter++;
            if (dragCounter >= dragTime)
            {
                if (Vector3.Distance(Card.GetMousePos(), clickPos) > 0.2f)
                {
                    board.dragTargeting = true;
                    //Debug.Log("drag");
                }
            }
        }

    }
    private void OnMouseUp()
    {
        if (preview) return; 
        if (board.currTurn == false) return;
        dragCounter = 0;
        if (board.dragTargeting && board.targetingMinion==minion)
        {
            //cancel by LETTING GO OVER NOTHING
            if (board.hoveredMinion==null && board.hoveredHero==null) 
                board.EndTargeting(true);
        }
    }

    Vector3 clickPos = Vector3.zero;
    private void OnMouseDown()
    {
        if (preview) return; 
        if (board.currTurn == false) return;
        if (board.disableInput) return;
        if (board.targeting)
        {
            if (board.targetingMinion == minion)
            {
                //cancel by clicking on self
                board.EndTargeting(true);
                return;
            }

            board.TargetMinion(minion);
            return;
        }

        if (IsFriendly() == false) return;
        if (minion.canAttack == false) return;
        board.StartTargetingAttack(minion);
        clickPos = Card.GetMousePos();
    }

    public Vector3 boardPos;

    public bool floatEnabled = true;
    public IEnumerator floater()
    {
        int i = 0;
        float freq = 2F;// 1.5f;
        while (true)
        {
            if (floatEnabled)
            {
                while (board.animationManager.activeZooms.ContainsKey(this.gameObject)) yield return null;
                float ang = freq * i * Mathf.PI / 180;
                transform.localScale = Vector3.one * ((1.025f + 0.01f * Mathf.Sin(ang)));
                //transform.localEulerAngles = new Vector3(0, 0, 0.15F * Mathf.Sin(0.5f * ang));
                shadow.elevation = (0.4f + 0.1f * Mathf.Sin(ang));
                i++;
                if (i == 360) i = 0;
                yield return null;
            }
            else
            {
                i = 90;
                //transform.localEulerAngles = Vector3.zero;//Vector3.Lerp(transform.localEulerAngles,Vector3.zero,0.25f);
                yield return null;
            }
        }
    }
}
