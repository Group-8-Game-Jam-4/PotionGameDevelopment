using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestClass
{
    // what text line were on in the csv
    public int TextCounter = 0;

    // if items are needed we need the ui for that and it will just display text like hey have you got those things I asked for
    public bool AwaitingItems = false;

    // value 1 is item name value 2 is item quantity
    public List<string[]> SubmittedItems = new List<string[]>();

    // value 1 is item name value 2 is item quantity
    public List<string[]> NeededItems = new List<string[]>();
}
