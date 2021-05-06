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
            Deactivate();
        }
        else if (Action == Interaction.Heal) {
            objective.HealthRefill--;
            GameManager.Instance.Player.GetComponent<Player>().health.Heal(10);
            if (objective.HealthRefill <= 0) {
                Deactivate();
            }
        }
        else if (Action == Interaction.Save) {
            SaveManager.SaveWorld();
            GameManager.Instance.ChunkManager.SaveModifiedChunks();
            SaveManager.SavePlayer();
            objective.SaveTxt.text = "Game Saved!";
            Deactivate();

            StartCoroutine(ReActivateSave());
        }
        Vector3Int coord = Utilities.GetChunkCoordonate(objective.gameObject.transform.position);
        GameManager.Instance.ChunkManager.ModifiedChunkList[coord.ToString()] = new Chunk_Data(GameManager.Instance.ChunkManager.LoadChunk(coord));
    }

    private void Deactivate()
    {
        Active = false;
        GameManager.Instance.Player.GetComponent<Player>().InRangeState = Player.InRange.Nothing;
    }

    IEnumerator ReActivateSave()
    {
        yield return new WaitForSeconds(4);
        objective.SaveTxt.text = "Save Game?";
        Active = true;
    }


}
