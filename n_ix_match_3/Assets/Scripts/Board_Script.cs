using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_Script : MonoBehaviour {

	public GameObject tilePrefab;
	public int width;
	public int height;
	public GameObject[] cats;
	public GameObject[,] allCats;
	private TileBG_Script[,] allTiles;
	public int baseMatchValue = 100;
	private Score_Script Score_Script;
	

	// Use this for initialization
	void Start () {
		Score_Script = FindObjectOfType<Score_Script>();
		allTiles = new TileBG_Script[width,height];
		allCats = new GameObject[width,height];
		SetUp();
	}
	
private void SetUp() {
	for(int i = 0; i < width; i++) {
		for(int z = 0; z < height; z++) {
			Vector2 tempPosition = new Vector2(i, z);
			GameObject TileBG_Script = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
			TileBG_Script.transform.parent = this.transform;
			int catToUse = Random.Range(0, cats.Length);

			int maxIterations = 0;
			while(MatchesAt(i,z,cats[catToUse]) && maxIterations < 100){
				catToUse = Random.Range(0, cats.Length);
				maxIterations++;
			}
			maxIterations = 0;

			GameObject cat = Instantiate(cats[catToUse], tempPosition, Quaternion.identity );
			cat.transform.parent = this.transform;
			allCats[i,z] = cat;
		}
	}
}
private void DestroyMatchAt(int column, int row){
if(allCats[column,row].GetComponent<Cat_Script>().isMatch){
	Destroy(allCats[column,row]);
	Score_Script.IncreaseScore(baseMatchValue);
	allCats[column,row] = null;
}
}
public void DestroyMatch(){
for(int i = 0; i < width; i++){
	for(int z = 0; z < height; z++){
			if(allCats[i,z] != null){
				DestroyMatchAt(i,z);
			}
			}
		}
		StartCoroutine(FallRowCorout());
	}
private IEnumerator FallRowCorout(){
	int nullCount = 0;
	for(int i = 0; i < width; i++){
	for(int z = 0; z < height; z++){
		if(allCats[i,z] == null){
			nullCount++;
		}else if(nullCount > 0){
			allCats[i,z].GetComponent<Cat_Script>().row -= nullCount;
			allCats[i,z] = null;
		}
		}
		nullCount = 0;
	}	
	yield return new WaitForSeconds(0.4f);
	StartCoroutine(RefillBoardCorout());
}
private void RefillBoard(){
	for(int i = 0; i < width; i++){
	for(int z = 0; z < height; z++){
		if(allCats[i,z] == null){
			Vector2 tempPosition = new Vector2(i,z);
			int catToUse = Random.Range(0,cats.Length);
			GameObject piece = Instantiate(cats[catToUse], tempPosition, Quaternion.identity);
			allCats[i,z] = piece;
		}
		}
	}
}
private bool MatchesOnBoard(){
	for(int i = 0; i < width; i++){
	for(int z = 0; z < height; z++){
		if(allCats[i,z] != null){
			if(allCats[i,z].GetComponent<Cat_Script>().isMatch){
				return true;
			}
			}
		}
	}
	return false;
}
private IEnumerator RefillBoardCorout(){
	RefillBoard();
	yield return new WaitForSeconds(0.5f);
	while(MatchesOnBoard()){
		yield return new WaitForSeconds(0.5f);
		DestroyMatch();
	}
}
private bool MatchesAt(int column, int row, GameObject piece){
	if(column > 1 && row > 1){
		if(allCats[column - 1, row].tag == piece.tag && allCats[column - 2, row].tag == piece.tag){
			return true;
		}
		if(allCats[column, row - 1].tag == piece.tag && allCats[column, row - 2].tag == piece.tag){
			return true;
		}
	}else if(column <= 1 || row <= 1){
		if(row > 1){
			if(allCats[column,row - 1].tag == piece.tag && allCats[column,row - 2].tag == piece.tag){
				return true;
			}
		}
		if(column > 1){
			if(allCats[column - 1,row].tag == piece.tag && allCats[column - 2,row].tag == piece.tag){
				return true;
			}
		}
	}
	return false;
}
}
