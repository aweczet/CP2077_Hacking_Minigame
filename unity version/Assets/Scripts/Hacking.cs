using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Hacking : MonoBehaviour
{
    private HackingGameLogic game;
    
    public Transform mainGameGrid;
    public Transform highlight;
    public Transform correctSequenceGrid;
    public Transform playerSequenceGrid;
    public ProgressBar progressBar;
    public Transform acces;
    public Camera cam;
    
    public int boardSize = 3;
    public int correctSequenceSize = 2;
    public int bufferSize = 5;

    public GraphicRaycaster raycaster;

    private bool firstClick = true;
    
    // Start is called before the first frame update
    void Start()
    {
        game = new HackingGameLogic(boardSize, correctSequenceSize, bufferSize);
        mainGameGrid.GetComponent<GridLayoutGroup>().constraintCount = boardSize;
        float range = boardSize * correctSequenceSize;
        progressBar.maxTime = Random.Range(range - 2.0f, range + 2.0f);
        FillUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar.GetCurTime() <= 0)
        {
            game.GameEnd = true;
            progressBar.maxTime = 0;
            progressBar.SetValues();
        }
        if (!game.GameEnd)
        {
            HighlightPossible(game.GETRowCol());
            if (Input.GetMouseButtonUp(0))
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                List<RaycastResult> results = new List<RaycastResult>();

                pointerEventData.position = Input.mousePosition;
                raycaster.Raycast(pointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.GetComponent<BoxCollider2D>())
                    {
                        PlayerClick(result.gameObject.transform.name);
                    }
                }
            }
        }
        else
        {
            GameEnd();
            if (Input.GetKeyDown("space"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }            
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    private void HighlightPossible(Vector2 rowcol)
    {
        RectTransform h = highlight.GetComponent<RectTransform>();
        if (game.row)
        {
            h.pivot = new Vector2(.5f, .5f);
            h.sizeDelta = new Vector2(700, 90);
            h.anchoredPosition = new Vector2(300, mainGameGrid.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y - (rowcol.x * 100));
        }
        else
        {
            h.pivot = new Vector2(.5f, 1f);
            h.sizeDelta = new Vector2(90, boardSize * 100 + 200);
            h.anchoredPosition = new Vector2(mainGameGrid.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x + (rowcol.y * 100), 50);

        }
    }

    private void GameEnd()
    {
        progressBar.StopTimer();
        acces.gameObject.SetActive(true);
        if (!game.Win)
        {
            acces.GetComponent<Text>().text = "- ACCES DENIED -";
            //acces.GetComponent<Text>().color = new Color(255, 27, 0, 255);
            acces.GetComponent<Text>().color = Color.red;
        }
    }
    
    private void PlayerClick(string input)
    {
        bool good = false;
        
        int c = 0, minIn = 0, maxIn = 0;
        foreach (Transform transform in mainGameGrid)
        {
            if(transform.name == input)
                break;
            c++;
        }
        Vector2 v = game.GETRowCol();
        
        if (game.row)
        {
            minIn = (int) v.x * boardSize;
            maxIn = minIn + boardSize - 1;
            if (c >= minIn && c <= maxIn)
            {
                good = game.PlayerInput(c % boardSize);
            }
        }
        else
        {
            List<int> correct = new List<int>();
            for (int i = 0; i < boardSize; i++)
            {
                correct.Add((int) v.y + i * boardSize);
            }

            if (correct.Contains(c))
            {
                int index = correct.IndexOf(c);
                good = game.PlayerInput(index);
            }
        }

        if (good)
        {
            FillMainGrid();
            FillPlayerSequence();
            if (firstClick)
            {
                progressBar.StartTimer();
                firstClick = false;
            }
        }
        
    }

    private void FillUI()
    {
        FillMainGrid();
        FillCorrectSequence();
        FillPlayerSequence();
        progressBar.SetValues();
    }
    
    private GameObject SetUpGameObject(GameObject obj, string filepath)
    {
        obj.AddComponent<CanvasRenderer>();
        obj.AddComponent<Image>();
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
        obj.GetComponent<Image>().sprite = Resources.Load<Sprite>(filepath);
        return obj;
    }

    private void FillMainGrid()
    {
        mainGameGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(600, boardSize * 100 + 100);
        foreach (Transform child in mainGameGrid.transform)
            GameObject.Destroy(child.gameObject);
        
        List<List<string>> board = game.GETBoard();
        foreach (List<string> list in board)
        {
            foreach (string s in list)
            {
                GameObject slot = new GameObject(Random.value.ToString());
                slot.transform.parent = mainGameGrid;
                SetUpGameObject(slot, s);
                slot.AddComponent<BoxCollider2D>();
                slot.GetComponent<BoxCollider2D>().size = new Vector2(80, 80);
            }
        }
    }
    private void FillCorrectSequence()
    {
        correctSequenceGrid.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(correctSequenceSize * 100, 100);
        List<string> correctSequence = game.GETCorrectSequence();
        foreach (string field in correctSequence)
        {
            GameObject correct = new GameObject(field);
            correct.transform.parent = correctSequenceGrid;
            SetUpGameObject(correct, field);
        }
    }
    private void FillPlayerSequence()
    {
        playerSequenceGrid.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(bufferSize * 90, 100);        
        foreach (Transform child in playerSequenceGrid.transform)
            GameObject.Destroy(child.gameObject);
        
        List<string> playerSequence = game.GETPlayerSequence();
        foreach (string field in playerSequence)
        {
            GameObject playersField = new GameObject(field);
            playersField.transform.parent = playerSequenceGrid;
            SetUpGameObject(playersField, field);
        }
        
    }
}
