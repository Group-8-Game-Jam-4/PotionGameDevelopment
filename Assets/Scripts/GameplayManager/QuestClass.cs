using System.Collections.Generic;

public class QuestClass
{
    // what text line were on in the csv
    public int TextCounter = 2;

    // 0 is storyline, 1 is needing items, 2 is no quests available
    public int state = 0;

    // if items are needed we need the ui for that and it will just display text like hey have you got those things I asked for
    public bool AwaitingItems = false;

    // value 1 is item name value 2 is item quantity
    public List<string[]> SubmittedItems = new List<string[]>();

    // value 1 is item name value 2 is item quantity
    public List<string[]> NeededItems = new List<string[]>();

    // various text things for the npc to display
    public string currentStoryline = "";
    public string doYouHaveThis = "";
    public string noQuestsAvailable = "";
}
