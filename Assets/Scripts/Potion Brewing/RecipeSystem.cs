using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class RecipeSystem : MonoBehaviour
{
    public BrewingSystem brewingSystem;

    private List<string[]> _recipeList = new List<string[]>();

    //This is the page number and the displayed potion. So display potion is just page number -1.
    private int _pageNumber = -1;
    private int _displayedPotion = 0;

    //UI objects to display title and content.
    [SerializeField] private TextMeshProUGUI _pageTitle;
    [SerializeField] private TextMeshProUGUI _pageContent;
    [SerializeField] private TextMeshProUGUI _pageRecipe;
    [SerializeField] private TextMeshProUGUI _pageNumText;

    [SerializeField] private GameObject _frontPage;
    [SerializeField] private GameObject _chaptersPage;
    [SerializeField] private GameObject _standardPage;

    // Start is called before the first frame update
    void Start()
    {
        _recipeList = brewingSystem.recipeList;
        PageChanged();
    }

    // called whenever the page is changed
    void PageChanged()
    {
        if(_pageNumber == -1)
        {
            //Show the front page.
            Debug.Log("On front page");
            _frontPage.SetActive(true);
            _chaptersPage.SetActive(false);
            _standardPage.SetActive(false);

        }
        else if(_pageNumber == 0)
        {
            //Show the chapters page.
            _frontPage.SetActive(false);
            _chaptersPage.SetActive(true);
            _standardPage.SetActive(false);

        }
        else
        {
            //Show the recipe pages.
            _frontPage.SetActive(false);
            _chaptersPage.SetActive(false);
            _standardPage.SetActive(true);

            _displayedPotion = _pageNumber - 1;
            // Displays the title, description and image whenever I get around to adding it.
            _pageTitle.text = _recipeList[_displayedPotion][0];
            _pageContent.text = _recipeList[_displayedPotion][4];

            // Displays the recipe.

            // Loops through the three ingredients.
            string outputRecipe = "";
            for(int i = 1; i <= 3; i++)
            {
                if(i == 1 && _recipeList[_displayedPotion][i] != "0")
                {
                    outputRecipe += "Brown Leaf: " + _recipeList[_displayedPotion][i] + " \n";
                }
                else if(i == 2 && _recipeList[_displayedPotion][i] != "0")
                {
                    outputRecipe += "Stick: " + _recipeList[_displayedPotion][i] + " \n";
                }
                else if(i == 3 && _recipeList[_displayedPotion][i] != "0")
                {
                    outputRecipe += "Fancy Leaf: " + _recipeList[_displayedPotion][i] + " \n";
                }
    
            }

            _pageRecipe.text = outputRecipe;
        }
    }

    // Change page number by defined amount
    public void ChangePage(int amount)
    {
        _pageNumber = _pageNumber += amount;
        if(_pageNumber < -1)
        {
            _pageNumber = -1;
        }
        if(_pageNumber > _recipeList.Count)
        {
            _pageNumber = _recipeList.Count;
        }

        _pageNumText.text = _pageNumber.ToString();
        PageChanged();
    }
}
