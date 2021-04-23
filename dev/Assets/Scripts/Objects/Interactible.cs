using System.Collections;
using UnityEngine;

public class Interactible : MonoBehaviour {
    public enum Interaction {
        Heal,
        Repair,
        Save
    }

    public Objective objective;
    public bool Active { get; set; } = true;
    public Interaction Action = Interaction.Repair;

    public void Interact()
    {
        if (Action == Interaction.Repair) {
            objective.Fixed = true;
            Active = false;
            GameManager.Instance.Player.GetComponent<Player>().InRangeState = Player.InRange.Nothing;
        }
        else if (Action == Interaction.Heal) {
            GameManager.Instance.Player.GetComponent<Player>().health.Heal(10);
        }
        else if (Action == Interaction.Save) {
            SaveManager.SaveWorld();
            GameManager.Instance.ChunkManager.SaveModifiedChunks();
            GameManager.Instance.Player.GetComponent<Player>().SavePlayer();
            GameManager.Instance.Player.GetComponent<Player>().InRangeState = Player.InRange.Nothing;
            objective.SaveTxt.text = "Game Saved!";
            Active = false;
            StartCoroutine(ReActivateSave());
        }

        Debug.Log(Action);
    }

    IEnumerator ReActivateSave()
    {
        yield return new WaitForSeconds(4);
        objective.SaveTxt.text = "Save Game?";
        Active = true;
    }


}
